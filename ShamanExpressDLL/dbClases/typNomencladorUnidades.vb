Public Class typNomencladorUnidades
    Inherits typAll
    Private clNomencladorId As typNomenclador
    Private clUnidadArancelariaId As typUnidadesArancelarias
    Private clCantidad As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property NomencladorId() As typNomenclador
        Get
            Return Me.GetTypProperty(clNomencladorId)
        End Get
        Set(ByVal value As typNomenclador)
            clNomencladorId = value
        End Set
    End Property
    Public Property UnidadArancelariaId() As typUnidadesArancelarias
        Get
            Return Me.GetTypProperty(clUnidadArancelariaId)
        End Get
        Set(ByVal value As typUnidadesArancelarias)
            clUnidadArancelariaId = value
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
