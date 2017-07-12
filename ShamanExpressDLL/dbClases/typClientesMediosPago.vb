Public Class typClientesMediosPago
    Inherits typAll
    Private clClienteId As typClientes
    Private clClasificacionId As Integer
    Private clBancoId As typBancos
    Private clTarjetaCreditoId As typTarjetasCredito
    Private clNroCuenta As String
    Private clVencimiento As Date
    Private clNroSeguridad As String
    Private clTipoCuentaId As typTiposCuentas
    Private clflgPropia As Integer
    Private clTitular As String
    Private clCUIT As String
    Private clflgPredeterminada As Integer
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
    Public Property ClasificacionId() As Integer
        Get
            Return clClasificacionId
        End Get
        Set(ByVal value As Integer)
            clClasificacionId = value
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
    Public Property NroCuenta() As String
        Get
            Return clNroCuenta
        End Get
        Set(ByVal value As String)
            clNroCuenta = value
        End Set
    End Property
    Public Property Vencimiento() As Date
        Get
            Return clVencimiento
        End Get
        Set(ByVal value As Date)
            clVencimiento = value
        End Set
    End Property
    Public Property NroSeguridad() As String
        Get
            Return clNroSeguridad
        End Get
        Set(ByVal value As String)
            clNroSeguridad = value
        End Set
    End Property
    Public Property TipoCuentaId() As typTiposCuentas
        Get
            Return Me.GetTypProperty(clTipoCuentaId)
        End Get
        Set(ByVal value As typTiposCuentas)
            clTipoCuentaId = value
        End Set
    End Property
    Public Property flgPropia() As Integer
        Get
            Return clflgPropia
        End Get
        Set(ByVal value As Integer)
            clflgPropia = value
        End Set
    End Property
    Public Property Titular() As String
        Get
            Return clTitular
        End Get
        Set(ByVal value As String)
            clTitular = value
        End Set
    End Property
    Public Property CUIT() As String
        Get
            Return clCUIT
        End Get
        Set(ByVal value As String)
            clCUIT = value
        End Set
    End Property
    Public Property flgPredeterminada() As Integer
        Get
            Return clflgPredeterminada
        End Get
        Set(ByVal value As Integer)
            clflgPredeterminada = value
        End Set
    End Property
End Class
