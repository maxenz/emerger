Public Class typPrestadoresMovilesTarifas
    Inherits typAll
    Private clPrestadorId As typPrestadores
    Private clMovilId As typMoviles
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
    Public Property MovilId() As typMoviles
        Get
            Return Me.GetTypProperty(clMovilId)
        End Get
        Set(ByVal value As typMoviles)
            clMovilId = value
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
