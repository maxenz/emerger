Public Class typlckJobs
    Inherits typAll
    Private clJobName As String
    Private clJobSubId As Int64
    Private clultEjecucion As DateTime
    Private clflgStatus As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property JobName() As String
        Get
            Return clJobName
        End Get
        Set(ByVal value As String)
            clJobName = value
        End Set
    End Property
    Public Property JobSubId() As Int64
        Get
            Return clJobSubId
        End Get
        Set(ByVal value As Int64)
            clJobSubId = value
        End Set
    End Property
    Public Property ultEjecucion() As DateTime
        Get
            Return clultEjecucion
        End Get
        Set(ByVal value As DateTime)
            clultEjecucion = value
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
