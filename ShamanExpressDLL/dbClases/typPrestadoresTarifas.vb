Public Class typPrestadoresTarifas
    Inherits typAll
    Private clPrestadorId As typPrestadores
    Private clTarifaId As typTarifas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PrestadorId() As typPrestadores
        Get
            Return Me.GetTypProperty(clPrestadorId)
        End Get
        Set(ByVal value As typPrestadores)
            clPrestadorId = value
        End Set
    End Property
    Public Property TarifaId() As typTarifas
        Get
            Return Me.GetTypProperty(clTarifaId)
        End Get
        Set(ByVal value As typTarifas)
            clTarifaId = value
        End Set
    End Property
End Class
