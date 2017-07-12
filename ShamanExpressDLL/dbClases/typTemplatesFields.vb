Public Class typTemplatesFields
    Inherits typAll
    Private clTemplateId As typTemplates
    Private clReporteadorFieldId As typReporteadoresFields
    Private clNroAgrupacion As Integer
    Private clValorDefault As String

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Property TemplateId() As typTemplates
        Get
            Return Me.GetTypProperty(clTemplateId)
        End Get
        Set(ByVal value As typTemplates)
            clTemplateId = value
        End Set
    End Property

    Public Property ReporteadorFieldId() As typReporteadoresFields
        Get
            Return Me.GetTypProperty(clReporteadorFieldId)
        End Get
        Set(ByVal value As typReporteadoresFields)
            clReporteadorFieldId = value
        End Set
    End Property

    Public Property NroAgrupacion() As Integer
        Get
            Return clNroAgrupacion
        End Get
        Set(ByVal value As Integer)
            clNroAgrupacion = value
        End Set
    End Property

    Public Property ValorDefault() As String
        Get
            Return clValorDefault
        End Get
        Set(ByVal value As String)
            clValorDefault = value
        End Set
    End Property

End Class
