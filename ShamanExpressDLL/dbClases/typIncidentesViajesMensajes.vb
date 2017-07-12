Public Class typIncidentesViajesMensajes
    Inherits typAll
    Private clIncidenteViajeId As typIncidentesViajes
    Private clMetodoMensajeriaId As typMetodosMensajeria
    Private clopeModoMensajeria As Integer
    Private clMovilId As typMoviles
    Private clMensaje As String
    Private clflgEnviado As Integer
    Private clMensajeError As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property IncidenteViajeId() As typIncidentesViajes
        Get
            Return Me.GetTypProperty(clIncidenteViajeId)
        End Get
        Set(ByVal value As typIncidentesViajes)
            clIncidenteViajeId = value
        End Set
    End Property
    Public Property MetodoMensajeriaId() As typMetodosMensajeria
        Get
            Return Me.GetTypProperty(clMetodoMensajeriaId)
        End Get
        Set(ByVal value As typMetodosMensajeria)
            clMetodoMensajeriaId = value
        End Set
    End Property
    Public Property opeModoMensajeria() As Integer
        Get
            Return clopeModoMensajeria
        End Get
        Set(ByVal value As Integer)
            clopeModoMensajeria = value
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
    Public Property Mensaje() As String
        Get
            Return clMensaje
        End Get
        Set(ByVal value As String)
            clMensaje = value
        End Set
    End Property
    Public Property flgEnviado() As Integer
        Get
            Return clflgEnviado
        End Get
        Set(ByVal value As Integer)
            clflgEnviado = value
        End Set
    End Property
    Public Property MensajeError() As String
        Get
            Return clMensajeError
        End Get
        Set(ByVal value As String)
            clMensajeError = value
        End Set
    End Property

End Class
