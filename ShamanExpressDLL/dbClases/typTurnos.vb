Public Class typTurnos
    Inherits typAll
    Private clFechaHoraTurno As DateTime
    Private clPracticaId As typPracticas
    Private clCentroAtencionSalaId As typCentrosAtencionSalas
    Private clPersonalId As typPersonal
    Private clClienteId As typClientes
    Private clClienteIntegranteId As typClientesIntegrantes
    Private clTelefono As String
    Private clTelefonoFix As Int64
    Private clNroAfiliado As String
    Private clPaciente As String
    Private clSexo As String
    Private clEdad As Decimal
    Private clPlanId As String
    Private clCoPago As Decimal
    Private clflgIvaGravado As Integer
    Private clAviso As String
    Private clObsEntrada As String
    Private clObsSalida As String
    Private clFechaHoraEstado As DateTime
    Private clEstadoTurnoId As typEstadosTurnos
    Private clTipoDocumentoId As typTiposDocumentos
    Private clNroDocumento As Int64
    Private clTpoTurno As Integer
    Private clNroOrdenConsulta As String
    Private clModalidad As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property FechaHoraTurno() As DateTime
        Get
            Return clFechaHoraTurno
        End Get
        Set(ByVal value As DateTime)
            clFechaHoraTurno = value
        End Set
    End Property
    Public Property PracticaId() As typPracticas
        Get
            Return Me.GetTypProperty(clPracticaId)
        End Get
        Set(ByVal value As typPracticas)
            clPracticaId = value
        End Set
    End Property
    Public Property CentroAtencionSalaId() As typCentrosAtencionSalas
        Get
            Return Me.GetTypProperty(clCentroAtencionSalaId)
        End Get
        Set(ByVal value As typCentrosAtencionSalas)
            clCentroAtencionSalaId = value
        End Set
    End Property
    Public Property PersonalId() As typPersonal
        Get
            Return Me.GetTypProperty(clPersonalId)
        End Get
        Set(ByVal value As typPersonal)
            clPersonalId = value
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
    Public Property ObsEntrada() As String
        Get
            Return clObsEntrada
        End Get
        Set(ByVal value As String)
            clObsEntrada = value
        End Set
    End Property
    Public Property ObsSalida() As String
        Get
            Return clObsSalida
        End Get
        Set(ByVal value As String)
            clObsSalida = value
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
    Public Property FechaHoraEstado() As DateTime
        Get
            Return clFechaHoraEstado
        End Get
        Set(ByVal value As DateTime)
            clFechaHoraEstado = value
        End Set
    End Property
    Public Property EstadoTurnoId() As typEstadosTurnos
        Get
            Return Me.GetTypProperty(clEstadoTurnoId)
        End Get
        Set(ByVal value As typEstadosTurnos)
            clEstadoTurnoId = value
        End Set
    End Property
    Public Property TipoDocumentoId() As typTiposDocumentos
        Get
            Return Me.GetTypProperty(clTipoDocumentoId)
        End Get
        Set(ByVal value As typTiposDocumentos)
            clTipoDocumentoId = value
        End Set
    End Property
    Public Property NroDocumento() As Int64
        Get
            Return clNroDocumento
        End Get
        Set(ByVal value As Int64)
            clNroDocumento = value
        End Set
    End Property
    Public Property TpoTurno() As Integer
        Get
            Return clTpoTurno
        End Get
        Set(ByVal value As Integer)
            clTpoTurno = value
        End Set
    End Property
    Public Property NroOrdenConsulta() As String
        Get
            Return clNroOrdenConsulta
        End Get
        Set(ByVal value As String)
            clNroOrdenConsulta = value
        End Set
    End Property
    Public Property Modalidad() As Integer
        Get
            Return clModalidad
        End Get
        Set(ByVal value As Integer)
            clModalidad = value
        End Set
    End Property

End Class
