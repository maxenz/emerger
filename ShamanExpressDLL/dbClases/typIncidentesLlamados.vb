Public Class typIncidentesLlamados
    Inherits typAll
    Private clIncidenteId As typIncidentes
    Private clAgenteId As String
    Private clDNIS As String
    Private clCampania As String
    Private clANI As String
    Private clNroInterno As Int64
    Private clGrabacionId As String
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
    Public Property AgenteId() As String
        Get
            Return clAgenteId
        End Get
        Set(ByVal value As String)
            clAgenteId = value
        End Set
    End Property
    Public Property DNIS() As String
        Get
            Return clDNIS
        End Get
        Set(ByVal value As String)
            clDNIS = value
        End Set
    End Property
    Public Property Campania() As String
        Get
            Return clCampania
        End Get
        Set(ByVal value As String)
            clCampania = value
        End Set
    End Property
    Public Property ANI() As String
        Get
            Return clANI
        End Get
        Set(ByVal value As String)
            clANI = value
        End Set
    End Property
    Public Property NroInterno() As Int64
        Get
            Return clNroInterno
        End Get
        Set(ByVal value As Int64)
            clNroInterno = value
        End Set
    End Property
    Public Property GrabacionId() As String
        Get
            Return clGrabacionId
        End Get
        Set(ByVal value As String)
            clGrabacionId = value
        End Set
    End Property

End Class
