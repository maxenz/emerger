Public MustInherit Class typGenericoMovil
    Inherits typAll
    Private clrelTabla As Integer
    Private clMovil As String
    Private clTipoMovilId As typTiposMoviles
    Private clBaseOperativaId As typBasesOperativas
    Private clVehiculoId As typVehiculos
    Private clPrestadorId As typPrestadores
    Private clPersonalId As typPersonal
    Private clflgGeoCobPropia As Integer
    Private clActivo As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property relTabla() As Integer
        Get
            Return clrelTabla
        End Get
        Set(ByVal value As Integer)
            clrelTabla = value
        End Set
    End Property
    Public Property Movil() As String
        Get
            Return clMovil
        End Get
        Set(ByVal value As String)
            clMovil = value
        End Set
    End Property
    Public Property TipoMovilId() As typTiposMoviles
        Get
            Return Me.GetTypProperty(clTipoMovilId)
        End Get
        Set(ByVal value As typTiposMoviles)
            clTipoMovilId = value
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
    Public Property VehiculoId() As typVehiculos
        Get
            Return Me.GetTypProperty(clVehiculoId)
        End Get
        Set(ByVal value As typVehiculos)
            clVehiculoId = value
        End Set
    End Property
    Public Property PrestadorId() As typPrestadores
        Get
            Return Me.GetTypProperty(clPrestadorId)
        End Get
        Set(ByVal value As typPrestadores)
            clPrestadorId = value
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
    Public Property flgGeoCobPropia() As Integer
        Get
            Return clflgGeoCobPropia
        End Get
        Set(ByVal value As Integer)
            clflgGeoCobPropia = value
        End Set
    End Property
    Public Property Activo() As Integer
        Get
            Return clActivo
        End Get
        Set(ByVal value As Integer)
            clActivo = value
        End Set
    End Property


End Class
