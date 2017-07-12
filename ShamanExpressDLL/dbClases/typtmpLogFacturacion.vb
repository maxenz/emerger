Public Class typtmpLogFacturacion
    Inherits typAll
    Private clPID As Int64
    Private clClienteId As typClientes
    Private clVentaDocumentoId As typVentasDocumentos
    Private clPeriodo As Int64
    Private clObservaciones As String
    Private clImporteAumento As Decimal
    Private clClasificacion As logClasificacion
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
    Public Property ClienteId() As typClientes
        Get
            Return Me.GetTypProperty(clClienteId)
        End Get
        Set(ByVal value As typClientes)
            clClienteId = value
        End Set
    End Property
    Public Property VentaDocumentoId() As typVentasDocumentos
        Get
            Return Me.GetTypProperty(clVentaDocumentoId)
        End Get
        Set(ByVal value As typVentasDocumentos)
            clVentaDocumentoId = value
        End Set
    End Property
    Public Property Periodo() As Int64
        Get
            Return clPeriodo
        End Get
        Set(ByVal value As Int64)
            clPeriodo = value
        End Set
    End Property
    Public Property Clasificacion() As logClasificacion
        Get
            Return clClasificacion
        End Get
        Set(ByVal value As logClasificacion)
            clClasificacion = value
        End Set
    End Property
    Public Property ImporteAumento() As Decimal
        Get
            Return clImporteAumento
        End Get
        Set(ByVal value As Decimal)
            clImporteAumento = value
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
