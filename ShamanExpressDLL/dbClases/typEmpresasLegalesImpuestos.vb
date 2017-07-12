Public Class typEmpresasLegalesImpuestos
    Inherits typAll
    Private clEmpresaLegalId As typEmpresasLegales
    Private clImpuestoId As typImpuestos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property EmpresaLegalId() As typEmpresasLegales
        Get
            Return Me.GetTypProperty(clEmpresaLegalId)
        End Get
        Set(ByVal value As typEmpresasLegales)
            clEmpresaLegalId = value
        End Set
    End Property
    Public Property ImpuestoId() As typImpuestos
        Get
            Return Me.GetTypProperty(clImpuestoId)
        End Get
        Set(ByVal value As typImpuestos)
            clImpuestoId = value
        End Set
    End Property

End Class
