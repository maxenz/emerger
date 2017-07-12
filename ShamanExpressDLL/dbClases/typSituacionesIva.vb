Public Class typSituacionesIva
    Inherits typAllGenerico01
    Private clLetra As String
    Private clAlicuotaIvaId As typAlicuotasIva
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Letra() As String
        Get
            Return clLetra
        End Get
        Set(ByVal value As String)
            clLetra = value
        End Set
    End Property
    Public Property AlicuotaIvaId() As typAlicuotasIva
        Get
            Return Me.GetTypProperty(clAlicuotaIvaId)
        End Get
        Set(ByVal value As typAlicuotasIva)
            clAlicuotaIvaId = value
        End Set
    End Property
End Class
