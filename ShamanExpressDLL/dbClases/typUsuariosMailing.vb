Public Class typUsuariosMailing
    Inherits typAll
    Private clUsuarioId As typUsuarios
    Private clMailingAccionId As accMailingUsuarios
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
    Public Property MailingAccionId() As accMailingUsuarios
        Get
            Return clMailingAccionId
        End Get
        Set(ByVal value As accMailingUsuarios)
            clMailingAccionId = value
        End Set
    End Property
End Class
