Imports System.Data
Imports System.Data.SqlClient
Public Class conselIncidentes
    Inherits typselIncidentes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Sub DelByPID(ByVal pPID As Int64)
        Try
            Dim SQL As String = "DELETE FROM selIncidentes WHERE PID = " & pPID
            Dim cmdDel As New SqlCommand(SQL, cnnsNET(cnnDefault), cnnsTransNET(cnnDefault))
            cmdDel.ExecuteNonQuery()
        Catch ex As Exception
            HandleError(Me.GetType.Name, "DelByPID", ex)
        End Try
    End Sub
End Class
