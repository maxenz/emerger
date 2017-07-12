Public Class typIntegrantesClasificaciones
    Inherits typAllGenerico01
    Private clflgIngresoMultiple As Integer
    Private clflgTitularSubGrupo As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property flgIngresoMultiple() As Integer
        Get
            Return clflgIngresoMultiple
        End Get
        Set(ByVal value As Integer)
            clflgIngresoMultiple = value
        End Set
    End Property
    Public Property flgTitularSubGrupo() As Integer
        Get
            Return clflgTitularSubGrupo
        End Get
        Set(ByVal value As Integer)
            clflgTitularSubGrupo = value
        End Set
    End Property
End Class
