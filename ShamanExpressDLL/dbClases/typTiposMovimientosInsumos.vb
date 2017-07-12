Public Class typTiposMovimientosInsumos
    Inherits typAllGenerico01
    Private clflgIncrementa As Integer
    Private clflgReposicion As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property flgIncrementa() As Integer
        Get
            Return clflgIncrementa
        End Get
        Set(ByVal value As Integer)
            clflgIncrementa = value
        End Set
    End Property
    Public Property flgReposicion() As Integer
        Get
            Return clflgReposicion
        End Get
        Set(ByVal value As Integer)
            clflgReposicion = value
        End Set
    End Property
End Class
