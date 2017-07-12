Public Class typConfiguracionesRegionalesReglas
    Inherits typAll
    Private clConfiguracionRegionalId As typConfiguracionesRegionales
    Private clTipoConfiguracion As Integer
    Private clValor1 As String
    Private clValor2 As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ConfiguracionRegionalId() As typConfiguracionesRegionales
        Get
            Return Me.GetTypProperty(clConfiguracionRegionalId)
        End Get
        Set(ByVal value As typConfiguracionesRegionales)
            clConfiguracionRegionalId = value
        End Set
    End Property
    Public Property TipoConfiguracion() As Integer
        Get
            Return clTipoConfiguracion
        End Get
        Set(ByVal value As Integer)
            clTipoConfiguracion = value
        End Set
    End Property
    Public Property Valor1() As String
        Get
            Return clValor1
        End Get
        Set(ByVal value As String)
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
