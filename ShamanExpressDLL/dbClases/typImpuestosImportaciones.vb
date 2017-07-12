Public Class typImpuestosImportaciones
    Inherits typAll
    Private clImpuestoId As typImpuestos
    Private clArchivo As String
    Private clPeriodo As Int64
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ImpuestoId() As typImpuestos
        Get
            Return Me.GetTypProperty(clImpuestoId)
        End Get
        Set(ByVal value As typImpuestos)
            clImpuestoId = value
        End Set
    End Property
    Public Property Archivo() As String
        Get
            Return clArchivo
        End Get
        Set(ByVal value As String)
            clArchivo = value
        End Set
    End Property
    Public Property Periodo() As Int64
        Get
            Return clPeriodo
        End Get
        Set(ByVal value As Int64)
            clPeriodo = value
        End Set
    End Property
End Class
