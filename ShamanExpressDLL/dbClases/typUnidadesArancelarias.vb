Public Class typUnidadesArancelarias
    Inherits typAllGenerico01
    Private clClasificacionId As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ClasificacionId() As Integer
        Get
            Return clClasificacionId
        End Get
        Set(ByVal value As Integer)
            clClasificacionId = value
        End Set
    End Property

End Class
