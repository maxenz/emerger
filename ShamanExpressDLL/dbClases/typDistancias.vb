Public Class typDistancias
    Inherits typAll
    Private clLocalidadOrigenId As typLocalidades
    Private clLocalidadDestinoId As typLocalidades
    Private clDistancia As Int64
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property LocalidadOrigenId() As typLocalidades
        Get
            Return Me.GetTypProperty(clLocalidadOrigenId)
        End Get
        Set(ByVal value As typLocalidades)
            clLocalidadOrigenId = value
        End Set
    End Property
    Public Property LocalidadDestinoId() As typLocalidades
        Get
            Return Me.GetTypProperty(clLocalidadDestinoId)
        End Get
        Set(ByVal value As typLocalidades)
            clLocalidadDestinoId = value
        End Set
    End Property
    Public Property Distancia() As Int64
        Get
            Return clDistancia
        End Get
        Set(ByVal value As Int64)
            clDistancia = value
        End Set
    End Property

End Class
