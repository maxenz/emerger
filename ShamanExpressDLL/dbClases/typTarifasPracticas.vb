Public Class typTarifasPracticas
    Inherits typAll
    Private clflgLiquidacion As Integer
    Private clAbreviaturaId As String
    Private clDescripcion As String
    Private clActivo As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property flgLiquidacion() As Integer
        Get
            Return clflgLiquidacion
        End Get
        Set(ByVal value As Integer)
            clflgLiquidacion = value
        End Set
    End Property
    Public Property AbreviaturaId() As String
        Get
            Return clAbreviaturaId
        End Get
        Set(ByVal value As String)
            clAbreviaturaId = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
        End Set
    End Property
    Public Property Activo() As Integer
        Get
            Return clActivo
        End Get
        Set(ByVal value As Integer)
            clActivo = value
        End Set
    End Property
End Class
