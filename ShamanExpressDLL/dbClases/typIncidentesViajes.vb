Public Class typIncidentesViajes
    Inherits typAll
    Private clIncidenteDomicilioId As typIncidentesDomicilios
    Private clViajeId As String
    Private clHorarioOperativo As usrHorarioOperativo
    Private clreqHorLlegada As DateTime
    Private clreqHorInternacion As DateTime
    Private clflgStatus As Integer
    Private clflgModoDespacho As Integer
    Private clMovilId As typMoviles
    Private clMovilPreasignadoId As typMoviles
    Private clDiagnosticoId As typDiagnosticos
    Private clMotivoNoRealizacionId As typMotivosNoRealizacion
    Private clDemora As Int64
    Private clflgKmtPersonalizado As Integer
    Private clKilometraje As Int64
    Private clflgFacPersonalizado As Integer
    Private clImpFacturacion As Decimal
    Private clflgLiqPersonalizado As Integer
    Private clImpLiquidacion As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property IncidenteDomicilioId() As typIncidentesDomicilios
        Get
            Return Me.GetTypProperty(clIncidenteDomicilioId)
        End Get
        Set(ByVal value As typIncidentesDomicilios)
            clIncidenteDomicilioId = value
        End Set
    End Property
    Public Property ViajeId() As String
        Get
            Return clViajeId
        End Get
        Set(ByVal value As String)
            clViajeId = value
        End Set
    End Property
    Public Property HorarioOperativo() As usrHorarioOperativo
        Get
            Return clHorarioOperativo
        End Get
        Set(ByVal value As usrHorarioOperativo)
            clHorarioOperativo = value
        End Set
    End Property
    Public Property reqHorLlegada() As DateTime
        Get
            Return clreqHorLlegada
        End Get
        Set(ByVal value As DateTime)
            clreqHorLlegada = value
        End Set
    End Property
    Public Property reqHorInternacion() As DateTime
        Get
            Return clreqHorInternacion
        End Get
        Set(ByVal value As DateTime)
            clreqHorInternacion = value
        End Set
    End Property
    Public Property flgStatus() As Integer
        Get
            Return clflgStatus
        End Get
        Set(ByVal value As Integer)
            clflgStatus = value
        End Set
    End Property
    Public Property flgModoDespacho() As Integer
        Get
            Return clflgModoDespacho
        End Get
        Set(ByVal value As Integer)
            clflgModoDespacho = value
        End Set
    End Property
    Public Property MovilId() As typMoviles
        Get
            Return Me.GetTypProperty(clMovilId)
        End Get
        Set(ByVal value As typMoviles)
            clMovilId = value
        End Set
    End Property
    Public Property MovilPreasignadoId() As typMoviles
        Get
            Return Me.GetTypProperty(clMovilPreasignadoId)
        End Get
        Set(ByVal value As typMoviles)
            clMovilPreasignadoId = value
        End Set
    End Property
    Public Property DiagnosticoId() As typDiagnosticos
        Get
            Return Me.GetTypProperty(clDiagnosticoId)
        End Get
        Set(ByVal value As typDiagnosticos)
            clDiagnosticoId = value
        End Set
    End Property
    Public Property MotivoNoRealizacionId() As typMotivosNoRealizacion
        Get
            Return Me.GetTypProperty(clMotivoNoRealizacionId)
        End Get
        Set(ByVal value As typMotivosNoRealizacion)
            clMotivoNoRealizacionId = value
        End Set
    End Property
    Public Property Demora() As Int64
        Get
            Return clDemora
        End Get
        Set(ByVal value As Int64)
            clDemora = value
        End Set
    End Property
    Public Property flgKmtPersonalizado() As Integer
        Get
            Return clflgKmtPersonalizado
        End Get
        Set(ByVal value As Integer)
            clflgKmtPersonalizado = value
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
    Public Property flgFacPersonalizado() As Integer
        Get
            Return clflgFacPersonalizado
        End Get
        Set(ByVal value As Integer)
            clflgFacPersonalizado = value
        End Set
    End Property
    Public Property ImpFacturacion() As Decimal
        Get
            Return clImpFacturacion
        End Get
        Set(ByVal value As Decimal)
            clImpFacturacion = value
        End Set
    End Property
    Public Property flgLiqPersonalizado() As Integer
        Get
            Return clflgLiqPersonalizado
        End Get
        Set(ByVal value As Integer)
            clflgLiqPersonalizado = value
        End Set
    End Property
    Public Property ImpLiquidacion() As Decimal
        Get
            Return clImpLiquidacion
        End Get
        Set(ByVal value As Decimal)
            clImpLiquidacion = value
        End Set
    End Property

End Class

