Public Class typReporteadoresFields
    Inherits typAll
    Private clReporteadorId As typReporteadores
    Private clAliasTabla As String
    Private clDescripcion As String
    Private clCampo As String
    Private clOrdenImportacion As Integer
    Private clflgReqImportacion As Integer
    Private clflgSoloImportacion As Integer

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Property ReporteadorId() As typReporteadores
        Get
            Return Me.GetTypProperty(clReporteadorId)
        End Get
        Set(ByVal value As typReporteadores)
            clReporteadorId = value
        End Set
    End Property

    Public Property AliasTabla() As String
        Get
            Return clAliasTabla
        End Get
        Set(ByVal value As String)
            clAliasTabla = value
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

    Public Property Campo() As String
        Get
            Return clCampo
        End Get
        Set(ByVal value As String)
            clCampo = value
        End Set
    End Property

    Public Property OrdenImportacion() As Integer
        Get
            Return clOrdenImportacion
        End Get
        Set(ByVal value As Integer)
            clOrdenImportacion = value
        End Set
    End Property

    Public Property flgReqImportacion() As Integer
        Get
            Return clflgReqImportacion
        End Get
        Set(ByVal value As Integer)
            clflgReqImportacion = value
        End Set
    End Property

    Public Property flgSoloImportacion() As Integer
        Get
            Return clflgSoloImportacion
        End Get
        Set(ByVal value As Integer)
            clflgSoloImportacion = value
        End Set
    End Property

End Class
