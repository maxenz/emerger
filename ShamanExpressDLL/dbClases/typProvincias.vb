Public Class typProvincias
    Inherits typAllGenerico01
    Private clPaisId As typPaises
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PaisId() As typPaises
        Get
            Return Me.GetTypProperty(clPaisId)
        End Get
        Set(ByVal value As typPaises)
            clPaisId = value
        End Set
    End Property

End Class
