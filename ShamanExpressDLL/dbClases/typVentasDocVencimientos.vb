Public Class typVentasDocVencimientos
    Inherits typAll
    Private clVentaDocumentoId As typVentasDocumentos
    Private clNroVencimiento As Integer
    Private clFecVencimiento As Date
    Private clPorcentajeRecargo As Decimal
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
    Public Property NroVencimiento() As marDocumentos
        Get
            Return clNroVencimiento
        End Get
        Set(ByVal value As marDocumentos)
            clNroVencimiento = value
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
    Public Property PorcentajeRecargo() As Decimal
        Get
            Return clPorcentajeRecargo
        End Get
        Set(ByVal value As Decimal)
            clPorcentajeRecargo = value
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
