Public Class typTarifasPracticasDetalle
    Inherits typAll
    Private clTarifaPracticaId As typTarifasPracticas
    Private clPracticaId As typPracticas
    Private clTipoAplicacion As Integer
    Private clValor As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TarifaPracticaId() As typTarifasPracticas
        Get
            Return Me.GetTypProperty(clTarifaPracticaId)
        End Get
        Set(ByVal value As typTarifasPracticas)
            clTarifaPracticaId = value
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
    Public Property TipoAplicacion() As Integer
        Get
            Return clTipoAplicacion
        End Get
        Set(ByVal value As Integer)
            clTipoAplicacion = value
        End Set
    End Property
    Public Property Valor() As Decimal
        Get
            Return clValor
        End Get
        Set(ByVal value As Decimal)
            clValor = value
        End Set
    End Property

End Class
