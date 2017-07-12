Public Class typPersonalPracticas
    Inherits typAll
    Private clPersonalId As typPersonal
    Private clPracticaId As typPracticas
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
    Public Property PracticaId() As typPracticas
        Get
            Return Me.GetTypProperty(clPracticaId)
        End Get
        Set(ByVal value As typPracticas)
            clPracticaId = value
        End Set
    End Property

End Class
