Public Class typAlicuotasIva
    Inherits typAll
    Private clImpuestoId As typImpuestos
    Private clDescripcion As String
    Private clPorcentaje As Decimal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property ImpuestoId() As typImpuestos
        Get
            Return Me.GetTypProperty(clImpuestoId)
        End Get
        Set(ByVal value As typImpuestos)
            clImpuestoId = value
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
    Public Property Porcentaje() As Decimal
        Get
            Return clPorcentaje
        End Get
        Set(ByVal value As Decimal)
            clPorcentaje = value
        End Set
    End Property
End Class
