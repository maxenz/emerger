Public Class typClientesIntegrantes
    Inherits typIntegrante
    Private clClienteId As typClientes
    Private clCliIntegranteSubGrupoId As typClientesIntegrantes
    Private clflgPurge As Integer
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
    Public Property CliIntegranteSubGrupoId() As typClientesIntegrantes
        Get
            Return Me.GetTypProperty(clCliIntegranteSubGrupoId)
        End Get
        Set(ByVal value As typClientesIntegrantes)
            clCliIntegranteSubGrupoId = value
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
