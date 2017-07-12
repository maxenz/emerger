Public Class typCentrosAtencionSalas
    Inherits typAll
    Private clCentroAtencionId As typCentrosAtencion
    Private clAbreviaturaId As String
    Private clClasificacionId As Integer
    Private clflgFeriados As Integer
    Private clcntCamas As Integer
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property CentroAtencionId() As typCentrosAtencion
        Get
            Return Me.GetTypProperty(clCentroAtencionId)
        End Get
        Set(ByVal value As typCentrosAtencion)
            clCentroAtencionId = value
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
    Public Property ClasificacionId() As Integer
        Get
            Return clClasificacionId
        End Get
        Set(ByVal value As Integer)
            clClasificacionId = value
        End Set
    End Property
    Public Property flgFeriados() As Integer
        Get
            Return clflgFeriados
        End Get
        Set(ByVal value As Integer)
            clflgFeriados = value
        End Set
    End Property
    Public Property cntCamas() As Integer
        Get
            Return clcntCamas
        End Get
        Set(ByVal value As Integer)
            clcntCamas = value
        End Set
    End Property
    Public Property Observaciones() As String
        Get
            Return clObservaciones
        End Get
        Set(ByVal value As String)
            clObservaciones = value
        End Set
    End Property

End Class
