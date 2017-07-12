Public Class typConfiguracionesRegionales
    Inherits typAllGenerico00
    Private clmodDomicilio As Integer
    Private clflgInterfaz As Integer
    Private clClienteId As typClientes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property modDomicilio() As Integer
        Get
            Return clmodDomicilio
        End Get
        Set(ByVal value As Integer)
            clmodDomicilio = value
        End Set
    End Property
    Public Property flgInterfaz() As Integer
        Get
            Return clflgInterfaz
        End Get
        Set(ByVal value As Integer)
            clflgInterfaz = value
        End Set
    End Property
    Public Property ClienteId() As typClientes
        Get
            Return Me.GetTypProperty(clClienteId)
        End Get
        Set(ByVal value As typClientes)
            clClienteId = value
        End Set
    End Property

End Class
