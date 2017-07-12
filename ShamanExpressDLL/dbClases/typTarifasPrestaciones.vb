Public Class typTarifasPrestaciones
    Inherits typAll
    Private clTarifaId As typTarifas
    Private clTarifaVigenciaId As typTarifasVigencias
    Private clConceptoFacturacionId As typConceptosFacturacion
    Private clKmDesde As Integer
    Private clKmHasta As Integer
    Private clImporte As String
    Private clRecNocturno As Decimal
    Private clRecPediatrico As Decimal
    Private clRecDerivacion As Decimal
    Private clImpDemora As Decimal
    Private clImpKmExcedente As Decimal
    Private clImpDtoRetorno As Decimal
    Private clAlias1 As String
    Private clAlias2 As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TarifaId() As typTarifas
        Get
            Return Me.GetTypProperty(clTarifaId)
        End Get
        Set(ByVal value As typTarifas)
            clTarifaId = value
        End Set
    End Property
    Public Property TarifaVigenciaId() As typTarifasVigencias
        Get
            Return Me.GetTypProperty(clTarifaVigenciaId)
        End Get
        Set(ByVal value As typTarifasVigencias)
            clTarifaVigenciaId = value
        End Set
    End Property
    Public Property ConceptoFacturacionId() As typConceptosFacturacion
        Get
            Return Me.GetTypProperty(clConceptoFacturacionId)
        End Get
        Set(ByVal value As typConceptosFacturacion)
            clConceptoFacturacionId = value
        End Set
    End Property
    Public Property KmDesde() As Integer
        Get
            Return clKmDesde
        End Get
        Set(ByVal value As Integer)
            clKmDesde = value
        End Set
    End Property
    Public Property KmHasta() As Integer
        Get
            Return clKmHasta
        End Get
        Set(ByVal value As Integer)
            clKmHasta = value
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
    Public Property RecNocturno() As Decimal
        Get
            Return clRecNocturno
        End Get
        Set(ByVal value As Decimal)
            clRecNocturno = value
        End Set
    End Property
    Public Property RecPediatrico() As Decimal
        Get
            Return clRecPediatrico
        End Get
        Set(ByVal value As Decimal)
            clRecPediatrico = value
        End Set
    End Property
    Public Property RecDerivacion() As Decimal
        Get
            Return clRecDerivacion
        End Get
        Set(ByVal value As Decimal)
            clRecDerivacion = value
        End Set
    End Property
    Public Property ImpDemora() As Decimal
        Get
            Return clImpDemora
        End Get
        Set(ByVal value As Decimal)
            clImpDemora = value
        End Set
    End Property
    Public Property ImpKmExcedente() As Decimal
        Get
            Return clImpKmExcedente
        End Get
        Set(ByVal value As Decimal)
            clImpKmExcedente = value
        End Set
    End Property
    Public Property ImpDtoRetorno() As Decimal
        Get
            Return clImpDtoRetorno
        End Get
        Set(ByVal value As Decimal)
            clImpDtoRetorno = value
        End Set
    End Property
    Public Property Alias1() As String
        Get
            Return clAlias1
        End Get
        Set(ByVal value As String)
            clAlias1 = value
        End Set
    End Property
    Public Property Alias2() As String
        Get
            Return clAlias2
        End Get
        Set(ByVal value As String)
            clAlias2 = value
        End Set
    End Property

End Class
