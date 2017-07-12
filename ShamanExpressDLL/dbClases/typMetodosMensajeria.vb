Public Class typMetodosMensajeria
    Inherits typAll
    Private clMetodoId As msgMetodos
    Private clDescripcion As String
    Private clReferencia1 As String
    Private clReferencia2 As String
    Private clReferencia3 As String
    Private clReferencia4 As String
    Private clReferencia5 As String
    Private clObservaciones As String
    Private clActivo As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property MetodoId() As msgMetodos
        Get
            Return clMetodoId
        End Get
        Set(ByVal value As msgMetodos)
            clMetodoId = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
        End Set
    End Property
    Public Property Referencia1() As String
        Get
            Return clReferencia1
        End Get
        Set(ByVal value As String)
            clReferencia1 = value
        End Set
    End Property
    Public Property Referencia2() As String
        Get
            Return clReferencia2
        End Get
        Set(ByVal value As String)
            clReferencia2 = value
        End Set
    End Property
    Public Property Referencia3() As String
        Get
            Return clReferencia3
        End Get
        Set(ByVal value As String)
            clReferencia3 = value
        End Set
    End Property
    Public Property Referencia4() As String
        Get
            Return clReferencia4
        End Get
        Set(ByVal value As String)
            clReferencia4 = value
        End Set
    End Property
    Public Property Referencia5() As String
        Get
            Return clReferencia5
        End Get
        Set(ByVal value As String)
            clReferencia5 = value
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
    Public Property Activo() As Integer
        Get
            Return clActivo
        End Get
        Set(ByVal value As Integer)
            clActivo = value
        End Set
    End Property

End Class
