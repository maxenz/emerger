Public Class typClientesTarifas
    Inherits typAll
    Private clClienteId As typClientes
    Private clTarifaId As typTarifas
    Private claplExcedente As Integer
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
    Public Property TarifaId() As typTarifas
        Get
            Return Me.GetTypProperty(clTarifaId)
        End Get
        Set(ByVal value As typTarifas)
            clTarifaId = value
        End Set
    End Property
    Public Property aplExcedente() As Integer
        Get
            Return claplExcedente
        End Get
        Set(ByVal value As Integer)
            claplExcedente = value
        End Set
    End Property
End Class
