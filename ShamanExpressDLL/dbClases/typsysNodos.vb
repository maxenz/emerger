Public Class typsysNodos
    Inherits typAllGenerico00
    Private clProductoId As shamanProductos
    Private clJerarquia As String
    Private clClave As String
    Private clImagen As String
    Private clflgOperativo As Integer
    Private clflgPurge As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ProductoId() As shamanProductos
        Get
            Return clProductoId
        End Get
        Set(ByVal value As shamanProductos)
            clProductoId = value
        End Set
    End Property
    Public Property Jerarquia() As String
        Get
            Return clJerarquia
        End Get
        Set(ByVal value As String)
            clJerarquia = value
        End Set
    End Property
    Public Property Clave() As String
        Get
            Return clClave
        End Get
        Set(ByVal value As String)
            clClave = value
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
    Public Property flgOperativo() As Integer
        Get
            Return clflgOperativo
        End Get
        Set(ByVal value As Integer)
            clflgOperativo = value
        End Set
    End Property
    Public Property flgPurge() As Integer
        Get
            Return clflgPurge
        End Get
        Set(ByVal value As Integer)
            clflgPurge = value
        End Set
    End Property
End Class
