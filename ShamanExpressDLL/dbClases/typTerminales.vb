Public Class typTerminales
    Inherits typAll
    Private clNombrePC As String
    Private clDireccionIP As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property NombrePC() As String
        Get
            Return clNombrePC
        End Get
        Set(ByVal value As String)
            clNombrePC = value
        End Set
    End Property
    Public Property DireccionIP() As String
        Get
            Return clDireccionIP
        End Get
        Set(ByVal value As String)
            clDireccionIP = value
        End Set
    End Property
End Class
