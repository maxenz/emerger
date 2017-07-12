Public Class typDepartamentosPuestos
    Inherits typAllGenerico00
    Private clDepartamentoId As typDepartamentos
    Private clPuestoGrilla As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property DepartamentoId() As typDepartamentos
        Get
            Return Me.GetTypProperty(clDepartamentoId)
        End Get
        Set(ByVal value As typDepartamentos)
            clDepartamentoId = value
        End Set
    End Property
    Public Property PuestoGrilla() As String
        Get
            Return clPuestoGrilla
        End Get
        Set(ByVal value As String)
            clPuestoGrilla = value
        End Set
    End Property

End Class
