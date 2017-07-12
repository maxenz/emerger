Public Class typTurnosEstados
    Inherits typAll
    Private clTurnoId As typTurnos
    Private clFechaHoraSuceso As DateTime
    Private clEstadoTurnoId As typEstadosTurnos
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TurnoId() As typTurnos
        Get
            Return Me.GetTypProperty(clTurnoId)
        End Get
        Set(ByVal value As typTurnos)
            clTurnoId = value
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
    Public Property EstadoTurnoId() As typEstadosTurnos
        Get
            Return Me.GetTypProperty(clEstadoTurnoId)
        End Get
        Set(ByVal value As typEstadosTurnos)
            clEstadoTurnoId = value
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
