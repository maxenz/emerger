Public Class typEncuestasRespuestas
    Inherits typAll
    Private clEncuestaId As typEncuestas
    Private clNroRespuesta As Integer
    Private clDescripcion As String
    Private clValor As Integer
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
    Public Property NroRespuesta() As Integer
        Get
            Return clNroRespuesta
        End Get
        Set(ByVal value As Integer)
            clNroRespuesta = value
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
    Public Property Valor() As Integer
        Get
            Return clValor
        End Get
        Set(ByVal value As Integer)
            clValor = value
        End Set
    End Property

End Class
