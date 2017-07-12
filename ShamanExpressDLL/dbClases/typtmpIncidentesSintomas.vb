Public Class typtmpIncidentesSintomas
    Inherits typAll
    Private clPID As Int64
    Private clSintomaId As typSintomas
    Private clDescripcion As String
    Private clGradoOperativoId As typGradosOperativos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PID() As Int64
        Get
            Return clPID
        End Get
        Set(ByVal value As Int64)
            clPID = value
        End Set
    End Property
    Public Property SintomaId() As typSintomas
        Get
            Return Me.GetTypProperty(clSintomaId)
        End Get
        Set(ByVal value As typSintomas)
            clSintomaId = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
        End Set
    End Property
    Public Property GradoOperativoId() As typGradosOperativos
        Get
            Return Me.GetTypProperty(clGradoOperativoId)
        End Get
        Set(ByVal value As typGradosOperativos)
            clGradoOperativoId = value
        End Set
    End Property

End Class
