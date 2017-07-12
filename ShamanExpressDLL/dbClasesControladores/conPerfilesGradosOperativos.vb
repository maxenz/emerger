Imports System.Data
Imports System.Data.SqlClient
Public Class conPerfilesGradosOperativos
    Inherits typPerfilesGradosOperativos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
End Class
