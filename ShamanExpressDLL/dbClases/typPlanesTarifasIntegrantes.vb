Public Class typPlanesTarifasIntegrantes
    Inherits typAll
    Private clPlanId As typPlanes
    Private clIntegranteClasificacionId As typIntegrantesClasificaciones
    Private clEdadDesde As Integer
    Private clEdadHasta As Integer
    Private clImporte As Decimal
    Private clflgRequerido As Integer
    Private clTipoExpresion As Integer
    Private clTextoRenglon As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
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
    Public Property EdadDesde() As Integer
        Get
            Return clEdadDesde
        End Get
        Set(ByVal value As Integer)
            clEdadDesde = value
        End Set
    End Property
    Public Property EdadHasta() As Integer
        Get
            Return clEdadHasta
        End Get
        Set(ByVal value As Integer)
            clEdadHasta = value
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
    Public Property flgRequerido() As Integer
        Get
            Return clflgRequerido
        End Get
        Set(ByVal value As Integer)
            clflgRequerido = value
        End Set
    End Property
    Public Property TipoExpresion() As Integer
        Get
            Return clTipoExpresion
        End Get
        Set(ByVal value As Integer)
            clTipoExpresion = value
        End Set
    End Property
    Public Property TextoRenglon() As String
        Get
            Return clTextoRenglon
        End Get
        Set(ByVal value As String)
            clTextoRenglon = value
        End Set
    End Property

End Class
