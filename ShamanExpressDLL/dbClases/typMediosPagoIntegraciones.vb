Public Class typMediosPagoIntegraciones
    Inherits typAll
    Private clDescripcion As String
    Private clIntegracionId As iteMediosPago
    Private clObservaciones As String
    Private clFormaPagoId As typFormasPago
    Private clBancoId As typBancos
    Private clTarjetaCreditoId As typTarjetasCredito
    Private clCobradorId As typCobradores
    Private clTipoAplicacion As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
        End Set
    End Property
    Public Property IntegracionId() As iteMediosPago
        Get
            Return clIntegracionId
        End Get
        Set(ByVal value As iteMediosPago)
            clIntegracionId = value
        End Set
    End Property
    Public Property Observaciones() As String
        Get
            Return clObservaciones
        End Get
        Set(ByVal value As String)
            clObservaciones = value
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
    Public Property TarjetaCreditoId() As typTarjetasCredito
        Get
            Return Me.GetTypProperty(clTarjetaCreditoId)
        End Get
        Set(ByVal value As typTarjetasCredito)
            clTarjetaCreditoId = value
        End Set
    End Property
    Public Property CobradorId() As typCobradores
        Get
            Return Me.GetTypProperty(clCobradorId)
        End Get
        Set(ByVal value As typCobradores)
            clCobradorId = value
        End Set
    End Property
    Public Property TipoAplicacion() As Integer
        Get
            Return clTipoAplicacion
        End Get
        Set(ByVal value As Integer)
            clTipoAplicacion = value
        End Set
    End Property

End Class
