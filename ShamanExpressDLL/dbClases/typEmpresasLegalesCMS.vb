Public Class typEmpresasLegalesCMS
    Inherits typAll
    Private clEmpresaLegalId As typEmpresasLegales
    Private clUniqueId As Int64
    Private clGenerationTime As Date
    Private clExpirationTime As Date
    Private clSign As String
    Private clToken As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property EmpresaLegalId() As typEmpresasLegales
        Get
            Return Me.GetTypProperty(clEmpresaLegalId)
        End Get
        Set(ByVal value As typEmpresasLegales)
            clEmpresaLegalId = value
        End Set
    End Property
    Public Property UniqueId() As Int64
        Get
            Return clUniqueId
        End Get
        Set(ByVal value As Int64)
            clUniqueId = value
        End Set
    End Property
    Public Property GenerationTime() As Date
        Get
            Return clGenerationTime
        End Get
        Set(ByVal value As Date)
            clGenerationTime = value
        End Set
    End Property
    Public Property ExpirationTime() As Date
        Get
            Return clExpirationTime
        End Get
        Set(ByVal value As Date)
            clExpirationTime = value
        End Set
    End Property
    Public Property Sign() As String
        Get
            Return clSign
        End Get
        Set(ByVal value As String)
            clSign = value
        End Set
    End Property
    Public Property Token() As String
        Get
            Return clToken
        End Get
        Set(ByVal value As String)
            clToken = value
        End Set
    End Property

End Class
