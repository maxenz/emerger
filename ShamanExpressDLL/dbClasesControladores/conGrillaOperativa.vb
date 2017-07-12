Imports System.Data
Imports System.Data.SqlClient
Public Class conGrillaOperativa
    Inherits typGrillaOperativa
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetTripulante(ByVal pMov As Int64, Optional ByVal pPue As String = "") As Int64

        GetTripulante = 0

        Try

            Dim dt As DataTable = Me.GetCurrentDotacion(pMov, pPue)

            If dt.Rows.Count > 0 Then
                GetTripulante = Val(dt.Rows(0)(1))
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetTripulante", ex)
        End Try

    End Function

    Public Function GetCurrentDotacion(ByVal pMov As Int64, Optional ByVal pPue As String = "", Optional pBajOpe As Boolean = False) As DataTable

        GetCurrentDotacion = Nothing

        Try

            '---------> Reviso si la grilla esta cerrada
            If Not Me.IsGrillaClose(Now.Date) Then
                CreateGrillaView(Now.Date, pMov)
            End If

            Dim SQL As String

            SQL = "SELECT gdt.ID, per.ID AS PersonalId, gdt.PuestoGrilla, per.Legajo, per.Apellido, per.Nombre, "
            SQL = SQL & "CASE gdt.FicEntrada WHEN '" & DateToSql(NullDateTime) & "' THEN gdt.pacEntrada ELSE gdt.FicEntrada END AS FicEntrada, "
            SQL = SQL & "CASE gdt.FicEntrada WHEN '" & DateToSql(NullDateTime) & "' THEN 0 ELSE 1 END AS flgEntrada, "
            SQL = SQL & "CASE gdt.FicSalida WHEN '" & DateToSql(NullDateTime) & "' THEN CASE gdt.PacSalida WHEN '" & DateToSql(NullDateTime) & "' THEN NULL ELSE gdt.PacSalida END ELSE gdt.FicSalida END AS PacSalida, "
            SQL = SQL & "CASE gdt.FicSalida WHEN '" & DateToSql(NullDateTime) & "' THEN 0 ELSE 1 END AS flgSalida, "
            SQL = SQL & "per.Apellido + ' ' + per.Nombre AS Personal "
            SQL = SQL & "FROM GrillaOperativaTripulantes gdt "
            SQL = SQL & "INNER JOIN GrillaOperativa grl ON (gdt.GrillaOperativaId = grl.ID) "
            SQL = SQL & "INNER JOIN Personal per ON (gdt.PersonalId = per.ID) "
            SQL = SQL & "WHERE (gdt.MovilId = " & pMov & ") AND (grl.FecGrilla = '" & DateToSql(Now.Date) & "') "
            SQL = SQL & "AND ('" & ShowDateTime(Now, 2) & "' BETWEEN "
            SQL = SQL & "CASE gdt.FicEntrada WHEN '" & DateToSql(NullDateTime) & "' THEN gdt.pacEntrada ELSE gdt.FicEntrada END AND "
            If Not pBajOpe Then
                SQL = SQL & "CASE gdt.FicSalida WHEN '" & DateToSql(NullDateTime) & "' THEN CASE gdt.PacSalida WHEN '" & DateToSql(NullDateTime) & "' THEN CURRENT_TIMESTAMP ELSE gdt.PacSalida END ELSE gdt.FicSalida END) "
            Else
                SQL = SQL & "CASE gdt.FicSalida WHEN '" & DateToSql(NullDateTime) & "' THEN CASE gdt.PacSalida WHEN '" & DateToSql(NullDateTime) & "' THEN DATEADD(hh, 2, CURRENT_TIMESTAMP) ELSE DATEADD(hh, 2, gdt.PacSalida) END ELSE DATEADD(hh, 2, gdt.FicSalida) END) "
            End If

            SQL = SQL & "AND (gdt.flgDelete = 0) "
            If pPue <> "" Then SQL = SQL & "AND (gdt.PuestoGrilla = '" & pPue & "') "
            SQL = SQL & "ORDER BY gdt.FicEntrada, gdt.FicSalida"

            Dim cmdDot As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            dt.Load(cmdDot.ExecuteReader)

            GetCurrentDotacion = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetCurrentDotacion", ex)
        End Try

    End Function

    Public Sub CreateGrillaView(ByVal pFec As Date, Optional ByVal pMov As Int64 = 0)
        Try
            Dim SQL As String
            Dim objGrl As New conGrillaOperativa(Me.myCnnName)
            Dim objRow As New conGrillaOperativaTripulantes(Me.myCnnName)
            Dim objJob As New conlckJobs(Me.myCnnName)

            '--------> Purgo
            If objGrl.Abrir(objGrl.GetIDByIndex(pFec, True)) Then

                If objJob.lockRun("GRILLA", objGrl.ID) Then

                    Me.SetPurge(objGrl.ID)

                    SQL = "SELECT pac.MovilId, mov.BaseOperativaId, mov.VehiculoId, pue.PuestoGrilla, pac.PersonalId, pac.HorEntrada, pac.HorSalida, pac.IngresoId "
                    SQL = SQL & "FROM PersonalPactados pac "
                    SQL = SQL & "INNER JOIN Moviles mov ON (pac.MovilId = mov.ID) "
                    SQL = SQL & "INNER JOIN Personal per ON (pac.PersonalId = per.ID) "
                    SQL = SQL & "LEFT JOIN DepartamentosPuestos pue ON (per.DepartamentoPuestoId = pue.ID) "
                    SQL = SQL & "WHERE (pac.DiaSemana = " & getNroDayWeek(pFec) & ") "
                    SQL = SQL & "AND (pue.PuestoGrilla IS NOT NULL) "
                    SQL = SQL & "AND (per.FecEgreso >= '" & DateToSql(pFec) & "') "
                    If pMov > 0 Then SQL = SQL & "AND (pac.MovilId = " & pMov & ") "
                    SQL = SQL & "ORDER BY mov.Movil, pac.HorEntrada, pac.HorSalida"

                    Dim cmdTmp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    Dim dt As New DataTable
                    Dim vIdx As Integer = 0
                    dt.Load(cmdTmp.ExecuteReader)

                    For vIdx = 0 To dt.Rows.Count - 1

                        Dim rsTmp As DataRow = dt.Rows(vIdx)
                        Dim vAdd As Boolean = True

                        '-------> Reviso si está en otro móvil...
                        SQL = "SELECT ID FROM GrillaOperativaTripulantes WHERE GrillaOperativaId = " & objGrl.ID & " AND PersonalId = " & rsTmp(4)
                        SQL = SQL & " AND IngresoId = " & rsTmp(7) & " AND MovilId <> " & rsTmp(0) & " AND flgDelete = 0"

                        Dim cmdPac As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                        If CType(cmdPac.ExecuteScalar, Int64) > 0 Then vAdd = False

                        If vAdd Then

                            '-----> Reviso si ya está en la grilla
                            If Not objRow.Abrir(objRow.GetIDByIndex(objGrl.ID, rsTmp(0), rsTmp(4), rsTmp(7))) Then

                                objRow.CleanProperties(objRow)
                                objRow.GrillaOperativaId.SetObjectId(objGrl.ID)
                                objRow.MovilId.SetObjectId(rsTmp(0))
                                objRow.PersonalId.SetObjectId(rsTmp(4))
                                objRow.IngresoId = rsTmp(7)

                            End If

                            '-----> Actualizo
                            If vAdd Then
                                If objRow.flgEdited = 0 Then
                                    objRow.BaseOperativaId.SetObjectId(rsTmp(1))
                                    objRow.VehiculoId.SetObjectId(rsTmp(2))
                                    objRow.PuestoGrilla = rsTmp(3)
                                    objRow.PacEntrada = SetToDateTime(pFec, rsTmp(5))
                                    If GetMinutes(rsTmp(5)) < GetMinutes(rsTmp(6)) Then
                                        objRow.PacSalida = setToDateTimeSalida(objRow.PacEntrada, pFec, rsTmp(6))
                                    Else
                                        objRow.PacSalida = setToDateTimeSalida(objRow.PacEntrada.AddDays(1), pFec, rsTmp(6))
                                    End If
                                    objRow.flgPurge = 0
                                    objRow.flgHorLegajo = 1
                                    objRow.Salvar(objRow)
                                End If
                            End If

                        Else

                            If objRow.Abrir(objRow.GetIDByIndex(objGrl.ID, rsTmp(0), rsTmp(4), rsTmp(7))) Then
                                objRow.flgPurge = 1
                                objRow.Salvar(objRow)
                            End If

                        End If

                    Next vIdx

                    Me.DelPurge(objGrl.ID)

                    objJob.unlockRun("GRILLA", objGrl.ID)

                End If

            End If
            objGrl = Nothing
            objRow = Nothing
        Catch ex As Exception
            HandleError(Me.GetType.Name, "createGrillaView", ex)
        End Try
    End Sub

    Private Function setToDateTimeSalida(ByVal pEnt As DateTime, ByVal pFec As Date, ByVal pHor As String)
        Dim vSal As DateTime = SetToDateTime(pFec, pHor)
        If pEnt > vSal Then
            vSal = vSal.AddDays(1)
        End If
        setToDateTimeSalida = vSal
    End Function

    Public Function IsGrillaClose(ByVal pFec As Date) As Boolean
        IsGrillaClose = False
        Try
            Dim objGrilla As New conGrillaOperativa(Me.myCnnName)
            If objGrilla.Abrir(objGrilla.GetIDByIndex(pFec)) Then
                If objGrilla.flgStatus = 1 Then IsGrillaClose = True
            End If
            objGrilla = Nothing
        Catch ex As Exception
            HandleError(Me.GetType.Name, "IsGrillaClose", ex)
        End Try
    End Function

    Public Function GetIDByIndex(ByVal pFec As Date, Optional ByVal pCreate As Boolean = False) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM GrillaOperativa WHERE FecGrilla = '" & DateToSql(pFec) & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            If Not vOutVal Is Nothing Then
                GetIDByIndex = CType(vOutVal, Int64)
            Else
                If pCreate Then
                    Dim objGrilla As New typGrillaOperativa
                    objGrilla.CleanProperties(objGrilla)
                    objGrilla.FecGrilla = pFec
                    objGrilla.flgStatus = 0
                    If objGrilla.Salvar(objGrilla) Then
                        GetIDByIndex = objGrilla.ID
                    End If
                    objGrilla = Nothing
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Sub SetPurge(ByVal pGrl As Int64)
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM GrillaOperativaTripulantes WHERE (GrillaOperativaId = " & pGrl & ") AND (FicEntrada = '" & SQLNullDate & "') AND (FicSalida = '" & SQLNullDate & "') AND (flgPactado = 0) AND (flgDelete = 0) AND (flgEdited = 0) "

            Dim cmdPur As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            dt.Load(cmdPur.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                Dim objTrip As New typGrillaOperativaTripulantes

                If objTrip.Abrir(dt(vIdx).Item(0)) Then
                    objTrip.flgPurge = 1
                    objTrip.Salvar(objTrip)
                End If

                objTrip = Nothing

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPurge", ex)
        End Try
    End Sub

    Public Sub DelPurge(ByVal pGrl As Long)
        Try
            Dim SQL As String
            Dim objTrip As New typGrillaOperativaTripulantes

            SQL = "SELECT ID FROM GrillaOperativaTripulantes WHERE (GrillaOperativaId = " & pGrl & ") AND (flgPurge = 1) "
            SQL = SQL & "AND (FicEntrada = '" & SQLNullDate & "') AND (FicSalida = '" & SQLNullDate & "') AND (flgPactado = 0) "

            Dim cmdDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            dt.Load(cmdDel.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                objTrip.Eliminar(dt(vIdx).Item(0))

            Next vIdx

            objTrip = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "DelPurge", ex)
        End Try
    End Sub

End Class
