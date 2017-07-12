Public Class typPerfiles
    Inherits typAllGenerico00
    Private clJerarquia As Integer
    Private clflgAdministrador As Integer
    Private clflgDespacha As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Jerarquia() As Integer
        Get
            Return clJerarquia
        End Get
        Set(ByVal value As Integer)
            clJerarquia = value
        End Set
    End Property
    Public Property flgAdministrador() As Integer
        Get
            Return clflgAdministrador
        End Get
        Set(ByVal value As Integer)
            clflgAdministrador = value
        End Set
    End Property
    Public Property flgDespacha() As Integer
        Get
            Return clflgDespacha
        End Get
        Set(ByVal value As Integer)
            clflgDespacha = value
        End Set
    End Property

End Class
