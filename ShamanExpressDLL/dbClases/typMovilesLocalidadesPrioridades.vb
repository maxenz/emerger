Public Class typMovilesLocalidadesPrioridades
    Inherits typAll
    Private clMovilLocalidadId As typMovilesLocalidades
    Private clGradoOperativoId As typGradosOperativos
    Private clNroPrioridad As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property MovilLocalidadId() As typMovilesLocalidades
        Get
            Return Me.GetTypProperty(clMovilLocalidadId)
        End Get
        Set(ByVal value As typMovilesLocalidades)
            clMovilLocalidadId = value
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
    Public Property NroPrioridad() As Integer
        Get
            Return clNroPrioridad
        End Get
        Set(ByVal value As Integer)
            clNroPrioridad = value
        End Set
    End Property
End Class
