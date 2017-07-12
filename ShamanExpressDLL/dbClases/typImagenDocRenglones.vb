Public Class typImagenDocRenglones
    Inherits typAll
    Private clImagenDocumentoId As typImagenDocumentos
    Private clDescripcion As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ImagenDocumentoId() As typImagenDocumentos
        Get
            Return Me.GetTypProperty(clImagenDocumentoId)
        End Get
        Set(ByVal value As typImagenDocumentos)
            clImagenDocumentoId = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
        End Set
    End Property
End Class
