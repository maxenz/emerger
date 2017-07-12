Public Class typPersonalMatriculas
    Inherits typAll
    Private clPersonalId As typPersonal
    Private clTipoMatriculaId As typTiposMatriculas
    Private clFecVencimiento As Date
    Private clReferencia As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PersonalId() As typPersonal
        Get
            Return Me.GetTypProperty(clPersonalId)
        End Get
        Set(ByVal value As typPersonal)
            clPersonalId = value
        End Set
    End Property
    Public Property TipoMatriculaId() As typTiposMatriculas
        Get
            Return Me.GetTypProperty(clTipoMatriculaId)
        End Get
        Set(ByVal value As typTiposMatriculas)
            clTipoMatriculaId = value
        End Set
    End Property
    Public Property FecVencimiento() As Date
        Get
            Return clFecVencimiento
        End Get
        Set(ByVal value As Date)
            clFecVencimiento = value
        End Set
    End Property
    Public Property Referencia() As String
        Get
            Return clReferencia
        End Get
        Set(ByVal value As String)
            clReferencia = value
        End Set
    End Property
End Class
