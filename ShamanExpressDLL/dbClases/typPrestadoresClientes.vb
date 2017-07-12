Public Class typPrestadoresClientes
    Inherits typAll
    Private clPrestadorId As typPrestadores
    Private clClienteId As typClientes
    Private clTipoMovilId As typTiposMoviles
    Private clflgCoberturaPropia As Integer
    Private clCantidad As Int64
    Private clImporte As Decimal
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PrestadorId() As typPrestadores
        Get
            Return Me.GetTypProperty(clPrestadorId)
        End Get
        Set(ByVal value As typPrestadores)
            clPrestadorId = value
        End Set
    End Property
    Public Property ClienteId() As typClientes
        Get
            Return Me.GetTypProperty(clClienteId)
        End Get
        Set(ByVal value As typClientes)
            clClienteId = value
        End Set
    End Property
    Public Property TipoMovilId() As typTiposMoviles
        Get
            Return Me.GetTypProperty(clTipoMovilId)
        End Get
        Set(ByVal value As typTiposMoviles)
            clTipoMovilId = value
        End Set
    End Property
    Public Property flgCoberturaPropia() As Integer
        Get
            Return clflgCoberturaPropia
        End Get
        Set(ByVal value As Integer)
            clflgCoberturaPropia = value
        End Set
    End Property
    Public Property Cantidad() As Int64
        Get
            Return clCantidad
        End Get
        Set(ByVal value As Int64)
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
    Public Property Observaciones() As String
        Get
            Return clObservaciones
        End Get
        Set(ByVal value As String)
            clObservaciones = value
        End Set
    End Property

End Class
