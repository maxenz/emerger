Public Class typTalonarios
    Inherits typAll
    Private clNroTalonario As Int64
    Private clEmpresaLegalId As typEmpresasLegales
    Private clLetra As String
    Private clNroSucursal As Int64
    Private clNroInicial As Int64
    Private clNroFinal As Int64
    Private clNroSiguiente As Int64
    Private clFecVencimiento As Date
    Private clNroInterno As String
    Private clImagenDocumentoId As typImagenDocumentos
    Private clflgSubDiario As Integer
    Private clflgBarCode As Integer
    Private clTipoControl As ctrTalonarios
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property NroTalonario() As Int64
        Get
            Return clNroTalonario
        End Get
        Set(ByVal value As Int64)
            clNroTalonario = value
        End Set
    End Property
    Public Property EmpresaLegalId() As typEmpresasLegales
        Get
            Return Me.GetTypProperty(clEmpresaLegalId)
        End Get
        Set(ByVal value As typEmpresasLegales)
            clEmpresaLegalId = value
        End Set
    End Property
    Public Property Letra() As String
        Get
            Return clLetra
        End Get
        Set(ByVal value As String)
            clLetra = value
        End Set
    End Property
    Public Property NroSucursal() As Int64
        Get
            Return clNroSucursal
        End Get
        Set(ByVal value As Int64)
            clNroSucursal = value
        End Set
    End Property
    Public Property NroInicial() As Int64
        Get
            Return clNroInicial
        End Get
        Set(ByVal value As Int64)
            clNroInicial = value
        End Set
    End Property
    Public Property NroFinal() As Int64
        Get
            Return clNroFinal
        End Get
        Set(ByVal value As Int64)
            clNroFinal = value
        End Set
    End Property
    Public Property NroSiguiente() As Int64
        Get
            Return clNroSiguiente
        End Get
        Set(ByVal value As Int64)
            clNroSiguiente = value
        End Set
    End Property
    Public Property FecVencimiento() As Date
        Get
            Return clFecVencimiento
        End Get
        Set(ByVal value As Date)
            clFecVencimiento = value
        End Set
    End Property
    Public Property NroInterno() As String
        Get
            Return clNroInterno
        End Get
        Set(ByVal value As String)
            clNroInterno = value
        End Set
    End Property
    Public Property ImagenDocumentoId() As typImagenDocumentos
        Get
            Return Me.GetTypProperty(clImagenDocumentoId)
        End Get
        Set(ByVal value As typImagenDocumentos)
            clImagenDocumentoId = value
        End Set
    End Property
    Public Property flgSubDiario() As Integer
        Get
            Return clflgSubDiario
        End Get
        Set(ByVal value As Integer)
            clflgSubDiario = value
        End Set
    End Property
    Public Property flgBarCode() As Int64
        Get
            Return clflgBarCode
        End Get
        Set(ByVal value As Int64)
            clflgBarCode = value
        End Set
    End Property
    Public Property TipoControl() As ctrTalonarios
        Get
            Return clTipoControl
        End Get
        Set(ByVal value As ctrTalonarios)
            clTipoControl = value
        End Set
    End Property

End Class
