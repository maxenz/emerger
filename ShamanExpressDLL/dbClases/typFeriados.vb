Public Class typFeriados
    Inherits typAll
    Private clFecFeriado As Date
    Private clDescripcion As String
    Private clflgRecurrente As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property FecFeriado() As Date
        Get
            Return clFecFeriado
        End Get
        Set(ByVal value As Date)
            clFecFeriado = value
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
    Public Property flgRecurrente() As Integer
        Get
            Return clflgRecurrente
        End Get
        Set(ByVal value As Integer)
            clflgRecurrente = value
        End Set
    End Property
End Class
