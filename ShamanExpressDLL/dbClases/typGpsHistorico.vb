Public Class typGpsHistorico
    Inherits typAll
    Private clMovilId As typMoviles
    Private clVehiculoId As typVehiculos
    Private clLatitud As Decimal
    Private clLongitud As Decimal
    Private clFecHorTransmision As DateTime
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
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
    Public Property Latitud() As Decimal
        Get
            Return clLatitud
        End Get
        Set(ByVal value As Decimal)
            clLatitud = value
        End Set
    End Property
    Public Property Longitud() As Decimal
        Get
            Return clLongitud
        End Get
        Set(ByVal value As Decimal)
            clLongitud = value
        End Set
    End Property
    Public Property FecHorTransmision() As DateTime
        Get
            Return clFecHorTransmision
        End Get
        Set(ByVal value As DateTime)
            clFecHorTransmision = value
        End Set
    End Property

End Class
