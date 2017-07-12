Public Class typPersonalPactados
    Inherits typAll
    Private clPersonalId As typPersonal
    Private clDiaSemana As Integer
    Private clIngresoId As Integer
    Private clHorEntrada As String
    Private clHorSalida As String
    Private clMovilId As typMoviles
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PersonalId() As typPersonal
        Get
            Return Me.GetTypProperty(clPersonalId)
        End Get
        Set(ByVal value As typPersonal)
            clPersonalId = value
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
    Public Property IngresoId() As Integer
        Get
            Return clIngresoId
        End Get
        Set(ByVal value As Integer)
            clIngresoId = value
        End Set
    End Property
    Public Property HorEntrada() As String
        Get
            Return clHorEntrada
        End Get
        Set(ByVal value As String)
            clHorEntrada = value
        End Set
    End Property
    Public Property HorSalida() As String
        Get
            Return clHorSalida
        End Get
        Set(ByVal value As String)
            clHorSalida = value
        End Set
    End Property
    Public Property MovilId() As typMoviles
        Get
            Return Me.GetTypProperty(clMovilId)
        End Get
        Set(ByVal value As typMoviles)
            clMovilId = value
        End Set
    End Property

End Class
