Public Class typClientesIntegrantesPlanes
    Inherits typAll
    Private clClienteIntegranteId As typClientesIntegrantes
    Private clPlanId As typPlanes
    Private clflgPurge As Integer
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
    Public Property PlanId() As typPlanes
        Get
            Return Me.GetTypProperty(clPlanId)
        End Get
        Set(ByVal value As typPlanes)
            clPlanId = value
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
