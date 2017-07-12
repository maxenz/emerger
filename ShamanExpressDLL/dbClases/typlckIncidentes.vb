Imports System.Data
Imports System.Data.SqlClient
Public Class typlckIncidentes
    Inherits typAll
    Private clFecIncidente As Date
    Private clNroIncidente As String
    Private clflgStatus As Integer
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
    Public Property flgStatus() As Integer
        Get
            Return clflgStatus
        End Get
        Set(ByVal value As Integer)
            clflgStatus = value
        End Set
    End Property
End Class
