Public Class typMovilesHistorias
    Inherits typGenericoMovil
    Private clMovilId As typMoviles
    Private clVigenciaDesde As Date
    Private clVigenciaHasta As Date
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
    Public Property VigenciaDesde() As Date
        Get
            Return clVigenciaDesde
        End Get
        Set(ByVal value As Date)
            clVigenciaDesde = value
        End Set
    End Property
    Public Property VigenciaHasta() As Date
        Get
            Return clVigenciaHasta
        End Get
        Set(ByVal value As Date)
            clVigenciaHasta = value
        End Set
    End Property
End Class
