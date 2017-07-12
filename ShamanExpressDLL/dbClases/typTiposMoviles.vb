Public Class typTiposMoviles
    Inherits typAllGenerico01
    Private clflgDespachable As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property flgDespachable() As Integer
        Get
            Return clflgDespachable
        End Get
        Set(ByVal value As Integer)
            clflgDespachable = value
        End Set
    End Property
End Class
