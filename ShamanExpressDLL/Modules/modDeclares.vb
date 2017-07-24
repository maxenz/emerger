Public Module modDeclares
    '----> Calls
    Public callInfo As String
    '----> Si es dllMode
    Public dllMode As Boolean = True
    '----> Usuario Logueado
    Public logID As Int64
    Public logUsuario As Int64
    Public logPID As Int64
    Public logHKeyPID As Int64
    Public logTerminal As Int64
    Public logAgenteId As String
    Public logPerfilId As Int64
    Public logDespacha As Boolean
    '----> Configuración
    Public shamanProducto As shamanProductos = shamanProductos.Express
    Public shamanConfig As conConfiguracion
    Public shamanConfigClinica As conConfiguracionClinica
    Public shamanSession As conUsuarios
    Public shamanLog As New LogShaman
    Public shamanMensajeria As New Mensajeria
    Public shamanRing As New conAgentesRing
    Public shamanGoogleMaps As New conGoogleMapsImagenes
    Public shamanStartUp As New StartUp
    '----> Constantes
    Public Const cnnDefault As String = "Default"
    Public Const nf As String = "^"
    Public Const sysAdm As String = "SUPERVISOR"
    Public sysHardKey As String
    Public sysLogInit As Boolean = False
    Public sysLogHKey As Boolean = False
    Public sysUsingWS As Boolean = False
    Public sysRemoto As Boolean = False
    Public sysProductos() As String
    Public sysSubExclude() As String
    Public sysVencimiento As Date
    '----> Constantes Nodos Operativos
    Public Const nodDespacho As String = "Despacho"
    Public Const nodRecepcion As String = "Recepcion"
    Public Const nodTraslados As String = "Traslados"
    Public Const nodIntDomDespacho As String = "IntDomDespacho"
    Public Const nodIntDomRecepcion As String = "IntDomRecepcion"
    Public Const nodBitacora As String = "Bitacora"
    Public Const nodGrillaOperativa As String = "GrillaOperativa"
    Public Const nodPreIncidentes As String = "PreIncidentes"
    Public Const nodGPShaman As String = "GPShaman"
    '----> Productos
    Enum shamanProductos
        Express = 1
        Clinicas = 100
        Extranet = 200
    End Enum
    '----> Tipo de Relación de Usos
    Enum modLogin
        lShamanExpress = 0
        lExtranet = 1
    End Enum
    '----> Tipo de Relación de Usos
    Enum tipUsoRelacion
        tReciente = 0
        tFavorito = 1
        tExtranet = 2
    End Enum
    '----> Clasificion
    Enum gdoClasificacion
        gdoTodos = -1
        gdoIncidente = 0
        gdoTraslado = 1
        gdoIntDomiciliaria = 50
        gdoOtro = 100
    End Enum
    '----> Seguridad
    Enum nodAccess
        sSinAcceso = 0
        sSoloLectura = 1
        sEscritura = 2
        sAdministracion = 3
    End Enum
    '-----> Clasificación Comprobantes
    Enum cmpClasificacion
        cmpFacturacion = 0
        cmpCobranzaFacturacion = 1
        cmpFondos = 2
    End Enum
    '-----> Log Facturacion
    Enum logClasificacion
        logInformacion = 0
        logAlerta = 1
        logError = 2
    End Enum
    '----> Productos
    Enum rptReporteadores
        Incidentes = 1
        Clientes = 2
        Personal = 3
        Turnos = 4
        Integrantes = 5
        Insumos = 6
        VentasDocumentos = 7
        Fondos = 8
        TurnosNomenclador = 9
    End Enum
    Enum dynAtributos
        Clientes = 0
        Personas = 1
        Domicilios = 2
        Barcos = 3
        Incidentes = 10
    End Enum
    Enum imgDiseño
        Documentos = 1
        Incidentes = 2
        Clientes = 3
        Integrantes = 4
        Recibos = 5
    End Enum
    Enum vijCombo
        vijConDiagnostico = 1
        vijConCierre = 2
        vijTodos = 3
    End Enum
    Enum msgMetodos
        Email = 1
        Android = 2
        MotoTurbo = 3
        RabbitMQ = 4
    End Enum
    Enum rcpMetodos
        SinMetodo = 0
        WS = 1
        Email = 2
        FTP = 3
        RabbitMQ = 4
    End Enum
    Enum incTiempoCarga
        Presente = 0
        Programado = 1
        Historico = 2
    End Enum
    Enum conClasificaciones
        Servicios = 0
        Adicionales = 1
        PlanesAbonos = 2
        Ajustes = 3
        Otros = 4
        SubConceptos = 5
        Todos = 99
    End Enum
    Enum accMailingClientes
        Recepcion = 1
        Aceptacion = 2
        Rechazos = 3
        Facturacion = 10
    End Enum
    Enum accMailingUsuarios
        BitacoraCritica = 1
        BitacoraImportante = 2
        BitacoraNormal = 3
    End Enum
    Enum segModoLogin
        Shaman = 0
        ActiveDirectory = 1
    End Enum
    Enum callIntegraciones
        NoIntegrado = 0
        Nosco = 1
    End Enum
    Enum ctrTalonarios
        SinControl = 0
        CAI = 1
        CAE = 2
    End Enum
    Enum marDocumentos
        marMailing = 10
    End Enum
    Enum scrOperativa
        EnCurso = 0
        Pendientes = 1
        Programados = 2
        Asignados = 3
    End Enum
    Enum scrMoviles
        Disponibles = 0
        Ocupados = 1
        Todos = 2
    End Enum
    Enum keyMode
        keyAll = 0
        keyHkey = 1
        keyWS = 2
        keyRegistry = 3
    End Enum
    Enum infAptoFisicoItem
        OtrasPatologias = 4
        AntecedenteQuirurgico = 5
    End Enum
    Enum iteMediosPago
        General = 0
        Naranja = 1
        GaliciaPD = 2
        SuCredito = 3
        CitiVentas = 4
        PagoFacil = 5
    End Enum
    Enum itoImpuestos
        IVA = 1
        IIBB = 2
    End Enum
End Module