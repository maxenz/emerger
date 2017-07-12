Public Class typTurnosNomenclador
    Inherits typAll
    Private clTurnoId As typTurnos
    Private clNomencladorId As typNomenclador
    Private clCantidad As Decimal
    Private clflgPurge As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TurnoId() As typTurnos
        Get
            Return Me.GetTypProperty(clTurnoId)
        End Get
        Set(ByVal value As typTurnos)
            clTurnoId = value
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
    Public Property flgPurge() As Integer
        Get
            Return clflgPurge
        End Get
        Set(ByVal value As Integer)
            clflgPurge = value
        End Set
    End Property

End Class
