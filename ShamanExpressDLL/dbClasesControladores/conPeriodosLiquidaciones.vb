Imports System.Data
Imports System.Data.SqlClient
Public Class conPeriodosLiquidaciones
    Inherits typPeriodosLiquidaciones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll(Optional pTop As Integer = 0) As DataTable
        GetAll = Nothing
        Try
            Dim SQL As String

            If pTop = 0 Then
                SQL = "SELECT "
            Else
                SQL = "SELECT TOP " & pTop & " "
            End If

            SQL = SQL & "ID, SUBSTRING(CAST(Periodo AS varchar(6)), 5, 2) + '/' + SUBSTRING(CAST(Periodo AS varchar(6)), 1, 4) AS PeriodoStr, FecDesde, FecHasta, flgCerrado "
            SQL = SQL & "FROM PeriodosLiquidaciones "
            SQL = SQL & "ORDER BY Periodo DESC"

            Dim cmdPer As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdPer.ExecuteReader)
            dt.Columns.Add("Cerrado", GetType(Boolean))

            For vIdx = 0 To dt.Rows.Count - 1
                dt(vIdx)("Cerrado") = setIntToBool(dt(vIdx)("flgCerrado"))
            Next vIdx

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            If Me.Periodo = 0 Then vRdo = "El período asignado es incorrecto"
            If Me.FecDesde = NullDateMin And vRdo = "" Then vRdo = "La fecha desde es incorrecta"
            If Me.FecHasta = NullDateMin And vRdo = "" Then vRdo = "La fecha hasta es incorrecta"
            If Me.FecDesde > Me.FecHasta And vRdo = "" Then vRdo = "La fecha desde debe ser inferior a la fecha hasta"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            Else
                Validar = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function


End Class
