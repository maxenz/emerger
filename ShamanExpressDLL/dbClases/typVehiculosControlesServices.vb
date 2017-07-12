Public Class typVehiculosControlesServices
    Inherits typAll
    Private clVehiculoControlId As typVehiculosControles
    Private clServiceVehiculoId As typServicesVehiculos
    Private clSectorArregloVehiculoId As typSectoresArreglosVehiculos
    Private clKmProximoControl As Int64
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property VehiculoControlId() As typVehiculosControles
        Get
            Return Me.GetTypProperty(clVehiculoControlId)
        End Get
        Set(ByVal value As typVehiculosControles)
            clVehiculoControlId = value
        End Set
    End Property
    Public Property ServiceVehiculoId() As typServicesVehiculos
        Get
            Return Me.GetTypProperty(clServiceVehiculoId)
        End Get
        Set(ByVal value As typServicesVehiculos)
            clServiceVehiculoId = value
        End Set
    End Property
    Public Property SectorArregloVehiculoId() As typSectoresArreglosVehiculos
        Get
            Return Me.GetTypProperty(clSectorArregloVehiculoId)
        End Get
        Set(ByVal value As typSectoresArreglosVehiculos)
            clSectorArregloVehiculoId = value
        End Set
    End Property
    Public Property KmProximoControl() As Int64
        Get
            Return clKmProximoControl
        End Get
        Set(ByVal value As Int64)
            clKmProximoControl = value
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
