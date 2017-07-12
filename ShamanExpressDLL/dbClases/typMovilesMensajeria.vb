Public Class typMovilesMensajeria
    Inherits typAll
    Private clMovilId As typMoviles
    Private clMetodoMensajeriaId As typMetodosMensajeria
    Private clrabbitAlias As String
    Private clEmail As String
    Private clflgPurge As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property MovilId() As typMoviles
        Get
            Return Me.GetTypProperty(clMovilId)
        End Get
        Set(ByVal value As typMoviles)
            clMovilId = value
        End Set
    End Property
    Public Property MetodoMensajeriaId() As typMetodosMensajeria
        Get
            Return Me.GetTypProperty(clMetodoMensajeriaId)
        End Get
        Set(ByVal value As typMetodosMensajeria)
            clMetodoMensajeriaId = value
        End Set
    End Property
    Public Property rabbitAlias() As String
        Get
            Return clrabbitAlias
        End Get
        Set(ByVal value As String)
            clrabbitAlias = value
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
    Public Property flgPurge() As Integer
        Get
            Return clflgPurge
        End Get
        Set(ByVal value As Integer)
            clflgPurge = value
        End Set
    End Property
End Class
