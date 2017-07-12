Public Class typPlanesModulosConceptos
    Inherits typAll
    Private clPlanModuloId As typPlanesModulos
    Private clConceptoFacturacionId As typConceptosFacturacion
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PlanModuloId() As typPlanesModulos
        Get
            Return Me.GetTypProperty(clPlanModuloId)
        End Get
        Set(ByVal value As typPlanesModulos)
            clPlanModuloId = value
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

End Class
