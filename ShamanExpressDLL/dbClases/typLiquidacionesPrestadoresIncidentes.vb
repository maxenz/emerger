Public Class typLiquidacionesPrestadoresIncidentes
    '----> Comentario
    Inherits typAll
    Private clLiquidacionPrestadorId As typLiquidacionesPrestadores
    Private clIncidenteViajeId As typIncidentesViajes
    Private clConceptoFacturacionId As typConceptosFacturacion
    Private clLocalidadOrigenId As typLocalidades
    Private clLocalidadDestinoId As typLocalidades
    Private clKilometraje As Int64
    Private clflgNocturno As Integer
    Private clflgPediatrico As Integer
    Private clflgDerivado As Integer
    Private clDemora As Int64
    Private clImporte As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property LiquidacionPrestadorId() As typLiquidacionesPrestadores
        Get
            Return Me.GetTypProperty(clLiquidacionPrestadorId)
        End Get
        Set(ByVal value As typLiquidacionesPrestadores)
            clLiquidacionPrestadorId = value
        End Set
    End Property
    Public Property IncidenteViajeId() As typIncidentesViajes
        Get
            Return Me.GetTypProperty(clIncidenteViajeId)
        End Get
        Set(ByVal value As typIncidentesViajes)
            clIncidenteViajeId = value
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
    Public Property LocalidadOrigenId() As typLocalidades
        Get
            Return Me.GetTypProperty(clLocalidadOrigenId)
        End Get
        Set(ByVal value As typLocalidades)
            clLocalidadOrigenId = value
        End Set
    End Property
    Public Property LocalidadDestinoId() As typLocalidades
        Get
            Return Me.GetTypProperty(clLocalidadDestinoId)
        End Get
        Set(ByVal value As typLocalidades)
            clLocalidadDestinoId = value
        End Set
    End Property
    Public Property Kilometraje() As Int64
        Get
            Return clKilometraje
        End Get
        Set(ByVal value As Int64)
            clKilometraje = value
        End Set
    End Property
    Public Property flgNocturno() As Integer
        Get
            Return clflgNocturno
        End Get
        Set(ByVal value As Integer)
            clflgNocturno = value
        End Set
    End Property
    Public Property flgPediatrico() As Integer
        Get
            Return clflgPediatrico
        End Get
        Set(ByVal value As Integer)
            clflgPediatrico = value
        End Set
    End Property
    Public Property flgDerivado() As Integer
        Get
            Return clflgDerivado
        End Get
        Set(ByVal value As Integer)
            clflgDerivado = value
        End Set
    End Property
    Public Property Demora() As Int64
        Get
            Return clDemora
        End Get
        Set(ByVal value As Int64)
            clDemora = value
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
