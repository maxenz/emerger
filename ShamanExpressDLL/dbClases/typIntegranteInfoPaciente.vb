Public Class typIntegranteInfoPaciente
    Inherits typAll
    Private clInfoPacienteItemId As typInfoPacienteItems
    Private clObservaciones As String
    Private clflgPurge As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property InfoPacienteItemId() As typInfoPacienteItems
        Get
            Return Me.GetTypProperty(clInfoPacienteItemId)
        End Get
        Set(ByVal value As typInfoPacienteItems)
            clInfoPacienteItemId = value
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
    Public Property flgPurge() As Integer
        Get
            Return clflgPurge
        End Get
        Set(ByVal value As Integer)
            clflgPurge = value
        End Set
    End Property

End Class
