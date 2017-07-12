Public Class typIncidentesProgramaciones
    Inherits typAll
    Private clIncidenteId As typIncidentes
    Private clIncidentePrgId As typIncidentes
    Private clestFechaHora As DateTime
    Private clflgSimultaneo As Integer
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
    Public Property IncidentePrgId() As typIncidentes
        Get
            Return Me.GetTypProperty(clIncidentePrgId)
        End Get
        Set(ByVal value As typIncidentes)
            clIncidentePrgId = value
        End Set
    End Property
    Public Property estFechaHora() As DateTime
        Get
            Return clestFechaHora
        End Get
        Set(ByVal value As DateTime)
            clestFechaHora = value
        End Set
    End Property
    Public Property flgSimultaneo() As Integer
        Get
            Return clflgSimultaneo
        End Get
        Set(ByVal value As Integer)
            clflgSimultaneo = value
        End Set
    End Property
End Class
