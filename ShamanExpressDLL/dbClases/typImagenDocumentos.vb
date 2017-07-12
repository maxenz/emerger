Public Class typImagenDocumentos
    Inherits typAll
    Private clClasificacionId As imgDiseño
    Private clAbreviaturaId As String
    Private clDescripcion As String
    Private clCantidadDocumentos As Integer
    Private clCantidadCopias As Integer
    Private clCantidadRenglones As Integer
    Private clflgImporteLetras As Integer
    Private clflgIntercalar As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ClasificacionId() As imgDiseño
        Get
            Return clClasificacionId
        End Get
        Set(ByVal value As imgDiseño)
            clClasificacionId = value
        End Set
    End Property
    Public Property AbreviaturaId() As String
        Get
            Return clAbreviaturaId
        End Get
        Set(ByVal value As String)
            clAbreviaturaId = value
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
    Public Property CantidadDocumentos() As Integer
        Get
            Return clCantidadDocumentos
        End Get
        Set(ByVal value As Integer)
            clCantidadDocumentos = value
        End Set
    End Property
    Public Property CantidadCopias() As Integer
        Get
            Return clCantidadCopias
        End Get
        Set(ByVal value As Integer)
            clCantidadCopias = value
        End Set
    End Property
    Public Property CantidadRenglones() As Integer
        Get
            Return clCantidadRenglones
        End Get
        Set(ByVal value As Integer)
            clCantidadRenglones = value
        End Set
    End Property
    Public Property flgImporteLetras() As Integer
        Get
            Return clflgImporteLetras
        End Get
        Set(ByVal value As Integer)
            clflgImporteLetras = value
        End Set
    End Property
    Public Property flgIntercalar() As Integer
        Get
            Return clflgIntercalar
        End Get
        Set(ByVal value As Integer)
            clflgIntercalar = value
        End Set
    End Property

End Class
