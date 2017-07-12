Public Class typBasesOperativasLocalidades
    Inherits typAll
    Private clBaseOperativaId As typBasesOperativas
    Private clLocalidadId As typLocalidades
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property BaseOperativaId() As typBasesOperativas
        Get
            Return Me.GetTypProperty(clBaseOperativaId)
        End Get
        Set(ByVal value As typBasesOperativas)
            clBaseOperativaId = value
        End Set
    End Property
    Public Property LocalidadId() As typLocalidades
        Get
            Return Me.GetTypProperty(clLocalidadId)
        End Get
        Set(ByVal value As typLocalidades)
            clLocalidadId = value
        End Set
    End Property

End Class
