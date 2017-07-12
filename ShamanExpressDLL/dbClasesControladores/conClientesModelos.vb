Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesModelos
    Inherits typClientesModelos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByCliente(ByVal pCli As Int64, Optional pMod As Integer = 0) As DataTable

        GetByCliente = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ren.ID, ren.Cantidad, ren.Descripcion, ISNULL(ali.Porcentaje, 0) AS Alicuota, ISNULL(ibb.Porcentaje, 0) AS AlicuotaIIBB, ren.Importe, "
            SQL = SQL & "ROUND(ren.Cantidad * ren.Importe, 2) AS SubTotal, "
            SQL = SQL & "ROUND(ren.Cantidad * ren.Importe, 2) + ((ROUND(ren.Cantidad * ren.Importe, 2) * ISNULL(ali.Porcentaje, 0)) / 100) AS Total "
            SQL = SQL & "FROM ClientesModelos ren "
            SQL = SQL & "LEFT JOIN AlicuotasIva ali ON ren.AlicuotaIvaId = ali.ID "
            SQL = SQL & "LEFT JOIN AlicuotasIva ibb ON ren.AlicuotaBsAsIIBBId = ibb.ID "
            SQL = SQL & "WHERE (ren.ClienteId = " & pCli & ") AND (ren.ModeloId = " & pMod & ") ORDER BY ren.RenglonId "

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetByCliente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByCliente", ex)
        End Try
    End Function

    Public Function GetImporteCapitas(ByVal pCli As Int64, Optional pMod As Integer = 0) As Double
        GetImporteCapitas = 0
        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(ROUND(SUM(Cantidad * Importe), 2), 0) "
            SQL = SQL & "FROM ClientesModelos "
            SQL = SQL & "WHERE (ClienteId = " & pCli & ") AND (ModeloId = " & pMod & ") "

            Dim cmdSum As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdSum.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetImporteCapitas = CType(vOutVal, Double)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetImporteCapitas", ex)
        End Try
    End Function

    Public Function GetNextRenglon(ByVal pCli As Int64, Optional ByVal pMod As Integer = 0) As Int64
        GetNextRenglon = 0
        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(MAX(RenglonId), 0) + 1 FROM ClientesModelos WHERE ClienteId = " & pCli & " AND ModeloId = " & pMod

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetNextRenglon = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNextRenglon", ex)
        End Try
    End Function

    Public Sub ReorderFrom(pCli As Int64, pRen As Integer, Optional pMod As Integer = 0)
        Try
            Dim SQL As String
            SQL = "UPDATE ClientesModelos SET RenglonId = RenglonId - 1 "
            SQL = SQL & "WHERE (ClienteId = " & pCli & ") AND (ModeloId = " & pMod & ") "
            SQL = SQL & "AND (RenglonID < " & pRen & ")"
            Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdUpd.ExecuteNonQuery()
        Catch ex As Exception
            HandleError(Me.GetType.Name, "RefreshGrid", ex)
        End Try
    End Sub

    Public Sub EliminarByCliente(pCli As Int64)
        Try
            Dim SQL As String
            SQL = "DELETE FROM ClientesModelos WHERE (ClienteId = " & pCli & ") "
            Dim cmdDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdDel.ExecuteNonQuery()
        Catch ex As Exception
            HandleError(Me.GetType.Name, "EliminarByCliente", ex)
        End Try
    End Sub

End Class
