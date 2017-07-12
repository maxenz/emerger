Public Class typInsumosStock
    Inherits typAll
    Private clInsumoId As typInsumos
    Private clCentroAtencionId As typCentrosAtencion
    Private clStockActual As Decimal
    Private clStockOptimo As Decimal
    Private clStockMinimo As Decimal
    Private clStockMaximo As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property InsumoId() As typInsumos
        Get
            Return Me.GetTypProperty(clInsumoId)
        End Get
        Set(ByVal value As typInsumos)
            clInsumoId = value
        End Set
    End Property
    Public Property CentroAtencionId() As typCentrosAtencion
        Get
            Return Me.GetTypProperty(clCentroAtencionId)
        End Get
        Set(ByVal value As typCentrosAtencion)
            clCentroAtencionId = value
        End Set
    End Property
    Public Property StockActual() As Decimal
        Get
            Return clStockActual
        End Get
        Set(ByVal value As Decimal)
            clStockActual = value
        End Set
    End Property
    Public Property StockOptimo() As Decimal
        Get
            Return clStockOptimo
        End Get
        Set(ByVal value As Decimal)
            clStockOptimo = value
        End Set
    End Property
    Public Property StockMinimo() As Decimal
        Get
            Return clStockMinimo
        End Get
        Set(ByVal value As Decimal)
            clStockMinimo = value
        End Set
    End Property
    Public Property StockMaximo() As Decimal
        Get
            Return clStockMaximo
        End Get
        Set(ByVal value As Decimal)
            clStockMaximo = value
        End Set
    End Property

End Class
