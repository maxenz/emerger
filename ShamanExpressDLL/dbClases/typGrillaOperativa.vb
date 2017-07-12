Public Class typGrillaOperativa
    Inherits typAll
    Private clFecGrilla As Date
    Private clObservaciones As String
    Private clflgStatus As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property FecGrilla() As Date
        Get
            Return clFecGrilla
        End Get
        Set(ByVal value As Date)
            clFecGrilla = value
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
    Public Property flgStatus() As Integer
        Get
            Return clflgStatus
        End Get
        Set(ByVal value As Integer)
            clflgStatus = value
        End Set
    End Property
End Class
