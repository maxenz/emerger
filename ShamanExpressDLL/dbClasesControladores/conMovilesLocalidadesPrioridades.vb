Imports System.Data
Imports System.Data.SqlClient
Public Class conMovilesLocalidadesPrioridades
    Inherits typMovilesLocalidadesPrioridades
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIdByIndex(ByVal pMovLoc As Int64, ByVal pGdo As Int64) As Int64
        GetIdByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM MovilesLocalidadesPrioridades WHERE (MovilLocalidadId = " & pMovLoc & ") AND (GradoOperativoId = " & pGdo & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIdByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIdByIndex", ex)
        End Try
    End Function
End Class
