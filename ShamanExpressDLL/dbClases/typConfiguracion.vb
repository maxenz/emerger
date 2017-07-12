Public Class typConfiguracion
    Inherits typAll
    Private clprmLimitePediatrico As Integer
    Private clhsNocDesde As String
    Private clhsNocHasta As String
    Private clrcpTpoHC As Integer
    Private clUrlMapas As String
    Private clUrlGPShaman As String
    Private clflgServicioMapas As Integer
    Private clflgTpoSalidaBase As Integer
    Private clopeRefresh As Integer
    Private clflgCargaHistorica As Integer
    Private clmodSinCobertura As Integer
    Private clpathProduccion As String
    Private clopeColumnaCliente As Integer
    Private clopeColumnaLlamTraslado As Integer
    Private clopeOrdenamiento As Integer
    Private clflgEduCarga As Integer
    Private cltpoMorosidad As Integer
    Private clmodMorosidad As Integer
    Private clincReposicionCierre As Integer
    Private clincPracticasCierre As Integer
    Private clincEncuestaCierre As Integer
    Private clmodNumeracion As Integer
    Private clConfiguracionRegionalId As typConfiguracionesRegionales
    Private clClienteDefaultId As typClientes
    Private clflgNauticoCarga As Integer
    Private clcliGeneracionCodigo As Integer
    Private clfacTextoRenglon As String
    Private clfacTextoAbono As String
    Private clfacTextoBonificacion As String
    Private clfacTextoRecargo As String
    Private clfacModuloPlan As Integer
    Private clopeModoMensajeria As Integer
    Private clopeMensajeriaRef As String
    Private clopeNroInterno As Integer
    Private clusrAndroidId As typUsuarios
    Private clflgIncImagenPer As Integer
    Private clflgModCierre As Integer
    Private clflgLaboratorio As Integer
    Private clopeRecibeServicios As Integer
    Private clVersion As String
    Private clNodesVerifyVersion As String
    Private clsegModoLogin As segModoLogin
    Private clsegServidorDominio As String
    Private clIntegracionCallCenter As callIntegraciones
    Private clUrlGrabaciones As String
    Private clflgDespachoPerfiles As Integer
    Private clcliPadronesImportacion As Integer
    Private clopeRecPadronInc As Integer
    Private clopeRecPadronTra As Integer
    Private clinsStockNegativo As Integer
    Private clopeClientesInactivos As Integer
    Private clkmtObtencion As Integer
    Private clopeCoberturaMoviles As Integer
    Private clfacPrecioPlanIva As Integer
    Private clfacPrecioAbonoIva As Integer
    Private clopeDspObsEmpresa As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property prmLimitePediatrico() As Integer
        Get
            Return clprmLimitePediatrico
        End Get
        Set(ByVal value As Integer)
            clprmLimitePediatrico = value
        End Set
    End Property
    Public Property hsNocDesde() As String
        Get
            Return clhsNocDesde
        End Get
        Set(ByVal value As String)
            clhsNocDesde = value
        End Set
    End Property
    Public Property hsNocHasta() As String
        Get
            Return clhsNocHasta
        End Get
        Set(ByVal value As String)
            clhsNocHasta = value
        End Set
    End Property
    Public Property rcpTpoHC() As Integer
        Get
            Return clrcpTpoHC
        End Get
        Set(ByVal value As Integer)
            clrcpTpoHC = value
        End Set
    End Property
    Public Property UrlMapas() As String
        Get
            Return clUrlMapas
        End Get
        Set(ByVal value As String)
            clUrlMapas = value
        End Set
    End Property
    Public Property UrlGPShaman() As String
        Get
            Return clUrlGPShaman
        End Get
        Set(ByVal value As String)
            clUrlGPShaman = value
        End Set
    End Property
    Public Property flgServicioMapas() As Integer
        Get
            Return clflgServicioMapas
        End Get
        Set(ByVal value As Integer)
            clflgServicioMapas = value
        End Set
    End Property
    Public Property flgTpoSalidaBase() As Integer
        Get
            Return clflgTpoSalidaBase
        End Get
        Set(ByVal value As Integer)
            clflgTpoSalidaBase = value
        End Set
    End Property
    Public Property opeRefresh() As Integer
        Get
            Return clopeRefresh
        End Get
        Set(ByVal value As Integer)
            clopeRefresh = value
        End Set
    End Property
    Public Property flgCargaHistorica() As Integer
        Get
            Return clflgCargaHistorica
        End Get
        Set(ByVal value As Integer)
            clflgCargaHistorica = value
        End Set
    End Property
    Public Property modSinCobertura() As Integer
        Get
            Return clmodSinCobertura
        End Get
        Set(ByVal value As Integer)
            clmodSinCobertura = value
        End Set
    End Property
    Public Property pathProduccion() As String
        Get
            Return clpathProduccion
        End Get
        Set(ByVal value As String)
            clpathProduccion = value
        End Set
    End Property
    Public Property opeColumnaCliente() As Integer
        Get
            Return clopeColumnaCliente
        End Get
        Set(ByVal value As Integer)
            clopeColumnaCliente = value
        End Set
    End Property
    Public Property opeColumnaLlamTraslado() As Integer
        Get
            Return clopeColumnaLlamTraslado
        End Get
        Set(ByVal value As Integer)
            clopeColumnaLlamTraslado = value
        End Set
    End Property
    Public Property opeOrdenamiento() As Integer
        Get
            Return clopeOrdenamiento
        End Get
        Set(ByVal value As Integer)
            clopeOrdenamiento = value
        End Set
    End Property
    Public Property flgEduCarga() As Integer
        Get
            Return clflgEduCarga
        End Get
        Set(ByVal value As Integer)
            clflgEduCarga = value
        End Set
    End Property
    Public Property tpoMorosidad() As Integer
        Get
            Return cltpoMorosidad
        End Get
        Set(ByVal value As Integer)
            cltpoMorosidad = value
        End Set
    End Property
    Public Property modMorosidad() As Integer
        Get
            Return clmodMorosidad
        End Get
        Set(ByVal value As Integer)
            clmodMorosidad = value
        End Set
    End Property
    Public Property incReposicionCierre() As Integer
        Get
            Return clincReposicionCierre
        End Get
        Set(ByVal value As Integer)
            clincReposicionCierre = value
        End Set
    End Property
    Public Property incPracticasCierre() As Integer
        Get
            Return clincPracticasCierre
        End Get
        Set(ByVal value As Integer)
            clincPracticasCierre = value
        End Set
    End Property
    Public Property incEncuestaCierre() As Integer
        Get
            Return clincEncuestaCierre
        End Get
        Set(ByVal value As Integer)
            clincEncuestaCierre = value
        End Set
    End Property
    Public Property modNumeracion() As Integer
        Get
            Return clmodNumeracion
        End Get
        Set(ByVal value As Integer)
            clmodNumeracion = value
        End Set
    End Property
    Public Property ConfiguracionRegionalId() As typConfiguracionesRegionales
        Get
            Return Me.GetTypProperty(clConfiguracionRegionalId)
        End Get
        Set(ByVal value As typConfiguracionesRegionales)
            clConfiguracionRegionalId = value
        End Set
    End Property
    Public Property ClienteDefaultId() As typClientes
        Get
            Return Me.GetTypProperty(clClienteDefaultId)
        End Get
        Set(ByVal value As typClientes)
            clClienteDefaultId = value
        End Set
    End Property
    Public Property flgNauticoCarga() As Integer
        Get
            Return clflgNauticoCarga
        End Get
        Set(ByVal value As Integer)
            clflgNauticoCarga = value
        End Set
    End Property
    Public Property cliGeneracionCodigo() As Integer
        Get
            Return clcliGeneracionCodigo
        End Get
        Set(ByVal value As Integer)
            clcliGeneracionCodigo = value
        End Set
    End Property
    Public Property facTextoRenglon() As String
        Get
            Return clfacTextoRenglon
        End Get
        Set(ByVal value As String)
            clfacTextoRenglon = value
        End Set
    End Property
    Public Property facTextoAbono() As String
        Get
            Return clfacTextoAbono
        End Get
        Set(ByVal value As String)
            clfacTextoAbono = value
        End Set
    End Property
    Public Property facTextoBonificacion() As String
        Get
            Return clfacTextoBonificacion
        End Get
        Set(ByVal value As String)
            clfacTextoBonificacion = value
        End Set
    End Property
    Public Property facTextoRecargo() As String
        Get
            Return clfacTextoRecargo
        End Get
        Set(ByVal value As String)
            clfacTextoRecargo = value
        End Set
    End Property
    Public Property facModuloPlan() As Integer
        Get
            Return clfacModuloPlan
        End Get
        Set(ByVal value As Integer)
            clfacModuloPlan = value
        End Set
    End Property
    Public Property opeModoMensajeria() As Integer
        Get
            Return clopeModoMensajeria
        End Get
        Set(ByVal value As Integer)
            clopeModoMensajeria = value
        End Set
    End Property
    Public Property opeMensajeriaRef() As String
        Get
            Return clopeMensajeriaRef
        End Get
        Set(ByVal value As String)
            clopeMensajeriaRef = value
        End Set
    End Property
    Public Property opeNroInterno() As Integer
        Get
            Return clopeNroInterno
        End Get
        Set(ByVal value As Integer)
            clopeNroInterno = value
        End Set
    End Property
    Public Property usrAndroidId() As typUsuarios
        Get
            Return Me.GetTypProperty(clusrAndroidId)
        End Get
        Set(ByVal value As typUsuarios)
            clusrAndroidId = value
        End Set
    End Property
    Public Property flgIncImagenPer() As Integer
        Get
            Return clflgIncImagenPer
        End Get
        Set(ByVal value As Integer)
            clflgIncImagenPer = value
        End Set
    End Property
    Public Property flgModCierre() As Integer
        Get
            Return clflgModCierre
        End Get
        Set(ByVal value As Integer)
            clflgModCierre = value
        End Set
    End Property
    Public Property opeRecibeServicios() As Integer
        Get
            Return clopeRecibeServicios
        End Get
        Set(ByVal value As Integer)
            clopeRecibeServicios = value
        End Set
    End Property
    Public Property flgLaboratorio() As Integer
        Get
            Return clflgLaboratorio
        End Get
        Set(ByVal value As Integer)
            clflgLaboratorio = value
        End Set
    End Property
    Public Property Version() As String
        Get
            Return clVersion
        End Get
        Set(ByVal value As String)
            clVersion = value
        End Set
    End Property
    Public Property NodesVerifyVersion() As String
        Get
            Return clNodesVerifyVersion
        End Get
        Set(ByVal value As String)
            clNodesVerifyVersion = value
        End Set
    End Property
    Public Property segModoLogin() As segModoLogin
        Get
            Return clsegModoLogin
        End Get
        Set(ByVal value As segModoLogin)
            clsegModoLogin = value
        End Set
    End Property
    Public Property segServidorDominio() As String
        Get
            Return clsegServidorDominio
        End Get
        Set(ByVal value As String)
            clsegServidorDominio = value
        End Set
    End Property
    Public Property UrlGrabaciones() As String
        Get
            Return clUrlGrabaciones
        End Get
        Set(ByVal value As String)
            clUrlGrabaciones = value
        End Set
    End Property
    Public Property IntegracionCallCenter() As callIntegraciones
        Get
            Return clIntegracionCallCenter
        End Get
        Set(ByVal value As callIntegraciones)
            clIntegracionCallCenter = value
        End Set
    End Property
    Public Property flgDespachoPerfiles() As Integer
        Get
            Return clflgDespachoPerfiles
        End Get
        Set(ByVal value As Integer)
            clflgDespachoPerfiles = value
        End Set
    End Property
    Public Property cliPadronesImportacion() As Integer
        Get
            Return clcliPadronesImportacion
        End Get
        Set(ByVal value As Integer)
            clcliPadronesImportacion = value
        End Set
    End Property
    Public Property opeRecPadronInc() As Integer
        Get
            Return clopeRecPadronInc
        End Get
        Set(ByVal value As Integer)
            clopeRecPadronInc = value
        End Set
    End Property
    Public Property opeRecPadronTra() As Integer
        Get
            Return clopeRecPadronTra
        End Get
        Set(ByVal value As Integer)
            clopeRecPadronTra = value
        End Set
    End Property
    Public Property insStockNegativo() As Integer
        Get
            Return clinsStockNegativo
        End Get
        Set(ByVal value As Integer)
            clinsStockNegativo = value
        End Set
    End Property
    Public Property opeClientesInactivos() As Integer
        Get
            Return clopeClientesInactivos
        End Get
        Set(ByVal value As Integer)
            clopeClientesInactivos = value
        End Set
    End Property
    Public Property kmtObtencion() As Integer
        Get
            Return clkmtObtencion
        End Get
        Set(ByVal value As Integer)
            clkmtObtencion = value
        End Set
    End Property
    Public Property opeCoberturaMoviles() As Integer
        Get
            Return clopeCoberturaMoviles
        End Get
        Set(ByVal value As Integer)
            clopeCoberturaMoviles = value
        End Set
    End Property
    Public Property facPrecioPlanIva() As Integer
        Get
            Return clfacPrecioPlanIva
        End Get
        Set(ByVal value As Integer)
            clfacPrecioPlanIva = value
        End Set
    End Property
    Public Property facPrecioAbonoIva() As Integer
        Get
            Return clfacPrecioAbonoIva
        End Get
        Set(ByVal value As Integer)
            clfacPrecioAbonoIva = value
        End Set
    End Property
    Public Property opeDspObsEmpresa() As Integer
        Get
            Return clopeDspObsEmpresa
        End Get
        Set(ByVal value As Integer)
            clopeDspObsEmpresa = value
        End Set
    End Property

End Class
