Public Class typClientesIntegrantesInfoPaciente
    Inherits typIntegranteInfoPaciente
    Private clClienteIntegranteId As typClientesIntegrantes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ClienteIntegranteId() As typClientesIntegrantes
        Get
            Return Me.GetTypProperty(clClienteIntegranteId)
        End Get
        Set(ByVal value As typClientesIntegrantes)
            clClienteIntegranteId = value
        End Set
    End Property
End Class
