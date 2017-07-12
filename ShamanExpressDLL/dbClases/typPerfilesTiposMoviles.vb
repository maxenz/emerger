Public Class typPerfilesTiposMoviles
    Inherits typAll
    Private clPerfilId As typPerfiles
    Private clTipoMovilId As typTiposMoviles
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
    Public Property TipoMovilId() As typTiposMoviles
        Get
            Return Me.GetTypProperty(clTipoMovilId)
        End Get
        Set(ByVal value As typTiposMoviles)
            clTipoMovilId = value
        End Set
    End Property

End Class
