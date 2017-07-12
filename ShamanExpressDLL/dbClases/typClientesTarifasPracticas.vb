Public Class typClientesTarifasPracticas
    Inherits typAll
    Private clClienteId As typClientes
    Private clTarifaPracticaId As typTarifasPracticas
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
    Public Property TarifaPracticaId() As typTarifasPracticas
        Get
            Return Me.GetTypProperty(clTarifaPracticaId)
        End Get
        Set(ByVal value As typTarifasPracticas)
            clTarifaPracticaId = value
        End Set
    End Property

End Class
