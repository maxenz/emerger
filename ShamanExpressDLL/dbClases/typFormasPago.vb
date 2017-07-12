Public Class typFormasPago
    Inherits typAllGenerico01
    Private clreqCliente As Integer
    Private clflgDefaultCobranza As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property reqCliente() As Integer
        Get
            Return clreqCliente
        End Get
        Set(ByVal value As Integer)
            clreqCliente = value
        End Set
    End Property
    Public Property flgDefaultCobranza() As Integer
        Get
            Return clflgDefaultCobranza
        End Get
        Set(ByVal value As Integer)
            clflgDefaultCobranza = value
        End Set
    End Property
End Class
