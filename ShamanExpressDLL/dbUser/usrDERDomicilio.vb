Public Class usrDERDomicilio
    Private clDERdmCalle As String
    Private clDERdmAltura As Int64
    Private clDERdmPiso As String
    Private clDERdmDepto As String
    Private clDERdmEntreCalle1 As String
    Private clDERdmEntreCalle2 As String
    Private clDERdmReferencia As String
    Private clDERdmLatitud As Decimal
    Private clDERdmLongitud As Decimal
    Private clDERDomicilio As String
    Public Property DERdmCalle() As String
        Get
            Return clDERdmCalle
        End Get
        Set(ByVal value As String)
            If Not shamanConfig Is Nothing Then
                If shamanConfig.ConfiguracionRegionalId.modDomicilio = 0 And value.Length > 70 Then
                    value = value.Substring(0, 70)
                ElseIf shamanConfig.ConfiguracionRegionalId.modDomicilio = 1 And value.Length > 200 Then
                    value = value.Substring(0, 200)
                End If
            Else
                If value.Length > 70 Then value = value.Substring(0, 70)
            End If
            clDERdmCalle = value
        End Set
    End Property
    Public Property DERdmAltura() As Int64
        Get
            Return clDERdmAltura
        End Get
        Set(ByVal value As Int64)
            clDERdmAltura = value
        End Set
    End Property
    Public Property DERdmPiso() As String
        Get
            Return clDERdmPiso
        End Get
        Set(ByVal value As String)
            clDERdmPiso = value
        End Set
    End Property
    Public Property DERdmDepto() As String
        Get
            Return clDERdmDepto
        End Get
        Set(ByVal value As String)
            clDERdmDepto = value
        End Set
    End Property
    Public Property DERdmEntreCalle1() As String
        Get
            Return clDERdmEntreCalle1
        End Get
        Set(ByVal value As String)
            If value.Length > 100 Then value = value.Substring(0, 100)
            clDERdmEntreCalle1 = value
        End Set
    End Property
    Public Property DERdmEntreCalle2() As String
        Get
            Return clDERdmEntreCalle2
        End Get
        Set(ByVal value As String)
            If value.Length > 100 Then value = value.Substring(0, 100)
            clDERdmEntreCalle2 = value
        End Set
    End Property
    Public Property DERdmReferencia() As String
        Get
            Return clDERdmReferencia
        End Get
        Set(ByVal value As String)
            If value.Length > 100 Then value = value.Substring(0, 100)
            clDERdmReferencia = value
        End Set
    End Property
    Public Property DERdmLatitud() As Decimal
        Get
            Return clDERdmLatitud
        End Get
        Set(ByVal value As Decimal)
            clDERdmLatitud = value
        End Set
    End Property
    Public Property DERdmLongitud() As Decimal
        Get
            Return clDERdmLongitud
        End Get
        Set(ByVal value As Decimal)
            clDERdmLongitud = value
        End Set
    End Property
    Public ReadOnly Property DERDomicilio() As String
        Get
            Dim vDom As String
            vDom = clDERdmCalle
            If clDERdmAltura > 0 Then vDom = vDom & " " & clDERdmAltura
            If clDERdmPiso <> "" And clDERdmPiso <> "0" Then vDom = vDom & " " & clDERdmPiso
            If clDERdmDepto <> "" Then vDom = vDom & " " & clDERdmDepto
            Return vDom
        End Get
    End Property
    Public Sub CleanProperties(ByVal pObj As usrDERDomicilio)
        pObj.DERdmCalle = ""
        pObj.DERdmAltura = 0
        pObj.DERdmPiso = 0
        pObj.DERdmDepto = ""
        pObj.DERdmEntreCalle1 = ""
        pObj.DERdmEntreCalle2 = ""
        pObj.DERdmReferencia = ""
        pObj.DERdmLatitud = 0
        pObj.DERdmLongitud = 0
    End Sub
End Class
