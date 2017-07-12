Public Class typVentasDocCobros
    Inherits typAll
    Private clVentaDocumentoId As typVentasDocumentos
    Private clFormaPagoId As typFormasPago
    Private clBancoId As typBancos
    Private clNroCheque As String
    Private clFecCheque As Date
    Private clImporte As Decimal
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
    Public Property FormaPagoId() As typFormasPago
        Get
            Return Me.GetTypProperty(clFormaPagoId)
        End Get
        Set(ByVal value As typFormasPago)
            clFormaPagoId = value
        End Set
    End Property
    Public Property BancoId() As typBancos
        Get
            Return Me.GetTypProperty(clBancoId)
        End Get
        Set(ByVal value As typBancos)
            clBancoId = value
        End Set
    End Property
    Public Property NroCheque() As String
        Get
            Return clNroCheque
        End Get
        Set(ByVal value As String)
            clNroCheque = value
        End Set
    End Property
    Public Property FecCheque() As Date
        Get
            Return clFecCheque
        End Get
        Set(ByVal value As Date)
            clFecCheque = value
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
