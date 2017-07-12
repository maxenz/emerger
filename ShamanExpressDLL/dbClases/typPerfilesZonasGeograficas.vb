Public Class typPerfilesZonasGeograficas
    Inherits typAll
    Private clPerfilId As typPerfiles
    Private clZonaGeograficaId As typZonasGeograficas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PerfilId() As typPerfiles
        Get
            Return Me.GetTypProperty(clPerfilId)
        End Get
        Set(ByVal value As typPerfiles)
            clPerfilId = value
        End Set
    End Property
    Public Property ZonaGeograficaId() As typZonasGeograficas
        Get
            Return Me.GetTypProperty(clZonaGeograficaId)
        End Get
        Set(ByVal value As typZonasGeograficas)
            clZonaGeograficaId = value
        End Set
    End Property

End Class
