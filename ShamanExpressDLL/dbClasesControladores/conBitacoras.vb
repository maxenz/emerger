Imports System.Data
Imports System.Data.SqlClient
Public Class conBitacoras
    Inherits typBitacoras
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
End Class
