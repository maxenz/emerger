Public Class typGrillaOperativaTripulantes
    Inherits typAll
    Private clGrillaOperativaId As typGrillaOperativa
    Private clMovilId As typMoviles
    Private clVehiculoId As typVehiculos
    Private clBaseOperativaId As typBasesOperativas
    Private clPersonalId As typPersonal
    Private clIngresoId As Integer
    Private clPuestoGrilla As String
    Private clPacEntrada As DateTime
    Private clPacSalida As Date
    Private clFicEntrada As DateTime
    Private clFicSalida As Date
    Private clObservaciones As String
    Private clflgPurge As Integer
    Private clflgPactado As Integer
    Private clflgHorLegajo As Integer
    Private clflgDelete As Integer
    Private clflgEdited As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property GrillaOperativaId() As typGrillaOperativa
        Get
            Return Me.GetTypProperty(clGrillaOperativaId)
        End Get
        Set(ByVal value As typGrillaOperativa)
            clGrillaOperativaId = value
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
    Public Property VehiculoId() As typVehiculos
        Get
            Return Me.GetTypProperty(clVehiculoId)
        End Get
        Set(ByVal value As typVehiculos)
            clVehiculoId = value
        End Set
    End Property
    Public Property BaseOperativaId() As typBasesOperativas
        Get
            Return Me.GetTypProperty(clBaseOperativaId)
        End Get
        Set(ByVal value As typBasesOperativas)
            clBaseOperativaId = value
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
    Public Property IngresoId() As Integer
        Get
            Return clIngresoId
        End Get
        Set(ByVal value As Integer)
            clIngresoId = value
        End Set
    End Property
    Public Property PuestoGrilla() As String
        Get
            Return clPuestoGrilla
        End Get
        Set(ByVal value As String)
            clPuestoGrilla = value
        End Set
    End Property
    Public Property PacEntrada() As DateTime
        Get
            Return clPacEntrada
        End Get
        Set(ByVal value As DateTime)
            clPacEntrada = value
        End Set
    End Property
    Public Property PacSalida() As DateTime
        Get
            Return clPacSalida
        End Get
        Set(ByVal value As DateTime)
            clPacSalida = value
        End Set
    End Property
    Public Property FicEntrada() As DateTime
        Get
            Return clFicEntrada
        End Get
        Set(ByVal value As DateTime)
            clFicEntrada = value
        End Set
    End Property
    Public Property FicSalida() As DateTime
        Get
            Return clFicSalida
        End Get
        Set(ByVal value As DateTime)
            clFicSalida = value
        End Set
    End Property
    Public Property Observaciones() As String
        Get
            Return clObservaciones
        End Get
        Set(ByVal value As String)
            clObservaciones = value
        End Set
    End Property
    Public Property flgPurge() As Integer
        Get
            Return clflgPurge
        End Get
        Set(ByVal value As Integer)
            clflgPurge = value
        End Set
    End Property
    Public Property flgPactado() As Integer
        Get
            Return clflgPactado
        End Get
        Set(ByVal value As Integer)
            clflgPactado = value
        End Set
    End Property
    Public Property flgHorLegajo() As Integer
        Get
            Return clflgHorLegajo
        End Get
        Set(ByVal value As Integer)
            clflgHorLegajo = value
        End Set
    End Property
    Public Property flgDelete() As Integer
        Get
            Return clflgDelete
        End Get
        Set(ByVal value As Integer)
            clflgDelete = value
        End Set
    End Property
    Public Property flgEdited() As Integer
        Get
            Return clflgEdited
        End Get
        Set(ByVal value As Integer)
            clflgEdited = value
        End Set
    End Property

End Class
