Public Class typClientes
    Inherits typAll
    Private clAbreviaturaId As String
    Private clRazonSocial As String
    Private clRubroClienteId As typRubrosClientes
    Private clDomicilio As usrDomicilio
    Private clLocalidadId As typLocalidades
    Private clCodigoPostal As String
    Private clCUIT As String
    Private clSituacionIvaId As typSituacionesIva
    Private clAlicuotaIvaId As typAlicuotasIva
    Private clEmpresaLegalId As typEmpresasLegales
    Private clLetraHabitual As String
    Private clflgCategorizacionPropia As Integer
    Private clFecIngreso As Date
    Private clFecEgreso As Date
    Private clActivo As Integer
    Private clPlanId As typPlanes
    Private clfacForma As Integer
    Private clfacFrecuencia As Integer
    Private clfacFormaBonificacion As Integer
    Private clfacImporte As Decimal
    Private clfacPorBonificacion As Decimal
    Private clfacFormaRecargo As Integer
    Private clFormaPagoId As typFormasPago
    Private clCobradorId As typCobradores
    Private clVendedorId As typCobradores
    Private clNroContrato As String
    Private clSaldo As Decimal
    Private clSaldoInicial As Decimal
    Private clUltimoPerEmitido As Int64
    Private clflgIngresoSubGrupos As Integer
    Private clTipoConvenio As Integer
    Private clConvenioId As typClientes
    Private clMotivoBajaId As typMotivosBajas
    Private clObservaciones As String
    Private clObservacionesBaja As String
    Private clrcpMetodoId As rcpMetodos
    Private clrcpWSKey As Int64
    Private clrcpRabbitAlias As String
    Private clrcpObservaciones As String
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
    Public Property RazonSocial() As String
        Get
            Return clRazonSocial
        End Get
        Set(ByVal value As String)
            clRazonSocial = value
        End Set
    End Property
    Public Property RubroClienteId() As typRubrosClientes
        Get
            Return Me.GetTypProperty(clRubroClienteId)
        End Get
        Set(ByVal value As typRubrosClientes)
            clRubroClienteId = value
        End Set
    End Property
    Public Property Domicilio() As usrDomicilio
        Get
            Return clDomicilio
        End Get
        Set(ByVal value As usrDomicilio)
            clDomicilio = value
        End Set
    End Property
    Public Property LocalidadId() As typLocalidades
        Get
            Return Me.GetTypProperty(clLocalidadId)
        End Get
        Set(ByVal value As typLocalidades)
            clLocalidadId = value
        End Set
    End Property
    Public Property CodigoPostal() As String
        Get
            Return clCodigoPostal
        End Get
        Set(ByVal value As String)
            clCodigoPostal = value
        End Set
    End Property
    Public Property CUIT() As String
        Get
            Return clCUIT
        End Get
        Set(ByVal value As String)
            clCUIT = value
        End Set
    End Property
    Public Property SituacionIvaId() As typSituacionesIva
        Get
            Return Me.GetTypProperty(clSituacionIvaId)
        End Get
        Set(ByVal value As typSituacionesIva)
            clSituacionIvaId = value
        End Set
    End Property
    Public Property AlicuotaIvaId() As typAlicuotasIva
        Get
            Return Me.GetTypProperty(clAlicuotaIvaId)
        End Get
        Set(ByVal value As typAlicuotasIva)
            clAlicuotaIvaId = value
        End Set
    End Property
    Public Property EmpresaLegalId() As typEmpresasLegales
        Get
            Return Me.GetTypProperty(clEmpresaLegalId)
        End Get
        Set(ByVal value As typEmpresasLegales)
            clEmpresaLegalId = value
        End Set
    End Property
    Public Property LetraHabitual() As String
        Get
            Return clLetraHabitual
        End Get
        Set(ByVal value As String)
            clLetraHabitual = value
        End Set
    End Property
    Public Property flgCategorizacionPropia() As Integer
        Get
            Return clflgCategorizacionPropia
        End Get
        Set(ByVal value As Integer)
            clflgCategorizacionPropia = value
        End Set
    End Property
    Public Property facForma() As Integer
        Get
            Return clfacForma
        End Get
        Set(ByVal value As Integer)
            clfacForma = value
        End Set
    End Property
    Public Property facFrecuencia() As Integer
        Get
            Return clfacFrecuencia
        End Get
        Set(ByVal value As Integer)
            clfacFrecuencia = value
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
    Public Property FecIngreso() As Date
        Get
            Return clFecIngreso
        End Get
        Set(ByVal value As Date)
            clFecIngreso = value
        End Set
    End Property
    Public Property FecEgreso() As Date
        Get
            Return clFecEgreso
        End Get
        Set(ByVal value As Date)
            clFecEgreso = value
        End Set
    End Property
    Public Property facImporte() As Decimal
        Get
            Return clfacImporte
        End Get
        Set(ByVal value As Decimal)
            clfacImporte = value
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
    Public Property FormaPagoId() As typFormasPago
        Get
            Return Me.GetTypProperty(clFormaPagoId)
        End Get
        Set(ByVal value As typFormasPago)
            clFormaPagoId = value
        End Set
    End Property
    Public Property CobradorId() As typCobradores
        Get
            Return Me.GetTypProperty(clCobradorId)
        End Get
        Set(ByVal value As typCobradores)
            clCobradorId = value
        End Set
    End Property
    Public Property VendedorId() As typCobradores
        Get
            Return Me.GetTypProperty(clVendedorId)
        End Get
        Set(ByVal value As typCobradores)
            clVendedorId = value
        End Set
    End Property
    Public Property PlanId() As typPlanes
        Get
            Return Me.GetTypProperty(clPlanId)
        End Get
        Set(ByVal value As typPlanes)
            clPlanId = value
        End Set
    End Property
    Public Property Activo() As Integer
        Get
            Return clActivo
        End Get
        Set(ByVal value As Integer)
            clActivo = value
        End Set
    End Property
    Public Property NroContrato() As String
        Get
            Return clNroContrato
        End Get
        Set(ByVal value As String)
            clNroContrato = value
        End Set
    End Property
    Public Property Saldo() As Decimal
        Get
            Return clSaldo
        End Get
        Set(ByVal value As Decimal)
            clSaldo = value
        End Set
    End Property
    Public Property SaldoInicial() As Decimal
        Get
            Return clSaldoInicial
        End Get
        Set(ByVal value As Decimal)
            clSaldoInicial = value
        End Set
    End Property
    Public Property UltimoPerEmitido() As Int64
        Get
            Return clUltimoPerEmitido
        End Get
        Set(ByVal value As Int64)
            clUltimoPerEmitido = value
        End Set
    End Property
    Public Property flgIngresoSubGrupos() As Integer
        Get
            Return clflgIngresoSubGrupos
        End Get
        Set(ByVal value As Integer)
            clflgIngresoSubGrupos = value
        End Set
    End Property
    Public Property TipoConvenio() As Integer
        Get
            Return clTipoConvenio
        End Get
        Set(ByVal value As Integer)
            clTipoConvenio = value
        End Set
    End Property
    Public Property ConvenioId() As typClientes
        Get
            Return Me.GetTypProperty(clConvenioId)
        End Get
        Set(ByVal value As typClientes)
            clConvenioId = value
        End Set
    End Property
    Public Property MotivoBajaId() As typMotivosBajas
        Get
            Return Me.GetTypProperty(clMotivoBajaId)
        End Get
        Set(ByVal value As typMotivosBajas)
            clMotivoBajaId = value
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
    Public Property ObservacionesBaja() As String
        Get
            Return clObservacionesBaja
        End Get
        Set(ByVal value As String)
            clObservacionesBaja = value
        End Set
    End Property
    Public Property rcpMetodoId() As rcpMetodos
        Get
            Return clrcpMetodoId
        End Get
        Set(ByVal value As rcpMetodos)
            clrcpMetodoId = value
        End Set
    End Property
    Public Property rcpWSKey() As Int64
        Get
            Return clrcpWSKey
        End Get
        Set(ByVal value As Int64)
            clrcpWSKey = value
        End Set
    End Property
    Public Property rcpRabbitAlias() As String
        Get
            Return clrcpRabbitAlias
        End Get
        Set(ByVal value As String)
            clrcpRabbitAlias = value
        End Set
    End Property
    Public Property rcpObservaciones() As String
        Get
            Return clrcpObservaciones
        End Get
        Set(ByVal value As String)
            clrcpObservaciones = value
        End Set
    End Property


End Class
