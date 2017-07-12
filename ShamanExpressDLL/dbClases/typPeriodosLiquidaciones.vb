Public Class typPeriodosLiquidaciones
    '----> Comentario
    Inherits typAll
    Private clPeriodo As Int64
    Private clFecDesde As DateTime
    Private clFecHasta As DateTime
    Private clflgCerrado As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Periodo() As Int64
        Get
            Return clPeriodo
        End Get
        Set(ByVal value As Int64)
            clPeriodo = value
        End Set
    End Property
    Public Property FecDesde() As DateTime
        Get
            Return clFecDesde
        End Get
        Set(ByVal value As DateTime)
            clFecDesde = value
        End Set
    End Property
    Public Property FecHasta() As DateTime
        Get
            Return clFecHasta
        End Get
        Set(ByVal value As DateTime)
            clFecHasta = value
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

End Class
