Public Class typEncuestasItems
    Inherits typAll
    Private clEncuestaId As typEncuestas
    Private clNroItem As Integer
    Private clDescripcion As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property EncuestaId() As typEncuestas
        Get
            Return Me.GetTypProperty(clEncuestaId)
        End Get
        Set(ByVal value As typEncuestas)
            clEncuestaId = value
        End Set
    End Property
    Public Property NroItem() As Integer
        Get
            Return clNroItem
        End Get
        Set(ByVal value As Integer)
            clNroItem = value
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
