Public Class typPartidos
    Inherits typAllGenerico01
    Private clProvinciaId As typProvincias
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ProvinciaId() As typProvincias
        Get
            Return Me.GetTypProperty(clProvinciaId)
        End Get
        Set(ByVal value As typProvincias)
            clProvinciaId = value
        End Set
    End Property
    Public Overrides ReadOnly Property Tabla() As String
        Get
            Return "Localidades"
        End Get
    End Property
End Class
