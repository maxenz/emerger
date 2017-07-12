Public Class typAptoFisicoGruposItems
    Inherits typAll
    Private clAptoFisicoGrupoId As typAptoFisicoGrupos
    Private clNroItemGrupo As Integer
    Private clSubGrupo As String
    Private clDescripcion As String
    Private clTipoValor As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property AptoFisicoGrupoId() As typAptoFisicoGrupos
        Get
            Return Me.GetTypProperty(clAptoFisicoGrupoId)
        End Get
        Set(ByVal value As typAptoFisicoGrupos)
            clAptoFisicoGrupoId = value
        End Set
    End Property
    Public Property NroItemGrupo() As Integer
        Get
            Return clNroItemGrupo
        End Get
        Set(ByVal value As Integer)
            clNroItemGrupo = value
        End Set
    End Property
    Public Property SubGrupo() As String
        Get
            Return clSubGrupo
        End Get
        Set(ByVal value As String)
            clSubGrupo = value
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
    Public Property TipoValor() As Integer
        Get
            Return clTipoValor
        End Get
        Set(ByVal value As Integer)
            clTipoValor = value
        End Set
    End Property

End Class
