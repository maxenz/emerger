Public Class typMovilesLocalidades
    Inherits typAll
    Private clMovilId As typMoviles
    Private clLocalidadId As typLocalidades
    Private clTipoMovilId As typTiposMoviles
    Private clPrestadorClienteId As typPrestadoresClientes
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
    Public Property LocalidadId() As typLocalidades
        Get
            Return Me.GetTypProperty(clLocalidadId)
        End Get
        Set(ByVal value As typLocalidades)
            clLocalidadId = value
        End Set
    End Property
    Public Property TipoMovilId() As typTiposMoviles
        Get
            Return Me.GetTypProperty(clTipoMovilId)
        End Get
        Set(ByVal value As typTiposMoviles)
            clTipoMovilId = value
        End Set
    End Property
    Public Property PrestadorClienteId() As typPrestadoresClientes
        Get
            Return Me.GetTypProperty(clPrestadorClienteId)
        End Get
        Set(ByVal value As typPrestadoresClientes)
            clPrestadorClienteId = value
        End Set
    End Property

End Class
