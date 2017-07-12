Public Class typtmpClientesIntegrantesInfoPaciente
    Inherits typIntegranteInfoPaciente
    Private cltmpClienteIntegranteId As typtmpClientesIntegrantes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property tmpClienteIntegranteId() As typtmpClientesIntegrantes
        Get
            Return Me.GetTypProperty(cltmpClienteIntegranteId)
        End Get
        Set(ByVal value As typtmpClientesIntegrantes)
            cltmpClienteIntegranteId = value
        End Set
    End Property
End Class
