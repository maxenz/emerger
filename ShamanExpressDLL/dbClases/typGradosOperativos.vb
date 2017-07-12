Public Class typGradosOperativos
    Inherits typAllGenerico01
    Private clConceptoFacturacion1Id As typConceptosFacturacion
    Private clConceptoFacturacion2Id As typConceptosFacturacion
    Private clOrden As Decimal
    Private clClasificacionId As gdoClasificacion
    Private clflgTraslado As Integer
    Private clflgUrgencia As Integer
    Private clflgEvento As Integer
    Private clflgIntDomiciliaria As Integer
    Private clGradoAgrupacionId As typGradosAgrupaciones
    Private clVisualColor As String
    Private clColorHexa As String
    Private clflgDefault As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ConceptoFacturacion1Id() As typConceptosFacturacion
        Get
            Return Me.GetTypProperty(clConceptoFacturacion1Id)
        End Get
        Set(ByVal value As typConceptosFacturacion)
            clConceptoFacturacion1Id = value
        End Set
    End Property
    Public Property ConceptoFacturacion2Id() As typConceptosFacturacion
        Get
            Return Me.GetTypProperty(clConceptoFacturacion2Id)
        End Get
        Set(ByVal value As typConceptosFacturacion)
            clConceptoFacturacion2Id = value
        End Set
    End Property
    Public Property Orden() As Decimal
        Get
            Return clOrden
        End Get
        Set(ByVal value As Decimal)
            clOrden = value
        End Set
    End Property
    Public Property ClasificacionId() As gdoClasificacion
        Get
            Return clClasificacionId
        End Get
        Set(ByVal value As gdoClasificacion)
            clClasificacionId = value
        End Set
    End Property
    Public Property flgTraslado() As Integer
        Get
            Return clflgTraslado
        End Get
        Set(ByVal value As Integer)
            clflgTraslado = value
        End Set
    End Property
    Public Property flgUrgencia() As Integer
        Get
            Return clflgUrgencia
        End Get
        Set(ByVal value As Integer)
            clflgUrgencia = value
        End Set
    End Property
    Public Property flgEvento() As Integer
        Get
            Return clflgEvento
        End Get
        Set(ByVal value As Integer)
            clflgEvento = value
        End Set
    End Property
    Public Property flgIntDomiciliaria() As Integer
        Get
            Return clflgIntDomiciliaria
        End Get
        Set(ByVal value As Integer)
            clflgIntDomiciliaria = value
        End Set
    End Property
    Public Property GradoAgrupacionId() As typGradosAgrupaciones
        Get
            Return Me.GetTypProperty(clGradoAgrupacionId)
        End Get
        Set(ByVal value As typGradosAgrupaciones)
            clGradoAgrupacionId = value
        End Set
    End Property
    Public Property VisualColor() As String
        Get
            Return clVisualColor
        End Get
        Set(ByVal value As String)
            clVisualColor = value
        End Set
    End Property
    Public Property ColorHexa() As String
        Get
            Return clColorHexa
        End Get
        Set(ByVal value As String)
            clColorHexa = value
        End Set
    End Property
    Public Property flgDefault() As Integer
        Get
            Return clflgDefault
        End Get
        Set(ByVal value As Integer)
            clflgDefault = value
        End Set
    End Property


End Class