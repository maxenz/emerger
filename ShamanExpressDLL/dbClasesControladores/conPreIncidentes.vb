Imports System.Data
Imports System.Data.SqlClient
Public Class conPreIncidentes
    Inherits typPreIncidentes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByClienteIncidenteOrigenId(ByVal pCli As Int64, ByVal pIncOri As Int64) As Int64
        GetIDByClienteIncidenteOrigenId = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM PreIncidentes WHERE ClienteId = " & pCli & " AND IncidenteOrigenId = " & pIncOri

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByClienteIncidenteOrigenId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByClienteIncidenteOrigenId", ex)
        End Try
    End Function
    Public Function GetIDByClienteNroServicio(ByVal pCli As Int64, ByVal pNro As String) As Int64
        GetIDByClienteNroServicio = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM PreIncidentes WHERE ClienteId = " & pCli & " AND NroServicio = '" & pNro & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByClienteNroServicio = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByClienteNroServicio", ex)
        End Try
    End Function
    Public Function GetIDByErrClienteNroServicio(ByVal pErrCli As String, ByVal pNro As String) As Int64
        GetIDByErrClienteNroServicio = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM PreIncidentes WHERE errCliente = '" & pErrCli & "' AND NroServicio = '" & pNro & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByErrClienteNroServicio = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByErrClienteNroServicio", ex)
        End Try
    End Function
    Public Function GetIDBySenderSubject(ByVal pSender As String, ByVal pSubject As String) As Int64
        GetIDBySenderSubject = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM PreIncidentes WHERE mailSender = '" & qyVal(pSender) & "' AND mailSubject = '" & qyVal(pSubject) & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDBySenderSubject = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDBySenderSubject", ex)
        End Try
    End Function

    Public Function havePendientes() As Boolean
        havePendientes = False
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM PreIncidentes WHERE flgStatus = 0"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then havePendientes = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "havePendientes", ex)
        End Try
    End Function

    Public Function GetInBox(Optional ByVal pEst As Integer = 0, Optional ByVal pCli As Integer = 0, Optional ByVal pGdo As Integer = 0, Optional ByVal pZon As Integer = 0) As DataTable

        GetInBox = Nothing

        Try

            Dim SQL As String

            '-----> Bandeja de Entrada

            SQL = "SELECT pre.ID, pre.ClienteId, CASE pre.ClienteId WHEN 0 THEN pre.errCliente ELSE cli.AbreviaturaId END AS Cliente, "
            SQL = SQL & "pre.NroServicio, pre.FecHorServicio, pre.Paciente, pre.Sintomas, pre.GradoOperativoId, "
            SQL = SQL & "CASE pre.GradoOperativoId WHEN 0 THEN pre.errGradoOperativo ELSE gdo.Descripcion END AS GradoOperativo, "
            SQL = SQL & "pre.Domicilio, pre.LocalidadId, CASE pre.LocalidadId WHEN 0 THEN pre.errLocalidad ELSE loc.Descripcion END AS Localidad, "
            SQL = SQL & "pre.IncidenteId "
            SQL = SQL & "FROM PreIncidentes pre "
            SQL = SQL & "LEFT JOIN Clientes cli ON pre.ClienteId = cli.ID "
            SQL = SQL & "LEFT JOIN GradosOperativos gdo ON pre.GradoOperativoId = gdo.ID "
            SQL = SQL & "LEFT JOIN Localidades loc ON pre.LocalidadId = loc.ID "
            SQL = SQL & "WHERE (pre.flgStatus = " & pEst & ") "
            If pCli > 0 Then SQL = SQL & "AND (pre.ClienteId = " & pCli & ") "
            If pGdo > 0 Then SQL = SQL & "AND (pre.GradoOperativoId = " & pGdo & ") "
            If pZon > 0 Then SQL = SQL & "AND (loc.ZonaGeograficaId = " & pZon & ") "
            SQL = SQL & "ORDER BY FecHorServicio"

            Dim cmdIn As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dtIn As New DataTable
            dtIn.Load(cmdIn.ExecuteReader)

            GetInBox = dtIn

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetInBox", ex)
        End Try
    End Function

    Public Function GetErrores() As DataTable

        GetErrores = Nothing

        Try

            Dim SQL As String
            '-----> Bandeja de Errores

            SQL = "SELECT ID, mailSender, mailSubject, errIncorporacion "
            SQL = SQL & "FROM PreIncidentes "
            SQL = SQL & "WHERE (flgStatus = 9) "
            SQL = SQL & "ORDER BY regFechaHora DESC"

            Dim cmdErr As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dtErr As New DataTable
            dtErr.Load(cmdErr.ExecuteReader)

            GetErrores = dtErr

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetErrores", ex)
        End Try
    End Function

    Public Function Confirmar(ByVal objPreIncidente As conPreIncidentes) As Boolean

        Confirmar = False

        Try
            Dim vCnfId As Int64 = 0

            Select Case objPreIncidente.GradoOperativoId.ClasificacionId
                Case gdoClasificacion.gdoIncidente : vCnfId = Me.SetIncidente(objPreIncidente)
                Case gdoClasificacion.gdoTraslado : vCnfId = Me.SetTraslado(objPreIncidente)
            End Select

            If vCnfId > 0 Then
                objPreIncidente.flgStatus = 1
                objPreIncidente.IncidenteId.SetObjectId(vCnfId)
                If objPreIncidente.Salvar(objPreIncidente) Then
                    shamanMensajeria.SendNotificacion(objPreIncidente.ID)
                    Confirmar = True
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Confirmar", ex)
        End Try

    End Function

    Private Function SetIncidente(ByVal objPreIncidente As conPreIncidentes) As Int64

        SetIncidente = 0

        Try

            Dim objIncidenteEdit As New conIncidentes
            Dim objIncidenteDomicilio As New conIncidentesDomicilios
            Dim objIncidenteObservacion As New conIncidentesObservaciones
            Dim objLocalidades As New conLocalidades
            Dim incTiempoCarga As incTiempoCarga = modDeclares.incTiempoCarga.Programado
            Dim objClientesIntegrantes As New conClientesIntegrantes

            objIncidenteEdit.CleanProperties(objIncidenteEdit)
            '---------> Cabecera del Inicidente
            With objIncidenteEdit
                .FecIncidente = objPreIncidente.FecHorServicio.Date

                If .FecIncidente < Now.Date Then
                    incTiempoCarga = modDeclares.incTiempoCarga.Historico
                ElseIf .FecIncidente = Now.Date Then
                    incTiempoCarga = modDeclares.incTiempoCarga.Presente
                End If

                .Telefono = objPreIncidente.Telefono
                .TelefonoFix = GetNumericValue(objPreIncidente.Telefono)
                .GradoOperativoId.SetObjectId(objPreIncidente.GradoOperativoId.ID)
                .ClienteId.SetObjectId(objPreIncidente.ClienteId.ID)
                .ClienteIntegranteId.SetObjectId(objClientesIntegrantes.GetIDByNroAfiliado(objPreIncidente.ClienteId.ID, objPreIncidente.NroAfiliado))
                .NroAfiliado = objPreIncidente.NroAfiliado
                .Paciente = objPreIncidente.Paciente
                .Sexo = objPreIncidente.Sexo
                .Edad = objPreIncidente.Edad
                .PlanId = objPreIncidente.PlanId
                .Sintomas = objPreIncidente.Sintomas
                .CoPago = objPreIncidente.CoPago
                .flgIvaGravado = objPreIncidente.flgIvaGravado
                .NroInterno = objPreIncidente.NroServicio
            End With

            '---------> Domiclio de IDA
            With objIncidenteDomicilio
                .CleanProperties(objIncidenteDomicilio)
                .LocalidadId.SetObjectId(objPreIncidente.LocalidadId.ID)
                .Domicilio.dmCalle = objPreIncidente.Domicilio.dmCalle
                .Domicilio.dmAltura = objPreIncidente.Domicilio.dmAltura
                .Domicilio.dmPiso = objPreIncidente.Domicilio.dmPiso
                .Domicilio.dmDepto = objPreIncidente.Domicilio.dmDepto
                .Domicilio.dmEntreCalle1 = objPreIncidente.Domicilio.dmEntreCalle1
                .Domicilio.dmEntreCalle2 = objPreIncidente.Domicilio.dmEntreCalle2
                .Domicilio.dmReferencia = objPreIncidente.Domicilio.dmReferencia
                SetLatLong(.Domicilio, .LocalidadId.GetObjectId)
            End With

            '--------> Observaciones
            With objIncidenteObservacion
                .CleanProperties(objIncidenteObservacion)
                .Observaciones = objPreIncidente.Observaciones
            End With

            '--------> Ajuste Histórico
            Dim vFhr As DateTime = NullDateTime
            If incTiempoCarga <> modDeclares.incTiempoCarga.Presente Then vFhr = objPreIncidente.FecHorServicio

            '--------> Salvo Incidente
            If objIncidenteEdit.SetIncidente(objIncidenteEdit, objIncidenteDomicilio, objIncidenteObservacion, vFhr, , , incTiempoCarga) Then

                '--------> Arreglo padrón
                Me.FixPadron(objIncidenteEdit, objIncidenteDomicilio)

                '--------> Unlock
                Dim objLocks As New conlckIncidentes

                objLocks.unlockIncidente(objIncidenteEdit.FecIncidente, objIncidenteEdit.NroIncidente)

                SetIncidente = objIncidenteEdit.ID

            End If

            objIncidenteEdit = Nothing
            objIncidenteDomicilio = Nothing
            objIncidenteObservacion = Nothing
            objLocalidades = Nothing
            objClientesIntegrantes = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetIncidente", ex)
        End Try
    End Function

    Private Function SetTraslado(ByVal objPreIncidente As conPreIncidentes) As Int64

        SetTraslado = 0

        Try

            Dim objIncidentes As New conIncidentes
            Dim vAddNew As Boolean = True
            Dim vAddPac As Boolean = False

            If objIncidentes.ValidarIncidente(objPreIncidente.FecHorServicio.Date, 0, objPreIncidente.ClienteId.AbreviaturaId, objPreIncidente.ClienteId.ID, objPreIncidente.NroAfiliado, objPreIncidente.GradoOperativoId.ID, objPreIncidente.Domicilio.dmCalle, objPreIncidente.LocalidadId.ID, _
                                              objPreIncidente.Paciente, vAddPac, gdoClasificacion.gdoTraslado, objPreIncidente.DERDomicilio.DERdmCalle, objPreIncidente.DERLocalidadId.ID, , 0) Then

                If objIncidentes.ValidarTraslado(objPreIncidente.GradoOperativoId.ID, setIntToBool(objPreIncidente.flgIdaVuelta), objPreIncidente.FecHorServicio, _
                                                  objPreIncidente.DERFecHorServicio, objPreIncidente.DERFecHorRetorno, objIncidentes.ID, incTiempoCarga.Programado) Then

                    Dim objIncidenteEdit As New conIncidentes
                    Dim objIncidenteDomicilio As New conIncidentesDomicilios
                    Dim objDstIncidenteDomicilio As New conIncidentesDomicilios
                    Dim objAn1IncidenteDomicilio As New conIncidentesDomicilios
                    Dim objAn2IncidenteDomicilio As New conIncidentesDomicilios
                    Dim objIncidenteObservacion As New conIncidentesObservaciones
                    Dim objClientesIntegrantes As New conClientesIntegrantes
                    Dim objLocks As New conlckTraslados


                    '---------> Cabecera del Inicidente
                    With objIncidenteEdit
                        objIncidenteEdit.CleanProperties(objIncidenteEdit)
                        .FecIncidente = objPreIncidente.FecHorRecepcion.Date
                        .flgCubierto = 1
                        .TrasladoId = objLocks.getNewTraslado()
                        .Telefono = objPreIncidente.Telefono
                        .TelefonoFix = GetNumericValue(objPreIncidente.Telefono)
                        .GradoOperativoId.SetObjectId(objPreIncidente.GradoOperativoId.ID)
                        .ClienteId.SetObjectId(objPreIncidente.ClienteId.ID)
                        .ClienteIntegranteId.SetObjectId(objClientesIntegrantes.GetIDByNroAfiliado(objPreIncidente.ClienteId.ID, objPreIncidente.NroAfiliado))
                        .NroAfiliado = objPreIncidente.NroAfiliado
                        .Paciente = objPreIncidente.Paciente
                        .Sexo = objPreIncidente.Sexo
                        .Edad = objPreIncidente.Edad
                        .PlanId = objPreIncidente.PlanId
                        .Sintomas = objPreIncidente.Sintomas
                        .NroInterno = objPreIncidente.NroServicio
                        .CoPago = objPreIncidente.CoPago
                        .trsIdaVuelta = objPreIncidente.flgIdaVuelta
                        .flgIvaGravado = objPreIncidente.flgIvaGravado
                    End With

                    '---------> Domiclio Origen
                    With objIncidenteDomicilio
                        .CleanProperties(objIncidenteDomicilio)
                        .SanatorioId.SetObjectId(objPreIncidente.SanatorioId.ID)
                        .LocalidadId.SetObjectId(objPreIncidente.LocalidadId.ID)
                        .Domicilio.dmCalle = objPreIncidente.Domicilio.dmCalle
                        .Domicilio.dmAltura = objPreIncidente.Domicilio.dmAltura
                        .Domicilio.dmPiso = objPreIncidente.Domicilio.dmPiso
                        .Domicilio.dmDepto = objPreIncidente.Domicilio.dmDepto
                        .Domicilio.dmEntreCalle1 = objPreIncidente.Domicilio.dmEntreCalle1
                        .Domicilio.dmEntreCalle2 = objPreIncidente.Domicilio.dmEntreCalle2
                        .Domicilio.dmReferencia = objPreIncidente.Domicilio.dmReferencia
                        SetLatLong(.Domicilio, .LocalidadId.GetObjectId)
                    End With

                    '---------> Domiclio Destino
                    With objDstIncidenteDomicilio
                        .CleanProperties(objDstIncidenteDomicilio)
                        .SanatorioId.SetObjectId(objPreIncidente.DERSanatorioId.ID)
                        .LocalidadId.SetObjectId(objPreIncidente.DERLocalidadId.ID)
                        .Domicilio.dmCalle = objPreIncidente.DERDomicilio.DERdmCalle
                        .Domicilio.dmAltura = objPreIncidente.DERDomicilio.DERdmAltura
                        .Domicilio.dmPiso = objPreIncidente.DERDomicilio.DERdmPiso
                        .Domicilio.dmDepto = objPreIncidente.DERDomicilio.DERdmDepto
                        .Domicilio.dmEntreCalle1 = objPreIncidente.DERDomicilio.DERdmEntreCalle1
                        .Domicilio.dmEntreCalle2 = objPreIncidente.DERDomicilio.DERdmEntreCalle2
                        .Domicilio.dmReferencia = objPreIncidente.DERDomicilio.DERdmReferencia
                        SetLatLong(.Domicilio, .LocalidadId.GetObjectId)
                    End With

                    '---------> Domiclio Anexo I
                    objAn1IncidenteDomicilio.CleanProperties(objAn1IncidenteDomicilio)
                    '---------> Domiclio Anexo II
                    objAn2IncidenteDomicilio.CleanProperties(objAn2IncidenteDomicilio)
                    '--------> Observaciones
                    With objIncidenteObservacion
                        .CleanProperties(objIncidenteObservacion)
                        .Observaciones = objPreIncidente.Observaciones
                    End With

                    '--------> Salvo Incidente
                    If objIncidenteEdit.SetTraslado(objIncidenteEdit, objIncidenteDomicilio, objDstIncidenteDomicilio, objAn1IncidenteDomicilio, objAn2IncidenteDomicilio, Nothing, objIncidenteObservacion, objPreIncidente.FecHorServicio, _
                        objPreIncidente.DERFecHorServicio, objPreIncidente.DERFecHorRetorno, objPreIncidente.DiagnosticoId.ID, incTiempoCarga.Programado) Then

                        objLocks.unlockTraslado(objIncidenteEdit.TrasladoId)
                        objIncidentes = objIncidenteEdit

                        '-----> Fix Padrón
                        Me.FixPadron(objIncidenteEdit, objIncidenteDomicilio)

                        '-----> ...
                        SetTraslado = objIncidenteEdit.ID

                    End If

                    '--------> Limpio variables
                    objIncidenteEdit = Nothing
                    objIncidenteDomicilio = Nothing
                    objDstIncidenteDomicilio = Nothing
                    objAn1IncidenteDomicilio = Nothing
                    objAn2IncidenteDomicilio = Nothing
                    objIncidenteObservacion = Nothing

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetTraslado", ex)
        End Try
    End Function

    Private Sub FixPadron(ByVal pObjInc As conIncidentes, ByVal pObjDom As conIncidentesDomicilios)
        Try
            If pObjInc.NroAfiliado <> "" Then

                If pObjInc.Abrir(pObjInc.ID) Then

                    Dim objIntegrante As New conClientesIntegrantes(pObjInc.myCnnName)

                    If Not objIntegrante.Abrir(objIntegrante.GetIDByNroAfiliado(pObjInc.ClienteId.ID, pObjInc.NroAfiliado)) Then
                        objIntegrante.CleanProperties(objIntegrante)
                        objIntegrante.ClienteId.SetObjectId(pObjInc.ClienteId.ID)
                        objIntegrante.NroAfiliado = pObjInc.NroAfiliado
                        objIntegrante.TipoIntegrante = "PER"
                        objIntegrante.FecIngreso = Now.Date
                    End If

                    If objIntegrante.Apellido = "" Then
                        objIntegrante.Apellido = Parcer(pObjInc.Paciente, " ")
                        If pObjInc.Paciente.Contains(" ") Then objIntegrante.Nombre = Parcer(pObjInc.Paciente, " ", 1)
                    End If

                    If objIntegrante.Telefono01 = "" Then
                        objIntegrante.Telefono01 = pObjInc.Telefono
                        objIntegrante.Telefono01Fix = pObjInc.TelefonoFix
                    End If

                    If objIntegrante.Domicilio.dmCalle = "" Then
                        If pObjDom.Abrir(pObjDom.ID) Then
                            objIntegrante.Domicilio.dmCalle = pObjDom.Domicilio.dmCalle
                            objIntegrante.Domicilio.dmAltura = pObjDom.Domicilio.dmAltura
                            objIntegrante.Domicilio.dmPiso = pObjDom.Domicilio.dmPiso
                            objIntegrante.Domicilio.dmDepto = pObjDom.Domicilio.dmDepto
                            objIntegrante.Domicilio.dmEntreCalle1 = pObjDom.Domicilio.dmEntreCalle1
                            objIntegrante.Domicilio.dmEntreCalle2 = pObjDom.Domicilio.dmEntreCalle2
                            objIntegrante.Domicilio.dmReferencia = pObjDom.Domicilio.dmReferencia
                            objIntegrante.Domicilio.dmLatitud = pObjDom.Domicilio.dmLatitud
                            objIntegrante.Domicilio.dmLongitud = pObjDom.Domicilio.dmLongitud
                            objIntegrante.LocalidadId.SetObjectId(pObjDom.LocalidadId.ID)
                        End If
                    End If

                    If GetEdad(objIntegrante.FecNacimiento) > 100 Or GetEdad(objIntegrante.FecNacimiento) < 0 Then
                        Try
                            objIntegrante.FecNacimiento = CDate(Now.Day & "/" & Now.Month & "/" & CInt(Now.Year - pObjInc.Edad))
                        Catch ex As Exception

                        End Try
                    End If

                    If objIntegrante.Sexo = "" Then
                        objIntegrante.Sexo = pObjInc.Sexo
                    End If

                    If objIntegrante.Salvar(objIntegrante) Then

                        pObjInc.ClienteIntegranteId.SetObjectId(objIntegrante.ID)
                        pObjInc.Salvar(pObjInc)

                    End If

                    objIntegrante = Nothing

                End If

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "FixPadron", ex)
        End Try
    End Sub

End Class
