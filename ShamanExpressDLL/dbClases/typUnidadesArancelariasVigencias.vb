Public Class typUnidadesArancelariasVigencias
    Inherits typAll
    Private clUnidadArancelariaId As typUnidadesArancelarias
    Private clFecDesde As Date
    Private clFecHasta As Date
    Private clImporte As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property UnidadArancelariaId() As typUnidadesArancelarias
        Get
            Return Me.GetTypProperty(clUnidadArancelariaId)
        End Get
        Set(ByVal value As typUnidadesArancelarias)
            clUnidadArancelariaId = value
        End Set
    End Property
    Public Property FecDesde() As Date
        Get
            Return clFecDesde
        End Get
        Set(ByVal value As Date)
            clFecDesde = value
        End Set
    End Property
    Public Property FecHasta() As Date
        Get
            Return clFecHasta
        End Get
        Set(ByVal value As Date)
            clFecHasta = value
        End Set
    End Property
    Public Property Importe() As Decimal
        Get
            Return clImporte
        End Get
        Set(ByVal value As Decimal)
            clImporte = value
        End Set
    End Property

End Class
