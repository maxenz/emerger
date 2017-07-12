Public Class typIntegranteAtributo
    Inherits typAll
    Private clAtributoDinamicoId As typAtributosDinamicos
    Private clValor1 As Decimal
    Private clValor2 As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property AtributoDinamicoId() As typAtributosDinamicos
        Get
            Return Me.GetTypProperty(clAtributoDinamicoId)
        End Get
        Set(ByVal value As typAtributosDinamicos)
            clAtributoDinamicoId = value
        End Set
    End Property
    Public Property Valor1() As Decimal
        Get
            Return clValor1
        End Get
        Set(ByVal value As Decimal)
            clValor1 = value
        End Set
    End Property
    Public Property Valor2() As String
        Get
            Return clValor2
        End Get
        Set(ByVal value As String)
            clValor2 = value
        End Set
    End Property

End Class
