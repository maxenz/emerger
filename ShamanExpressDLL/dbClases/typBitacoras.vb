Public Class typBitacoras
    Inherits typAll
    Private clEvento As String
    Private clCriticidad As Integer
    Private clPropietarioId As typUsuarios
    Private clFecHorBitacora As DateTime

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Property Evento() As String
        Get
            Return clEvento
        End Get
        Set(ByVal value As String)
            clEvento = value
        End Set
    End Property

    Public Property Criticidad() As Integer
        Get
            Return clCriticidad
        End Get
        Set(ByVal value As Integer)
            clCriticidad = value
        End Set
    End Property

    Public Property PropietarioId() As typUsuarios
        Get
            Return Me.GetTypProperty(clPropietarioId)
        End Get
        Set(ByVal value As typUsuarios)
            clPropietarioId = value
        End Set
    End Property

    Public Property FecHorBitacora() As DateTime
        Get
            Return clFecHorBitacora
        End Get
        Set(ByVal value As DateTime)
            clFecHorBitacora = value
        End Set
    End Property
End Class
