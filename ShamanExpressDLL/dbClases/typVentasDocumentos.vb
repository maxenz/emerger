Public Class typVentasDocumentos
    Inherits typAll
    Private clTipoComprobanteId As typTiposComprobantes
    Private clNroComprobante As String
    Private clTalonarioId As typTalonarios
    Private clClienteId As typClientes
    Private clCUIT As String
    Private clRazonSocial As String
    Private clFecComprobante As Date
    Private clFecVencimiento As Date
    Private clFecPreDesde As Date
    Private clFecPreHasta As Date
    Private clImporte As Decimal
    Private clImpExento As Decimal
    Private clImpGravado As Decimal
    Private clImpGravadoBsAsIIBB As Decimal
    Private clImpIva1 As Decimal
    Private clImpIva2 As Decimal
    Private clImpBsAsIIBB As Decimal
    Private clImpImpuesto As Decimal
    Private clImpSaldado As Decimal
    Private clSituacionIvaId As typSituacionesIva
    Private clAlicuotaIvaId As typAlicuotasIva
    Private clPorcentajeIva As Decimal
    Private clAlicuotaBsAsIIBBId As typAlicuotasIva
    Private clPorcentajeBsAsIIBB As Decimal
    Private clCobradorId As typCobradores
    Private clObservaciones As String
    Private clTipoProceso As String
    Private clNroCAE As Int64
    Private clCodigoBarra As String
    Private clFecVencimientoCAE As Date
    Private clPeriodo As Int64
    Private clflgStatus As Short
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
    Public Property TalonarioId() As typTalonarios
        Get
            Return Me.GetTypProperty(clTalonarioId)
        End Get
        Set(ByVal value As typTalonarios)
            clTalonarioId = value
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
    Public Property CUIT() As String
        Get
            Return clCUIT
        End Get
        Set(ByVal value As String)
            clCUIT = value
        End Set
    End Property
    Public Property RazonSocial() As String
        Get
            Return clRazonSocial
        End Get
        Set(ByVal value As String)
            clRazonSocial = value
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
    Public Property FecVencimiento() As Date
        Get
            Return clFecVencimiento
        End Get
        Set(ByVal value As Date)
            clFecVencimiento = value
        End Set
    End Property
    Public Property FecPreDesde() As Date
        Get
            Return clFecPreDesde
        End Get
        Set(ByVal value As Date)
            clFecPreDesde = value
        End Set
    End Property
    Public Property FecPreHasta() As Date
        Get
            Return clFecPreHasta
        End Get
        Set(ByVal value As Date)
            clFecPreHasta = value
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
    Public Property ImpExento() As Decimal
        Get
            Return clImpExento
        End Get
        Set(ByVal value As Decimal)
            clImpExento = value
        End Set
    End Property
    Public Property ImpGravado() As Decimal
        Get
            Return clImpGravado
        End Get
        Set(ByVal value As Decimal)
            clImpGravado = value
        End Set
    End Property
    Public Property ImpGravadoBsAsIIBB() As Decimal
        Get
            Return clImpGravadoBsAsIIBB
        End Get
        Set(ByVal value As Decimal)
            clImpGravadoBsAsIIBB = value
        End Set
    End Property
    Public Property ImpIva1() As Decimal
        Get
            Return clImpIva1
        End Get
        Set(ByVal value As Decimal)
            clImpIva1 = value
        End Set
    End Property
    Public Property ImpIva2() As Decimal
        Get
            Return clImpIva2
        End Get
        Set(ByVal value As Decimal)
            clImpIva2 = value
        End Set
    End Property
    Public Property ImpBsAsIIBB() As Decimal
        Get
            Return clImpBsAsIIBB
        End Get
        Set(ByVal value As Decimal)
            clImpBsAsIIBB = value
        End Set
    End Property
    Public Property ImpImpuesto() As Decimal
        Get
            Return clImpImpuesto
        End Get
        Set(ByVal value As Decimal)
            clImpImpuesto = value
        End Set
    End Property
    Public Property ImpSaldado() As Decimal
        Get
            Return clImpSaldado
        End Get
        Set(ByVal value As Decimal)
            clImpSaldado = value
        End Set
    End Property
    Public Property SituacionIvaId() As typSituacionesIva
        Get
            Return Me.GetTypProperty(clSituacionIvaId)
        End Get
        Set(ByVal value As typSituacionesIva)
            clSituacionIvaId = value
        End Set
    End Property
    Public Property AlicuotaIvaId() As typAlicuotasIva
        Get
            Return Me.GetTypProperty(clAlicuotaIvaId)
        End Get
        Set(ByVal value As typAlicuotasIva)
            clAlicuotaIvaId = value
        End Set
    End Property
    Public Property PorcentajeIva() As Decimal
        Get
            Return clPorcentajeIva
        End Get
        Set(ByVal value As Decimal)
            clPorcentajeIva = value
        End Set
    End Property
    Public Property AlicuotaBsAsIIBBId() As typAlicuotasIva
        Get
            Return Me.GetTypProperty(clAlicuotaBsAsIIBBId)
        End Get
        Set(ByVal value As typAlicuotasIva)
            clAlicuotaBsAsIIBBId = value
        End Set
    End Property
    Public Property PorcentajeBsAsIIBB() As Decimal
        Get
            Return clPorcentajeBsAsIIBB
        End Get
        Set(ByVal value As Decimal)
            clPorcentajeBsAsIIBB = value
        End Set
    End Property
    Public Property TipoProceso() As String
        Get
            Return clTipoProceso
        End Get
        Set(ByVal value As String)
            clTipoProceso = value
        End Set
    End Property
    Public Property CobradorId() As typCobradores
        Get
            Return Me.GetTypProperty(clCobradorId)
        End Get
        Set(ByVal value As typCobradores)
            clCobradorId = value
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
    Public Property NroCAE() As Int64
        Get
            Return clNroCAE
        End Get
        Set(ByVal value As int64)
            clNroCAE = value
        End Set
    End Property
    Public Property CodigoBarra() As String
        Get
            Return clCodigoBarra
        End Get
        Set(ByVal value As String)
            clCodigoBarra = value
        End Set
    End Property
    Public Property FecVencimientoCAE() As Date
        Get
            Return clFecVencimientoCAE
        End Get
        Set(ByVal value As Date)
            clFecVencimientoCAE = value
        End Set
    End Property
    Public Property Periodo() As Int64
        Get
            Return clPeriodo
        End Get
        Set(ByVal value As Int64)
            clPeriodo = value
        End Set
    End Property
    Public Property flgStatus() As Short
        Get
            Return clflgStatus
        End Get
        Set(ByVal value As Short)
            clflgStatus = value
        End Set
    End Property

End Class
