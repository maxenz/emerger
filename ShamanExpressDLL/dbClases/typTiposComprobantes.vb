Public Class typTiposComprobantes
    Inherits typAll
    Private clAbreviaturaId As String
    Private clDescripcion As String
    Private clClasificacionId As cmpClasificacion
    Private clflgSubDiario As Integer
    Private clflgCobranza As Integer
    Private clCuentaAsientoId As typCuentasAsientos
    Private clDebeHaber As String
    Private clAplicable As Integer
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
    Public Property ClasificacionId() As cmpClasificacion
        Get
            Return clClasificacionId
        End Get
        Set(ByVal value As cmpClasificacion)
            clClasificacionId = value
        End Set
    End Property
    Public Property flgSubDiario() As Integer
        Get
            Return clflgSubDiario
        End Get
        Set(ByVal value As Integer)
            clflgSubDiario = value
        End Set
    End Property
    Public Property flgCobranza() As Integer
        Get
            Return clflgCobranza
        End Get
        Set(ByVal value As Integer)
            clflgCobranza = value
        End Set
    End Property
    Public Property CuentaAsientoId() As typCuentasAsientos
        Get
            Return Me.GetTypProperty(clCuentaAsientoId)
        End Get
        Set(ByVal value As typCuentasAsientos)
            clCuentaAsientoId = value
        End Set
    End Property
    Public Property DebeHaber() As String
        Get
            Return clDebeHaber
        End Get
        Set(ByVal value As String)
            clDebeHaber = value
        End Set
    End Property
    Public Property Aplicable() As Integer
        Get
            Return clAplicable
        End Get
        Set(ByVal value As Integer)
            clAplicable = value
        End Set
    End Property

End Class
