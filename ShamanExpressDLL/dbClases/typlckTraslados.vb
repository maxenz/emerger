Public Class typlckTraslados
    Inherits typAll
    Private clTrasladoId As Int64
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TrasladoId() As Int64
        Get
            Return clTrasladoId
        End Get
        Set(ByVal value As Int64)
            clTrasladoId = value
        End Set
    End Property
End Class

