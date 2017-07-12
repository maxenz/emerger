Public Class typConceptosFacturacion
    Inherits typAll
    Private clAbreviaturaId As String
    Private clDescripcion As String
    Private clClasificacion As conClasificaciones
    Private clflgLaboratorioTest As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property AbreviaturaId() As String
        Get
            Return clAbreviaturaId
        End Get
        Set(ByVal value As String)
            clAbreviaturaId = value
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
    Public Property Clasificacion() As conClasificaciones
        Get
            Return clClasificacion
        End Get
        Set(ByVal value As conClasificaciones)
            clClasificacion = value
        End Set
    End Property
    Public Property flgLaboratorioTest() As Integer
        Get
            Return clflgLaboratorioTest
        End Get
        Set(ByVal value As Integer)
            clflgLaboratorioTest = value
        End Set
    End Property
End Class
