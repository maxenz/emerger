Public Class typMovilesSucesos
    Inherits typAll
    Private clMovilId As typMoviles
    Private clVehiculoId As typVehiculos
    Private clTipoMovilId As typTiposMoviles
    Private clBaseOperativaId As typBasesOperativas
    Private clFechaHoraSuceso As DateTime
    Private clFechaHoraFinal As DateTime
    Private clIncidenteSucesoId As typIncidentesSucesos
    Private clMotivoCondicionalId As typMotivosCondicionales
    Private clKilometraje As Int64
    Private clKilometrajeFinal As Int64
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
    Public Property FechaHoraSuceso() As DateTime
        Get
            Return clFechaHoraSuceso
        End Get
        Set(ByVal value As DateTime)
            clFechaHoraSuceso = value
        End Set
    End Property
    Public Property FechaHoraFinal() As DateTime
        Get
            Return clFechaHoraFinal
        End Get
        Set(ByVal value As DateTime)
            clFechaHoraFinal = value
        End Set
    End Property
    Public Property IncidenteSucesoId() As typIncidentesSucesos
        Get
            Return Me.GetTypProperty(clIncidenteSucesoId)
        End Get
        Set(ByVal value As typIncidentesSucesos)
            clIncidenteSucesoId = value
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
    Public Property Kilometraje() As Int64
        Get
            Return clKilometraje
        End Get
        Set(ByVal value As Int64)
            clKilometraje = value
        End Set
    End Property
    Public Property KilometrajeFinal() As Int64
        Get
            Return clKilometrajeFinal
        End Get
        Set(ByVal value As Int64)
            clKilometrajeFinal = value
        End Set
    End Property
End Class
