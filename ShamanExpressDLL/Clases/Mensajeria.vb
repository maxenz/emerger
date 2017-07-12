Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

Public Class Mensajeria

#Region "General"
    Public Function EnviarIncidente(ByVal pVijId As Int64, ByVal pMovId As Int64, Optional ByVal pAsk As Boolean = False) As Boolean

        EnviarIncidente = False

        Try
            Dim vSend As Boolean = False
            Dim vSendSta As Byte = 0
            Dim vErrMsg As String = ""
            Dim objMetodo As New typMetodosMensajeria

            Dim SQL As String

            SQL = "SELECT met.MetodoMensajeriaId, mov.Movil FROM MovilesMensajeria met "
            SQL = SQL & "INNER JOIN Moviles mov ON met.MovilId = mov.ID "
            SQL = SQL & "WHERE (met.MovilId = " & pMovId & ") "

            Dim cmdMet As New SqlCommand(SQL, cnnsNET(cnnDefault), cnnsTransNET(cnnDefault))
            Dim dtMet As New DataTable
            Dim vIdx As Integer
            dtMet.Load(cmdMet.ExecuteReader)

            For vIdx = 0 To dtMet.Rows.Count - 1

                If objMetodo.Abrir(dtMet(vIdx)(0)) Then

                    Select Case objMetodo.MetodoId
                        Case msgMetodos.Email
                            vSend = Me.EnviarEmailViaje(pVijId, pMovId, objMetodo, vErrMsg, pAsk)
                            vSendSta = setBoolToInt(vSend)
                        Case msgMetodos.Android
                            vSend = Me.EnviarAndroid(pVijId, dtMet(vIdx)(1), objMetodo, vErrMsg, pAsk)
                            vSendSta = setBoolToInt(vSend)
                        Case msgMetodos.MotoTurbo
                            vErrMsg = "Servicio en cola de espera"
                            vSendSta = 2
                            If Me.GetNroRadioByMovil(pMovId) = "" Then
                                vErrMsg = "El móvil no tiene asignado el número de radio"
                                vSendSta = 0
                            End If
                        Case msgMetodos.RabbitMQ
                            vErrMsg = "Servicio en cola de espera del móvil/prestador"
                            vSendSta = 2
                    End Select

                    '---------> Ajuste de preasignación...
                    If Not vSend And vErrMsg = "" Then
                        Exit Function
                    End If

                    Dim objEnvio As New conIncidentesViajesMensajes

                    If Not objEnvio.Abrir(objEnvio.GetIDByIndex(pVijId, pMovId)) Then
                        objEnvio.IncidenteViajeId.SetObjectId(pVijId)
                        objEnvio.MovilId.SetObjectId(pMovId)
                    End If

                    objEnvio.MetodoMensajeriaId.SetObjectId(objMetodo.ID)
                    objEnvio.opeModoMensajeria = objMetodo.MetodoId

                    objEnvio.flgEnviado = vSendSta
                    objEnvio.MensajeError = vErrMsg

                    If objEnvio.Salvar(objEnvio) Then
                        EnviarIncidente = vSend
                    End If

                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "EnviarIncidente", ex)
        End Try

    End Function

    Public Function SendNotificacion(ByVal pPre As Int64, Optional ByVal pAcc As accMailingClientes = accMailingClientes.Aceptacion) As Boolean

        SendNotificacion = False

        Try
            Dim SQL As String
            Dim objMetodo As New conMetodosMensajeria

            If objMetodo.Abrir(objMetodo.GetIDByIndex(msgMetodos.Email)) Then

                SQL = "SELECT pre.NroServicio, cli.Email "
                SQL = SQL & "FROM PreIncidentes pre "
                SQL = SQL & "INNER JOIN ClientesContactos cli ON pre.ClienteId = cli.ClienteId "
                SQL = SQL & "INNER JOIN ClientesMailing mil ON cli.ID = mil.ClienteContactoId "
                SQL = SQL & "WHERE (pre.ID = " & pPre & ") AND (mil.MailingAccionId = " & pAcc & ") "

                Dim cmdMet As New SqlCommand(SQL, cnnsNET(cnnDefault), cnnsTransNET(cnnDefault))
                Dim dtMet As New DataTable
                Dim vIdx As Integer
                dtMet.Load(cmdMet.ExecuteReader)

                For vIdx = 0 To dtMet.Rows.Count - 1

                    Dim vErrMsg As String = ""

                    If pAcc = accMailingClientes.Aceptacion Then
                        EnviarEmailPreIncidente(dtMet(vIdx)(0), "Aceptado", dtMet(vIdx)(1), objMetodo, vErrMsg)
                    Else
                        EnviarEmailPreIncidente(dtMet(vIdx)(0), "Rechazado", dtMet(vIdx)(1), objMetodo, vErrMsg)
                    End If

                Next vIdx

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SendNotificacion", ex)
        End Try

    End Function

    Public Function EnviarBitacora(ByVal pBit As Int64, Optional ByVal pAsk As Boolean = False) As Boolean

        EnviarBitacora = False

        Try

            Dim vSend As Boolean = False
            Dim vErrMsg As String = ""
            Dim objMetodo As New conMetodosMensajeria

            Dim SQL As String

            Dim objBitacora As New conBitacoras

            If objMetodo.Abrir(objMetodo.GetIDByIndex(msgMetodos.Email)) Then

                If objBitacora.Abrir(pBit) Then

                    SQL = "SELECT usr.Email FROM UsuariosMailing mli "
                    SQL = SQL & "INNER JOIN Usuarios usr ON (mli.UsuarioId = usr.ID) "
                    SQL = SQL & "WHERE (mli.MailingAccionId = " & objBitacora.Criticidad + 1 & ") "

                    Dim cmdLis As New SqlCommand(SQL, cnnsNET(cnnDefault), cnnsTransNET(cnnDefault))
                    Dim dtLis As New DataTable
                    Dim vIdx As Integer
                    dtLis.Load(cmdLis.ExecuteReader)

                    For vIdx = 0 To dtLis.Rows.Count - 1

                        vSend = Me.EnviarEmailBitacora(dtLis(vIdx)(0), objBitacora.Evento, objBitacora.Criticidad, objMetodo, vErrMsg, pAsk)

                        '---------> Ajuste de preasignación...
                        If Not vSend And vErrMsg = "" Then
                            Exit Function
                        End If

                    Next vIdx

                End If

            End If

            objBitacora = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "EnviarBitacora", ex)
        End Try

    End Function

#End Region

#Region "Email"

    Private Function ValidMetodo(ByVal objMetodo As typMetodosMensajeria, ByRef pErrMsg As String) As Boolean
        ValidMetodo = False
        pErrMsg = ""
        Try
            If objMetodo.Referencia1 = "" Then
                pErrMsg = "La cuenta enviadora no ha sido configurada"
            ElseIf objMetodo.Referencia2 = "" Then
                pErrMsg = "La contraseña de la cuenta enviadora no ha sido configurada"
            ElseIf objMetodo.Referencia3 = "" Then
                pErrMsg = "El servidor SMTP no se encuentra correctamente configurado"
            Else
                ValidMetodo = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "ValidMetodo", ex)
        End Try
    End Function

    Private Function EnviarEmailViaje(ByVal pVijId As Int64, ByVal pMovId As Int64, ByVal objMetodo As typMetodosMensajeria, ByRef pErrMsg As String, Optional ByVal pAsk As Boolean = False) As Boolean

        EnviarEmailViaje = False

        Try

            Dim _Message As New System.Net.Mail.MailMessage()
            Dim _SMTP As New System.Net.Mail.SmtpClient

            Dim vMail As String = Me.GetMovilEmail(pMovId)
            Dim objViaje As New conIncidentesViajes

            If Me.ValidMetodo(objMetodo, pErrMsg) Then

                If vMail = "" Then

                    pErrMsg = "El mail del móvil no se encuentra correctamente configurado"

                Else

                    If objViaje.Abrir(pVijId) Then

                        'CONFIGURACIÓN DEL SMTP
                        _SMTP.Credentials = New System.Net.NetworkCredential(objMetodo.Referencia1, objMetodo.Referencia2)
                        _SMTP.Host = objMetodo.Referencia3
                        If Val(objMetodo.Referencia4) > 0 Then
                            _SMTP.Port = Val(objMetodo.Referencia4)
                        Else
                            _SMTP.Port = 587
                        End If
                        If Val(objMetodo.Referencia5) = 0 Then
                            _SMTP.EnableSsl = True
                        End If


                        ' CONFIGURACION DEL MENSAJE
                        _Message.[To].Add(vMail) 'Cuenta de Correo al que se le quiere enviar el e-mail
                        _Message.From = New System.Net.Mail.MailAddress(objMetodo.Referencia1, objMetodo.Referencia1, System.Text.Encoding.UTF8) 'Quien lo envía
                        _Message.Subject = "Servicio " & objViaje.IncidenteDomicilioId.IncidenteId.FecIncidente & " - " & objViaje.IncidenteDomicilioId.IncidenteId.NroIncidente
                        _Message.SubjectEncoding = System.Text.Encoding.UTF8 'Codificacion
                        _Message.Body = GetMensajeEmail(objViaje) 'contenido del mail
                        _Message.BodyEncoding = System.Text.Encoding.UTF8
                        _Message.Priority = System.Net.Mail.MailPriority.Normal
                        _Message.IsBodyHtml = False

                        Dim vGo As Boolean = True
                        If pAsk Then
                            If MsgBox("¿ Desea notificar vía e-mail el servicio ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1, "Preasignación") = MsgBoxResult.No Then vGo = False
                        End If

                        'ENVIO
                        If vGo Then

                            'Certificado OUT
                            If Val(objMetodo.Referencia5) = 1 Then
                                ServicePointManager.ServerCertificateValidationCallback = Function(s As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) True
                            End If

                            _SMTP.Send(_Message)
                            EnviarEmailViaje = True

                        End If

                    Else

                        pErrMsg = "No pudo instanciarse el viaje asignado"

                    End If

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "EnviarEmailViaje", ex)
            pErrMsg = ex.Message
        End Try

    End Function



    Private Function EnviarEmailPreIncidente(ByVal pSrv As String, ByVal pEst As String, ByVal pDst As String, ByVal objMetodo As typMetodosMensajeria, ByRef pErrMsg As String, Optional ByVal pAsk As Boolean = False) As Boolean

        EnviarEmailPreIncidente = False

        Try

            Dim _Message As New System.Net.Mail.MailMessage()
            Dim _SMTP As New System.Net.Mail.SmtpClient

            If Me.ValidMetodo(objMetodo, pErrMsg) Then

                If pDst = "" Then

                    pErrMsg = "El mail del cliente no se encuentra correctamente configurado"

                Else

                    'CONFIGURACIÓN DEL SMTP
                    _SMTP.Credentials = New System.Net.NetworkCredential(objMetodo.Referencia1, objMetodo.Referencia2)
                    _SMTP.Host = objMetodo.Referencia3
                    If Val(objMetodo.Referencia4) > 0 Then
                        _SMTP.Port = Val(objMetodo.Referencia4)
                    Else
                        _SMTP.Port = 587
                    End If
                    _SMTP.EnableSsl = True

                    ' CONFIGURACION DEL MENSAJE
                    _Message.[To].Add(pDst) 'Cuenta de Correo al que se le quiere enviar el e-mail
                    _Message.From = New System.Net.Mail.MailAddress(objMetodo.Referencia1, objMetodo.Referencia1, System.Text.Encoding.UTF8) 'Quien lo envía
                    _Message.Subject = "Servicio " & pSrv & " fue " & pEst
                    _Message.SubjectEncoding = System.Text.Encoding.UTF8 'Codificacion
                    _Message.BodyEncoding = System.Text.Encoding.UTF8
                    _Message.Priority = System.Net.Mail.MailPriority.Normal
                    _Message.IsBodyHtml = False

                    Dim vGo As Boolean = True
                    If pAsk Then
                        If MsgBox("¿ Desea notificar vía e-mail el servicio ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1, "Preasignación") = MsgBoxResult.No Then vGo = False
                    End If

                    'ENVIO
                    If vGo Then

                        'Certificado OUT
                        If Val(objMetodo.Referencia5) = 1 Then
                            ServicePointManager.ServerCertificateValidationCallback = Function(s As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) True
                        End If

                        _SMTP.Send(_Message)

                        EnviarEmailPreIncidente = True
                    End If

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "EnviarEmailPreIncidente", ex)
            pErrMsg = ex.Message
        End Try

    End Function

    Private Function EnviarEmailBitacora(ByVal pDst As String, ByVal pEve As String, ByVal pCri As Integer, ByVal objMetodo As conMetodosMensajeria, ByRef pErrMsg As String, Optional ByVal pAsk As Boolean = False) As Boolean

        EnviarEmailBitacora = False

        Try

            Dim _Message As New System.Net.Mail.MailMessage()
            Dim _SMTP As New System.Net.Mail.SmtpClient

            If Me.ValidMetodo(objMetodo, pErrMsg) Then

                If pDst = "" Then

                    pErrMsg = "El mail del usuario no se encuentra correctamente configurado"

                Else

                    'CONFIGURACIÓN DEL SMTP
                    _SMTP.Credentials = New System.Net.NetworkCredential(objMetodo.Referencia1, objMetodo.Referencia2)
                    _SMTP.Host = objMetodo.Referencia3
                    If Val(objMetodo.Referencia4) > 0 Then
                        _SMTP.Port = Val(objMetodo.Referencia4)
                    Else
                        _SMTP.Port = 587
                    End If
                    _SMTP.EnableSsl = True

                    ' CONFIGURACION DEL MENSAJE
                    _Message.[To].Add(pDst) 'Cuenta de Correo al que se le quiere enviar el e-mail
                    _Message.From = New System.Net.Mail.MailAddress(objMetodo.Referencia1, objMetodo.Referencia1, System.Text.Encoding.UTF8) 'Quien lo envía

                    Select Case pCri
                        Case 0
                            _Message.Subject = "Shaman - Aviso"
                            _Message.Priority = System.Net.Mail.MailPriority.Low
                        Case 1
                            _Message.Subject = "Shaman - Aviso Importante"
                            _Message.Priority = System.Net.Mail.MailPriority.Normal
                        Case 2
                            _Message.Subject = "Shaman - Aviso Crítico"
                            _Message.Priority = System.Net.Mail.MailPriority.High
                    End Select

                    _Message.SubjectEncoding = System.Text.Encoding.UTF8 'Codificacion
                    _Message.Body = pEve 'contenido del mail
                    _Message.BodyEncoding = System.Text.Encoding.UTF8
                    _Message.IsBodyHtml = False

                    Dim vGo As Boolean = True
                    If pAsk Then
                        If MsgBox("¿ Desea notificar vía e-mail la bitácora ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1, "Preasignación") = MsgBoxResult.No Then vGo = False
                    End If

                    'ENVIO
                    If vGo Then

                        'Certificado OUT
                        If Val(objMetodo.Referencia5) = 1 Then
                            ServicePointManager.ServerCertificateValidationCallback = Function(s As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) True
                        End If

                        _SMTP.Send(_Message)

                        EnviarEmailBitacora = True
                    End If

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "EnviarEmailBitacora", ex)
            pErrMsg = ex.Message
        End Try

    End Function

    Public Function EnviarEmailVentaDocumento(ByVal pDst As String, ByVal pNroCmp As String, ByVal pFil As String, ByVal objMetodo As conMetodosMensajeria, ByRef pErrMsg As String, Optional ByVal pEmp As String = "", Optional pFilDet As String = "") As Boolean

        EnviarEmailVentaDocumento = False

        Try

            Dim _Message As New System.Net.Mail.MailMessage()
            Dim _SMTP As New System.Net.Mail.SmtpClient

            If Me.ValidMetodo(objMetodo, pErrMsg) Then

                If pDst = "" Then

                    pErrMsg = "El mail del cliente no se encuentra correctamente configurado"

                Else


                    'CONFIGURACIÓN DEL SMTP
                    _SMTP.Credentials = New System.Net.NetworkCredential(objMetodo.Referencia1, objMetodo.Referencia2)
                    _SMTP.Host = objMetodo.Referencia3
                    If Val(objMetodo.Referencia4) > 0 Then
                        _SMTP.Port = Val(objMetodo.Referencia4)
                    Else
                        _SMTP.Port = 587
                    End If
                    _SMTP.EnableSsl = True

                    ' CONFIGURACION DEL MENSAJE
                    Dim vDestinos() As String = pDst.Split(";")
                    Dim vIdx As Integer

                    For vIdx = 0 To UBound(vDestinos)
                        _Message.[To].Add(vDestinos(vIdx)) 'Cuenta de Correo al que se le quiere enviar el e-mail
                    Next

                    _Message.From = New System.Net.Mail.MailAddress(objMetodo.Referencia1, objMetodo.Referencia1, System.Text.Encoding.UTF8) 'Quien lo envía

                    If pEmp <> "" Then
                        _Message.Subject = "Ud. ha recibido un nuevo comprobante de " & pEmp
                    Else
                        _Message.Subject = "Ud. ha recibido un nuevo comprobante"
                    End If

                    _Message.Priority = System.Net.Mail.MailPriority.High

                    _Message.SubjectEncoding = System.Text.Encoding.UTF8 'Codificacion
                    _Message.Body = "Adjuntamos en el presente correo nuestro comprobante " & pNroCmp & " en formato PDF." & vbCrLf
                    _Message.Body = _Message.Body & vbCrLf
                    _Message.Body = _Message.Body & "Muchas gracias" & vbCrLf
                    If pEmp <> "" Then _Message.Body = _Message.Body & pEmp & vbCrLf

                    _Message.Attachments.Add(New System.Net.Mail.Attachment(pFil))
                    If pFilDet <> "" Then
                        _Message.Attachments.Add(New System.Net.Mail.Attachment(pFilDet))
                    End If

                    _Message.BodyEncoding = System.Text.Encoding.UTF8
                    _Message.IsBodyHtml = False

                    'Certificado OUT
                    If Val(objMetodo.Referencia5) = 1 Then
                        ServicePointManager.ServerCertificateValidationCallback = Function(s As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) True
                    End If

                    'ENVIO
                    _SMTP.Send(_Message)
                    EnviarEmailVentaDocumento = True

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "EnviarEmailVentaDocumento", ex)
            pErrMsg = ex.Message
        End Try

    End Function


    Public Function GetMovilEmail(ByVal pMovId As Int64) As String

        GetMovilEmail = ""

        Try
            Dim SQL As String

            'Cambiar por nueva función
            SQL = "SELECT mov.Email FROM MovilesMensajeria mov "
            SQL = SQL & "INNER JOIN MetodosMensajeria met ON mov.MetodoMensajeriaId = met.ID "
            SQL = SQL & "WHERE (mov.MovilId = " & pMovId & ") "
            SQL = SQL & "AND (met.MetodoId = " & msgMetodos.Email & ") "
            SQL = SQL & "AND (ISNULL(mov.Email, '') = '') "

            'SQL = "SELECT TOP 1 DireccionEmail FROM Equipamientos "
            'SQL = SQL & "WHERE (MovilId = " & pMovId & ") AND (Estado = 1) AND (ISNULL(DireccionEmail, '') <> '') "

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(cnnDefault))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetMovilEmail = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMovilEmail", ex)
        End Try

    End Function

    Public Function GetNroRadioByMovil(ByVal pMovId As Int64) As String

        GetNroRadioByMovil = ""

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 NroRadio FROM Equipamientos "
            SQL = SQL & "WHERE (MovilId = " & pMovId & ") AND (Estado = 1) AND (ISNULL(NroRadio, '') <> '') "

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(cnnDefault))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetNroRadioByMovil = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNroRadioByMovil", ex)
        End Try

    End Function

    Public Function GetMovilByNroRadio(ByVal pNroRadio As String) As Int64

        GetMovilByNroRadio = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 eqp.MovilId FROM Equipamientos eqp "
            SQL = SQL & "INNER JOIN Moviles mov ON (eqp.MovilId = mov.ID) "
            SQL = SQL & "WHERE (eqp.NroRadio = '" & pNroRadio & "') AND (eqp.Estado = 1) AND (ISNULL(eqp.NroRadio, '') <> '') "

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(cnnDefault))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetMovilByNroRadio = Val(vOutVal)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMovilByNroRadio", ex)
        End Try

    End Function

    Public Function GetMensajeEmail(ByVal objViaje As conIncidentesViajes) As String
        GetMensajeEmail = ""
        Try
            Dim vMsg As String = ""

            vMsg = "Localidad: " & objViaje.IncidenteDomicilioId.LocalidadId.Descripcion & vbCrLf
            vMsg = vMsg & "Domicilio: " & objViaje.IncidenteDomicilioId.Domicilio.Domicilio & vbCrLf
            vMsg = vMsg & "Entre Calle 1: " & objViaje.IncidenteDomicilioId.Domicilio.dmEntreCalle1 & vbCrLf
            vMsg = vMsg & "Entre Calle 2: " & objViaje.IncidenteDomicilioId.Domicilio.dmEntreCalle2 & vbCrLf
            vMsg = vMsg & "Referencias: " & objViaje.IncidenteDomicilioId.Domicilio.dmReferencia & vbCrLf
            vMsg = vMsg & "Síntomas: " & objViaje.IncidenteDomicilioId.IncidenteId.Sintomas & vbCrLf
            vMsg = vMsg & "Grado Operativo: " & objViaje.IncidenteDomicilioId.IncidenteId.GradoOperativoId.AbreviaturaId & vbCrLf
            vMsg = vMsg & "Aviso Especial: " & objViaje.IncidenteDomicilioId.IncidenteId.Aviso & vbCrLf
            vMsg = vMsg & vbCrLf
            vMsg = vMsg & "Paciente: " & objViaje.IncidenteDomicilioId.IncidenteId.Paciente & vbCrLf
            vMsg = vMsg & "Sexo: " & objViaje.IncidenteDomicilioId.IncidenteId.Sexo & vbCrLf
            vMsg = vMsg & "Edad: " & Math.Round(objViaje.IncidenteDomicilioId.IncidenteId.Edad, 0) & vbCrLf
            vMsg = vMsg & "Entidad: " & objViaje.IncidenteDomicilioId.IncidenteId.ClienteId.AbreviaturaId & vbCrLf
            vMsg = vMsg & "Nro. Afiliado: " & objViaje.IncidenteDomicilioId.IncidenteId.NroAfiliado & vbCrLf
            If objViaje.IncidenteDomicilioId.IncidenteId.NroInterno <> "" Then
                vMsg = vMsg & "Nro. Ref Cliente: " & objViaje.IncidenteDomicilioId.IncidenteId.NroInterno & vbCrLf
            End If
            vMsg = vMsg & "CoPago: " & objViaje.IncidenteDomicilioId.IncidenteId.CoPago & vbCrLf
            vMsg = vMsg & vbCrLf
            vMsg = vMsg & "Observaciones: " & objViaje.IncidenteDomicilioId.IncidenteId.Observaciones & vbCrLf

            GetMensajeEmail = vMsg

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMovilEmail", ex)
        End Try
    End Function

#End Region

#Region "Android"

    Private Function EnviarAndroid(ByVal pVijId As Int64, ByVal pNroMov As String, ByVal objMetodo As typMetodosMensajeria, ByRef pErrMsg As String, Optional ByVal pAsk As Boolean = False) As Boolean

        EnviarAndroid = False

        Try

            Dim objWS As New GPShamanWS.ServiceSoapClient
            Dim objViaje As New conIncidentesViajes

            If objViaje.Abrir(pVijId) Then

                If objWS.setPushNotification(sysHardKey.Replace("/", "").Replace(" ", ""), pNroMov, "Ud. tienen un nuevo servicio en " & objViaje.IncidenteDomicilioId.LocalidadId.Descripcion) Then

                    EnviarAndroid = True

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "EnviarAndroid", ex)
            pErrMsg = ex.Message
        End Try

    End Function

#End Region

#Region "MotoTurbo"

    Public Function GetMotoTurboPendientes() As DataTable

        GetMotoTurboPendientes = Nothing
        Try
            Dim dtDev As New DataTable

            dtDev.Columns.Add("ID", GetType(Int64))
            dtDev.Columns.Add("NroRadio", GetType(Int64))
            dtDev.Columns.Add("Mensaje", GetType(String))

            Dim SQL As String

            SQL = "SELECT ID, MovilId, IncidenteViajeId "
            SQL = SQL & "FROM IncidentesViajesMensajes "
            SQL = SQL & "WHERE (opeModoMensajeria = " & msgMetodos.MotoTurbo & ") "
            SQL = SQL & "AND (flgEnviado = 2) "

            Dim cmdPen As New SqlCommand(SQL, cnnsNET(cnnDefault), cnnsTransNET(cnnDefault))
            Dim dtPen As New DataTable
            Dim vIdx As Integer
            dtPen.Load(cmdPen.ExecuteReader)

            For vIdx = 0 To dtPen.Rows.Count - 1

                Dim objViaje As New conIncidentesViajes

                If objViaje.Abrir(dtPen(vIdx)("IncidenteViajeId")) Then

                    Dim dtRow As DataRow = dtDev.NewRow

                    dtRow("ID") = dtPen(vIdx)("ID")
                    dtRow("NroRadio") = Val(Me.GetNroRadioByMovil(dtPen(vIdx)("MovilId")))
                    dtRow("Mensaje") = Me.GetMensajeEmail(objViaje)

                    dtDev.Rows.Add(dtRow)

                End If

                objViaje = Nothing

            Next vIdx

            GetMotoTurboPendientes = dtDev

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMotoTurboPendientes", ex)
        End Try

    End Function

#End Region

#Region "Rabbit"

    Public Function GetRabbitMQPendientes() As DataTable

        GetRabbitMQPendientes = Nothing
        Try
            Dim SQL As String

            SQL = "SELECT msg.ID, mov.Movil, met.rabbitAlias "

            SQL = SQL & "FROM IncidentesViajesMensajes msg "
            SQL = SQL & "INNER JOIN Moviles mov ON msg.MovilId = mov.ID "
            SQL = SQL & "INNER JOIN MovilesMensajeria met ON msg.MovilId = met.MovilId AND msg.MetodoMensajeriaId =  met.MetodoMensajeriaId "

            SQL = SQL & "WHERE (msg.opeModoMensajeria = " & msgMetodos.RabbitMQ & ") "
            SQL = SQL & "AND (msg.flgEnviado = 2) "

            Dim cmdPen As New SqlCommand(SQL, cnnsNET(cnnDefault), cnnsTransNET(cnnDefault))
            Dim dtDev As New DataTable
            dtDev.Load(cmdPen.ExecuteReader)

            GetRabbitMQPendientes = dtDev

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetRabbitMQPendientes", ex)
        End Try

    End Function

#End Region

End Class
