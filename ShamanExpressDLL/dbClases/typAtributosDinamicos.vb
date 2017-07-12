Public Class typAtributosDinamicos
    Inherits typAll
    Private clTablaDestinoId As Integer
    Private clNroOrden As Integer
    Private clAbreviaturaId As String
    Private clDescripcion As String
    Private clTipoDatoSalida As Integer
    Private clValor1 As Decimal
    Private clValor2 As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TablaDestinoId() As Integer
        Get
            Return clTablaDestinoId
        End Get
        Set(ByVal value As Integer)
            clTablaDestinoId = value
        End Set
    End Property
    Public Property NroOrden() As Integer
        Get
            Return clNroOrden
        End Get
        Set(ByVal value As Integer)
            clNroOrden = value
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
    Public Property TipoDatoSalida() As Integer
        Get
            Return clTipoDatoSalida
        End Get
        Set(ByVal value As Integer)
            clTipoDatoSalida = value
        End Set
    End Property
    Public Property Valor1() As Decimal
        Get
            Return clValor1
        End Get
        Set(ByVal value As Decimal)
            clValor1 = value
        End Set
    End Property
    Public Property Valor2() As String
        Get
            Return clValor2
        End Get
        Set(ByVal value As String)
            clValor2 = value
        End Set
    End Property

End Class
