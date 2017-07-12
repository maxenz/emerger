Public Class typSintomasPreguntas
    Inherits typAll
    Private clSintomaId As typSintomas
    Private clflgPediatrico As Integer
    Private clPreguntaId As Integer
    Private clTipoFrase As String
    Private clFrase As String
    Private clRespuesta1 As Integer
    Private clRespuesta2 As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property SintomaId() As typSintomas
        Get
            Return Me.GetTypProperty(clSintomaId)
        End Get
        Set(ByVal value As typSintomas)
            clSintomaId = value
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
    Public Property Respuesta1() As Integer
        Get
            Return clRespuesta1
        End Get
        Set(ByVal value As Integer)
            clRespuesta1 = value
        End Set
    End Property
    Public Property Respuesta2() As Integer
        Get
            Return clRespuesta2
        End Get
        Set(ByVal value As Integer)
            clRespuesta2 = value
        End Set
    End Property

End Class
