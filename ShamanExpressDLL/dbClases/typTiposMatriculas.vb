Public Class typTiposMatriculas
    Inherits typAll
    Private clAplicacionId As Integer
    Private clDescripcion As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property AplicacionId() As Integer
        Get
            Return clAplicacionId
        End Get
        Set(ByVal value As Integer)
            clAplicacionId = value
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
