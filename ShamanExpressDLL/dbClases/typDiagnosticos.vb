Public Class typDiagnosticos
    Inherits typAllGenerico01
    Private clGradoOperativoId As typGradosOperativos
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
End Class

