Public Class typPerfilesNodos
    Inherits typAll
    Private clPerfilId As typPerfiles
    Private clsysNodoId As typsysNodos
    Private clAcceso As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PerfilId() As typPerfiles
        Get
            Return Me.GetTypProperty(clPerfilId)
        End Get
        Set(ByVal value As typPerfiles)
            clPerfilId = value
        End Set
    End Property
    Public Property sysNodoId() As typsysNodos
        Get
            Return Me.GetTypProperty(clsysNodoId)
        End Get
        Set(ByVal value As typsysNodos)
            clsysNodoId = value
        End Set
    End Property
    Public Property Acceso() As Integer
        Get
            Return clAcceso
        End Get
        Set(ByVal value As Integer)
            clAcceso = value
        End Set
    End Property
End Class
