Public Class typIncidentesPracticas
    Inherits typAll
    Private clIncidenteId As typIncidentes
    Private clMovilId As typMoviles
    Private clMedicoId As typPersonal
    Private clClasificacion As Integer
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
    Public Property MovilId() As typMoviles
        Get
            Return Me.GetTypProperty(clMovilId)
        End Get
        Set(ByVal value As typMoviles)
            clMovilId = value
        End Set
    End Property
    Public Property MedicoId() As typPersonal
        Get
            Return Me.GetTypProperty(clMedicoId)
        End Get
        Set(ByVal value As typPersonal)
            clMedicoId = value
        End Set
    End Property
    Public Property Clasificacion() As Integer
        Get
            Return clClasificacion
        End Get
        Set(ByVal value As Integer)
            clClasificacion = value
        End Set
    End Property
End Class
