Public Class typVentasDocRenglones
    Inherits typAll
    Private clVentaDocumentoId As typVentasDocumentos
    Private clRenglonId As Int64
    Private clConceptoFacturacionId As typConceptosFacturacion
    Private clDescripcion As String
    Private clAlicuotaIvaId As typAlicuotasIva
    Private clPorcentajeIva As Decimal
    Private clAlicuotaBsAsIIBBId As typAlicuotasIva
    Private clPorcentajeBsAsIIBB As Decimal
    Private clCantidad As Decimal
    Private clImporte As Decimal
    Private clPlanId As typPlanes
    Private clIntegranteClasificacionId As typIntegrantesClasificaciones
    Private clClienteId As typClientes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property VentaDocumentoId() As typVentasDocumentos
        Get
            Return Me.GetTypProperty(clVentaDocumentoId)
        End Get
        Set(ByVal value As typVentasDocumentos)
            clVentaDocumentoId = value
        End Set
    End Property
    Public Property RenglonId() As Int64
        Get
            Return clRenglonId
        End Get
        Set(ByVal value As Int64)
            clRenglonId = value
        End Set
    End Property
    Public Property ConceptoFacturacionId() As typConceptosFacturacion
        Get
            Return Me.GetTypProperty(clConceptoFacturacionId)
        End Get
        Set(ByVal value As typConceptosFacturacion)
            clConceptoFacturacionId = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
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
    Public Property Cantidad() As Decimal
        Get
            Return clCantidad
        End Get
        Set(ByVal value As Decimal)
            clCantidad = value
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
    Public Property PlanId() As typPlanes
        Get
            Return Me.GetTypProperty(clPlanId)
        End Get
        Set(ByVal value As typPlanes)
            clPlanId = value
        End Set
    End Property
    Public Property IntegranteClasificacionId() As typIntegrantesClasificaciones
        Get
            Return Me.GetTypProperty(clIntegranteClasificacionId)
        End Get
        Set(ByVal value As typIntegrantesClasificaciones)
            clIntegranteClasificacionId = value
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

End Class
