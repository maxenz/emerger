Public Class typPerfilesGradosOperativos
    Inherits typAll
    Private clPerfilId As typPerfiles
    Private clGradoOperativoId As typGradosOperativos
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
    Public Property GradoOperativoId() As typGradosOperativos
        Get
            Return Me.GetTypProperty(clGradoOperativoId)
        End Get
        Set(ByVal value As typGradosOperativos)
            clGradoOperativoId = value
        End Set
    End Property

End Class
