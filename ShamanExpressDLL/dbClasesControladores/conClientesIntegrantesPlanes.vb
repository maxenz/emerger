Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesIntegrantesPlanes
    Inherits typClientesIntegrantesPlanes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pItg As Int64, ByVal pPla As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM ClientesIntegrantesPlanes WHERE ClienteIntegranteId = " & pItg & " AND PlanId = " & pPla

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Sub EliminarPlanes(ByVal pItg As Int64)
        Try
            Dim SQL As String = "DELETE FROM ClientesIntegrantesPlanes WHERE ClienteIntegranteId = " & pItg
            Dim cmdDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdDel.ExecuteNonQuery()
        Catch ex As Exception
            HandleError(Me.GetType.Name, "EliminarPlanes", ex)
        End Try
    End Sub

End Class
