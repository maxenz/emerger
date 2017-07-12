Public Class typUsuariosLogueados
    Inherits typAll
    Private clPID As Int64
    Private clUsuarioId As typUsuarios
    Private clTerminalId As typTerminales
    Private clHKeyPID As Int64
    Private clFechaHoraInicio As Date
    Private clAgenteId As String
    Private clPerfilId As typPerfiles
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
    Public Property UsuarioId() As typUsuarios
        Get
            Return Me.GetTypProperty(clUsuarioId)
        End Get
        Set(ByVal value As typUsuarios)
            clUsuarioId = value
        End Set
    End Property
    Public Property TerminalId() As typTerminales
        Get
            Return Me.GetTypProperty(clTerminalId)
        End Get
        Set(ByVal value As typTerminales)
            clTerminalId = value
        End Set
    End Property
    Public Property HKeyPID() As Int64
        Get
            Return clHKeyPID
        End Get
        Set(ByVal value As Int64)
            clHKeyPID = value
        End Set
    End Property
    Public Property FechaHoraInicio() As Date
        Get
            Return clFechaHoraInicio
        End Get
        Set(ByVal value As Date)
            clFechaHoraInicio = value
        End Set
    End Property
    Public Property AgenteId() As String
        Get
            Return clAgenteId
        End Get
        Set(ByVal value As String)
            clAgenteId = value
        End Set
    End Property
    Public Property PerfilId() As typPerfiles
        Get
            Return Me.GetTypProperty(clPerfilId)
        End Get
        Set(ByVal value As typPerfiles)
            clPerfilId = value
        End Set
    End Property


End Class
