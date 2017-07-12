Public Class typCentrosAtencionHorarios
    Inherits typAll
    Private clCentroAtencionSalaId As typCentrosAtencionSalas
    Private clDiaSemana As Integer
    Private clAperturaId As Integer
    Private clHorInicio As String
    Private clHorFinal As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property CentroAtencionSalaId() As typCentrosAtencionSalas
        Get
            Return Me.GetTypProperty(clCentroAtencionSalaId)
        End Get
        Set(ByVal value As typCentrosAtencionSalas)
            clCentroAtencionSalaId = value
        End Set
    End Property
    Public Property DiaSemana() As Integer
        Get
            Return clDiaSemana
        End Get
        Set(ByVal value As Integer)
            clDiaSemana = value
        End Set
    End Property
    Public Property AperturaId() As Integer
        Get
            Return clAperturaId
        End Get
        Set(ByVal value As Integer)
            clAperturaId = value
        End Set
    End Property
    Public Property HorInicio() As String
        Get
            Return clHorInicio
        End Get
        Set(ByVal value As String)
            clHorInicio = value
        End Set
    End Property
    Public Property HorFinal() As String
        Get
            Return clHorFinal
        End Get
        Set(ByVal value As String)
            clHorFinal = value
        End Set
    End Property

End Class
