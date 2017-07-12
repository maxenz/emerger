Public Class typSanatorios
    Inherits typAllGenerico01
    Private clDomicilio As usrDomicilio
    Private clLocalidadId As typLocalidades
    Private clCodigoPostal As String
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
    Public Property Observaciones() As String
        Get
            Return clObservaciones
        End Get
        Set(ByVal value As String)
            clObservaciones = value
        End Set
    End Property
End Class
