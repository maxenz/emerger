Public Class typselVentasDocCupones
    Inherits typAll
    Private clPID As Int64
    Private clNroCupon As Integer
    Private clVentaDocumento1Id As typVentasDocumentos
    Private clVentaDocumento2Id As typVentasDocumentos
    Private clVentaDocumento3Id As typVentasDocumentos
    Private clVentaDocumento4Id As typVentasDocumentos
    Private clVentaDocumento5Id As typVentasDocumentos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PID() As Int64
        Get
            Return clPID
        End Get
        Set(ByVal value As Int64)
            clPID = value
        End Set
    End Property
    Public Property NroCupon() As Integer
        Get
            Return clNroCupon
        End Get
        Set(ByVal value As Integer)
            clNroCupon = value
        End Set
    End Property
    Public Property VentaDocumento1Id() As typVentasDocumentos
        Get
            Return Me.GetTypProperty(clVentaDocumento1Id)
        End Get
        Set(ByVal value As typVentasDocumentos)
            clVentaDocumento1Id = value
        End Set
    End Property
    Public Property VentaDocumento2Id() As typVentasDocumentos
        Get
            Return Me.GetTypProperty(clVentaDocumento2Id)
        End Get
        Set(ByVal value As typVentasDocumentos)
            clVentaDocumento2Id = value
        End Set
    End Property
    Public Property VentaDocumento3Id() As typVentasDocumentos
        Get
            Return Me.GetTypProperty(clVentaDocumento3Id)
        End Get
        Set(ByVal value As typVentasDocumentos)
            clVentaDocumento3Id = value
        End Set
    End Property
    Public Property VentaDocumento4Id() As typVentasDocumentos
        Get
            Return Me.GetTypProperty(clVentaDocumento4Id)
        End Get
        Set(ByVal value As typVentasDocumentos)
            clVentaDocumento4Id = value
        End Set
    End Property
    Public Property VentaDocumento5Id() As typVentasDocumentos
        Get
            Return Me.GetTypProperty(clVentaDocumento5Id)
        End Get
        Set(ByVal value As typVentasDocumentos)
            clVentaDocumento5Id = value
        End Set
    End Property

End Class
