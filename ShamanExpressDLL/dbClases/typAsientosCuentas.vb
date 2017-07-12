Public Class typAsientosCuentas
    Inherits typAll
    Private clAsientoId As typAsientos
    Private clCuentaAsientoId As typCuentasAsientos
    Private clNroRenglon As Integer
    Private clDebeHaber As String
    Private clImporte As Decimal
    Private clflgPurge As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property AsientoId() As typAsientos
        Get
            Return Me.GetTypProperty(clAsientoId)
        End Get
        Set(ByVal value As typAsientos)
            clAsientoId = value
        End Set
    End Property
    Public Property CuentaAsientoId() As typCuentasAsientos
        Get
            Return Me.GetTypProperty(clCuentaAsientoId)
        End Get
        Set(ByVal value As typCuentasAsientos)
            clCuentaAsientoId = value
        End Set
    End Property
    Public Property NroRenglon() As Integer
        Get
            Return clNroRenglon
        End Get
        Set(ByVal value As Integer)
            clNroRenglon = value
        End Set
    End Property
    Public Property DebeHaber() As String
        Get
            Return clDebeHaber
        End Get
        Set(ByVal value As String)
            clDebeHaber = value
        End Set
    End Property
    Public Property Importe() As Decimal
        Get
            Return clImporte
        End Get
        Set(ByVal value As Decimal)
            clImporte = value
        End Set
    End Property
    Public Property flgPurge() As Integer
        Get
            Return clflgPurge
        End Get
        Set(ByVal value As Integer)
            clflgPurge = value
        End Set
    End Property

End Class
