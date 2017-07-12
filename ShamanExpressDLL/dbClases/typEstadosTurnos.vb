Public Class typEstadosTurnos
    Inherits typAllGenerico01
    Private clNroOrden As Integer
    Private clVisualColor As String
    Private clflgDisponible As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property NroOrden() As Integer
        Get
            Return clNroOrden
        End Get
        Set(ByVal value As Integer)
            clNroOrden = value
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
    Public Property flgDisponible() As Integer
        Get
            Return clflgDisponible
        End Get
        Set(ByVal value As Integer)
            clflgDisponible = value
        End Set
    End Property

End Class
