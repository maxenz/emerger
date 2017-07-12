Public Class typEquipamientos
    Inherits typAll
    Private clTipoEquipamientoId As typTiposEquipamiento
    Private clMarcaEquipamientoId As typMarcasEquipamiento
    Private clNroSerie As String
    Private clEstado As Integer
    Private clMovilId As typMoviles
    Private clPersonalId As typPersonal
    Private clNroRadio As String
    Private clNroTelefono As String
    Private clNroInterno As String
    Private clDireccionEmail As String
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TipoEquipamientoId() As typTiposEquipamiento
        Get
            Return Me.GetTypProperty(clTipoEquipamientoId)
        End Get
        Set(ByVal value As typTiposEquipamiento)
            clTipoEquipamientoId = value
        End Set
    End Property
    Public Property MarcaEquipamientoId() As typMarcasEquipamiento
        Get
            Return Me.GetTypProperty(clMarcaEquipamientoId)
        End Get
        Set(ByVal value As typMarcasEquipamiento)
            clMarcaEquipamientoId = value
        End Set
    End Property
    Public Property NroSerie() As String
        Get
            Return clNroSerie
        End Get
        Set(ByVal value As String)
            clNroSerie = value
        End Set
    End Property
    Public Property Estado() As Integer
        Get
            Return clEstado
        End Get
        Set(ByVal value As Integer)
            clEstado = value
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
    Public Property PersonalId() As typPersonal
        Get
            Return Me.GetTypProperty(clPersonalId)
        End Get
        Set(ByVal value As typPersonal)
            clPersonalId = value
        End Set
    End Property
    Public Property NroRadio() As String
        Get
            Return clNroRadio
        End Get
        Set(ByVal value As String)
            clNroRadio = value
        End Set
    End Property
    Public Property NroTelefono() As String
        Get
            Return clNroTelefono
        End Get
        Set(ByVal value As String)
            clNroTelefono = value
        End Set
    End Property
    Public Property NroInterno() As String
        Get
            Return clNroInterno
        End Get
        Set(ByVal value As String)
            clNroInterno = value
        End Set
    End Property
    Public Property DireccionEmail() As String
        Get
            Return clDireccionEmail
        End Get
        Set(ByVal value As String)
            clDireccionEmail = value
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


End Class

