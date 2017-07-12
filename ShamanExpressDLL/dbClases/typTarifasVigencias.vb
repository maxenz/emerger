Public Class typTarifasVigencias
    Inherits typAll
    Private clTarifaId As typTarifas
    Private clVigenciaDesde As Date
    Private clVigenciaHasta As Date
    Private clLocalidadOrigenId As typLocalidades
    Private clflgKmtAgrupado As Integer
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
    Public Property VigenciaDesde() As Date
        Get
            Return clVigenciaDesde
        End Get
        Set(ByVal value As Date)
            clVigenciaDesde = value
        End Set
    End Property
    Public Property VigenciaHasta() As Date
        Get
            Return clVigenciaHasta
        End Get
        Set(ByVal value As Date)
            clVigenciaHasta = value
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
    Public Property flgKmtAgrupado() As Integer
        Get
            Return clflgKmtAgrupado
        End Get
        Set(ByVal value As Integer)
            clflgKmtAgrupado = value
        End Set
    End Property
End Class
