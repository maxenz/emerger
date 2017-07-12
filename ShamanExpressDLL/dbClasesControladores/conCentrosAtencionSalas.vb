Imports System.Data
Imports System.Data.SqlClient
Public Class conCentrosAtencionSalas
    Inherits typCentrosAtencionSalas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByCentro(ByVal pCen As Int64) As DataTable

        GetByCentro = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, CASE ClasificacionId WHEN 0 THEN 'Turnos / Atención' ELSE 'Internación' END AS Clasificacion, "
            SQL = SQL & "cntCamas, Observaciones, CASE flgFeriados WHEN 1 THEN 'Si' ELSE 'No' END AS Feriados "
            SQL = SQL & "FROM CentrosAtencionSalas "
            SQL = SQL & "WHERE (CentroAtencionId = " & pCen & ") "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByCentro = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByCentro", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador de la sala"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
    Public Sub SetHorarios(ByVal pCenSal As Int64, ByVal dtHorarios As DataTable)
        Try
            Dim vIdx As Integer, objHorario As typCentrosAtencionHorarios

            Dim SQL As String = "DELETE FROM CentrosAtencionHorarios WHERE CentroAtencionSalaId = " & pCenSal
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            For vIdx = 0 To dtHorarios.Rows.Count - 1

                If dtHorarios(vIdx)("HorInicio1") <> "" Then
                    objHorario = New typCentrosAtencionHorarios(Me.myCnnName)
                    objHorario.CleanProperties(objHorario)
                    objHorario.CentroAtencionSalaId.SetObjectId(pCenSal)
                    objHorario.DiaSemana = vIdx + 1
                    objHorario.AperturaId = getNextIngresoId(pCenSal, vIdx + 1)
                    objHorario.HorInicio = setTime(dtHorarios(vIdx)("HorInicio1"))
                    objHorario.HorFinal = setTime(dtHorarios(vIdx)("HorFinal1"))
                    objHorario.Salvar(objHorario)
                    objHorario = Nothing
                End If
                If dtHorarios(vIdx)("HorInicio2") <> "" Then
                    objHorario = New typCentrosAtencionHorarios(Me.myCnnName)
                    objHorario.CleanProperties(objHorario)
                    objHorario.CentroAtencionSalaId.SetObjectId(pCenSal)
                    objHorario.DiaSemana = vIdx + 1
                    objHorario.AperturaId = getNextIngresoId(pCenSal, vIdx + 1)
                    objHorario.HorInicio = setTime(dtHorarios(vIdx)("HorInicio2"))
                    objHorario.HorFinal = setTime(dtHorarios(vIdx)("HorFinal2"))
                    objHorario.Salvar(objHorario)
                    objHorario = Nothing
                End If
                If dtHorarios(vIdx)("HorInicio3") <> "" Then
                    objHorario = New typCentrosAtencionHorarios(Me.myCnnName)
                    objHorario.CleanProperties(objHorario)
                    objHorario.CentroAtencionSalaId.SetObjectId(pCenSal)
                    objHorario.DiaSemana = vIdx + 1
                    objHorario.AperturaId = getNextIngresoId(pCenSal, vIdx + 1)
                    objHorario.HorInicio = setTime(dtHorarios(vIdx)("HorInicio3"))
                    objHorario.HorFinal = setTime(dtHorarios(vIdx)("HorFinal3"))
                    objHorario.Salvar(objHorario)
                    objHorario = Nothing
                End If
            Next
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetHorarios", ex)
        End Try
    End Sub

    Private Function getNextIngresoId(ByVal pCenSal As Decimal, ByVal pDia As Integer) As Integer
        getNextIngresoId = 1
        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(MAX(AperturaId), 0) + 1 FROM CentrosAtencionHorarios "
            SQL = SQL & "WHERE (CentroAtencionSalaId = " & pCenSal & ") AND (DiaSemana = " & pDia & ") "

            Dim cmBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmBas.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getNextIngresoId = CType(vOutVal, Integer)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getNextIngresoId", ex)
        End Try
    End Function

    Public Sub GetHorarios(ByVal pCenSal As Int64, ByRef dtHorarios As DataTable)
        Try
            Dim SQL As String

            SQL = "SELECT DiaSemana, HorInicio, HorFinal "
            SQL = SQL & "FROM CentrosAtencionHorarios pac "
            SQL = SQL & "WHERE CentroAtencionSalaId = " & pCenSal
            SQL = SQL & " ORDER BY DiaSemana, HorInicio, HorFinal, AperturaId"


            Dim cmdPac As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdPac.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                Dim dtMatch() As DataRow = dtHorarios.Select("DiaSemana='" & getDayWeek(dt(vIdx)(0) - 1) & "'")

                If dtMatch.Length > 0 Then

                    Dim dtRow As DataRow = dtMatch(0)

                    If dtRow("HorInicio1") = "" Then
                        dtRow("HorInicio1") = dt(vIdx)("HorInicio").ToString.Substring(0, 5)
                        dtRow("HorFinal1") = dt(vIdx)("HorFinal").ToString.Substring(0, 5)
                    ElseIf dtRow("HorInicio2") = "" Then
                        dtRow("HorInicio2") = dt(vIdx)("HorInicio").ToString.Substring(0, 5)
                        dtRow("HorFinal2") = dt(vIdx)("HorFinal").ToString.Substring(0, 5)
                    Else
                        dtRow("HorInicio3") = dt(vIdx)("HorInicio").ToString.Substring(0, 5)
                        dtRow("HorFinal3") = dt(vIdx)("HorFinal").ToString.Substring(0, 5)
                    End If

                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetPactados", ex)
        End Try
    End Sub

    Public Function CheckHorario(ByVal pCenSal As Int64, ByVal pDes As DateTime, ByVal pHas As DateTime, Optional ByVal pPer As Int64 = 0) As String
        CheckHorario = ""
        Try
            '-----------> Verifico si el horario extiende (DESDE)
            Dim SQL As String

            SQL = "SELECT ID FROM CentrosAtencionHorarios "
            SQL = SQL & "WHERE CentroAtencionSalaId = " & pCenSal
            SQL = SQL & "AND (DiaSemana = " & getNroDayWeek(pDes.Date) & ") "
            SQL = SQL & "AND ('" & pDes.TimeOfDay.ToString & "' BETWEEN HorInicio AND HorFinal) "

            Dim cmdQry As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmdQry.ExecuteScalar Is Nothing Then
                Return "La sala de atención no está disponible el día " & getDayWeek(getNroDayWeek(pDes.Date) - 1) & " en dicho horario"
            End If

            '-----------> Verifico si el horario extiende (HASTA)
            SQL = "SELECT ID FROM CentrosAtencionHorarios "
            SQL = SQL & "WHERE CentroAtencionSalaId = " & pCenSal
            SQL = SQL & "AND (DiaSemana = " & getNroDayWeek(pDes.Date) & ") "
            SQL = SQL & "AND ('" & pHas.TimeOfDay.ToString & "' BETWEEN HorInicio AND HorFinal) "

            cmdQry.CommandText = SQL
            If cmdQry.ExecuteScalar Is Nothing Then
                Return "La sala de atención no está disponible el día " & getDayWeek(getNroDayWeek(pDes.Date) - 1) & " en dicho horario"
            End If

            '-----------> Verifico ocupación
            If pPer > 0 Then
                SQL = "SELECT per.Apellido + ' ' + per.Nombre FROM PersonalHorariosAtencion sal "
                SQL = SQL & "INNER JOIN Personal per ON (sal.PersonalId = per.ID) "
                SQL = SQL & "WHERE (sal.CentroAtencionSalaId = " & pCenSal & ") "
                SQL = SQL & "AND (sal.DiaSemana = " & getNroDayWeek(pDes.Date) & ") "
                SQL = SQL & "AND ('" & pHas.TimeOfDay.ToString & "' BETWEEN sal.HorEntrada AND sal.HorSalida) "
                SQL = SQL & "AND (sal.PersonalId <> " & pPer & ") "
                SQL = SQL & "AND (sal.PersonalId NOT IN(SELECT PersonalId FROM TurnosExcepciones WHERE (TipoExcepcion = 0) "
                SQL = SQL & "AND (CentroAtencionSalaId = " & pCenSal & ") AND (FechaHoraDesde BETWEEN '" & DateTimeToSql(pDes) & "' AND '" & DateTimeToSql(pHas) & "'))) "

                cmdQry.CommandText = SQL
                Dim vPerOcu As String = CType(cmdQry.ExecuteScalar, String)

                If Not vPerOcu Is Nothing Then
                    Return "La sala de atención está ocupado por " & vPerOcu & " en dicho día/horario"
                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CheckHorario", ex)
        End Try
    End Function
    Private Function setTime(ByVal pVal As String, Optional ByVal pMax As Boolean = False) As String
        If pVal <> "" Then
            If Not pMax Then
                setTime = pVal & ":00"
            Else
                setTime = pVal & ":59"
            End If
        Else
            setTime = ""
        End If
    End Function
End Class
