Public Class typAptoFisicoExamenes
    Inherits typAll
    Private clClienteId As typClientes
    Private clClienteIntegranteId As typClientesIntegrantes
    Private clNroDocumento As Int64
    Private clApellido As String
    Private clNombre As String
    Private cleduIntegranteCursoId As typIntegrantesCursos
    Private cleduDivision As String
    Private cleduTurno As String
    Private clFecNacimiento As Date
    Private clFecExamen As Date
    Private clLugar As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ClienteId() As typClientes
        Get
            Return Me.GetTypProperty(clClienteId)
        End Get
        Set(ByVal value As typClientes)
            clClienteId = value
        End Set
    End Property
    Public Property ClienteIntegranteId() As typClientesIntegrantes
        Get
            Return Me.GetTypProperty(clClienteIntegranteId)
        End Get
        Set(ByVal value As typClientesIntegrantes)
            clClienteIntegranteId = value
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
    Public Property FecNacimiento() As Date
        Get
            Return clFecNacimiento
        End Get
        Set(ByVal value As Date)
            clFecNacimiento = value
        End Set
    End Property
    Public Property FecExamen() As Date
        Get
            Return clFecExamen
        End Get
        Set(ByVal value As Date)
            clFecExamen = value
        End Set
    End Property
    Public Property Lugar() As String
        Get
            Return clLugar
        End Get
        Set(ByVal value As String)
            clLugar = value
        End Set
    End Property

End Class
