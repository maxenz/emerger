Public Class typIncidentesDomicilios
    Inherits typAll
    Private clIncidenteId As typIncidentes
    Private clTipoDomicilio As Integer
    Private clNroAnexo As Integer
    Private clTipoOrigen As Integer
    Private clDomicilio As usrDomicilio
    Private clLocalidadId As typLocalidades
    Private clSanatorioId As typSanatorios
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
    Public Property TipoDomicilio() As Integer
        Get
            Return clTipoDomicilio
        End Get
        Set(ByVal value As Integer)
            clTipoDomicilio = value
        End Set
    End Property
    Public Property NroAnexo() As Integer
        Get
            Return clNroAnexo
        End Get
        Set(ByVal value As Integer)
            clNroAnexo = value
        End Set
    End Property
    Public Property TipoOrigen() As Integer
        Get
            Return clTipoOrigen
        End Get
        Set(ByVal value As Integer)
            clTipoOrigen = value
        End Set
    End Property
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
    Public Property SanatorioId() As typSanatorios
        Get
            Return Me.GetTypProperty(clSanatorioId)
        End Get
        Set(ByVal value As typSanatorios)
            clSanatorioId = value
        End Set
    End Property

End Class
