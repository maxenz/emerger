Imports System.Data
Imports System.Data.SqlClient
Public Class conInsumosStock
    Inherits typInsumosStock
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pIns As Int64, ByVal pCen As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM InsumosStock WHERE (InsumoId = " & pIns & ") AND (CentroAtencionId = " & pCen & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Decimal)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetAll(Optional ByVal pIns As Int64 = 0) As DataTable

        GetAll = Nothing

        Try

            Dim objStock As New conInsumosStock
            Dim SQL As String

            SQL = "SELECT 0 AS ID, ID AS CentroAtencionId, Descripcion AS CentroAtencion, 0.00 AS StockActual, 0.00 AS StockOptimo, "
            SQL = SQL & "0.00 AS StockMinimo, 0.00 AS StockMaximo "
            SQL = SQL & "FROM CentrosAtencion "
            SQL = SQL & "ORDER BY Descripcion"

            Dim cmdCen As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            dt.Load(cmdCen.ExecuteReader)

            dt.Columns("ID").ReadOnly = False
            dt.Columns("StockActual").ReadOnly = False
            dt.Columns("StockOptimo").ReadOnly = False
            dt.Columns("StockMinimo").ReadOnly = False
            dt.Columns("StockMaximo").ReadOnly = False

            For vIdx = 0 To dt.Rows.Count - 1
                If objStock.Abrir(Me.GetIDByIndex(pIns, dt(vIdx)(1))) Then
                    dt(vIdx)("ID") = objStock.ID
                    dt(vIdx)("StockActual") = objStock.StockActual
                    dt(vIdx)("StockOptimo") = objStock.StockOptimo
                    dt(vIdx)("StockMinimo") = objStock.StockMinimo
                    dt(vIdx)("StockMaximo") = objStock.StockMaximo
                End If
            Next vIdx

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

End Class
