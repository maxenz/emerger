Public Class typConfigSACPuntajes
    Inherits typAll
    Private clpunDesde As Integer
    Private clpunHasta As Integer
    Private clGradoOperativoId As typGradosOperativos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property punDesde() As Integer
        Get
            Return clpunDesde
        End Get
        Set(ByVal value As Integer)
            clpunDesde = value
        End Set
    End Property
    Public Property punHasta() As Integer
        Get
            Return clpunHasta
        End Get
        Set(ByVal value As Integer)
            clpunHasta = value
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
