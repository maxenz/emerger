Public Class typIncidentesPracticasDetalle
    Inherits typAll
    Private clIncidentePracticaId As typIncidentesPracticas
    Private clSanatorioId As typSanatorios
    Private clPracticaId As typPracticas
    Private clCantidad As Integer
    Private clflgConsumida As Integer
    Private clNroComprobante As String
    Private clImporte As Decimal
    Private clMedicoSanatorio As String
    Private clObservaciones As String
    Private clflgPurge As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property IncidentePracticaId() As typIncidentesPracticas
        Get
            Return Me.GetTypProperty(clIncidentePracticaId)
        End Get
        Set(ByVal value As typIncidentesPracticas)
            clIncidentePracticaId = value
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
    Public Property PracticaId() As typPracticas
        Get
            Return Me.GetTypProperty(clPracticaId)
        End Get
        Set(ByVal value As typPracticas)
            clPracticaId = value
        End Set
    End Property
    Public Property Cantidad() As Integer
        Get
            Return clCantidad
        End Get
        Set(ByVal value As Integer)
            clCantidad = value
        End Set
    End Property
    Public Property flgConsumida() As Integer
        Get
            Return clflgConsumida
        End Get
        Set(ByVal value As Integer)
            clflgConsumida = value
        End Set
    End Property
    Public Property NroComprobante() As String
        Get
            Return clNroComprobante
        End Get
        Set(ByVal value As String)
            clNroComprobante = value
        End Set
    End Property
    Public Property Importe() As Decimal
        Get
            Return clImporte
        End Get
        Set(ByVal value As Decimal)
            clImporte = value
        End Set
    End Property
    Public Property MedicoSanatorio() As String
        Get
            Return clMedicoSanatorio
        End Get
        Set(ByVal value As String)
            clMedicoSanatorio = value
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
    Public Property flgPurge() As Integer
        Get
            Return clflgPurge
        End Get
        Set(ByVal value As Integer)
            clflgPurge = value
        End Set
    End Property

End Class
