Public Class typPersonalEspecialidades
    Inherits typAll
    Private clPersonalId As typPersonal
    Private clEspecialidadId As typEspecialidadesMedicas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property PersonalId() As typPersonal
        Get
            Return Me.GetTypProperty(clPersonalId)
        End Get
        Set(ByVal value As typPersonal)
            clPersonalId = value
        End Set
    End Property
    Public Property EspecialidadId() As typEspecialidadesMedicas
        Get
            Return Me.GetTypProperty(clEspecialidadId)
        End Get
        Set(ByVal value As typEspecialidadesMedicas)
            clEspecialidadId = value
        End Set
    End Property

End Class
