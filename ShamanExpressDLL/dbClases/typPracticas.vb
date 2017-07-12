Public Class typPracticas
    Inherits typAllGenerico00
    Private clClasificacionId As Integer
    Private clPracticaAgrupacionId As typPracticasAgrupaciones
    Private clImpAjuste As Decimal
    Private clTpoTurno As Integer
    Private clflgLaboratorioTest As Integer
    Private clActivo As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ClasificacionId() As Integer
        Get
            Return clClasificacionId
        End Get
        Set(ByVal value As Integer)
            clClasificacionId = value
        End Set
    End Property
    Public Property PracticaAgrupacionId() As typPracticasAgrupaciones
        Get
            Return Me.GetTypProperty(clPracticaAgrupacionId)
        End Get
        Set(ByVal value As typPracticasAgrupaciones)
            clPracticaAgrupacionId = value
        End Set
    End Property
    Public Property ImpAjuste() As Decimal
        Get
            Return clImpAjuste
        End Get
        Set(ByVal value As Decimal)
            clImpAjuste = value
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
    Public Property Activo() As Integer
        Get
            Return clActivo
        End Get
        Set(ByVal value As Integer)
            clActivo = value
        End Set
    End Property


End Class
