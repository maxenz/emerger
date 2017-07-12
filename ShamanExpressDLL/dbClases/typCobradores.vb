Public Class typCobradores
    Inherits typAllGenerico01
    Private clPersonalId As typPersonal
    Private clflgCobranzas As Integer
    Private clflgVentas As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PersonalId() As typPersonal
        Get
            Return Me.GetTypProperty(clPersonalId)
        End Get
        Set(ByVal value As typPersonal)
            clPersonalId = value
        End Set
    End Property
    Public Property flgCobranzas() As Integer
        Get
            Return clflgCobranzas
        End Get
        Set(ByVal value As Integer)
            clflgCobranzas = value
        End Set
    End Property
    Public Property flgVentas() As Integer
        Get
            Return clflgVentas
        End Get
        Set(ByVal value As Integer)
            clflgVentas = value
        End Set
    End Property

End Class
