Public Class typAdjuntosClasificaciones
    '----> Comentario
    Inherits typAll
    Private clDescripcion As String
    Private clRutaRemota As String
    Private clExtensiones As String
    Private clImagen As String
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
    Public Property RutaRemota() As String
        Get
            Return clRutaRemota
        End Get
        Set(ByVal value As String)
            clRutaRemota = value
        End Set
    End Property
    Public Property Extensiones() As String
        Get
            Return clExtensiones
        End Get
        Set(ByVal value As String)
            clExtensiones = value
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
