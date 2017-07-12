Public Class typConfiguracionClinica
    Inherits typAll
    Private clTpoTurno As Integer
    Private clEstadoDisponibleId As typEstadosTurnos
    Private clEstadoSuspendidoId As typEstadosTurnos
    Private clflgCargaHistorica As Integer
    Private clmodSinCobertura As Integer
    Private clreqIngresoTurno As Integer
    Private clEstiloImpresion As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TpoTurno() As Integer
        Get
            Return clTpoTurno
        End Get
        Set(ByVal value As Integer)
            clTpoTurno = value
        End Set
    End Property
    Public Property EstadoDisponibleId() As typEstadosTurnos
        Get
            Return Me.GetTypProperty(clEstadoDisponibleId)
        End Get
        Set(ByVal value As typEstadosTurnos)
            clEstadoDisponibleId = value
        End Set
    End Property
    Public Property EstadoSuspendidoId() As typEstadosTurnos
        Get
            Return Me.GetTypProperty(clEstadoSuspendidoId)
        End Get
        Set(ByVal value As typEstadosTurnos)
            clEstadoSuspendidoId = value
        End Set
    End Property
    Public Property flgCargaHistorica() As Integer
        Get
            Return clflgCargaHistorica
        End Get
        Set(ByVal value As Integer)
            clflgCargaHistorica = value
        End Set
    End Property
    Public Property modSinCobertura() As Integer
        Get
            Return clmodSinCobertura
        End Get
        Set(ByVal value As Integer)
            clmodSinCobertura = value
        End Set
    End Property
    Public Property reqIngresoTurno() As Integer
        Get
            Return clreqIngresoTurno
        End Get
        Set(ByVal value As Integer)
            clreqIngresoTurno = value
        End Set
    End Property
    Public Property EstiloImpresion() As Integer
        Get
            Return clEstiloImpresion
        End Get
        Set(ByVal value As Integer)
            clEstiloImpresion = value
        End Set
    End Property
End Class
