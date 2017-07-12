Public Class typClientesModulosConceptos
    Inherits typAll
    Private clClienteModuloId As typClientesModulos
    Private clConceptoFacturacionId As typConceptosFacturacion
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ClienteModuloId() As typClientesModulos
        Get
            Return Me.GetTypProperty(clClienteModuloId)
        End Get
        Set(ByVal value As typClientesModulos)
            clClienteModuloId = value
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
