Public Class typUsuarios
    Inherits typAll
    Private clIdentificacion As String
    Private clNombre As String
    Private clActivo As Integer
    Private clFecCambioPassword As Date
    Private clEmail As String
    Private clPersonalId As typPersonal
    Private clviewProductoId As shamanProductos
    Private clviewArbolOpciones As Integer
    Private clviewReciente As Integer
    Private clviewFavoritos As Integer
    Private clviewIndicadores As Integer
    Private clviewPanelOperativo As Integer
    Private cltryFecha As Date
    Private cltryCantidad As Integer
    Private clwebUser As String
    Private clwebPassword As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Identificacion() As String
        Get
            Return clIdentificacion
        End Get
        Set(ByVal value As String)
            clIdentificacion = value
        End Set
    End Property
    Public Property Nombre() As String
        Get
            Return clNombre
        End Get
        Set(ByVal value As String)
            clNombre = value
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
    Public Property FecCambioPassword() As Date
        Get
            Return clFecCambioPassword
        End Get
        Set(ByVal value As Date)
            clFecCambioPassword = value
        End Set
    End Property
    Public Property Email() As String
        Get
            Return clEmail
        End Get
        Set(ByVal value As String)
            clEmail = value
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
    Public Property viewProductoId() As shamanProductos
        Get
            Return clviewProductoId
        End Get
        Set(ByVal value As shamanProductos)
            clviewProductoId = value
        End Set
    End Property
    Public Property viewArbolOpciones() As Integer
        Get
            Return clviewArbolOpciones
        End Get
        Set(ByVal value As Integer)
            clviewArbolOpciones = value
        End Set
    End Property
    Public Property viewReciente() As Integer
        Get
            Return clviewReciente
        End Get
        Set(ByVal value As Integer)
            clviewReciente = value
        End Set
    End Property
    Public Property viewFavoritos() As Integer
        Get
            Return clviewFavoritos
        End Get
        Set(ByVal value As Integer)
            clviewFavoritos = value
        End Set
    End Property
    Public Property viewIndicadores() As Integer
        Get
            Return clviewIndicadores
        End Get
        Set(ByVal value As Integer)
            clviewIndicadores = value
        End Set
    End Property
    Public Property viewPanelOperativo() As Integer
        Get
            Return clviewPanelOperativo
        End Get
        Set(ByVal value As Integer)
            clviewPanelOperativo = value
        End Set
    End Property
    Public Property tryFecha() As Date
        Get
            Return cltryFecha
        End Get
        Set(ByVal value As Date)
            cltryFecha = value
        End Set
    End Property
    Public Property tryCantidad() As Integer
        Get
            Return cltryCantidad
        End Get
        Set(ByVal value As Integer)
            cltryCantidad = value
        End Set
    End Property
    Public Property webUser() As String
        Get
            Return clwebUser
        End Get
        Set(ByVal value As String)
            clwebUser = value
        End Set
    End Property
    Public Property webPassword() As String
        Get
            Return clwebPassword
        End Get
        Set(ByVal value As String)
            clwebPassword = value
        End Set
    End Property

End Class
