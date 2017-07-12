Public Class typInsumosMovimientos
    Inherits typAll
    Private clFecMovimiento As Date
    Private clNroComprobante As String
    Private clTipoMovimientoInsumoId As typTiposMovimientosInsumos
    Private clCentroAtencionId As typCentrosAtencion
    Private clProveedorId As typProveedores
    Private clPersonalId As typPersonal
    Private clMovilId As typMoviles
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property FecMovimiento() As Date
        Get
            Return clFecMovimiento
        End Get
        Set(ByVal value As Date)
            clFecMovimiento = value
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
    Public Property TipoMovimientoInsumoId() As typTiposMovimientosInsumos
        Get
            Return Me.GetTypProperty(clTipoMovimientoInsumoId)
        End Get
        Set(ByVal value As typTiposMovimientosInsumos)
            clTipoMovimientoInsumoId = value
        End Set
    End Property
    Public Property CentroAtencionId() As typCentrosAtencion
        Get
            Return Me.GetTypProperty(clCentroAtencionId)
        End Get
        Set(ByVal value As typCentrosAtencion)
            clCentroAtencionId = value
        End Set
    End Property
    Public Property ProveedorId() As typProveedores
        Get
            Return Me.GetTypProperty(clProveedorId)
        End Get
        Set(ByVal value As typProveedores)
            clProveedorId = value
        End Set
    End Property
    Public Property PersonalId() As typPersonal
        Get
            Return Me.GetTypProperty(clPersonalId)
        End Get
        Set(ByVal value As typPersonal)
            clPersonalId = value
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

End Class
