Public Class typIncidentesObservaciones
    Inherits typAll
    Private clIncidenteId As typIncidentes
    Private clflgReclamo As Integer
    Private clSanatorioId As typSanatorios
    Private clSanatorioNombre As String
    Private clMovilId As typMoviles
    Private clNroPrioridadEmpresa As Integer
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
    Public Property flgReclamo() As Integer
        Get
            Return clflgReclamo
        End Get
        Set(ByVal value As Integer)
            clflgReclamo = value
        End Set
    End Property
    Public Property SanatorioId() As typSanatorios
        Get
            Return Me.GetTypProperty(clSanatorioId)
        End Get
        Set(ByVal value As typSanatorios)
            clSanatorioId = value
        End Set
    End Property
    Public Property SanatorioNombre() As String
        Get
            Return clSanatorioNombre
        End Get
        Set(ByVal value As String)
            clSanatorioNombre = value
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
    Public Property Observaciones() As String
        Get
            Return clObservaciones
        End Get
        Set(ByVal value As String)
            clObservaciones = value
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
End Class
