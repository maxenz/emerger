Public Class typInsumosMovimientosDetalle
    Inherits typAll
    Private clInsumoMovimientoId As typInsumosMovimientos
    Private clIncidenteId As typIncidentes
    Private clTurnoId As typTurnos
    Private clInsumoId As typInsumos
    Private clCantidad As Decimal
    Private clImporte As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property InsumoMovimientoId() As typInsumosMovimientos
        Get
            Return Me.GetTypProperty(clInsumoMovimientoId)
        End Get
        Set(ByVal value As typInsumosMovimientos)
            clInsumoMovimientoId = value
        End Set
    End Property
    Public Property IncidenteId() As typIncidentes
        Get
            Return Me.GetTypProperty(clIncidenteId)
        End Get
        Set(ByVal value As typIncidentes)
            clIncidenteId = value
        End Set
    End Property
    Public Property TurnoId() As typTurnos
        Get
            Return Me.GetTypProperty(clTurnoId)
        End Get
        Set(ByVal value As typTurnos)
            clTurnoId = value
        End Set
    End Property
    Public Property InsumoId() As typInsumos
        Get
            Return Me.GetTypProperty(clInsumoId)
        End Get
        Set(ByVal value As typInsumos)
            clInsumoId = value
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
    Public Property Importe() As Decimal
        Get
            Return clImporte
        End Get
        Set(ByVal value As Decimal)
            clImporte = value
        End Set
    End Property

End Class
