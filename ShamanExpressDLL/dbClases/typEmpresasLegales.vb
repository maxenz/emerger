Public Class typEmpresasLegales
    Inherits typAll
    Private clAbreviaturaId As String
    Private clDescripcion As String
    Private clCUIT As String
    Private clDomicilio As String
    Private clFecInicio As Date
    Private clNroIngresosBrutos As String
    Private clcaeCertificado As String
    Private clcaePassword As String
    Private clflgDefault As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property AbreviaturaId() As String
        Get
            Return clAbreviaturaId
        End Get
        Set(ByVal value As String)
            clAbreviaturaId = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
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
    Public Property Domicilio() As String
        Get
            Return clDomicilio
        End Get
        Set(ByVal value As String)
            clDomicilio = value
        End Set
    End Property
    Public Property FecInicio() As Date
        Get
            Return clFecInicio
        End Get
        Set(ByVal value As Date)
            clFecInicio = value
        End Set
    End Property
    Public Property NroIngresosBrutos() As String
        Get
            Return clNroIngresosBrutos
        End Get
        Set(ByVal value As String)
            clNroIngresosBrutos = value
        End Set
    End Property
    Public Property caeCertificado() As String
        Get
            Return clcaeCertificado
        End Get
        Set(ByVal value As String)
            clcaeCertificado = value
        End Set
    End Property
    Public Property caePassword() As String
        Get
            Return clcaePassword
        End Get
        Set(ByVal value As String)
            clcaePassword = value
        End Set
    End Property
    Public Property flgDefault() As Integer
        Get
            Return clflgDefault
        End Get
        Set(ByVal value As Integer)
            clflgDefault = value
        End Set
    End Property

End Class
