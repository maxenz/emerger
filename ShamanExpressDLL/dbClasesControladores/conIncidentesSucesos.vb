Imports System.Data
Imports System.Data.SqlClient
Public Class conIncidentesSucesos
    Inherits typIncidentesSucesos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDBySuceso(ByVal pVij As Long, pSus As Long) As Int64
        GetIDBySuceso = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM IncidentesSucesos WHERE IncidenteViajeId = " & pVij & " AND SucesoIncidenteId = " & pSus

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDBySuceso = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDBySuceso", ex)
        End Try
    End Function


    Public Function addSuceso(ByVal objSuceso As conIncidentesSucesos, Optional ByVal pDig As Long = 0, Optional ByVal pMot As Long = 0, Optional ByVal objDomicilio As conIncidentesDomicilios = Nothing, Optional ByVal pInTrans As Boolean = False, Optional ByVal objObservacion As conIncidentesObservaciones = Nothing, Optional pRunTrigger As Boolean = True, Optional pTpoDem As Integer = 0, Optional pAnuRet As Boolean = False) As Boolean
        addSuceso = False
        Try

            Dim vSav As Boolean = False, vVal As Boolean = True, objTipo As New conSucesosIncidentes(objSuceso.myCnnName), vErr As String = "", vMovAnt As Long
            Dim vChkSim As Boolean = False, vInc As Int64 = 0
            '-----> Valido basico del suceso

            Me.myCnnName = objSuceso.myCnnName

            If Not objSuceso Is Nothing Then
                If objTipo.Abrir(objSuceso.SucesoIncidenteId.GetObjectId) Then
                    If objTipo.TipoAplicacion = "MOV" Then
                        If objSuceso.MovilId.GetObjectId = 0 Then
                            vErr = "No ha especificado el móvil"
                        End If
                    End If
                    If objSuceso.IncidenteViajeId.GetObjectId = 0 And vErr = "" Then
                        vErr = "No hay incidente relacionado al suceso"
                    End If
                Else
                    vErr = "Debe determinar el tipo de suceso"
                End If
            Else
                vErr = "No hay instancia del suceso"
            End If

            '-----> Valido previo si hay domicilio
            If Not objDomicilio Is Nothing Then
                If objDomicilio.Domicilio.dmCalle = "" Then vErr = "Debe especificar la calle del domicilio donde deriva"
                If objDomicilio.LocalidadId.GetObjectId = 0 And vErr = "" Then vErr = "Debe especificar la localidad del domicilio donde deriva"
            End If
            If vErr = "" Then
                If Not pInTrans Then
                    Me.myCnnName = "addSuceso"
                    If shamanStartUp.AbrirConexion(Me.myCnnName) Then
                        Me.myCnnName = objSuceso.myCnnName
                        cnnsTransNET.Add(Me.myCnnName, cnnsNET(Me.myCnnName).BeginTransaction)
                    End If
                End If
                If Not objDomicilio Is Nothing Then
                    If objDomicilio.ID = 0 Then
                        objDomicilio.ID = objDomicilio.GetIDByIndex(objDomicilio.IncidenteId.ID, objDomicilio.TipoDomicilio)
                        vVal = objDomicilio.Salvar(objDomicilio)
                    End If
                End If
                If Not objObservacion Is Nothing Then
                    objObservacion.myCnnName = objSuceso.myCnnName
                    vVal = objObservacion.Salvar(objObservacion)
                    If vVal Then
                        Dim objIncidente As New conIncidentes(Me.myCnnName)
                        If objIncidente.Abrir(objObservacion.IncidenteId.ID) Then
                            objIncidente.Observaciones = objObservacion.GetByIncidenteId(objIncidente.ID)
                            vVal = objIncidente.Salvar(objIncidente)
                        End If
                    End If
                End If
                If vVal Then
                    If objSuceso.Salvar(objSuceso) Then
                        '----> Verifico Creaciones de Horarios
                        If objTipo.Abrir(objSuceso.SucesoIncidenteId.ID) Then
                            Select Case objTipo.TipoAplicacion
                                Case "HOR"
                                    '-----> Horarios del Servicio
                                    Dim objViaje As New conIncidentesViajes(objSuceso.myCnnName)
                                    If objViaje.Abrir(objSuceso.IncidenteViajeId.ID) Then
                                        Select Case objTipo.AbreviaturaId
                                            Case "G" : objViaje.HorarioOperativo.horLlamada = objSuceso.FechaHoraSuceso
                                            Case "O" : objViaje.HorarioOperativo.horInicial = objSuceso.FechaHoraSuceso
                                        End Select
                                        vSav = objViaje.Salvar(objViaje)
                                    End If
                                    objViaje = Nothing
                                Case "MOV"
                                    '-----> Horarios del Servicio
                                    Dim objViaje As New conIncidentesViajes(objSuceso.myCnnName)
                                    If objViaje.Abrir(objSuceso.IncidenteViajeId.ID) Then
                                        Select Case objTipo.AbreviaturaId
                                            Case "B", "S"
                                                If objViaje.HorarioOperativo.horDespacho.Year = 2999 Then
                                                    objViaje.HorarioOperativo.horDespacho = objSuceso.FechaHoraSuceso
                                                End If
                                                If objTipo.AbreviaturaId = "S" Then
                                                    If objViaje.HorarioOperativo.horSalida.Year = 2999 Then
                                                        objViaje.HorarioOperativo.horSalida = objSuceso.FechaHoraSuceso
                                                    End If
                                                End If
                                                If objSuceso.Condicion <> "APO" Then
                                                    vMovAnt = objViaje.MovilId.ID
                                                    objViaje.MovilId.SetObjectId(objSuceso.MovilId.ID)
                                                    If objSuceso.Condicion = "REE" Then
                                                        '----> Libero el otro móvil
                                                        LiberarMovil(vMovAnt)
                                                    End If
                                                End If
                                            Case "Z"
                                                If Year(objViaje.HorarioOperativo.horDespacho) = 2999 Then
                                                    objViaje.HorarioOperativo.horDespacho = objSuceso.FechaHoraSuceso
                                                End If
                                                vMovAnt = objViaje.MovilId.ID
                                                objViaje.HorarioOperativo.horSalida = objSuceso.FechaHoraSuceso
                                                objViaje.MovilId.SetObjectId(objSuceso.MovilId.ID)
                                                objViaje.flgModoDespacho = 1
                                                '----> Libero el otro móvil
                                                LiberarMovil(vMovAnt)
                                            Case "A"
                                                objViaje.HorarioOperativo.horLlegada = objSuceso.FechaHoraSuceso
                                                If objSuceso.Condicion <> "APO" Then objViaje.MovilId.SetObjectId(objSuceso.MovilId.ID)
                                            Case "D"
                                                objViaje.HorarioOperativo.horDerivacion = objSuceso.FechaHoraSuceso
                                                objViaje.MovilId.SetObjectId(objSuceso.MovilId.ID)
                                                If Not objDomicilio Is Nothing Then objViaje.IncidenteDomicilioId.SetObjectId(objDomicilio.ID)
                                                If pDig > 0 Then objViaje.DiagnosticoId.SetObjectId(pDig)
                                            Case "H"
                                                objViaje.HorarioOperativo.horInternacion = objSuceso.FechaHoraSuceso
                                                objViaje.MovilId.SetObjectId(objSuceso.MovilId.ID)
                                            Case "R"
                                                objViaje.HorarioOperativo.horFinalizacion = objSuceso.FechaHoraSuceso
                                                Dim vMovLst As Single = objSuceso.GetMovilPendiente(objViaje.ID, objSuceso.MovilId.ID)
                                                If vMovLst = 0 Then
                                                    objViaje.MovilId.SetObjectId(objSuceso.MovilId.ID)
                                                    objViaje.flgStatus = 1
                                                    vChkSim = True
                                                    vInc = objViaje.IncidenteDomicilioId.IncidenteId.ID
                                                    Dim vIncDom As Int64 = GetDomicilioInicial(objViaje.IncidenteDomicilioId.IncidenteId.ID, objViaje.ViajeId)
                                                    If vIncDom > 0 Then objViaje.IncidenteDomicilioId.SetObjectId(vIncDom)
                                                Else
                                                    objViaje.MovilId.SetObjectId(vMovLst)
                                                End If
                                                If pDig > 0 Then
                                                    objViaje.DiagnosticoId.SetObjectId(pDig)
                                                    objViaje.MotivoNoRealizacionId.SetObjectId(0)
                                                ElseIf pMot > 0 Then
                                                    objViaje.MotivoNoRealizacionId.SetObjectId(pMot)
                                                    objViaje.DiagnosticoId.SetObjectId(0)
                                                End If
                                                objViaje.Demora = objViaje.Demora + pTpoDem
                                        End Select
                                        vSav = objViaje.Salvar(objViaje)
                                    End If

                                    If vSav Then
                                        '-----> Horarios del Móvil
                                        Dim objMovilSuceso As New conMovilesSucesos(objSuceso.myCnnName)
                                        objMovilSuceso.CleanProperties(objMovilSuceso)
                                        objMovilSuceso.MovilId.SetObjectId(objSuceso.MovilId.ID)
                                        objMovilSuceso.FechaHoraSuceso = objSuceso.FechaHoraSuceso
                                        objMovilSuceso.IncidenteSucesoId.SetObjectId(objSuceso.ID)
                                        vSav = objMovilSuceso.SetSucesoIncidente(objMovilSuceso, , pRunTrigger)
                                    End If
                                    If vSav And pAnuRet Then
                                        '-----> Anulo el retorno
                                        If objViaje.Abrir(objViaje.GetIDByIndex(objViaje.IncidenteDomicilioId.IncidenteId.ID, "VUE")) Then
                                            objViaje.flgStatus = 1
                                            objViaje.MotivoNoRealizacionId.SetObjectId(pMot)
                                            If objViaje.Salvar(objViaje) Then
                                                'MsgBox("BREAK")
                                            End If
                                        End If
                                    End If

                                    objViaje = Nothing

                            End Select
                        End If
                        If Not pInTrans Then
                            If vSav Then
                                cnnsTransNET(Me.myCnnName).Commit()
                            Else
                                cnnsTransNET(Me.myCnnName).Rollback()
                            End If
                            cnnsTransNET.Remove(Me.myCnnName)
                            cnnsNET.Remove(Me.myCnnName)
                            '-----> Checkeo simultáneos
                            If vChkSim Then
                                Me.SetCierreSimultaneos(vInc)
                            End If
                        End If
                    End If
                Else
                    MsgBox("El domicilio de derivación es incorrecto", MsgBoxStyle.Critical, "Sucesos")
                End If
            Else
                MsgBox(vErr, MsgBoxStyle.Critical, "Sucesos")
            End If
            addSuceso = vSav
        Catch ex As Exception
            HandleError(Me.GetType.Name, "addSuceso", ex)
        End Try
    End Function
    Private Function SetCierreSimultaneos(pInc As Int64) As Boolean
        SetCierreSimultaneos = False
        Try
            Dim SQL As String

            SQL = "SELECT prg.IncidentePrgId AS ID, inc.NroAfiliado, inc.Paciente, inc.Sexo, inc.Edad, inc.NroInterno, inc.CoPago, inc.Sintomas "
            SQL = SQL & "FROM IncidentesProgramaciones prg "
            SQL = SQL & "INNER JOIN Incidentes inc ON (prg.IncidentePrgId = inc.ID) "
            SQL = SQL & "WHERE (prg.IncidenteId = " & pInc & ") AND (prg.flgSimultaneo = 1) "

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(cnnDefault))
            Dim dt As New DataTable
            dt.Load(cmdGrl.ExecuteReader)

            Dim objIncidentes As New conIncidentes

            objIncidentes.CrearSimultaneos(pInc, New DataView(dt))

            objIncidentes = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetCierreSimultaneos", ex)
        End Try
    End Function
    Public Function GetDomicilioInicial(pInc As Int64, pVij As String) As Int64

        GetDomicilioInicial = 0

        Try
            Dim SQL As String

            SQL = "SELECT ID "
            SQL = SQL & "FROM IncidentesDomicilios "
            SQL = SQL & "WHERE (IncidenteId = " & pInc & ") "
            Select Case pVij
                Case "IDA" : SQL = SQL & " AND (TipoDomicilio = 0) "
                Case "VUE" : SQL = SQL & " AND (TipoDomicilio = 1) "
                Case "AN1" : SQL = SQL & " AND (TipoDomicilio = 2) AND (NroAnexo = 1) "
                Case "AN1" : SQL = SQL & " AND (TipoDomicilio = 2) AND (NroAnexo = 2) "
            End Select

            Dim cmdDom As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdDom.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetDomicilioInicial = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDomicilioInicial", ex)
        End Try

    End Function

    Private Sub LiberarMovil(ByVal pMov As Long)
        Try
            Dim objMovilActual As New conMovilesActuales(Me.myCnnName)
            Dim objSucesoId As New conSucesosIncidentes(Me.myCnnName)

            If pMov > 0 Then
                If objMovilActual.Abrir(objMovilActual.GetIDByIndex(pMov)) Then
                    objMovilActual.SucesoIncidenteId.SetObjectId(objSucesoId.GetIDByAbreviaturaId("R"))
                    objMovilActual.Salvar(objMovilActual)
                End If
            End If

            objMovilActual = Nothing
            objSucesoId = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "LiberarMovil", ex)
        End Try
    End Sub

    Public Function GetMovilPendiente(ByVal pVij As Int64, ByVal pMov As Int64) As Int64
        GetMovilPendiente = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 mov.MovilId FROM MovilesActuales mov "
            SQL = SQL & "INNER JOIN SucesosIncidentes sus ON (mov.SucesoIncidenteId = sus.ID) "
            SQL = SQL & "WHERE (mov.IncidenteViajeId = " & pVij & ") AND (mov.MovilId <> " & pMov & ") AND (sus.AbreviaturaId <> 'R') "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetMovilPendiente = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMovilPendiente", ex)
        End Try
    End Function

    Public Function SetCierreTercerizacion(pInc As Int64, ByVal pIsLla As conIncidentesSucesos, ByVal pIsTer As conIncidentesSucesos, ByVal pIsSal As conIncidentesSucesos, ByVal pIsLle As conIncidentesSucesos, ByVal pIsFin As conIncidentesSucesos, ByVal pDig As Int64, ByVal pMot As Int64, Optional ByVal pIsDer As conIncidentesSucesos = Nothing, Optional ByVal pIsHos As conIncidentesSucesos = Nothing, Optional ByVal pDomInc As conIncidentesDomicilios = Nothing, Optional ByVal pObsInc As conIncidentesObservaciones = Nothing, Optional pVijDel As Int64 = 0, Optional pMovDel As Int64 = 0, Optional pTpoDem As Integer = 0)

        Dim cnnKey As String = "Cierre"
        SetCierreTercerizacion = False

        Try
            Dim vSav As Boolean = False

            If shamanStartUp.AbrirConexion(cnnKey) Then

                cnnsTransNET.Add(cnnKey, cnnsNET(cnnKey).BeginTransaction)

                Me.myCnnName = cnnKey

                '----> Elimino anteriores
                If pVijDel > 0 And pMovDel > 0 Then
                    Dim SQL As String = "DELETE FROM IncidentesSucesos WHERE IncidenteViajeId = " & pVijDel
                    Dim cmdDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    cmdDel.ExecuteNonQuery()
                    '------> Llamada...
                    pIsLla.myCnnName = cnnKey
                    vSav = addSuceso(pIsLla, , , , True, , False)
                    '------> Tercerización...
                    If Not pIsTer Is Nothing Then
                        pIsTer.myCnnName = cnnKey
                        vSav = addSuceso(pIsTer, , , , True, , False)
                    End If
                Else
                    vSav = True
                End If

                '----> Suceso de Salida
                If vSav Then

                    vSav = False

                    pIsSal.myCnnName = cnnKey
                    If addSuceso(pIsSal, , , , True, , False) Then
                        '----> Suceso de Llegada
                        pIsLle.myCnnName = cnnKey
                        If addSuceso(pIsLle, , , , True, , False) Then
                            vSav = True
                            '----> Suceso de Derivación
                            If Not pIsDer Is Nothing Then
                                vSav = False
                                pIsDer.myCnnName = cnnKey
                                If addSuceso(pIsDer, , , pDomInc, True, , False) Then
                                    pIsHos.myCnnName = cnnKey
                                    If addSuceso(pIsHos, , , , True, , False) Then
                                        vSav = True
                                    End If
                                End If
                            End If
                            If vSav Then
                                pIsFin.myCnnName = cnnKey
                                vSav = addSuceso(pIsFin, pDig, pMot, , True, , False, pTpoDem)
                            End If
                        End If
                    End If
                End If

                If vSav Then
                    If Not pObsInc Is Nothing Then
                        pObsInc.myCnnName = cnnKey
                        vSav = pObsInc.Salvar(pObsInc)
                        If vSav Then
                            Dim objIncidente As typIncidentes = pObsInc.IncidenteId
                            '----> Resconstruyo Observaciones
                            objIncidente.myCnnName = cnnKey
                            objIncidente.Observaciones = pObsInc.GetByIncidenteId(objIncidente.ID)
                            vSav = objIncidente.Salvar(objIncidente)
                        End If
                    End If
                End If

                If vSav Then
                    cnnsTransNET(cnnKey).Commit()
                Else
                    cnnsTransNET(cnnKey).Rollback()
                End If

                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)

                '-----> Checkeo simultáneos
                Me.SetCierreSimultaneos(pInc)

                SetCierreTercerizacion = vSav

            End If

        Catch ex As Exception

            If cnnsNET.Contains(cnnKey) Then
                cnnsTransNET(cnnKey).Rollback()
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)
            End If

            HandleError(Me.GetType.Name, "SetCierreTercerizacion", ex)

        End Try

    End Function

    Public Function GetCondicionMovil(pVij As Int64, pMov As Int64) As String

        GetCondicionMovil = "TIT"

        Try

            Dim SQL As String

            SQL = "SELECT TOP 1 Condicion FROM IncidentesSucesos "
            SQL = SQL & "WHERE (IncidenteViajeId = " & pVij & ") AND (MovilId = " & pMov & ") "
            SQL = SQL & "ORDER BY FechaHoraSuceso DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetCondicionMovil = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetCondicionMovil", ex)
        End Try

    End Function

    Public Sub SetEstadoBySP(pMov As String, pVij As Int64, Optional pSucSP As Integer = 0, Optional pDig As Int64 = 0, Optional pMot As Int64 = 0, Optional pObs As String = "")
        Try
            Dim vSpNombre As String

            Select Case pSucSP
                Case 1 : vSpNombre = "sp_SetLlegada"
                Case 2 : vSpNombre = "sp_SetFinal"
                Case Else : vSpNombre = "sp_SetSalida"
            End Select

            Dim cmd As New SqlCommand(vSpNombre, cnnsNET(cnnDefault))
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("@viajeId", SqlDbType.BigInt, 8).Value = pVij
            cmd.Parameters.Add("@movil", SqlDbType.VarChar, 10).Value = pMov
            If pSucSP = 2 Then
                cmd.Parameters.Add("@diagnosticoId", SqlDbType.BigInt, 10).Value = pDig
                cmd.Parameters.Add("@motivoNoRealizacionId", SqlDbType.BigInt, 10).Value = pMot
                cmd.Parameters.Add("@observaciones", SqlDbType.VarChar, 600).Value = pObs
            End If
            cmd.Parameters.Add("@latitud", SqlDbType.Decimal).Value = 0
            cmd.Parameters.Add("@longitud", SqlDbType.Decimal).Value = 0
            cmd.Parameters.Add("@usuarioId", SqlDbType.BigInt, 8).Value = logUsuario
            cmd.Parameters.Add("@terminalId", SqlDbType.BigInt, 8).Value = logTerminal

            cmd.Parameters.Add("@execRdo", SqlDbType.TinyInt).Value = 0
            cmd.Parameters("@execRdo").Direction = ParameterDirection.InputOutput
            cmd.Parameters.Add("@execMsg", SqlDbType.VarChar, 100).Value = ""
            cmd.Parameters("@execMsg").Direction = ParameterDirection.InputOutput

            cmd.ExecuteNonQuery()

            MsgBox(cmd.Parameters("@execRdo").Value & ": " & cmd.Parameters("@execMsg").Value)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetEstadoBySP", ex)
        End Try
    End Sub

End Class
