Public Class typIncidentesAdjuntos
    Inherits typAll
    Private clIncidenteId As typIncidentes
    Private clAdjuntoClasificacionId As typAdjuntosClasificaciones
    Private clArchivo As String
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property IncidenteId() As typIncidentes
        Get
            Return Me.GetTypProperty(clIncidenteId)
        End Get
        Set(ByVal value As typIncidentes)
            clIncidenteId = value
        End Set
    End Property
    Public Property AdjuntoClasificacionId() As typAdjuntosClasificaciones
        Get
            Return Me.GetTypProperty(clAdjuntoClasificacionId)
        End Get
        Set(ByVal value As typAdjuntosClasificaciones)
            clAdjuntoClasificacionId = value
        End Set
    End Property
    Public Property Archivo() As String
        Get
            Return clArchivo
        End Get
        Set(ByVal value As String)
            clArchivo = value
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