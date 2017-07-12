Public Class typIncidentesAtributos
    Inherits typIntegranteAtributo
    Private clIncidenteId As typIncidentes
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

End Class
