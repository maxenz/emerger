Public Class typTalonariosDocumentos
    Inherits typAll
    Private clTalonarioId As typTalonarios
    Private clTipoComprobanteId As typTiposComprobantes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TalonarioId() As typTalonarios
        Get
            Return Me.GetTypProperty(clTalonarioId)
        End Get
        Set(ByVal value As typTalonarios)
            clTalonarioId = value
        End Set
    End Property
    Public Property TipoComprobanteId() As typTiposComprobantes
        Get
            Return Me.GetTypProperty(clTipoComprobanteId)
        End Get
        Set(ByVal value As typTiposComprobantes)
            clTipoComprobanteId = value
        End Set
    End Property

End Class
