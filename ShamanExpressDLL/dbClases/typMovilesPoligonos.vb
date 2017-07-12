Public Class typMovilesPoligonos
    Inherits typAll
    Private clMovilId As typMoviles
    Private clPoligonoId As typPoligonos
    Private clflgEventual As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property MovilId() As typMoviles
        Get
            Return Me.GetTypProperty(clMovilId)
        End Get
        Set(ByVal value As typMoviles)
            clMovilId = value
        End Set
    End Property
    Public Property PoligonoId() As typPoligonos
        Get
            Return Me.GetTypProperty(clPoligonoId)
        End Get
        Set(ByVal value As typPoligonos)
            clPoligonoId = value
        End Set
    End Property
    Public Property flgEventual() As Integer
        Get
            Return clflgEventual
        End Get
        Set(ByVal value As Integer)
            clflgEventual = value
        End Set
    End Property
End Class
