Public Class typEncuestasIncidentes
    Inherits typAll
    Private clIncidenteId As typIncidentes
    Private clFecHorEncuesta As Date
    Private clEncuestadorId As typUsuarios
    Private clEntrevistado As String
    Private clTelefono As String
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property IncidenteId() As typIncidentes
        Get
            Return Me.GetTypProperty(clIncidenteId)
        End Get
        Set(ByVal value As typIncidentes)
            clIncidenteId = value
        End Set
    End Property
    Public Property FecHorEncuesta() As Date
        Get
            Return clFecHorEncuesta
        End Get
        Set(ByVal value As Date)
            clFecHorEncuesta = value
        End Set
    End Property
    Public Property EncuestadorId() As typUsuarios
        Get
            Return Me.GetTypProperty(clEncuestadorId)
        End Get
        Set(ByVal value As typUsuarios)
            clEncuestadorId = value
        End Set
    End Property
    Public Property Entrevistado() As String
        Get
            Return clEntrevistado
        End Get
        Set(ByVal value As String)
            clEntrevistado = value
        End Set
    End Property
    Public Property Telefono() As String
        Get
            Return clTelefono
        End Get
        Set(ByVal value As String)
            clTelefono = value
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
