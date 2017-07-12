Public Class typClientesAlicuotas
    Inherits typAll
    Private clClienteId As typClientes
    Private clAlicuotaImpuestoId As typAlicuotasIva
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ClienteId() As typClientes
        Get
            Return Me.GetTypProperty(clClienteId)
        End Get
        Set(ByVal value As typClientes)
            clClienteId = value
        End Set
    End Property
    Public Property AlicuotaImpuestoId() As typAlicuotasIva
        Get
            Return Me.GetTypProperty(clAlicuotaImpuestoId)
        End Get
        Set(ByVal value As typAlicuotasIva)
            clAlicuotaImpuestoId = value
        End Set
    End Property

End Class
