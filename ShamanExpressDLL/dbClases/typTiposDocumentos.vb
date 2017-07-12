Public Class typTiposDocumentos
    Inherits typAllGenerico01
    Private clflgDefault As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property flgDefault() As Integer
        Get
            Return clflgDefault
        End Get
        Set(ByVal value As Integer)
            clflgDefault = value
        End Set
    End Property

End Class
