Public Class typtmpClientesIntegrantes
    Inherits typIntegrante
    Private clPID As Int64
    Private clCliIntegranteSubGrupoId As typtmpClientesIntegrantes
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
    Public Property CliIntegranteSubGrupoId() As typtmpClientesIntegrantes
        Get
            Return Me.GetTypProperty(clCliIntegranteSubGrupoId)
        End Get
        Set(ByVal value As typtmpClientesIntegrantes)
            clCliIntegranteSubGrupoId = value
        End Set
    End Property
End Class
