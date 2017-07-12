Public Class typInfoPacienteItems
    Inherits typAll
    Private clDescripcion As String
    Private clInfoPacienteGrupoId As typInfoPacienteGrupos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Descripcion() As String
        Get
            Return clDescripcion
        End Get
        Set(ByVal value As String)
            clDescripcion = value
        End Set
    End Property

    Public Property InfoPacienteGrupoId() As typInfoPacienteGrupos
        Get
            Return Me.GetTypProperty(clInfoPacienteGrupoId)
        End Get
        Set(ByVal value As typInfoPacienteGrupos)
            clInfoPacienteGrupoId = value
        End Set
    End Property

End Class
