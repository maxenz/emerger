Public Class typPlanesTarifasIntegrantesRangos
    Inherits typAll
    Private clPlanTarifaIntegranteId As typPlanesTarifasIntegrantes
    Private clDesde As Integer
    Private clHasta As Integer
    Private clImporte As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PlanTarifaIntegranteId() As typPlanesTarifasIntegrantes
        Get
            Return Me.GetTypProperty(clPlanTarifaIntegranteId)
        End Get
        Set(ByVal value As typPlanesTarifasIntegrantes)
            clPlanTarifaIntegranteId = value
        End Set
    End Property
    Public Property Desde() As Integer
        Get
            Return clDesde
        End Get
        Set(ByVal value As Integer)
            clDesde = value
        End Set
    End Property
    Public Property Hasta() As Integer
        Get
            Return clHasta
        End Get
        Set(ByVal value As Integer)
            clHasta = value
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
