Public Class typSanatoriosContactos
    Inherits typGenericoContacto
    Private clSanatorioId As typSanatorios
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property SanatorioId() As typSanatorios
        Get
            Return Me.GetTypProperty(clSanatorioId)
        End Get
        Set(ByVal value As typSanatorios)
            clSanatorioId = value
        End Set
    End Property
End Class