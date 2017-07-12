Public Class typAsientos
    Inherits typAll
    Private clTipoComprobanteId As typTiposComprobantes
    Private clNroComprobante As String
    Private clFecComprobante As Date
    Private clProveedorId As typProveedores
    Private clPersonalId As typPersonal
    Private clClienteId As typClientes
    Private clVentaDocumentoId As typVentasDocumentos
    Private clConcepto As String
    Private clTipoGastoId As typTiposGastos
    Private clTipoOtroIngresoId As typTiposOtrosIngresos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TipoComprobanteId() As typTiposComprobantes
        Get
            Return Me.GetTypProperty(clTipoComprobanteId)
        End Get
        Set(ByVal value As typTiposComprobantes)
            clTipoComprobanteId = value
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
    Public Property FecComprobante() As Date
        Get
            Return clFecComprobante
        End Get
        Set(ByVal value As Date)
            clFecComprobante = value
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
    Public Property ClienteId() As typClientes
        Get
            Return Me.GetTypProperty(clClienteId)
        End Get
        Set(ByVal value As typClientes)
            clClienteId = value
        End Set
    End Property
    Public Property VentaDocumentoId() As typVentasDocumentos
        Get
            Return Me.GetTypProperty(clVentaDocumentoId)
        End Get
        Set(ByVal value As typVentasDocumentos)
            clVentaDocumentoId = value
        End Set
    End Property
    Public Property Concepto() As String
        Get
            Return clConcepto
        End Get
        Set(ByVal value As String)
            clConcepto = value
        End Set
    End Property
    Public Property TipoGastoId() As typTiposGastos
        Get
            Return Me.GetTypProperty(clTipoGastoId)
        End Get
        Set(ByVal value As typTiposGastos)
            clTipoGastoId = value
        End Set
    End Property
    Public Property TipoOtroIngresoId() As typTiposOtrosIngresos
        Get
            Return Me.GetTypProperty(clTipoOtroIngresoId)
        End Get
        Set(ByVal value As typTiposOtrosIngresos)
            clTipoOtroIngresoId = value
        End Set
    End Property

End Class
