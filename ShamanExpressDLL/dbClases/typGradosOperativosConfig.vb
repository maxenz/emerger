Public Class typGradosOperativosConfig
    Inherits typAll
    Private clGradoOperativoId As typGradosOperativos
    Private clTipoConfiguracion As String
    Private clvalDesde As Decimal
    Private clvalHasta As Decimal
    Private clvalRefString As String
    Private clvalRefNumeric As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property GradoOperativoId() As typGradosOperativos
        Get
            Return Me.GetTypProperty(clGradoOperativoId)
        End Get
        Set(ByVal value As typGradosOperativos)
            clGradoOperativoId = value
        End Set
    End Property
    Public Property TipoConfiguracion() As String
        Get
            Return clTipoConfiguracion
        End Get
        Set(ByVal value As String)
            clTipoConfiguracion = value
        End Set
    End Property
    Public Property valDesde() As Decimal
        Get
            Return clvalDesde
        End Get
        Set(ByVal value As Decimal)
            clvalDesde = value
        End Set
    End Property
    Public Property valHasta() As Decimal
        Get
            Return clvalHasta
        End Get
        Set(ByVal value As Decimal)
            clvalHasta = value
        End Set
    End Property
    Public Property valRefString() As String
        Get
            Return clvalRefString
        End Get
        Set(ByVal value As String)
            clvalRefString = value
        End Set
    End Property
    Public Property valRefNumeric() As Decimal
        Get
            Return clvalRefNumeric
        End Get
        Set(ByVal value As Decimal)
            clvalRefNumeric = value
        End Set
    End Property

End Class