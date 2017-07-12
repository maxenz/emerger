Public Class typUsuariosNodos
    Inherits typAll
    Private clUsuarioId As typUsuarios
    Private clTipoRelacion As tipUsoRelacion
    Private clsysNodoId As typsysNodos
    Private clAcceso As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property UsuarioId() As typUsuarios
        Get
            Return Me.GetTypProperty(clUsuarioId)
        End Get
        Set(ByVal value As typUsuarios)
            clUsuarioId = value
        End Set
    End Property
    Public Property TipoRelacion() As tipUsoRelacion
        Get
            Return clTipoRelacion
        End Get
        Set(ByVal value As tipUsoRelacion)
            clTipoRelacion = value
        End Set
    End Property
    Public Property sysNodoId() As typsysNodos
        Get
            Return Me.GetTypProperty(clsysNodoId)
        End Get
        Set(ByVal value As typsysNodos)
            clsysNodoId = value
        End Set
    End Property
    Public Property Acceso() As Integer
        Get
            Return clAcceso
        End Get
        Set(ByVal value As Integer)
            clAcceso = value
        End Set
    End Property

End Class
