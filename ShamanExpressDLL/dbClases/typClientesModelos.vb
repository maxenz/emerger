Public Class typClientesModelos
    Inherits typAll
    Private clClienteId As typClientes
    Private clModeloId As Integer
    Private clRenglonId As Integer
    Private clDescripcion As String
    Private clAlicuotaIvaId As typAlicuotasIva
    Private clAlicuotaBsAsIIBBId As typAlicuotasIva
    Private clCantidad As Decimal
    Private clImporte As Decimal
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
    Public Property ModeloId() As Integer
        Get
            Return clModeloId
        End Get
        Set(ByVal value As Integer)
            clModeloId = value
        End Set
    End Property
    Public Property RenglonId() As Integer
        Get
            Return clRenglonId
        End Get
        Set(ByVal value As Integer)
            clRenglonId = value
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
    Public Property AlicuotaBsAsIIBBId() As typAlicuotasIva
        Get
            Return Me.GetTypProperty(clAlicuotaBsAsIIBBId)
        End Get
        Set(ByVal value As typAlicuotasIva)
            clAlicuotaBsAsIIBBId = value
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

End Class
