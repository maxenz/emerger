Public Class typSucesosIncidentes
    Inherits typAllGenerico01
    Private clTipoAplicacion As String
    Private clOrden As Integer
    Private clVisualColor As String
    Private clValorGrilla As String
    Private clColorHexa As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TipoAplicacion() As String
        Get
            Return clTipoAplicacion
        End Get
        Set(ByVal value As String)
            clTipoAplicacion = value
        End Set
    End Property
    Public Property Orden() As Integer
        Get
            Return clOrden
        End Get
        Set(ByVal value As Integer)
            clOrden = value
        End Set
    End Property
    Public Property VisualColor() As String
        Get
            Return clVisualColor
        End Get
        Set(ByVal value As String)
            clVisualColor = value
        End Set
    End Property
    Public Property ValorGrilla() As String
        Get
            Return clValorGrilla
        End Get
        Set(ByVal value As String)
            clValorGrilla = value
        End Set
    End Property
    Public Property ColorHexa() As String
        Get
            Return clColorHexa
        End Get
        Set(ByVal value As String)
            clColorHexa = value
        End Set
    End Property

End Class
