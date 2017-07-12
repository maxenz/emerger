Public Class typIncidentesPersonal
    Inherits typAll
    Private clIncidenteViajeId As typIncidentesViajes
    Private clMovilId As typMoviles
    Private clPuestoGrilla As String
    Private clPersonalId As typPersonal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property IncidenteViajeId() As typIncidentesViajes
        Get
            Return Me.GetTypProperty(clIncidenteViajeId)
        End Get
        Set(ByVal value As typIncidentesViajes)
            clIncidenteViajeId = value
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
    Public Property PuestoGrilla() As String
        Get
            Return clPuestoGrilla
        End Get
        Set(ByVal value As String)
            clPuestoGrilla = value
        End Set
    End Property
    Public Property PersonalId() As typPersonal
        Get
            Return Me.GetTypProperty(clPersonalId)
        End Get
        Set(ByVal value As typPersonal)
            clPersonalId = value
        End Set
    End Property

End Class