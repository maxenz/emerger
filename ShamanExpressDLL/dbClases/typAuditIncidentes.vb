Public Class typAuditIncidentes
    Inherits typAll
    Private clIncidenteId As typIncidentes
    Private clReporteadorFieldId As typReporteadoresFields
    Private clValorAnterior As String
    Private clValorNuevo As String
    Private clregAntUsuarioId As typUsuarios
    Private clregAntFechaHora As DateTime
    Private clregAntTerminalId As typTerminales
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
    Public Property ReporteadorFieldId() As typReporteadoresFields
        Get
            Return Me.GetTypProperty(clReporteadorFieldId)
        End Get
        Set(ByVal value As typReporteadoresFields)
            clReporteadorFieldId = value
        End Set
    End Property
    Public Property ValorAnterior() As String
        Get
            Return clValorAnterior
        End Get
        Set(ByVal value As String)
            clValorAnterior = value
        End Set
    End Property
    Public Property ValorNuevo() As String
        Get
            Return clValorNuevo
        End Get
        Set(ByVal value As String)
            clValorNuevo = value
        End Set
    End Property
    Public Property regAntUsuarioId() As typUsuarios
        Get
            Return Me.GetTypProperty(clregAntUsuarioId)
        End Get
        Set(ByVal value As typUsuarios)
            clregAntUsuarioId = value
        End Set
    End Property
    Public Property regAntFechaHora() As DateTime
        Get
            Return clregAntFechaHora
        End Get
        Set(ByVal value As DateTime)
            clregAntFechaHora = value
        End Set
    End Property
    Public Property regAntTerminalId() As typTerminales
        Get
            Return Me.GetTypProperty(clregAntTerminalId)
        End Get
        Set(ByVal value As typTerminales)
            clregAntTerminalId = value
        End Set
    End Property
End Class
