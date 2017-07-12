Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesAlicuotas
    Inherits typClientesAlicuotas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIdByIndex(ByVal pCli As Int64, ByVal pAli As Int64) As Int64
        GetIdByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM ClientesAlicuotas "
            SQL = SQL & "WHERE ClienteId = " & pCli & " AND AlicuotaImpuestoId = " & pAli

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIdByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIdByIndex", ex)
        End Try
    End Function

    Public Function GetAlicuotaIdByImpuesto(ByVal pCli As Int64, ByVal pIto As itoImpuestos) As Int64
        GetAlicuotaIdByImpuesto = 0
        Try
            Dim SQL As String

            SQL = "SELECT AlicuotaImpuestoId FROM ClientesAlicuotas cli "
            SQL = SQL & "INNER JOIN AlicuotasIva ali ON cli.AlicuotaImpuestoId = ali.ID "
            SQL = SQL & "WHERE cli.ClienteId = " & pCli & " AND ali.ImpuestoId = " & pIto

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetAlicuotaIdByImpuesto = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAlicuotaIdByImpuesto", ex)
        End Try
    End Function
    Public Function GetByCliente(ByVal pCli As Int64) As DataTable

        GetByCliente = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT 0 AS ID, ImpuestoId, Descripcion, 0 AS AlicuotaImpuestoId "
            SQL = SQL & "FROM AlicuotasIva "
            SQL = SQL & "WHERE (ImpuestoId <> 1)"
            SQL = SQL & "ORDER BY "

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetByCliente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByCliente", ex)
        End Try
    End Function
    Public Function EliminarByCliente(ByVal pId As Int64) As Boolean
        Dim SQL As String

        EliminarByCliente = False
        Try
            Dim cmOpe As New SqlCommand

            SQL = "DELETE FROM ClientesContactos WHERE ClienteId = " & pId
            cmOpe.Connection = cnnsNET(Me.myCnnName)
            cmOpe.Transaction = cnnsTransNET(Me.myCnnName)
            cmOpe.CommandText = SQL
            cmOpe.ExecuteNonQuery()

            EliminarByCliente = True
        Catch ex As Exception
            HandleError(Me.GetType.Name, "EliminarByCliente", ex)
        End Try
    End Function

End Class
