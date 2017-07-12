Public Class typEncuestas
    Inherits typAll
    Private clNroEncuesta As Integer
    Private clDescripcion As String
    Private clActivo As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property NroEncuesta() As Integer
        Get
            Return clNroEncuesta
        End Get
        Set(ByVal value As Integer)
            clNroEncuesta = value
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
    Public Property Activo() As Integer
        Get
            Return clActivo
        End Get
        Set(ByVal value As Integer)
            clActivo = value
        End Set
    End Property

End Class
