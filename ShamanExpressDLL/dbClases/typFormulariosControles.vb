Public Class typFormulariosControles
    Inherits typAll

    Private clNombreFormulario As String
    Private clUsuariosPreferenciasID As typUsuariosPreferencias
    Private clNombreControl As String
    Private clValor As String
    Private clXML As String

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Property UsuariosPreferenciasID() As typUsuariosPreferencias
        Get
            Return clUsuariosPreferenciasID
        End Get
        Set(ByVal value As typUsuariosPreferencias)
            clUsuariosPreferenciasID = value
        End Set

    End Property
    Public Property NombreFormulario() As String
        Get
            Return clNombreFormulario
        End Get
        Set(ByVal value As String)
            clNombreFormulario = value
        End Set
    End Property
    
    Public Property NombreControl() As String
        Get
            Return clNombreControl
        End Get
        Set(ByVal value As String)
            clNombreControl = value
        End Set
    End Property
    Public Property Valor() As String
        Get
            Return clValor
        End Get
        Set(ByVal value As String)
            clValor = value
        End Set
    End Property
    Public Property XML() As String
        Get
            Return clXML
        End Get
        Set(ByVal value As String)
            clXML = value
        End Set
    End Property
    
End Class

