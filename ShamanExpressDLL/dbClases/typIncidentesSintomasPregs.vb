Public Class typIncidentesSintomasPregs
    Inherits typAll
    Private clIncidenteSintomaId As typIncidentesSintomas
    Private clSintomaPreguntaId As typSintomasPreguntas
    Private clflgPediatrico As Integer
    Private clPreguntaId As Integer
    Private clTipoFrase As String
    Private clFrase As String
    Private clRespuesta As Integer
    Private clPuntaje As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property IncidenteSintomaId() As typIncidentesSintomas
        Get
            Return Me.GetTypProperty(clIncidenteSintomaId)
        End Get
        Set(ByVal value As typIncidentesSintomas)
            clIncidenteSintomaId = value
        End Set
    End Property
    Public Property SintomaPreguntaId() As typSintomasPreguntas
        Get
            Return Me.GetTypProperty(clSintomaPreguntaId)
        End Get
        Set(ByVal value As typSintomasPreguntas)
            clSintomaPreguntaId = value
        End Set
    End Property
    Public Property flgPediatrico() As Integer
        Get
            Return clflgPediatrico
        End Get
        Set(ByVal value As Integer)
            clflgPediatrico = value
        End Set
    End Property
    Public Property PreguntaId() As Integer
        Get
            Return clPreguntaId
        End Get
        Set(ByVal value As Integer)
            clPreguntaId = value
        End Set
    End Property
    Public Property TipoFrase() As String
        Get
            Return clTipoFrase
        End Get
        Set(ByVal value As String)
            clTipoFrase = value
        End Set
    End Property
    Public Property Frase() As String
        Get
            Return clFrase
        End Get
        Set(ByVal value As String)
            clFrase = value
        End Set
    End Property
    Public Property Respuesta() As Integer
        Get
            Return clRespuesta
        End Get
        Set(ByVal value As Integer)
            clRespuesta = value
        End Set
    End Property
    Public Property Puntaje() As Integer
        Get
            Return clPuntaje
        End Get
        Set(ByVal value As Integer)
            clPuntaje = value
        End Set
    End Property

End Class
