Public Class typVehiculos
    Inherits typAll
    Private clDominio As String
    Private clMarcaModeloId As typMarcasModelos
    Private clSituacion As Short
    Private clflgPropio As Short
    Private clPrestadorId As typPrestadores
    Private clNroMotor As String
    Private clNroChasis As String
    Private clAnio As Integer
    Private clTipoCombustible As String
    Private clKilometraje As Int64
    Private clregKilometraje As DateTime
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Dominio() As String
        Get
            Return clDominio
        End Get
        Set(ByVal value As String)
            clDominio = value
        End Set
    End Property
    Public Property MarcaModeloId() As typMarcasModelos
        Get
            Return Me.GetTypProperty(clMarcaModeloId)
        End Get
        Set(ByVal value As typMarcasModelos)
            clMarcaModeloId = value
        End Set
    End Property
    Public Property Situacion() As Short
        Get
            Return clSituacion
        End Get
        Set(ByVal value As Short)
            clSituacion = value
        End Set
    End Property
    Public Property flgPropio() As Short
        Get
            Return clflgPropio
        End Get
        Set(ByVal value As Short)
            clflgPropio = value
        End Set
    End Property
    Public Property PrestadorId() As typPrestadores
        Get
            Return Me.GetTypProperty(clPrestadorId)
        End Get
        Set(ByVal value As typPrestadores)
            clPrestadorId = value
        End Set
    End Property
    Public Property NroMotor() As String
        Get
            Return clNroMotor
        End Get
        Set(ByVal value As String)
            clNroMotor = value
        End Set
    End Property
    Public Property NroChasis() As String
        Get
            Return clNroChasis
        End Get
        Set(ByVal value As String)
            clNroChasis = value
        End Set
    End Property
    Public Property Anio() As Integer
        Get
            Return clAnio
        End Get
        Set(ByVal value As Integer)
            clAnio = value
        End Set
    End Property
    Public Property TipoCombustible() As String
        Get
            Return clTipoCombustible
        End Get
        Set(ByVal value As String)
            clTipoCombustible = value
        End Set
    End Property
    Public Property Kilometraje() As Int64
        Get
            Return clKilometraje
        End Get
        Set(ByVal value As Int64)
            clKilometraje = value
        End Set
    End Property
    Public Property regKilometraje() As DateTime
        Get
            Return clregKilometraje
        End Get
        Set(ByVal value As DateTime)
            clregKilometraje = value
        End Set
    End Property

End Class

