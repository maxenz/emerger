Public Class typUsuariosPreferencias
    Inherits typAll

    Private clUsuarioId As typUsuarios
    Private clHojaEstilo As String

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Property UsuarioId() As typUsuarios
        Get
            Return clUsuarioId
        End Get
        Set(ByVal value As typUsuarios)
            clUsuarioId = value
        End Set
    End Property

    Public Property HojaEstilo() As String
        Get
            Return clHojaEstilo
        End Get
        Set(ByVal value As String)
            clHojaEstilo = value
        End Set
    End Property

End Class

