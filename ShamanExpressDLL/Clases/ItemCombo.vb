Public Class ItemCombo
    Private itmID As Long
    Private itmDescripcion As String

    Public ReadOnly Property ID() As Long
        Get
            Return itmID
        End Get
    End Property

    Public ReadOnly Property Descripcion() As String
        Get
            Return itmDescripcion
        End Get
    End Property

    Public Sub New(ByVal pId As Long, ByVal pDes As String)
        itmID = pId
        itmDescripcion = pDes
    End Sub

    Public Overrides Function ToString() As String
        Return itmDescripcion
    End Function
End Class
