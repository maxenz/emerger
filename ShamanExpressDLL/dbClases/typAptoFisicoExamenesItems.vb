Public Class typAptoFisicoExamenesItems
    Inherits typAll
    Private clAptoFisicoExamenId As typAptoFisicoExamenes
    Private clAptoFisicoGrupoItemId As typAptoFisicoGruposItems
    Private clValor1 As Int64
    Private clValor2 As String
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property AptoFisicoExamenId() As typAptoFisicoExamenes
        Get
            Return Me.GetTypProperty(clAptoFisicoExamenId)
        End Get
        Set(ByVal value As typAptoFisicoExamenes)
            clAptoFisicoExamenId = value
        End Set
    End Property
    Public Property AptoFisicoGrupoItemId() As typAptoFisicoGruposItems
        Get
            Return Me.GetTypProperty(clAptoFisicoGrupoItemId)
        End Get
        Set(ByVal value As typAptoFisicoGruposItems)
            clAptoFisicoGrupoItemId = value
        End Set
    End Property
    Public Property Valor1() As Int64
        Get
            Return clValor1
        End Get
        Set(ByVal value As Int64)
            clValor1 = value
        End Set
    End Property
    Public Property Valor2() As String
        Get
            Return clValor2
        End Get
        Set(ByVal value As String)
            clValor2 = value
        End Set
    End Property

End Class
