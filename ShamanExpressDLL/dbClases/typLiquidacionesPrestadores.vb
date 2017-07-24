Public Class typLiquidacionesPrestadores
    '----> Comentario
    Inherits typAll
    Private clTipoLiquidacion As Integer
    Private clPrestadorId As typPrestadores
    Private clPersonalId As typPersonal
    Private clPeriodoLiquidacionId As typPeriodosLiquidaciones
    Private clflgCerrado As Integer
    Private clImporte As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TipoLiquidacion() As Integer
        Get
            Return clTipoLiquidacion
        End Get
        Set(ByVal value As Integer)
            clTipoLiquidacion = value
        End Set
    End Property
    Public Property PrestadorId() As typPrestadores
        Get
            Return Me.GetTypProperty(clPrestadorId)
        End Get
        Set(ByVal value As typPrestadores)
            clPrestadorId = value
        End Set
    End Property
    Public Property PersonalId() As typPersonal
        Get
            Return Me.GetTypProperty(clPersonalId)
        End Get
        Set(ByVal value As typPersonal)
            clPersonalId = value
        End Set
    End Property
    Public Property PeriodoLiquidacionId() As typPeriodosLiquidaciones
        Get
            Return Me.GetTypProperty(clPeriodoLiquidacionId)
        End Get
        Set(ByVal value As typPeriodosLiquidaciones)
            clPeriodoLiquidacionId = value
        End Set
    End Property
    Public Property flgCerrado() As Integer
        Get
            Return clflgCerrado
        End Get
        Set(ByVal value As Integer)
            clflgCerrado = value
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
