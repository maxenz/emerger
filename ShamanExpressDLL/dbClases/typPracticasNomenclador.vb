Public Class typPracticasNomenclador
    Inherits typAll
    Private clPracticaId As typPracticas
    Private clNomencladorId As typNomenclador
    Private clCantidad As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PracticaId() As typPracticas
        Get
            Return Me.GetTypProperty(clPracticaId)
        End Get
        Set(ByVal value As typPracticas)
            clPracticaId = value
        End Set
    End Property
    Public Property NomencladorId() As typNomenclador
        Get
            Return Me.GetTypProperty(clNomencladorId)
        End Get
        Set(ByVal value As typNomenclador)
            clNomencladorId = value
        End Set
    End Property
    Public Property Cantidad() As Decimal
        Get
            Return clCantidad
        End Get
        Set(ByVal value As Decimal)
            clCantidad = value
        End Set
    End Property

End Class
