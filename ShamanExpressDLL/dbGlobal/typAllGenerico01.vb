Imports System.Data
Imports System.Data.SqlClient
Public MustInherit Class typAllGenerico01
    Inherits typAllGenerico00
    Private clAbreviaturaId As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property AbreviaturaId() As String
        Get
            Return clAbreviaturaId
        End Get
        Set(ByVal value As String)
            clAbreviaturaId = value
        End Set
    End Property

End Class
