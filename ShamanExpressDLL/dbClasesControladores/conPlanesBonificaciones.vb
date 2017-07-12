Imports System.Data
Imports System.Data.SqlClient
Public Class conPlanesBonificaciones
    Inherits typPlanesBonificaciones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pPla As Int64, ByVal pMes As Int64, Optional pRec As Integer = 0) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM PlanesBonificaciones WHERE PlanId = " & pPla & " AND Mes = " & pMes
            SQL = SQL & " AND flgRecargo = " & pRec

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

End Class
