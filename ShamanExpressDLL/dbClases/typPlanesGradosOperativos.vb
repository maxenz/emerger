Public Class typPlanesGradosOperativos
    Inherits typAll
    Private clPlanId As typPlanes
    Private clGradoOperativoId As typGradosOperativos
    Private clflgCubierto As Integer
    Private clCoPago As Decimal
    Private clflgPurge As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PlanId() As typPlanes
        Get
            Return Me.GetTypProperty(clPlanId)
        End Get
        Set(ByVal value As typPlanes)
            clPlanId = value
        End Set
    End Property
    Public Property GradoOperativoId() As typGradosOperativos
        Get
            Return Me.GetTypProperty(clGradoOperativoId)
        End Get
        Set(ByVal value As typGradosOperativos)
            clGradoOperativoId = value
        End Set
    End Property
    Public Property flgCubierto() As Integer
        Get
            Return clflgCubierto
        End Get
        Set(ByVal value As Integer)
            clflgCubierto = value
        End Set
    End Property
    Public Property CoPago() As Decimal
        Get
            Return clCoPago
        End Get
        Set(ByVal value As Decimal)
            clCoPago = value
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
