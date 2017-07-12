Public Class typPoligonos
    Inherits typAll
    Private clDescripcion As String
    Private clAbreviaturaId As String
    Private clClasificacionPoligono As Integer
    Private clActivo As Integer
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
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
        End Set
    End Property
    Public Property ClasificacionPoligono() As Integer
        Get
            Return clClasificacionPoligono
        End Get
        Set(ByVal value As Integer)
            clClasificacionPoligono = value
        End Set
    End Property
    Public Property Activo() As Integer
        Get
            Return clActivo
        End Get
        Set(ByVal value As Integer)
            clActivo = value
        End Set
    End Property

End Class
