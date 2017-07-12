Public Class typAptoFisicoInfo
    Inherits typAll
    Private clClasificacionId As infAptoFisicoItem
    Private clDescripcion As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ClasificacionId() As infAptoFisicoItem
        Get
            Return clClasificacionId
        End Get
        Set(ByVal value As infAptoFisicoItem)
            clClasificacionId = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
        End Set
    End Property
End Class

