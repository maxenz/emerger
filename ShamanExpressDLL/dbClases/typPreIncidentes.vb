Public Class typPreIncidentes
    Inherits typAll
    Private clClienteId As typClientes
    Private clNroServicio As String
    Private clFecHorServicio As DateTime
    Private clTelefono As String
    Private clPaciente As String
    Private clNroAfiliado As String
    Private clSexo As String
    Private clEdad As Decimal
    Private clSintomas As String
    Private clGradoOperativoId As typGradosOperativos
    Private clPlanId As String
    Private clCoPago As Decimal
    Private clflgIvaGravado As Integer
    Private clSanatorioId As typSanatorios
    Private clDomicilio As usrDomicilio
    Private clLocalidadId As typLocalidades
    Private clObservaciones As String
    Private clFecHorRecepcion As DateTime
    Private clIncidenteId As typIncidentes
    Private clIncidenteOrigenId As Int64
    Private clerrCliente As String
    Private clerrLocalidad As String
    Private clerrGradoOperativo As String
    Private clerrSanatorio As String
    Private clerrDERLocalidad As String
    Private clerrDERSanatorio As String
    Private clDERSanatorioId As typSanatorios
    Private clDERDomicilio As usrDERDomicilio
    Private clDERLocalidadId As typLocalidades
    Private clDERFecHorServicio As DateTime
    Private clerrDiagnostico As String
    Private clDiagnosticoId As typDiagnosticos
    Private clmailSender As String
    Private clmailSubject As String
    Private clflgIdaVuelta As Integer
    Private clDERFecHorRetorno As DateTime
    Private clflgStatus As Integer
    Private clerrIncorporacion As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ClienteId() As typClientes
        Get
            Return Me.GetTypProperty(clClienteId)
        End Get
        Set(ByVal value As typClientes)
            clClienteId = value
        End Set
    End Property
    Public Property FecHorServicio() As DateTime
        Get
            Return clFecHorServicio
        End Get
        Set(ByVal value As DateTime)
            clFecHorServicio = value
        End Set
    End Property
    Public Property NroServicio() As String
        Get
            Return clNroServicio
        End Get
        Set(ByVal value As String)
            clNroServicio = value
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
    Public Property Paciente() As String
        Get
            Return clPaciente
        End Get
        Set(ByVal value As String)
            clPaciente = value
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
    Public Property Sintomas() As String
        Get
            Return clSintomas
        End Get
        Set(ByVal value As String)
            clSintomas = value
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
    Public Property SanatorioId() As typSanatorios
        Get
            Return Me.GetTypProperty(clSanatorioId)
        End Get
        Set(ByVal value As typSanatorios)
            clSanatorioId = value
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
    Public Property Observaciones() As String
        Get
            Return clObservaciones
        End Get
        Set(ByVal value As String)
            clObservaciones = value
        End Set
    End Property
    Public Property FecHorRecepcion() As DateTime
        Get
            Return clFecHorRecepcion
        End Get
        Set(ByVal value As DateTime)
            clFecHorRecepcion = value
        End Set
    End Property
    Public Property IncidenteId() As typIncidentes
        Get
            Return Me.GetTypProperty(clIncidenteId)
        End Get
        Set(ByVal value As typIncidentes)
            clIncidenteId = value
        End Set
    End Property
    Public Property IncidenteOrigenId() As Int64
        Get
            Return clIncidenteOrigenId
        End Get
        Set(ByVal value As Int64)
            clIncidenteOrigenId = value
        End Set
    End Property
    Public Property errCliente() As String
        Get
            Return clerrCliente
        End Get
        Set(ByVal value As String)
            clerrCliente = value
        End Set
    End Property
    Public Property errLocalidad() As String
        Get
            Return clerrLocalidad
        End Get
        Set(ByVal value As String)
            clerrLocalidad = value
        End Set
    End Property
    Public Property errGradoOperativo() As String
        Get
            Return clerrGradoOperativo
        End Get
        Set(ByVal value As String)
            clerrGradoOperativo = value
        End Set
    End Property
    Public Property errSanatorio() As String
        Get
            Return clerrSanatorio
        End Get
        Set(ByVal value As String)
            clerrSanatorio = value
        End Set
    End Property
    Public Property errDERSanatorio() As String
        Get
            Return clerrDERSanatorio
        End Get
        Set(ByVal value As String)
            clerrDERSanatorio = value
        End Set
    End Property
    Public Property errDERLocalidad() As String
        Get
            Return clerrDERLocalidad
        End Get
        Set(ByVal value As String)
            clerrDERLocalidad = value
        End Set
    End Property
    Public Property mailSender() As String
        Get
            Return clmailSender
        End Get
        Set(ByVal value As String)
            clmailSender = value
        End Set
    End Property
    Public Property mailSubject() As String
        Get
            Return clmailSubject
        End Get
        Set(ByVal value As String)
            clmailSubject = value
        End Set
    End Property
    Public Property errDiagnostico() As String
        Get
            Return clerrDiagnostico
        End Get
        Set(ByVal value As String)
            clerrDiagnostico = value
        End Set
    End Property
    Public Property flgIdaVuelta() As Integer
        Get
            Return clflgIdaVuelta
        End Get
        Set(ByVal value As Integer)
            clflgIdaVuelta = value
        End Set
    End Property
    Public Property flgStatus() As Integer
        Get
            Return clflgStatus
        End Get
        Set(ByVal value As Integer)
            clflgStatus = value
        End Set
    End Property
    Public Property errIncorporacion() As String
        Get
            Return clerrIncorporacion
        End Get
        Set(ByVal value As String)
            clerrIncorporacion = value
        End Set
    End Property
    Public Property DERSanatorioId() As typSanatorios
        Get
            Return Me.GetTypProperty(clDERSanatorioId)
        End Get
        Set(ByVal value As typSanatorios)
            clDERSanatorioId = value
        End Set
    End Property
    Public Property DERDomicilio() As usrDERDomicilio
        Get
            Return clDERDomicilio
        End Get
        Set(ByVal value As usrDERDomicilio)
            clDERDomicilio = value
        End Set
    End Property
    Public Property DERLocalidadId() As typLocalidades
        Get
            Return Me.GetTypProperty(clDERLocalidadId)
        End Get
        Set(ByVal value As typLocalidades)
            clDERLocalidadId = value
        End Set
    End Property
    Public Property DERFecHorServicio() As DateTime
        Get
            Return clDERFecHorServicio
        End Get
        Set(ByVal value As DateTime)
            clDERFecHorServicio = value
        End Set
    End Property
    Public Property DERFecHorRetorno() As DateTime
        Get
            Return clDERFecHorRetorno
        End Get
        Set(ByVal value As DateTime)
            clDERFecHorRetorno = value
        End Set
    End Property
    Public Property DiagnosticoId() As typDiagnosticos
        Get
            Return Me.GetTypProperty(clDiagnosticoId)
        End Get
        Set(ByVal value As typDiagnosticos)
            clDiagnosticoId = value
        End Set
    End Property

End Class
