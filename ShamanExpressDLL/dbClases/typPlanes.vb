Public Class typPlanes
    Inherits typAll
    Private clAbreviaturaId As String
    Private clDescripcion As String
    Private clPlanFamiliaId As typPlanesFamilias
    Private clTarifaId As typTarifas
    Private clfacAgpReqIntegrante As Integer
    Private clfacFormaBonificacion As Integer
    Private clfacPorBonificacion As Decimal
    Private clfacFormaRecargo As Integer
    Private claplExcedente As Integer
    Private clObservaciones As String
    Private clClienteId As typClientes
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
    Public Property PlanFamiliaId() As typPlanesFamilias
        Get
            Return Me.GetTypProperty(clPlanFamiliaId)
        End Get
        Set(ByVal value As typPlanesFamilias)
            clPlanFamiliaId = value
        End Set
    End Property
    Public Property TarifaId() As typTarifas
        Get
            Return Me.GetTypProperty(clTarifaId)
        End Get
        Set(ByVal value As typTarifas)
            clTarifaId = value
        End Set
    End Property
    Public Property facAgpReqIntegrante() As Integer
        Get
            Return clfacAgpReqIntegrante
        End Get
        Set(ByVal value As Integer)
            clfacAgpReqIntegrante = value
        End Set
    End Property
    Public Property facFormaBonificacion() As Integer
        Get
            Return clfacFormaBonificacion
        End Get
        Set(ByVal value As Integer)
            clfacFormaBonificacion = value
        End Set
    End Property
    Public Property facFormaRecargo() As Integer
        Get
            Return clfacFormaRecargo
        End Get
        Set(ByVal value As Integer)
            clfacFormaRecargo = value
        End Set
    End Property
    Public Property aplExcedente() As Integer
        Get
            Return claplExcedente
        End Get
        Set(ByVal value As Integer)
            claplExcedente = value
        End Set
    End Property
    Public Property facPorBonificacion() As Decimal
        Get
            Return clfacPorBonificacion
        End Get
        Set(ByVal value As Decimal)
            clfacPorBonificacion = value
        End Set
    End Property
    Public Property Observaciones() As String
        Get
            Return clObservaciones
        End Get
        Set(ByVal value As String)
            clObservaciones = value
        End Set
    End Property
    Public Property ClienteId() As typClientes
        Get
            Return Me.GetTypProperty(clClienteId)
        End Get
        Set(ByVal value As typClientes)
            clClienteId = value
        End Set
    End Property
End Class

