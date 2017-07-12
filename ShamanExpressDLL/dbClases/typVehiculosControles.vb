Public Class typVehiculosControles
    Inherits typAll
    Private clVehiculoId As typVehiculos
    Private clFecHorControl As DateTime
    Private clKilometraje As Int64
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property VehiculoId() As typVehiculos
        Get
            Return Me.GetTypProperty(clVehiculoId)
        End Get
        Set(ByVal value As typVehiculos)
            clVehiculoId = value
        End Set
    End Property
    Public Property FecHorControl() As DateTime
        Get
            Return clFecHorControl
        End Get
        Set(ByVal value As DateTime)
            clFecHorControl = value
        End Set
    End Property
    Public Property Kilometraje() As Int64
        Get
            Return clKilometraje
        End Get
        Set(ByVal value As Int64)
            clKilometraje = value
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
