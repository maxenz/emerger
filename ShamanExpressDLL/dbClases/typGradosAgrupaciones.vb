Public Class typGradosAgrupaciones
    Inherits typAllGenerico01
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Overrides ReadOnly Property Tabla() As String
        Get
            Return "GradosOperativos"
        End Get
    End Property
End Class
