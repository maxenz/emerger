Imports System.Data
Imports System.Data.SqlClient
Public Class conInsumos
    Inherits typInsumos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetQueryBase(Optional ByVal pAct As Integer = 1, Optional ByVal pCen As Int64 = 0) As DataTable

        GetQueryBase = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ins.ID, ins.AbreviaturaId, ins.Descripcion, tip.Descripcion AS Clasificacion "
            SQL = SQL & "FROM Insumos ins "
            SQL = SQL & "INNER JOIN TiposInsumos tip ON (ins.TipoInsumoId = tip.ID) "
            If pAct = 1 Then SQL = sqlWhere(SQL) & "(ins.Activo = 1)"
            If pAct = 2 Then SQL = sqlWhere(SQL) & "(ins.Activo = 0)"
            SQL = SQL & "ORDER BY ins.AbreviaturaId"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdEqp.ExecuteReader)

            dt.Columns.Add("StockActual", GetType(Integer))
            dt.Columns.Add("StockOptimo", GetType(Integer))
            dt.Columns.Add("StockFaltante", GetType(Integer))

            For vIdx = 0 To dt.Rows.Count - 1

                SQL = "SELECT ISNULL(SUM(StockActual), 0), ISNULL(SUM(StockOptimo), 0) FROM InsumosStock "
                SQL = SQL & "WHERE (InsumoId = " & dt(vIdx)(0) & ") "
                If pCen > 0 Then SQL = SQL & "AND (CentroAtencionId = " & pCen & ") "

                Dim cmdStock As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dtStock As New DataTable
                dtStock.Load(cmdStock.ExecuteReader)

                If dtStock.Rows.Count > 0 Then
                    dt(vIdx)("StockActual") = dtStock(0)(0)
                    dt(vIdx)("StockOptimo") = dtStock(0)(1)
                    dt(vIdx)("StockFaltante") = dtStock(0)(1) - dtStock(0)(0)
                Else
                    dt(vIdx)("StockActual") = 0
                    dt(vIdx)("StockOptimo") = 0
                    dt(vIdx)("StockFaltante") = 0
                End If

            Next

            GetQueryBase = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryBase", ex)
        End Try
    End Function

    Public Function GetIDByAbreviaturaId(ByVal pVal As String) As Int64

        GetIDByAbreviaturaId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM Insumos WHERE (AbreviaturaId = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAbreviaturaId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbreviaturaId", ex)
        End Try

    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código del insumo/medicamento"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del insumo/medicamento"
            If Me.TipoInsumoId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe determinar la clasificación"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class