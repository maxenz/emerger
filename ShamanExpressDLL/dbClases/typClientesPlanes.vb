Public Class typClientesPlanes
    Inherits typAll
    Private clClienteId As typClientes
    Private clPlanId As typPlanes
    Private clflgPrincipal As Integer
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
    Public Property PlanId() As typPlanes
        Get
            Return Me.GetTypProperty(clPlanId)
        End Get
        Set(ByVal value As typPlanes)
            clPlanId = value
        End Set
    End Property
    Public Property flgPrincipal() As Integer
        Get
            Return clflgPrincipal
        End Get
        Set(ByVal value As Integer)
            clflgPrincipal = value
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
