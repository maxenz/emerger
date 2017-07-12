Public Class typIncidentes
    Inherits typAll
    Private clFecIncidente As Date
    Private clNroIncidente As String
    Private clTrasladoId As Int64
    Private clGradoOperativoId As typGradosOperativos
    Private clClienteId As typClientes
    Private clClienteIntegranteId As typClientesIntegrantes
    Private clTelefono As String
    Private clTelefonoFix As Int64
    Private clNroAfiliado As String
    Private clPaciente As String
    Private clSexo As String
    Private clEdad As Decimal
    Private clPlanId As String
    Private clSintomas As String
    Private clCoPago As Decimal
    Private clflgIvaGravado As Integer
    Private clHorarioOperativo As usrHorarioOperativo
    Private cltrsIdaVuelta As Integer
    Private clAviso As String
    Private clObservaciones As String
    Private cleduIntegranteCursoId As typIntegrantesCursos
    Private cleduDivision As String
    Private cleduTurno As String
    Private clNroInterno As String
    Private cllabNroProtocolo As String
    Private cllabTestRapido As Integer
    Private cllabCantidad As Integer
    Private clflgCubierto As Integer
    Private clCoberturaIndividualId As typCoberturasIndividuales
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property FecIncidente() As Date
        Get
            Return clFecIncidente
        End Get
        Set(ByVal value As Date)
            clFecIncidente = value
        End Set
    End Property
    Public Property NroIncidente() As String
        Get
            Return clNroIncidente
        End Get
        Set(ByVal value As String)
            clNroIncidente = value
        End Set
    End Property
    Public Property TrasladoId() As Int64
        Get
            Return clTrasladoId
        End Get
        Set(ByVal value As Int64)
            clTrasladoId = value
        End Set
    End Property
    Public Property GradoOperativoId() As typGradosOperativos
        Get
            Return Me.GetTypProperty(clGradoOperativoId)
        End Get
        Set(ByVal value As typGradosOperativos)
            clGradoOperativoId = value
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
    Public Property ClienteIntegranteId() As typClientesIntegrantes
        Get
            Return Me.GetTypProperty(clClienteIntegranteId)
        End Get
        Set(ByVal value As typClientesIntegrantes)
            clClienteIntegranteId = value
        End Set
    End Property
    Public Property Telefono() As String
        Get
            Return clTelefono
        End Get
        Set(ByVal value As String)
            clTelefono = value
        End Set
    End Property
    Public Property TelefonoFix() As Int64
        Get
            Return clTelefonoFix
        End Get
        Set(ByVal value As Int64)
            clTelefonoFix = value
        End Set
    End Property
    Public Property NroAfiliado() As String
        Get
            Return clNroAfiliado
        End Get
        Set(ByVal value As String)
            clNroAfiliado = value
        End Set
    End Property
    Public Property Paciente() As String
        Get
            Return clPaciente
        End Get
        Set(ByVal value As String)
            clPaciente = value
        End Set
    End Property
    Public Property Sexo() As String
        Get
            Return clSexo
        End Get
        Set(ByVal value As String)
            clSexo = value
        End Set
    End Property
    Public Property Edad() As Decimal
        Get
            Return clEdad
        End Get
        Set(ByVal value As Decimal)
            clEdad = value
        End Set
    End Property
    Public Property PlanId() As String
        Get
            Return clPlanId
        End Get
        Set(ByVal value As String)
            clPlanId = value
        End Set
    End Property
    Public Property Sintomas() As String
        Get
            Return clSintomas
        End Get
        Set(ByVal value As String)
            clSintomas = value
        End Set
    End Property
    Public Property CoPago() As Decimal
        Get
            Return clCoPago
        End Get
        Set(ByVal value As Decimal)
            clCoPago = value
        End Set
    End Property
    Public Property flgIvaGravado() As Integer
        Get
            Return clflgIvaGravado
        End Get
        Set(ByVal value As Integer)
            clflgIvaGravado = value
        End Set
    End Property
    Public Property HorarioOperativo() As usrHorarioOperativo
        Get
            Return clHorarioOperativo
        End Get
        Set(ByVal value As usrHorarioOperativo)
            clHorarioOperativo = value
        End Set
    End Property
    Public Property trsIdaVuelta() As Integer
        Get
            Return cltrsIdaVuelta
        End Get
        Set(ByVal value As Integer)
            cltrsIdaVuelta = value
        End Set
    End Property
    Public Property Aviso() As String
        Get
            Return clAviso
        End Get
        Set(ByVal value As String)
            clAviso = value
        End Set
    End Property
    Public Property eduIntegranteCursoId() As typIntegrantesCursos
        Get
            Return Me.GetTypProperty(cleduIntegranteCursoId)
        End Get
        Set(ByVal value As typIntegrantesCursos)
            cleduIntegranteCursoId = value
        End Set
    End Property
    Public Property eduDivision() As String
        Get
            Return cleduDivision
        End Get
        Set(ByVal value As String)
            cleduDivision = value
        End Set
    End Property
    Public Property eduTurno() As String
        Get
            Return cleduTurno
        End Get
        Set(ByVal value As String)
            cleduTurno = value
        End Set
    End Property
    Public Property NroInterno() As String
        Get
            Return clNroInterno
        End Get
        Set(ByVal value As String)
            clNroInterno = value
        End Set
    End Property
    Public Property labNroProtocolo() As String
        Get
            Return cllabNroProtocolo
        End Get
        Set(ByVal value As String)
            cllabNroProtocolo = value
        End Set
    End Property
    Public Property labTestRapido() As Integer
        Get
            Return cllabTestRapido
        End Get
        Set(ByVal value As Integer)
            cllabTestRapido = value
        End Set
    End Property
    Public Property labCantidad() As Integer
        Get
            Return cllabCantidad
        End Get
        Set(ByVal value As Integer)
            cllabCantidad = value
        End Set
    End Property
    Public Property flgCubierto() As Integer
        Get
            Return clflgCubierto
        End Get
        Set(ByVal value As Integer)
            clflgCubierto = value
        End Set
    End Property
    Public Property CoberturaIndividualId() As typCoberturasIndividuales
        Get
            Return Me.GetTypProperty(clCoberturaIndividualId)
        End Get
        Set(ByVal value As typCoberturasIndividuales)
            clCoberturaIndividualId = value
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

End Class
