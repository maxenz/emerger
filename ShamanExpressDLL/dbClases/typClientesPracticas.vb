Public Class typClientesPracticas
    Inherits typAll
    Private clClienteId As typClientes
    Private clClientePlanInternoId As typPlanes
    Private clPracticaId As typPracticas
    Private clflgCubierto As Integer
    Private clCoPago As Decimal
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
    Public Property ClientePlanInternoId() As typPlanes
        Get
            Return Me.GetTypProperty(clClientePlanInternoId)
        End Get
        Set(ByVal value As typPlanes)
            clClientePlanInternoId = value
        End Set
    End Property
    Public Property PracticaId() As typPracticas
        Get
            Return Me.GetTypProperty(clPracticaId)
        End Get
        Set(ByVal value As typPracticas)
            clPracticaId = value
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

End Class
