Public Class typVentasDocImputaciones
    Inherits typAll
    Private clVentaDocumentoId As typVentasDocumentos
    Private clVentaDocImputadoId As typVentasDocumentos
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
    Public Property VentaDocImputadoId() As typVentasDocumentos
        Get
            Return Me.GetTypProperty(clVentaDocImputadoId)
        End Get
        Set(ByVal value As typVentasDocumentos)
            clVentaDocImputadoId = value
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
