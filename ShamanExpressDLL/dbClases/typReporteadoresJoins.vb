Public Class typReporteadoresJoins
    Inherits typAll
    Private clReporteadorId As typReporteadores
    Private clAliasTabla1, clAliasTabla2, clAliasTabla3, clJoinDescripcion As String
    Private clFlgObligatorio, clNroOrden As Integer



    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Property ReporteadorId() As typReporteadores
        Get
            Return Me.GetTypProperty(clReporteadorId)
        End Get
        Set(ByVal value As typReporteadores)
            clReporteadorId = value
        End Set
    End Property

    Public Property AliasTabla1() As String
        Get
            Return clAliasTabla1
        End Get
        Set(ByVal value As String)
            clAliasTabla1 = value
        End Set
    End Property

    Public Property AliasTabla2() As String
        Get
            Return clAliasTabla2
        End Get
        Set(ByVal value As String)
            clAliasTabla2 = value
        End Set
    End Property

    Public Property AliasTabla3() As String
        Get
            Return clAliasTabla3
        End Get
        Set(ByVal value As String)
            clAliasTabla3 = value
        End Set
    End Property

    Public Property flgObligatorio() As Integer
        Get
            Return clFlgObligatorio
        End Get
        Set(ByVal value As Integer)
            clFlgObligatorio = value
        End Set
    End Property

    Public Property JoinDescripcion() As String
        Get
            Return clJoinDescripcion
        End Get
        Set(ByVal value As String)
            clJoinDescripcion = value
        End Set
    End Property
    Public Property NroOrden() As Integer
        Get
            Return clNroOrden
        End Get
        Set(ByVal value As Integer)
            clNroOrden = value
        End Set
    End Property


End Class