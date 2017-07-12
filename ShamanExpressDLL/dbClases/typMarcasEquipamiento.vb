Public Class typMarcasEquipamiento
    Inherits typAll
    Private clTipoEquipamientoId As typTiposEquipamiento
    Private clMarca As String
    Private clModelo As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property TipoEquipamientoId() As typTiposEquipamiento
        Get
            Return Me.GetTypProperty(clTipoEquipamientoId)
        End Get
        Set(ByVal value As typTiposEquipamiento)
            clTipoEquipamientoId = value
        End Set
    End Property
    Public Property Marca() As String
        Get
            Return clMarca
        End Get
        Set(ByVal value As String)
            clMarca = value
        End Set
    End Property
    Public Property Modelo() As String
        Get
            Return clModelo
        End Get
        Set(ByVal value As String)
            clModelo = value
        End Set
    End Property

End Class
