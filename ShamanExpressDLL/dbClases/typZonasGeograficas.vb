Public Class typZonasGeograficas
    Inherits typAllGenerico01
    Private clVisualColor As String
    Private clColorHexa As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property VisualColor() As String
        Get
            Return clVisualColor
        End Get
        Set(ByVal value As String)
            clVisualColor = value
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
