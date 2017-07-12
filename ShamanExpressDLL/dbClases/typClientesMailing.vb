Public Class typClientesMailing
    Inherits typAll
    Private clClienteContactoId As typClientesContactos
    Private clMailingAccionId As accMailingClientes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ClienteContactoId() As typClientesContactos
        Get
            Return Me.GetTypProperty(clClienteContactoId)
        End Get
        Set(ByVal value As typClientesContactos)
            clClienteContactoId = value
        End Set
    End Property
    Public Property MailingAccionId() As accMailingClientes
        Get
            Return clMailingAccionId
        End Get
        Set(ByVal value As accMailingClientes)
            clMailingAccionId = value
        End Set
    End Property
End Class
