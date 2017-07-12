Public Class typMovilesActuales
    Inherits typAll
    Private clMovilId As typMoviles
    Private clVehiculoId As typVehiculos
    Private clTipoMovilId As typTiposMoviles
    Private clBaseOperativaId As typBasesOperativas
    Private clSucesoIncidenteId As typSucesosIncidentes
    Private clFechaHoraMovimiento As DateTime
    Private clLocalidadId As typLocalidades
    Private clIncidenteViajeId As typIncidentesViajes
    Private clMotivoCondicionalId As typMotivosCondicionales
    Private clTpoCondicional As Integer
    Private clgpsLatitud As Decimal
    Private clgpsLongitud As Decimal
    Private clgpsFecHorTransmision As DateTime
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property MovilId() As typMoviles
        Get
            Return Me.GetTypProperty(clMovilId)
        End Get
        Set(ByVal value As typMoviles)
            clMovilId = value
        End Set
    End Property
    Public Property VehiculoId() As typVehiculos
        Get
            Return Me.GetTypProperty(clVehiculoId)
        End Get
        Set(ByVal value As typVehiculos)
            clVehiculoId = value
        End Set
    End Property
    Public Property TipoMovilId() As typTiposMoviles
        Get
            Return Me.GetTypProperty(clTipoMovilId)
        End Get
        Set(ByVal value As typTiposMoviles)
            clTipoMovilId = value
        End Set
    End Property
    Public Property BaseOperativaId() As typBasesOperativas
        Get
            Return Me.GetTypProperty(clBaseOperativaId)
        End Get
        Set(ByVal value As typBasesOperativas)
            clBaseOperativaId = value
        End Set
    End Property
    Public Property SucesoIncidenteId() As typSucesosIncidentes
        Get
            Return Me.GetTypProperty(clSucesoIncidenteId)
        End Get
        Set(ByVal value As typSucesosIncidentes)
            clSucesoIncidenteId = value
        End Set
    End Property
    Public Property FechaHoraMovimiento() As DateTime
        Get
            Return clFechaHoraMovimiento
        End Get
        Set(ByVal value As DateTime)
            clFechaHoraMovimiento = value
        End Set
    End Property
    Public Property LocalidadId() As typLocalidades
        Get
            Return Me.GetTypProperty(clLocalidadId)
        End Get
        Set(ByVal value As typLocalidades)
            clLocalidadId = value
        End Set
    End Property
    Public Property IncidenteViajeId() As typIncidentesViajes
        Get
            Return Me.GetTypProperty(clIncidenteViajeId)
        End Get
        Set(ByVal value As typIncidentesViajes)
            clIncidenteViajeId = value
        End Set
    End Property
    Public Property MotivoCondicionalId() As typMotivosCondicionales
        Get
            Return Me.GetTypProperty(clMotivoCondicionalId)
        End Get
        Set(ByVal value As typMotivosCondicionales)
            clMotivoCondicionalId = value
        End Set
    End Property
    Public Property TpoCondicional() As Integer
        Get
            Return clTpoCondicional
        End Get
        Set(ByVal value As Integer)
            clTpoCondicional = value
        End Set
    End Property
    Public Property gpsLatitud() As Decimal
        Get
            Return clgpsLatitud
        End Get
        Set(ByVal value As Decimal)
            clgpsLatitud = value
        End Set
    End Property
    Public Property gpsLongitud() As Decimal
        Get
            Return clgpsLongitud
        End Get
        Set(ByVal value As Decimal)
            clgpsLongitud = value
        End Set
    End Property
    Public Property gpsFecHorTransmision() As DateTime
        Get
            Return clgpsFecHorTransmision
        End Get
        Set(ByVal value As DateTime)
            clgpsFecHorTransmision = value
        End Set
    End Property

End Class
