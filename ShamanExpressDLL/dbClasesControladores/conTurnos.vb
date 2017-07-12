Imports System.Data
Imports System.Data.SqlClient

Public Class conTurnos
    Inherits typTurnos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function IsSuspendido(ByVal pMed As Int64, ByVal pFhr As DateTime, ByVal pPra As Int64, ByVal pSal As Int64) As Boolean
        IsSuspendido = False
        Try
            Dim SQL As String

            SQL = "SELECT exc.ID FROM TurnosExcepciones exc "
            SQL = SQL & "INNER JOIN CentrosAtencionSalas sal ON exc.CentroAtencionSalaId = sal.ID "
            SQL = SQL & "WHERE (exc.PersonalId = " & pMed & ") AND (exc.PracticaId = " & pPra & ") "
            SQL = SQL & "AND (exc.CentroAtencionSalaId = " & pSal & ") "
            SQL = SQL & "AND (exc.TipoExcepcion = 0) "
            SQL = SQL & "AND ('" & DateTimeToSql(pFhr) & "' BETWEEN exc.FechaHoraDesde AND exc.FechaHoraHasta) "

            Dim cmdSus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmdSus.ExecuteScalar > 0 Then IsSuspendido = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "IsSuspendido", ex)
        End Try
    End Function

    Public Function getDtHistoriaClinica(ByVal pDes As Date, ByVal pHas As Date, Optional ByVal pIte As Int64 = 0, Optional pFul As Boolean = True, Optional pOmiNoA As Boolean = False) As DataTable

        getDtHistoriaClinica = Nothing

        Try

            If pIte > 0 Then
                Dim SQL As String

                SQL = "SELECT 'TUR' AS TipoMovimiento, tur.ID, tur.FechaHoraTurno, pra.Descripcion AS Practica, cen.AbreviaturaId AS Centro, "
                SQL = SQL & "per.Apellido, per.Nombre, sal.AbreviaturaId AS Sala, 0 AS IncidenteId "
                SQL = SQL & "FROM Turnos tur "
                SQL = SQL & "INNER JOIN Practicas pra ON (tur.PracticaId = pra.ID) "
                SQL = SQL & "INNER JOIN CentrosAtencionSalas sal ON (tur.CentroAtencionSalaId = sal.ID) "
                SQL = SQL & "INNER JOIN CentrosAtencion cen ON (sal.CentroAtencionId = cen.ID) "
                SQL = SQL & "INNER JOIN Personal per ON (tur.PersonalId = per.ID) "
                SQL = SQL & "INNER JOIN EstadosTurnos est ON (tur.EstadoTurnoId = est.ID) "
                SQL = SQL & "WHERE (tur.FechaHoraTurno BETWEEN '" & DateToSql(pDes, , True) & "' AND '" & DateToSql(pHas, , True, False) & "') "
                SQL = SQL & "AND (est.flgDisponible = 0) "
                SQL = SQL & "AND (tur.ClienteIntegranteId = " & pIte & ") "
                If pOmiNoA Then SQL = SQL & "AND (est.AbreviaturaId = 'A') "

                If pFul Then

                    SQL = SQL & "UNION "
                    SQL = SQL & "SELECT 'INC' AS TipoMovimiento, vij.ID, vij.horLlamada AS FechaHoraTurno, gdo.Descripcion AS Practica, mov.Movil AS Centro, "
                    SQL = SQL & "'' AS Apellido, '' AS Nombre, '' AS Sala, dom.IncidenteId "

                    SQL = SQL & "FROM Incidentes inc "
                    SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (inc.ID = dom.IncidenteId) "
                    SQL = SQL & "INNER JOIN IncidentesViajes vij ON (dom.ID = vij.IncidenteDomicilioId) "
                    SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
                    SQL = SQL & "INNER JOIN Clientes cli ON (inc.ClienteId = cli.ID) "
                    SQL = SQL & "LEFT JOIN Diagnosticos dig ON (vij.DiagnosticoId = dig.ID) "
                    SQL = SQL & "LEFT JOIN Moviles mov ON (vij.MovilId = mov.ID) "
                    SQL = SQL & "WHERE (inc.FecIncidente BETWEEN '" & DateToSql(pDes) & "' AND '" & DateToSql(pHas) & "') "
                    SQL = SQL & "AND (vij.ViajeId <> 'VUE') "
                    SQL = SQL & "AND (inc.ClienteIntegranteId = " & pIte & ") "

                End If

                SQL = SQL & " ORDER BY FechaHoraTurno DESC"

                Dim cmdHis As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vHisTbl As New DataTable

                vHisTbl.Load(cmdHis.ExecuteReader)

                getDtHistoriaClinica = vHisTbl

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getRsHistoriaClinica", ex)
        End Try

    End Function

    Public Function Validar(ByVal pFecHor As String, ByVal pCliAbr As String, ByVal pCli As Int64, ByVal pAfl As String, ByVal pDoc As Int64, ByVal pPra As Int64, ByVal pCen As Int64, ByVal pSal As Int64, ByVal pMed As Int64, ByVal pPac As String, ByVal pPla As Int64, ByVal pPlaCod As String, ByVal pIsMed As Boolean, ByVal pHavNml As Boolean, pId As Int64, ByRef pAddPac As Boolean) As Boolean
        Validar = False
        Try
            pAddPac = False
            Dim vMsgErr As String = ""
            If CDate(pFecHor) < Now.Date And Me.ID = 0 And shamanConfigClinica.flgCargaHistorica = 0 Then vMsgErr = "La fecha/hora del turno no puede ser superior a este momento"
            If vMsgErr = "" Then
                If pCli = 0 Then
                    vMsgErr = "Debe establecer el código de cliente"
                ElseIf pAfl <> "" Then
                    Dim objIntegrante As New conClientesIntegrantes
                    If objIntegrante.GetIDByNroAfiliado(pCli, pAfl) = 0 Then
                        If MsgBox("El afiliado no se encuentra en padrón de " & pCliAbr & vbCrLf & "Confirma ?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Afiliados") = MsgBoxResult.No Then
                            Exit Function
                        Else
                            pAddPac = True
                        End If
                    End If
                End If
                If vMsgErr = "" Then
                    If pPra = 0 Then vMsgErr = "Debe establecer la práctica que solicita"
                    If pCen = 0 And vMsgErr = "" Then vMsgErr = "Debe establecer el centro de atención"
                    If pSal = 0 And vMsgErr = "" Then vMsgErr = "Debe establecer la sala del centro de atención"
                    If pMed = 0 And vMsgErr = "" Then vMsgErr = "Debe establecer el profesional que atenderá"
                    If pPac = "" And vMsgErr = "" Then vMsgErr = "Debe establecer el nombre del paciente"
                End If
            End If
            If vMsgErr = "" And shamanConfigClinica.reqIngresoTurno > 0 Then
                If (shamanConfigClinica.reqIngresoTurno = 1 Or shamanConfigClinica.reqIngresoTurno = 3) And pDoc = 0 Then vMsgErr = "Debe establecer el nro. de documento del paciente"
                If (shamanConfigClinica.reqIngresoTurno = 2 Or shamanConfigClinica.reqIngresoTurno = 3) And pAfl = "" Then vMsgErr = "Debe establecer el nro. de afiliado del paciente"
            End If
            If vMsgErr = "" And pIsMed And Not pHavNml Then vMsgErr = "Debe establecer al menos una práctica de nomenclador"

            '---> Valido sobreturno
            If vMsgErr = "" And CDate(pFecHor) > Now Then
                vMsgErr = Me.chekSobreturno(CDate(pFecHor), pPra, pSal, pMed, pId)
                If vMsgErr <> "" Then
                    If MsgBox(vMsgErr & vbCrLf & "¿ Confirma sobreturno ?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1, "Sobreturnos") = MsgBoxResult.Yes Then
                        vMsgErr = ""
                    End If
                End If
            End If

            '---> Valido Disponibilidad
            If vMsgErr = "" And CDate(pFecHor) > Now Then
                vMsgErr = Me.checkAtencion(pFecHor, pPra, pSal, pMed)
            End If

            '---> Valido Cobertura
            If vMsgErr = "" And shamanConfigClinica.modSinCobertura = 2 Then
                HaveCoberturaPractica(pCli, pPra, pPla, pPlaCod, 0, vMsgErr)
            End If

            '---> Valido Morosidad
            If vMsgErr = "" And shamanConfig.modMorosidad = 2 Then
                Dim objClientes As New conClientes
                vMsgErr = objClientes.GetEstadoMorosidad(pCli)
                objClientes = Nothing
            End If

            If vMsgErr <> "" Then
                MsgBox(vMsgErr, MsgBoxStyle.Critical, "Turnos")
            Else
                Validar = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Private Function chekSobreturno(ByVal pFecHor As Date, ByVal pPra As Int64, ByVal pSal As Int64, ByVal pMed As Int64, Optional pIdExc As Int64 = 0) As String
        chekSobreturno = ""
        Try
            Dim SQL As String

            SQL = "SELECT tur.Paciente FROM Turnos tur "
            SQL = SQL & "INNER JOIN EstadosTurnos etu ON tur.EstadoTurnoId = etu.ID "
            SQL = SQL & "WHERE (tur.PersonalId = " & pMed & ") AND (tur.PracticaId = " & pPra & ") "
            SQL = SQL & "AND (tur.CentroAtencionSalaId = " & pSal & ") "
            SQL = SQL & "AND (tur.FechaHoraTurno = '" & DateTimeToSql(pFecHor) & "') "
            SQL = SQL & "AND (tur.ID <> " & pIdExc & ") AND (etu.flgDisponible = 0) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vPac As String = cmFind.ExecuteScalar
            If Not vPac Is Nothing Then
                chekSobreturno = "El horario del turno está ocupado por " & vPac
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "chekSobreturno", ex)
        End Try

    End Function


    Private Function checkAtencion(ByVal pFecHor As String, ByVal pPra As Int64, ByVal pSal As Int64, ByVal pMed As Int64) As String
        checkAtencion = ""
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM PersonalHorariosAtencion "
            SQL = SQL & "WHERE (PersonalId = " & pMed & ") AND (PracticaId = " & pPra & ") "
            SQL = SQL & "AND (CentroAtencionSalaId = " & pSal & ") "
            SQL = SQL & "AND (DiaSemana = '" & getNroDayWeek(CDate(pFecHor)) & "') "
            SQL = SQL & "AND ('" & CDate(pFecHor).TimeOfDay.ToString & "' BETWEEN HorEntrada AND HorSalida) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmFind.ExecuteScalar Is Nothing Then
                checkAtencion = "El horario del turno no coincide con los horarios disponibles del médico y la práctica"
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "checkAtencion", ex)
        End Try

    End Function

    Public Function SetTurno(ByVal objTurno As conTurnos) As Boolean

        Dim cnnKey As String = "Turnos"
        SetTurno = False

        Try

            Dim vSav As Boolean = False

            If shamanStartUp.AbrirConexion(cnnKey) Then

                objTurno.myCnnName = cnnKey

                cnnsTransNET.Add(cnnKey, cnnsNET(cnnKey).BeginTransaction)

                If objTurno.Salvar(objTurno) Then

                    Dim vNroEst As Integer = 1

                    If Not haveEstado(objTurno.ID, vNroEst) Then

                        Dim objTurnoEstado As New typTurnosEstados(cnnKey)
                        Dim objEstadoTurno As New conEstadosTurnos(cnnKey)

                        objTurnoEstado.CleanProperties(objTurnoEstado)
                        objTurnoEstado.TurnoId.SetObjectId(objTurno.ID)
                        objTurnoEstado.EstadoTurnoId.SetObjectId(objEstadoTurno.GetIDByNroOrden(vNroEst))
                        objTurnoEstado.FechaHoraSuceso = GetCurrentTime()

                        vSav = Me.SetEstadoTurno(objTurnoEstado, objTurno)

                    Else

                        vSav = True

                    End If

                End If

                If vSav Then
                    cnnsTransNET(cnnKey).Commit()
                Else
                    cnnsTransNET(cnnKey).Rollback()
                End If
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)

                '----> Restauro Valores
                objTurno.myCnnName = cnnDefault
                '----> Resultado
                SetTurno = vSav

            End If

        Catch ex As Exception
            If cnnsNET.Contains(cnnKey) Then
                cnnsTransNET(cnnKey).Rollback()
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)
            End If
            '----> Restauro Valores
            objTurno.myCnnName = cnnDefault

            HandleError(Me.GetType.Name, "SetTurno", ex)
        End Try
    End Function

    Private Function haveEstado(ByVal pTur As Int64, ByVal pEst As Integer) As Boolean
        haveEstado = False
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 tur.ID FROM TurnosEstados tur "
            SQL = SQL & "INNER JOIN EstadosTurnos est ON (tur.EstadoTurnoId = est.ID) "
            SQL = SQL & "WHERE (tur.TurnoId = " & pTur & ") AND (est.NroOrden >= " & pEst & ")"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmFind.ExecuteScalar > 0 Then haveEstado = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "haveEstado", ex)
        End Try
    End Function

    Public Function SetEstadoTurno(ByVal objTurnoEstado As typTurnosEstados, ByVal objTurno As conTurnos, Optional ByVal pNewCnn As Boolean = False) As Boolean
        Dim cnnKey As String = "TurnosEstados"
        SetEstadoTurno = False

        Try
            Dim vSav As Boolean = False


            If pNewCnn Then

                If shamanStartUp.AbrirConexion(cnnKey) Then
                    objTurno.myCnnName = cnnKey

                    cnnsTransNET.Add(cnnKey, cnnsNET(cnnKey).BeginTransaction)

                    objTurnoEstado.myCnnName = cnnKey
                    objTurno.myCnnName = cnnKey
                Else
                    Exit Function
                End If
            End If

            If objTurnoEstado.Salvar(objTurnoEstado) Then
                objTurno.EstadoTurnoId.SetObjectId(objTurnoEstado.EstadoTurnoId.GetObjectId)
                objTurno.FechaHoraEstado = objTurnoEstado.FechaHoraSuceso
                If objTurno.Salvar(objTurno) Then
                    vSav = True
                End If
            End If

            SetEstadoTurno = vSav

            If pNewCnn Then

                If vSav Then
                    cnnsTransNET(cnnKey).Commit()
                Else
                    cnnsTransNET(cnnKey).Rollback()
                End If
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)

                objTurnoEstado.myCnnName = cnnDefault
                objTurno.myCnnName = cnnDefault
            End If

        Catch ex As Exception

            If cnnsNET.Contains(cnnKey) Then
                cnnsTransNET(cnnKey).Rollback()
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)
            End If

            objTurnoEstado.myCnnName = cnnDefault
            objTurno.myCnnName = cnnDefault

            HandleError(Me.GetType.Name, "SetEstadoTurno", ex)
        End Try
    End Function
    Public Function HaveTurnosProximos(pIte As Int64) As Boolean
        HaveTurnosProximos = False
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM Turnos WHERE (ClienteIntegranteId = " & pIte & ") AND (FechaHoraTurno >= '" & DateTimeToSql(Now.Date) & "') "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmFind.ExecuteScalar > 0 Then HaveTurnosProximos = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "HaveTurnosProximos", ex)
        End Try
    End Function
    Public Function GetTurnosProximos(pIte As Int64, Optional ByVal pFec As String = "", Optional ByVal pHor As String = "", Optional ByVal pCen As String = "", Optional ByVal pSal As String = "", Optional ByVal pPra As String = "", Optional ByVal pMed As String = "") As DataTable

        GetTurnosProximos = Nothing

        Try

            Dim SQL As String
            SQL = "SELECT tur.ID, tur.FechaHoraTurno, cen.AbreviaturaId, sal.AbreviaturaId, pra.Descripcion, med.Apellido + ' ' + med.Nombre "
            SQL = SQL & "FROM Turnos tur "
            SQL = SQL & "INNER JOIN CentrosAtencionSalas sal ON (tur.CentroAtencionSalaId = sal.ID) "
            SQL = SQL & "INNER JOIN CentrosAtencion cen ON (sal.CentroAtencionId = cen.ID) "
            SQL = SQL & "INNER JOIN Practicas pra ON (tur.PracticaId = pra.ID) "
            SQL = SQL & "INNER JOIN Personal med ON (tur.PersonalId = med.ID) "
            SQL = SQL & "WHERE (ClienteIntegranteId = " & pIte & ") AND (FechaHoraTurno >= '" & DateTimeToSql(Now.Date) & "') "
            If pFec.Length > 0 Then SQL = sqlWhere(SQL) & "(tur.FechaHoraTurno LIKE '%" & pFec & "%')"
            If pHor.Length > 0 Then SQL = sqlWhere(SQL) & "(tur.FechaHoraTurno LIKE '%" & pHor & "%')"
            If pCen.Length > 0 Then SQL = sqlWhere(SQL) & "(cen.AbreviaturaId LIKE '%" & pCen & "%')"
            If pSal.Length > 0 Then SQL = sqlWhere(SQL) & "(sal.AbreviaturaId LIKE '%" & pSal & "%')"
            If pPra.Length > 0 Then SQL = sqlWhere(SQL) & "(pra.Descripcion LIKE '%" & pPra & "%')"
            If pMed.Length > 0 Then SQL = sqlWhere(SQL) & "(med.Apellido LIKE '%" & pMed & "%')"
            SQL = SQL & " ORDER BY tur.FechaHoraTurno"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdGrl.ExecuteReader)

            GetTurnosProximos = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetTurnosProximos", ex)
        End Try
    End Function

    Public Function MoveFirst(ByVal pFec As Date, Optional ByVal pPra As Int64 = 0, Optional ByVal pCen As Int64 = 0, Optional ByVal pSal As Int64 = 0, Optional ByVal pMed As Int64 = 0) As Int64
        MoveFirst = Mover(pFec, pPra, pCen, pSal, pMed)
    End Function
    Public Function MovePrevious(ByVal pFec As Date, ByVal pLstId As Int64, Optional ByVal pPra As Int64 = 0, Optional ByVal pCen As Int64 = 0, Optional ByVal pSal As Int64 = 0, Optional ByVal pMed As Int64 = 0) As Int64
        MovePrevious = Mover(pFec, pPra, pCen, pSal, pMed, , pLstId)
    End Function
    Public Function MoveNext(ByVal pFec As Date, ByVal pLstId As Int64, Optional ByVal pPra As Int64 = 0, Optional ByVal pCen As Int64 = 0, Optional ByVal pSal As Int64 = 0, Optional ByVal pMed As Int64 = 0) As Int64
        MoveNext = Mover(pFec, pPra, pCen, pSal, pMed, True, pLstId)
    End Function
    Public Function MoveLast(ByVal pFec As Date, Optional ByVal pPra As Int64 = 0, Optional ByVal pCen As Int64 = 0, Optional ByVal pSal As Int64 = 0, Optional ByVal pMed As Int64 = 0) As Int64
        MoveLast = Mover(pFec, pPra, pCen, pSal, pMed, True)
    End Function
    Private Function Mover(ByVal pFec As Date, Optional ByVal pPra As Int64 = 0, Optional ByVal pCen As Int64 = 0, Optional ByVal pSal As Int64 = 0, Optional ByVal pMed As Int64 = 0, Optional ByVal pDer As Boolean = False, Optional ByVal pLstId As Int64 = 0) As Int64
        Mover = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 tur.ID FROM Turnos tur "
            SQL = SQL & "INNER JOIN CentrosAtencionSalas sal ON (tur.CentroAtencionSalaId = sal.ID) "
            SQL = SQL & "WHERE (tur.FechaHoraTurno BETWEEN '" & DateToSql(pFec, , True) & "' AND '" & DateToSql(pFec, , True, False) & "') "
            If pLstId > 0 Then
                If Not pDer Then
                    SQL = SQL & "AND (tur.FechaHoraTurno < "
                Else
                    SQL = SQL & "AND (tur.FechaHoraTurno > "
                End If
                SQL = SQL & "(SELECT FechaHoraTurno FROM Turnos WHERE (ID = " & pLstId & "))) "
            End If
            If pPra > 0 Then SQL = SQL & "AND (tur.PracticaId = " & pPra & ") "
            If pCen > 0 Then SQL = SQL & "AND (sal.CentroAtencionId = " & pCen & ") "
            If pSal > 0 Then SQL = SQL & "AND (tur.CentroAtencionSalaId = " & pSal & ") "
            If pMed > 0 Then SQL = SQL & "AND (tur.PersonalId = " & pMed & ") "
            If Not pDer Then
                SQL = SQL & "ORDER BY tur.FechaHoraTurno"
            Else
                SQL = SQL & "ORDER BY tur.FechaHoraTurno DESC"
            End If

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then Mover = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Mover", ex)
        End Try

    End Function
    Public Function GetByFechas(ByVal pDes As Date, ByVal pHas As Date) As DataTable

        GetByFechas = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT tur.ID, tur.Paciente, tur.FechaHoraTurno, cen.AbreviaturaId AS Centro, sal.AbreviaturaId AS Sala, "
            SQL = SQL & "pra.Descripcion AS Practica, per.Apellido AS Medico, etu.Descripcion AS Estado "
            SQL = SQL & "FROM Turnos tur "
            SQL = SQL & "LEFT JOIN EstadosTurnos etu ON tur.EstadoTurnoId = etu.ID "
            SQL = SQL & "LEFT JOIN CentrosAtencionSalas sal ON (tur.CentroAtencionSalaId = sal.ID) "
            SQL = SQL & "LEFT JOIN CentrosAtencion cen ON (sal.CentroAtencionId = cen.ID) "
            SQL = SQL & "LEFT JOIN Practicas pra ON (tur.PracticaId = pra.ID) "
            SQL = SQL & "LEFT JOIN Personal per ON (tur.PersonalId = per.ID) "
            SQL = SQL & "WHERE (tur.FechaHoraTurno BETWEEN '" & DateToSql(pDes, , True) & "' AND '" & DateToSql(pHas, , True, False) & "') "
            SQL = SQL & "ORDER BY tur.FechaHoraTurno"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByFechas = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByFechas", ex)
        End Try
    End Function

    Public Function GetForCombo(ByVal pFec As Date) As DataTable

        GetForCombo = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT tur.ID, cli.AbreviaturaId + ' - ' + tur.Paciente AS Descripcion "
            SQL = SQL & "FROM Turnos tur "
            SQL = SQL & "LEFT JOIN Clientes cli ON tur.ClienteId = cli.ID "
            SQL = SQL & "WHERE (tur.FechaHoraTurno BETWEEN '" & DateToSql(pFec, , True) & "' AND '" & DateToSql(pFec, , True, False) & "') "
            SQL = SQL & "ORDER BY Descripcion"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetForCombo = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetForCombo", ex)
        End Try
    End Function

    Public Function GetProximosByPaciente(pIte As Int64) As DataTable

        GetProximosByPaciente = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT tur.ID, CONVERT(DATE, tur.FechaHoraTurno, 101) AS Fecha, SUBSTRING(CAST(CONVERT(time, FechaHoraTurno) AS VARCHAR), 1, 5) AS Hora, cen.AbreviaturaId AS Centro, sal.AbreviaturaId AS Sala, pra.Descripcion AS Practica, "
            SQL = SQL & "med.Apellido + ' ' + med.Nombre AS Medico, etu.Descripcion AS Estado "
            SQL = SQL & "FROM Turnos tur "
            SQL = SQL & "INNER JOIN EstadosTurnos etu ON tur.EstadoTurnoId = etu.ID "
            SQL = SQL & "INNER JOIN CentrosAtencionSalas sal ON (tur.CentroAtencionSalaId = sal.ID) "
            SQL = SQL & "INNER JOIN CentrosAtencion cen ON (sal.CentroAtencionId = cen.ID) "
            SQL = SQL & "INNER JOIN Practicas pra ON (tur.PracticaId = pra.ID) "
            SQL = SQL & "INNER JOIN Personal med ON (tur.PersonalId = med.ID) "
            SQL = SQL & "WHERE (ClienteIntegranteId = " & pIte & ") AND (FechaHoraTurno >= '" & DateTimeToSql(Now.Date) & "') "
            SQL = SQL & " ORDER BY tur.FechaHoraTurno"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdGrl.ExecuteReader)

            GetProximosByPaciente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetProximosByPaciente", ex)
        End Try
    End Function

End Class
