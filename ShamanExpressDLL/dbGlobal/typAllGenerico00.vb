Imports System.Data
Imports System.Data.SqlClient
Public MustInherit Class typAllGenerico00
    Inherits typAll
    Private clDescripcion As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
        End Set
    End Property

End Class
