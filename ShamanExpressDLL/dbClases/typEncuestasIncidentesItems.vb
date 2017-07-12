Public Class typEncuestasIncidentesItems
    Inherits typAll
    Private clEncuestaIncidenteId As typEncuestasIncidentes
    Private clEncuestaItemId As typEncuestasItems
    Private clEncuestaRespuestaId As typEncuestasRespuestas
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property EncuestaIncidenteId() As typEncuestasIncidentes
        Get
            Return Me.GetTypProperty(clEncuestaIncidenteId)
        End Get
        Set(ByVal value As typEncuestasIncidentes)
            clEncuestaIncidenteId = value
        End Set
    End Property
    Public Property EncuestaItemId() As typEncuestasItems
        Get
            Return Me.GetTypProperty(clEncuestaItemId)
        End Get
        Set(ByVal value As typEncuestasItems)
            clEncuestaItemId = value
        End Set
    End Property
    Public Property EncuestaRespuestaId() As typEncuestasRespuestas
        Get
            Return Me.GetTypProperty(clEncuestaRespuestaId)
        End Get
        Set(ByVal value As typEncuestasRespuestas)
            clEncuestaRespuestaId = value
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
