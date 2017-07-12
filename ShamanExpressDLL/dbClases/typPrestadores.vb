Public Class typPrestadores
    Inherits typAll
    Private clTipoPrestador As Integer
    Private clAbreviaturaId As String
    Private clRazonSocial As String
    Private clDomicilio As usrDomicilio
    Private clLocalidadId As typLocalidades
    Private clCodigoPostal As String
    Private clCUIT As String
    Private clSituacionIvaId As typSituacionesIva
    Private clAlicuotaIvaId As typAlicuotasIva
    Private clWeb As String
    Private clClienteId As typClientes
    Private clObservaciones As String
    Private clActivo As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TipoPrestador() As Integer
        Get
            Return clTipoPrestador
        End Get
        Set(ByVal value As Integer)
            clTipoPrestador = value
        End Set
    End Property
    Public Property AbreviaturaId() As String
        Get
            Return clAbreviaturaId
        End Get
        Set(ByVal value As String)
            clAbreviaturaId = value
        End Set
    End Property
    Public Property RazonSocial() As String
        Get
            Return clRazonSocial
        End Get
        Set(ByVal value As String)
            clRazonSocial = value
        End Set
    End Property
    Public Property Domicilio() As usrDomicilio
        Get
            Return clDomicilio
        End Get
        Set(ByVal value As usrDomicilio)
            clDomicilio = value
        End Set
    End Property
    Public Property LocalidadId() As typLocalidades
        Get
            Return Me.GetTypProperty(clLocalidadId)
        End Get
        Set(ByVal value As typLocalidades)
            clLocalidadId = value
        End Set
    End Property
    Public Property CodigoPostal() As String
        Get
            Return clCodigoPostal
        End Get
        Set(ByVal value As String)
            clCodigoPostal = value
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
    Public Property SituacionIvaId() As typSituacionesIva
        Get
            Return Me.GetTypProperty(clSituacionIvaId)
        End Get
        Set(ByVal value As typSituacionesIva)
            clSituacionIvaId = value
        End Set
    End Property
    Public Property AlicuotaIvaId() As typAlicuotasIva
        Get
            Return Me.GetTypProperty(clAlicuotaIvaId)
        End Get
        Set(ByVal value As typAlicuotasIva)
            clAlicuotaIvaId = value
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
    Public Property Web() As String
        Get
            Return clWeb
        End Get
        Set(ByVal value As String)
            clWeb = value
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
    Public Property Activo() As Integer
        Get
            Return clActivo
        End Get
        Set(ByVal value As Integer)
            clActivo = value
        End Set
    End Property
End Class
