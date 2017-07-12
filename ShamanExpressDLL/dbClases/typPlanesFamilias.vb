Public Class typPlanesFamilias
    Inherits typAllGenerico00
    Private clAlicuotaIvaId As typAlicuotasIva
    Private clNroImpresion As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property AlicuotaIvaId() As typAlicuotasIva
        Get
            Return Me.GetTypProperty(clAlicuotaIvaId)
        End Get
        Set(ByVal value As typAlicuotasIva)
            clAlicuotaIvaId = value
        End Set
    End Property
    Public Property NroImpresion() As Integer
        Get
            Return clNroImpresion
        End Get
        Set(ByVal value As Integer)
            clNroImpresion = value
        End Set
    End Property


End Class
