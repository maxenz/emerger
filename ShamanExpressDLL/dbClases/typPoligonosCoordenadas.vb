Public Class typPoligonosCoordenadas
    Inherits typAll
    Private clPoligonoId As typPoligonos
    Private clLatitud As Decimal
    Private clLongitud As Decimal

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Property PoligonoId() As typPoligonos
        Get
            Return Me.GetTypProperty(clPoligonoId)
        End Get
        Set(ByVal value As typPoligonos)
            clPoligonoId = value
        End Set
    End Property

    Public Property Latitud() As Decimal
        Get
            Return clLatitud
        End Get
        Set(ByVal value As Decimal)
            clLatitud = value
        End Set
    End Property

    Public Property Longitud() As Decimal
        Get
            Return clLongitud
        End Get
        Set(ByVal value As Decimal)
            clLongitud = value
        End Set
    End Property
End Class
