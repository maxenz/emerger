Public Class typInsumos
    Inherits typAllGenerico01
    Private clTipoInsumoId As typTiposInsumos
    Private clActivo As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TipoInsumoId() As typTiposInsumos
        Get
            Return Me.GetTypProperty(clTipoInsumoId)
        End Get
        Set(ByVal value As typTiposInsumos)
            clTipoInsumoId = value
        End Set
    End Property
    Public Property Activo() As Integer
        Get
            Return clActivo
        End Get
        Set(ByVal value As Integer)
            clActivo = value
        End Set
    End Property

End Class
