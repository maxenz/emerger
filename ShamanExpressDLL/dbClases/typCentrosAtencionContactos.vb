Public Class typCentrosAtencionContactos
    Inherits typGenericoContacto
    Private clCentroAtencionId As typCentrosAtencion
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property CentroAtencionId() As typCentrosAtencion
        Get
            Return Me.GetTypProperty(clCentroAtencionId)
        End Get
        Set(ByVal value As typCentrosAtencion)
            clCentroAtencionId = value
        End Set
    End Property
End Class
