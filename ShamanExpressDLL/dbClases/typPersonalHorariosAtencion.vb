Public Class typPersonalHorariosAtencion
    Inherits typAll
    Private clPersonalId As typPersonal
    Private clCentroAtencionSalaId As typCentrosAtencionSalas
    Private clDiaSemana As Integer
    Private clIngresoId As Integer
    Private clHorEntrada As String
    Private clHorSalida As String
    Private clPracticaId As typPracticas
    Private clmodDisponibilidad As Integer
    Private clmodDisponibilidadStr As String
    Private clFecVigenciaDesde As Date
    Private clFecVigenciaHasta As Date
    Private clTpoTurno As Integer
    Private clflgFeriados As Integer
    Private clModalidad As Integer
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
    Public Property PracticaId() As typPracticas
        Get
            Return Me.GetTypProperty(clPracticaId)
        End Get
        Set(ByVal value As typPracticas)
            clPracticaId = value
        End Set
    End Property
    Public Property modDisponibilidad() As Integer
        Get
            Return clmodDisponibilidad
        End Get
        Set(ByVal value As Integer)
            clmodDisponibilidad = value
        End Set
    End Property
    Public Property modDisponibilidadStr() As String
        Get
            Return clmodDisponibilidadStr
        End Get
        Set(ByVal value As String)
            clmodDisponibilidadStr = value
        End Set
    End Property
    Public Property FecVigenciaDesde() As Date
        Get
            Return clFecVigenciaDesde
        End Get
        Set(ByVal value As Date)
            clFecVigenciaDesde = value
        End Set
    End Property
    Public Property FecVigenciaHasta() As Date
        Get
            Return clFecVigenciaHasta
        End Get
        Set(ByVal value As Date)
            clFecVigenciaHasta = value
        End Set
    End Property
    Public Property TpoTurno() As Integer
        Get
            Return clTpoTurno
        End Get
        Set(ByVal value As Integer)
            clTpoTurno = value
        End Set
    End Property
    Public Property flgFeriados() As Integer
        Get
            Return clflgFeriados
        End Get
        Set(ByVal value As Integer)
            clflgFeriados = value
        End Set
    End Property
    Public Property Modalidad() As Integer
        Get
            Return clModalidad
        End Get
        Set(ByVal value As Integer)
            clModalidad = value
        End Set
    End Property

End Class
