Public Class typIncidentesSucesos
    Inherits typAll
    Private clIncidenteViajeId As typIncidentesViajes
    Private clFechaHoraSuceso As DateTime
    Private clSucesoIncidenteId As typSucesosIncidentes
    Private clMovilId As typMoviles
    Private clCondicion As String
    Private clDiagnosticoId As typDiagnosticos
    Private clMotivoNoRealizacionId As typMotivosNoRealizacion
    Private clflgCopagoNoCob As Integer
    Private clDemora As Integer
    Private clNroPrioridadEmpresa As Integer
    Private clObservaciones As String
    Private clgpsLatitud As Decimal
    Private clgpsLongitud As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property IncidenteViajeId() As typIncidentesViajes
        Get
            Return Me.GetTypProperty(clIncidenteViajeId)
        End Get
        Set(ByVal value As typIncidentesViajes)
            clIncidenteViajeId = value
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
    Public Property SucesoIncidenteId() As typSucesosIncidentes
        Get
            Return Me.GetTypProperty(clSucesoIncidenteId)
        End Get
        Set(ByVal value As typSucesosIncidentes)
            clSucesoIncidenteId = value
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
    Public Property Condicion() As String
        Get
            Return clCondicion
        End Get
        Set(ByVal value As String)
            clCondicion = value
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
    Public Property flgCopagoNoCob() As Integer
        Get
            Return clflgCopagoNoCob
        End Get
        Set(ByVal value As Integer)
            clflgCopagoNoCob = value
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
    Public Property NroPrioridadEmpresa() As Integer
        Get
            Return clNroPrioridadEmpresa
        End Get
        Set(ByVal value As Integer)
            clNroPrioridadEmpresa = value
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

End Class
