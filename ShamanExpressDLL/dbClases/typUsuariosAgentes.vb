Public Class typUsuariosAgentes
    Inherits typAll
    Private clUsuarioId As typUsuarios
    Private clAgenteId As String
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
    Public Property AgenteId() As String
        Get
            Return clAgenteId
        End Get
        Set(ByVal value As String)
            clAgenteId = value
        End Set
    End Property

End Class
