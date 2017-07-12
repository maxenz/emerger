Public Class typTemplates
    Inherits typAll
    Private clReporteadorId As typReporteadores
    Private clPropietarioId As typUsuarios
    Private clflgPublico As Integer
    Private clTitulo As String
    Private clEstilo As Integer
    Private clflgImportacion As Integer
    Private clClienteId As typClientes

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Property ReporteadorId() As typReporteadores
        Get
            Return Me.GetTypProperty(clReporteadorId)
        End Get
        Set(ByVal value As typReporteadores)
            clReporteadorId = value
        End Set
    End Property

    Public Property PropietarioId() As typUsuarios
        Get
            Return Me.GetTypProperty(clPropietarioId)
        End Get
        Set(ByVal value As typUsuarios)
            clPropietarioId = value
        End Set
    End Property

    Public Property flgPublico() As Integer
        Get
            Return clflgPublico
        End Get
        Set(ByVal value As Integer)
            clflgPublico = value
        End Set
    End Property

    Public Property Titulo() As String
        Get
            Return clTitulo
        End Get
        Set(ByVal value As String)
            clTitulo = value
        End Set
    End Property

    Public Property Estilo() As Integer
        Get
            Return clEstilo
        End Get
        Set(ByVal value As Integer)
            clEstilo = value
        End Set
    End Property

    Public Property flgImportacion() As Integer
        Get
            Return clflgImportacion
        End Get
        Set(ByVal value As Integer)
            clflgImportacion = value
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

End Class
