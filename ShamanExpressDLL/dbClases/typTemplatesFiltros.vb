Public Class typTemplatesFiltros
    Inherits typAll
    Private clNombreObjeto, clTxtObjeto As String
    Private clTemplateId As typTemplates

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Property NombreObjeto() As String
        Get
            Return clNombreObjeto
        End Get
        Set(ByVal value As String)
            clNombreObjeto = value
        End Set
    End Property

    Public Property TxtObjeto() As String
        Get
            Return clTxtObjeto
        End Get
        Set(ByVal value As String)
            clTxtObjeto = value
        End Set
    End Property

    Public Property TemplateId() As typTemplates
        Get
            Return Me.GetTypProperty(clTemplateId)
        End Get
        Set(ByVal value As typTemplates)
            clTemplateId = value
        End Set
    End Property

End Class
