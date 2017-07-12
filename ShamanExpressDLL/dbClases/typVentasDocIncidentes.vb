Public Class typVentasDocIncidentes
    Inherits typAll
    Private clVentaDocumentoId As typVentasDocumentos
    Private clConceptoFacturacionId As typConceptosFacturacion
    Private clIncidenteId As typIncidentes
    Private clAlicuotaIvaId As typAlicuotasIva
    Private clPorcentajeIva As Decimal
    Private clAlicuotaBsAsIIBBId As typAlicuotasIva
    Private clPorcentajeBsAsIIBB As Decimal
    Private clImporte As Decimal
    Private clCoPago As Decimal
    Private clcntViajes As Integer
    Private clcntKilometros As Integer
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
    Public Property ConceptoFacturacionId() As typConceptosFacturacion
        Get
            Return Me.GetTypProperty(clConceptoFacturacionId)
        End Get
        Set(ByVal value As typConceptosFacturacion)
            clConceptoFacturacionId = value
        End Set
    End Property
    Public Property IncidenteId() As typIncidentes
        Get
            Return Me.GetTypProperty(clIncidenteId)
        End Get
        Set(ByVal value As typIncidentes)
            clIncidenteId = value
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
    Public Property Importe() As Decimal
        Get
            Return clImporte
        End Get
        Set(ByVal value As Decimal)
            clImporte = value
        End Set
    End Property
    Public Property CoPago() As Decimal
        Get
            Return clCoPago
        End Get
        Set(ByVal value As Decimal)
            clCoPago = value
        End Set
    End Property
    Public Property cntViajes() As Integer
        Get
            Return clcntViajes
        End Get
        Set(ByVal value As Integer)
            clcntViajes = value
        End Set
    End Property
    Public Property cntKilometros() As Integer
        Get
            Return clcntKilometros
        End Get
        Set(ByVal value As Integer)
            clcntKilometros = value
        End Set
    End Property

End Class
