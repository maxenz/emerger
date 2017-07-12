Public Class typGoogleMapsImagenes
    Inherits typAll
    Private clGradoOperativoId As typGradosOperativos
    Private clImagen As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property GradoOperativoId() As typGradosOperativos
        Get
            Return Me.GetTypProperty(clGradoOperativoId)
        End Get
        Set(ByVal value As typGradosOperativos)
            clGradoOperativoId = value
        End Set
    End Property
    Public Property Imagen() As String
        Get
            Return clImagen
        End Get
        Set(ByVal value As String)
            clImagen = value
        End Set
    End Property
End Class
