Public Class typIntegrante
    Inherits typAll
    Private clTipoIntegrante As String
    Private clIntegranteClasificacionId As typIntegrantesClasificaciones
    Private clNroAfiliado As String
    Private clApellido As String
    Private clNombre As String
    Private clTipoDocumentoId As typTiposDocumentos
    Private clNroDocumento As Int64
    Private clFecNacimiento As Date
    Private clSexo As String
    Private clDomicilio As usrDomicilio
    Private clLocalidadId As typLocalidades
    Private clCodigoPostal As String
    Private clTelefono01 As String
    Private clTelefono02 As String
    Private clTelefono01Fix As Int64
    Private clTelefono02Fix As Int64
    Private clFecIngreso As Date
    Private clFecEgreso As Date
    Private cleduIntegranteCursoId As typIntegrantesCursos
    Private cleduDivision As String
    Private cleduTurno As String
    Private clbarBanderaBarcoId As typBanderasBarcos
    Private clbarNroOficial As String
    Private clbarNroIMO As String
    Private clbarTipoBarcoId As typTiposBarcos
    Private clflgIvaGravado As Integer
    Private clCoberturaIndividualId As typCoberturasIndividuales
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TipoIntegrante() As String
        Get
            Return clTipoIntegrante
        End Get
        Set(ByVal value As String)
            clTipoIntegrante = value
        End Set
    End Property
    Public Property IntegranteClasificacionId() As typIntegrantesClasificaciones
        Get
            Return Me.GetTypProperty(clIntegranteClasificacionId)
        End Get
        Set(ByVal value As typIntegrantesClasificaciones)
            clIntegranteClasificacionId = value
        End Set
    End Property
    Public Property NroAfiliado() As String
        Get
            Return clNroAfiliado
        End Get
        Set(ByVal value As String)
            clNroAfiliado = value
        End Set
    End Property
    Public Property Apellido() As String
        Get
            Return clApellido
        End Get
        Set(ByVal value As String)
            clApellido = value
        End Set
    End Property
    Public Property Nombre() As String
        Get
            Return clNombre
        End Get
        Set(ByVal value As String)
            clNombre = value
        End Set
    End Property
    Public Property TipoDocumentoId() As typTiposDocumentos
        Get
            Return Me.GetTypProperty(clTipoDocumentoId)
        End Get
        Set(ByVal value As typTiposDocumentos)
            clTipoDocumentoId = value
        End Set
    End Property
    Public Property NroDocumento() As Int64
        Get
            Return clNroDocumento
        End Get
        Set(ByVal value As Int64)
            clNroDocumento = value
        End Set
    End Property
    Public Property FecNacimiento() As Date
        Get
            Return clFecNacimiento
        End Get
        Set(ByVal value As Date)
            clFecNacimiento = value
        End Set
    End Property
    Public Property Sexo() As String
        Get
            Return clSexo
        End Get
        Set(ByVal value As String)
            clSexo = value
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
    Public Property Telefono01() As String
        Get
            Return clTelefono01
        End Get
        Set(ByVal value As String)
            clTelefono01 = value
        End Set
    End Property
    Public Property Telefono02() As String
        Get
            Return clTelefono02
        End Get
        Set(ByVal value As String)
            clTelefono02 = value
        End Set
    End Property
    Public Property Telefono01Fix() As Int64
        Get
            Return clTelefono01Fix
        End Get
        Set(ByVal value As Int64)
            clTelefono01Fix = value
        End Set
    End Property
    Public Property Telefono02Fix() As Int64
        Get
            Return clTelefono02Fix
        End Get
        Set(ByVal value As Int64)
            clTelefono02Fix = value
        End Set
    End Property
    Public Property FecIngreso() As Date
        Get
            Return clFecIngreso
        End Get
        Set(ByVal value As Date)
            clFecIngreso = value
        End Set
    End Property
    Public Property FecEgreso() As Date
        Get
            Return clFecEgreso
        End Get
        Set(ByVal value As Date)
            clFecEgreso = value
        End Set
    End Property
    Public Property eduIntegranteCursoId() As typIntegrantesCursos
        Get
            Return Me.GetTypProperty(cleduIntegranteCursoId)
        End Get
        Set(ByVal value As typIntegrantesCursos)
            cleduIntegranteCursoId = value
        End Set
    End Property
    Public Property eduDivision() As String
        Get
            Return cleduDivision
        End Get
        Set(ByVal value As String)
            cleduDivision = value
        End Set
    End Property
    Public Property eduTurno() As String
        Get
            Return cleduTurno
        End Get
        Set(ByVal value As String)
            cleduTurno = value
        End Set
    End Property
    Public Property barBanderaBarcoId() As typBanderasBarcos
        Get
            Return Me.GetTypProperty(clbarBanderaBarcoId)
        End Get
        Set(ByVal value As typBanderasBarcos)
            clbarBanderaBarcoId = value
        End Set
    End Property
    Public Property barNroOficial() As String
        Get
            Return clbarNroOficial
        End Get
        Set(ByVal value As String)
            clbarNroOficial = value
        End Set
    End Property
    Public Property barNroIMO() As String
        Get
            Return clbarNroIMO
        End Get
        Set(ByVal value As String)
            clbarNroIMO = value
        End Set
    End Property
    Public Property barTipoBarcoId() As typTiposBarcos
        Get
            Return Me.GetTypProperty(clbarTipoBarcoId)
        End Get
        Set(ByVal value As typTiposBarcos)
            clbarTipoBarcoId = value
        End Set
    End Property
    Public Property flgIvaGravado() As Integer
        Get
            Return clflgIvaGravado
        End Get
        Set(ByVal value As Integer)
            clflgIvaGravado = value
        End Set
    End Property
    Public Property CoberturaIndividualId() As typCoberturasIndividuales
        Get
            Return Me.GetTypProperty(clCoberturaIndividualId)
        End Get
        Set(ByVal value As typCoberturasIndividuales)
            clCoberturaIndividualId = value
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
