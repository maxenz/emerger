Public Class typClientesBonificaciones
    Inherits typAll
    Private clClienteId As typClientes
    Private clflgRecargo As Integer
    Private clMes As Integer
    Private clPorcentaje As Decimal
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
    Public Property flgRecargo() As Integer
        Get
            Return clflgRecargo
        End Get
        Set(ByVal value As Integer)
            clflgRecargo = value
        End Set
    End Property
    Public Property Mes() As Integer
        Get
            Return clMes
        End Get
        Set(ByVal value As Integer)
            clMes = value
        End Set
    End Property
    Public Property Porcentaje() As Decimal
        Get
            Return clPorcentaje
        End Get
        Set(ByVal value As Decimal)
            clPorcentaje = value
        End Set
    End Property

End Class
