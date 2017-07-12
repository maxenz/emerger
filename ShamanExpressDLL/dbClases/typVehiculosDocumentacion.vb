Public Class typVehiculosDocumentacion
    Inherits typAll
    Private clVehiculoId As typVehiculos
    Private clDocumentacionVehiculoId As typDocumentacionVehiculos
    Private clFecVencimiento As DateTime
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
    Public Property DocumentacionVehiculoId() As typDocumentacionVehiculos
        Get
            Return Me.GetTypProperty(clDocumentacionVehiculoId)
        End Get
        Set(ByVal value As typDocumentacionVehiculos)
            clDocumentacionVehiculoId = value
        End Set
    End Property
    Public Property FecVencimiento() As DateTime
        Get
            Return clFecVencimiento
        End Get
        Set(ByVal value As DateTime)
            clFecVencimiento = value
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
