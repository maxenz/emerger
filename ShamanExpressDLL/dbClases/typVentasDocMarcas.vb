Public Class typVentasDocMarcas
    Inherits typAll
    Private clVentaDocumentoId As typVentasDocumentos
    Private clMarcaId As marDocumentos
    Private clValor As Int64
    Private clReferencia1 As String
    Private clReferencia2 As String
    Private clReferencia3 As String
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
    Public Property MarcaId() As marDocumentos
        Get
            Return clMarcaId
        End Get
        Set(ByVal value As marDocumentos)
            clMarcaId = value
        End Set
    End Property
    Public Property Valor() As Int64
        Get
            Return clValor
        End Get
        Set(ByVal value As Int64)
            clValor = value
        End Set
    End Property
    Public Property Referencia1() As String
        Get
            Return clReferencia1
        End Get
        Set(ByVal value As String)
            clReferencia1 = value
        End Set
    End Property
    Public Property Referencia2() As String
        Get
            Return clReferencia2
        End Get
        Set(ByVal value As String)
            clReferencia2 = value
        End Set
    End Property
    Public Property Referencia3() As String
        Get
            Return clReferencia3
        End Get
        Set(ByVal value As String)
            clReferencia3 = value
        End Set
    End Property

End Class
