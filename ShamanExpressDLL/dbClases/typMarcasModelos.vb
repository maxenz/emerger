Public Class typMarcasModelos
    Inherits typAll
    Private clMarca As String
    Private clModelo As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Marca() As String
        Get
            Return clMarca
        End Get
        Set(ByVal value As String)
            clMarca = value
        End Set
    End Property
    Public Property Modelo() As String
        Get
            Return clModelo
        End Get
        Set(ByVal value As String)
            clModelo = value
        End Set
    End Property
End Class
