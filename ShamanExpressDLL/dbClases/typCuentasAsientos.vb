Public Class typCuentasAsientos
    Inherits typAllGenerico01
    Private clClasificacion As Integer
    Private clFormaPagoId As typFormasPago
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Clasificacion() As Integer
        Get
            Return clClasificacion
        End Get
        Set(ByVal value As Integer)
            clClasificacion = value
        End Set
    End Property
    Public Property FormaPagoId() As typFormasPago
        Get
            Return Me.GetTypProperty(clFormaPagoId)
        End Get
        Set(ByVal value As typFormasPago)
            clFormaPagoId = value
        End Set
    End Property
End Class
