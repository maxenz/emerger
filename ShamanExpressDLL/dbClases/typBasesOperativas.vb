Public Class typBasesOperativas
    Inherits typAllGenerico01
    Private clDomicilio As usrDomicilio
    Private clLocalidadId As typLocalidades
    Private clCodigoPostal As String
    Private clTelefono01 As String
    Private clTelefono02 As String
    Private clTelefono03 As String
    Private clCentroAtencionId As typCentrosAtencion
    Private clObservaciones As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Domicilio() As usrDomicilio
        Get
            Return clDomicilio
        End Get
        Set(ByVal value As usrDomicilio)
            clDomicilio = value
        End Set
    End Property
    Public Property LocalidadId() As typLocalidades
        Get
            Return Me.GetTypProperty(clLocalidadId)
        End Get
        Set(ByVal value As typLocalidades)
            clLocalidadId = value
        End Set
    End Property
    Public Property CodigoPostal() As String
        Get
            Return clCodigoPostal
        End Get
        Set(ByVal value As String)
            clCodigoPostal = value
        End Set
    End Property
    Public Property Telefono01() As String
        Get
            Return clTelefono01
        End Get
        Set(ByVal value As String)
            clTelefono01 = value
        End Set
    End Property
    Public Property Telefono02() As String
        Get
            Return clTelefono02
        End Get
        Set(ByVal value As String)
            clTelefono02 = value
        End Set
    End Property
    Public Property Telefono03() As String
        Get
            Return clTelefono03
        End Get
        Set(ByVal value As String)
            clTelefono03 = value
        End Set
    End Property
    Public Property CentroAtencionId() As typCentrosAtencion
        Get
            Return Me.GetTypProperty(clCentroAtencionId)
        End Get
        Set(ByVal value As typCentrosAtencion)
            clCentroAtencionId = value
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
