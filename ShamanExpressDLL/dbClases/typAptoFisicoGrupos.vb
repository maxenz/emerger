Public Class typAptoFisicoGrupos
    Inherits typAll
    Private clNroOrden As Integer
    Private clDescripcion As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property NroOrden() As Integer
        Get
            Return clNroOrden
        End Get
        Set(ByVal value As Integer)
            clNroOrden = value
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
