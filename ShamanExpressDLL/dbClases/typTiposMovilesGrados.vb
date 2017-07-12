Public Class typTiposMovilesGrados
    Inherits typAll
    Private clTipoMovilId As typTiposMoviles
    Private clGradoOperativoId As typGradosOperativos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TipoMovilId() As typTiposMoviles
        Get
            Return Me.GetTypProperty(clTipoMovilId)
        End Get
        Set(ByVal value As typTiposMoviles)
            clTipoMovilId = value
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
