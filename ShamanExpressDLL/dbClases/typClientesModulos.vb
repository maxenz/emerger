Public Class typClientesModulos
    Inherits typAll
    Private clClienteId As typClientes
    Private clNroModulo As Integer
    Private clCantidad As Integer
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
    Public Property NroModulo() As Integer
        Get
            Return clNroModulo
        End Get
        Set(ByVal value As Integer)
            clNroModulo = value
        End Set
    End Property
    Public Property Cantidad() As Integer
        Get
            Return clCantidad
        End Get
        Set(ByVal value As Integer)
            clCantidad = value
        End Set
    End Property
End Class
