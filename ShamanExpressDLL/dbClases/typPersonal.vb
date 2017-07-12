Public Class typPersonal
    Inherits typAll
    Private clLegajo As String
    Private clApellido As String
    Private clNombre As String
    Private clSexo As Integer
    Private clDomicilio As usrDomicilio
    Private clLocalidadId As typLocalidades
    Private clCodigoPostal As String
    Private clTelefono01 As String
    Private clTelefono02 As String
    Private clTelefono03 As String
    Private clEmail As String
    Private clFecNacimiento As Date
    Private clTipoDocumentoId As typTiposDocumentos
    Private clNroDocumento As Int64
    Private clNacionalidadId As typNacionalidades
    Private clEstadoCivilId As typEstadosCivil
    Private clmedMatriculaNacional As String
    Private clmedMatriculaProvincial As String
    Private clenfMatriculaTipo As String
    Private clenfMatriculaNro As String
    Private clchfRegistroCategoria As String
    Private clchfRegistroVencimiento As Date
    Private clDepartamentoPuestoId As typDepartamentosPuestos
    Private clFecIngreso As Date
    Private clFecEgreso As Date
    Private clmodDisponibilidad As Integer
    Private clActivo As Integer
    Private clflgPrestador As Integer
    Private clObservaciones As String
    Private clSintesisCurricular As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Legajo() As String
        Get
            Return clLegajo
        End Get
        Set(ByVal value As String)
            clLegajo = value
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
    Public Property Sexo() As Integer
        Get
            Return clSexo
        End Get
        Set(ByVal value As Integer)
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
    Public Property Telefono03() As String
        Get
            Return clTelefono03
        End Get
        Set(ByVal value As String)
            clTelefono03 = value
        End Set
    End Property
    Public Property Email() As String
        Get
            Return clEmail
        End Get
        Set(ByVal value As String)
            clEmail = value
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
    Public Property NacionalidadId() As typNacionalidades
        Get
            Return Me.GetTypProperty(clNacionalidadId)
        End Get
        Set(ByVal value As typNacionalidades)
            clNacionalidadId = value
        End Set
    End Property
    Public Property EstadoCivilId() As typEstadosCivil
        Get
            Return Me.GetTypProperty(clEstadoCivilId)
        End Get
        Set(ByVal value As typEstadosCivil)
            clEstadoCivilId = value
        End Set
    End Property
    Public Property medMatriculaNacional() As String
        Get
            Return clmedMatriculaNacional
        End Get
        Set(ByVal value As String)
            clmedMatriculaNacional = value
        End Set
    End Property
    Public Property medMatriculaProvincial() As String
        Get
            Return clmedMatriculaProvincial
        End Get
        Set(ByVal value As String)
            clmedMatriculaProvincial = value
        End Set
    End Property
    Public Property enfMatriculaTipo() As String
        Get
            Return clenfMatriculaTipo
        End Get
        Set(ByVal value As String)
            clenfMatriculaTipo = value
        End Set
    End Property
    Public Property enfMatriculaNro() As String
        Get
            Return clenfMatriculaNro
        End Get
        Set(ByVal value As String)
            clenfMatriculaNro = value
        End Set
    End Property
    Public Property chfRegistroCategoria() As String
        Get
            Return clchfRegistroCategoria
        End Get
        Set(ByVal value As String)
            clchfRegistroCategoria = value
        End Set
    End Property
    Public Property chfRegistroVencimiento() As Date
        Get
            Return clchfRegistroVencimiento
        End Get
        Set(ByVal value As Date)
            clchfRegistroVencimiento = value
        End Set
    End Property
    Public Property DepartamentoPuestoId() As typDepartamentosPuestos
        Get
            Return Me.GetTypProperty(clDepartamentoPuestoId)
        End Get
        Set(ByVal value As typDepartamentosPuestos)
            clDepartamentoPuestoId = value
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
    Public Property modDisponibilidad() As Integer
        Get
            Return clmodDisponibilidad
        End Get
        Set(ByVal value As Integer)
            clmodDisponibilidad = value
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
    Public Property flgPrestador() As Integer
        Get
            Return clflgPrestador
        End Get
        Set(ByVal value As Integer)
            clflgPrestador = value
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
    Public Property SintesisCurricular() As String
        Get
            Return clSintesisCurricular
        End Get
        Set(ByVal value As String)
            clSintesisCurricular = value
        End Set
    End Property

End Class
