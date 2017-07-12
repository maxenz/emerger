Public Class typMediosPagoIntegracionesAtr
    Inherits typAll
    Private clMedioPagoIntegracionId As typMediosPagoIntegraciones
    Private clAtributo As String
    Private clValor As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property MedioPagoIntegracionId() As typMediosPagoIntegraciones
        Get
            Return Me.GetTypProperty(clMedioPagoIntegracionId)
        End Get
        Set(ByVal value As typMediosPagoIntegraciones)
            clMedioPagoIntegracionId = value
        End Set
    End Property
    Public Property Atributo() As String
        Get
            Return clAtributo
        End Get
        Set(ByVal value As String)
            clAtributo = value
        End Set
    End Property
    Public Property Valor() As String
        Get
            Return clValor
        End Get
        Set(ByVal value As String)
            clValor = value
        End Set
    End Property
End Class
