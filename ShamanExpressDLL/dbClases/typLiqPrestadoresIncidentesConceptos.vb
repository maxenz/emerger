Public Class typLiqPrestadoresIncidentesConceptos
    '----> Comentario
    Inherits typAll
    Private clLiqPrestadorIncidenteId As typLiquidacionesPrestadoresIncidentes
    Private clConceptoFacturacionId As typConceptosFacturacion
    Private clCantidad As Int64
    Private clImporte As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property LiqPrestadorIncidenteId() As typLiquidacionesPrestadoresIncidentes
        Get
            Return Me.GetTypProperty(clLiqPrestadorIncidenteId)
        End Get
        Set(ByVal value As typLiquidacionesPrestadoresIncidentes)
            clLiqPrestadorIncidenteId = value
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
    Public Property Cantidad() As Int64
        Get
            Return clCantidad
        End Get
        Set(ByVal value As Int64)
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

End Class
