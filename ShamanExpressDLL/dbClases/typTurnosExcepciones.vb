Public Class typTurnosExcepciones
    Inherits typAll
    Private clFechaHoraDesde As DateTime
    Private clFechaHoraHasta As DateTime
    Private clPracticaId As typPracticas
    Private clCentroAtencionSalaId As typCentrosAtencionSalas
    Private clPersonalId As typPersonal
    Private clTipoExcepcion As Integer
    Private clModalidad As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property FechaHoraDesde() As DateTime
        Get
            Return clFechaHoraDesde
        End Get
        Set(ByVal value As DateTime)
            clFechaHoraDesde = value
        End Set
    End Property
    Public Property FechaHoraHasta() As DateTime
        Get
            Return clFechaHoraHasta
        End Get
        Set(ByVal value As DateTime)
            clFechaHoraHasta = value
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
    Public Property CentroAtencionSalaId() As typCentrosAtencionSalas
        Get
            Return Me.GetTypProperty(clCentroAtencionSalaId)
        End Get
        Set(ByVal value As typCentrosAtencionSalas)
            clCentroAtencionSalaId = value
        End Set
    End Property
    Public Property PersonalId() As typPersonal
        Get
            Return Me.GetTypProperty(clPersonalId)
        End Get
        Set(ByVal value As typPersonal)
            clPersonalId = value
        End Set
    End Property
    Public Property TipoExcepcion() As Integer
        Get
            Return clTipoExcepcion
        End Get
        Set(ByVal value As Integer)
            clTipoExcepcion = value
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
