Public Class usrDomicilio
    Private cldmCalle As String
    Private cldmAltura As Int64
    Private cldmPiso As String
    Private cldmDepto As String
    Private cldmEntreCalle1 As String
    Private cldmEntreCalle2 As String
    Private cldmReferencia As String
    Private cldmLatitud As Decimal
    Private cldmLongitud As Decimal
    Private clDomicilio As String
    Public Property dmCalle() As String
        Get
            Return cldmCalle
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
            cldmCalle = value
        End Set
    End Property
    Public Property dmAltura() As Int64
        Get
            Return cldmAltura
        End Get
        Set(ByVal value As Int64)
            cldmAltura = value
        End Set
    End Property
    Public Property dmPiso() As String
        Get
            Return cldmPiso
        End Get
        Set(ByVal value As String)
            cldmPiso = value
        End Set
    End Property
    Public Property dmDepto() As String
        Get
            Return cldmDepto
        End Get
        Set(ByVal value As String)
            cldmDepto = value
        End Set
    End Property
    Public Property dmEntreCalle1() As String
        Get
            Return cldmEntreCalle1
        End Get
        Set(ByVal value As String)
            If value.Length > 100 Then value = value.Substring(0, 100)
            cldmEntreCalle1 = value
        End Set
    End Property
    Public Property dmEntreCalle2() As String
        Get
            Return cldmEntreCalle2
        End Get
        Set(ByVal value As String)
            If value.Length > 100 Then value = value.Substring(0, 100)
            cldmEntreCalle2 = value
        End Set
    End Property
    Public Property dmReferencia() As String
        Get
            Return cldmReferencia
        End Get
        Set(ByVal value As String)
            If value.Length > 100 Then value = value.Substring(0, 100)
            cldmReferencia = value
        End Set
    End Property
    Public Property dmLatitud() As Decimal
        Get
            Return cldmLatitud
        End Get
        Set(ByVal value As Decimal)
            cldmLatitud = value
        End Set
    End Property
    Public Property dmLongitud() As Decimal
        Get
            Return cldmLongitud
        End Get
        Set(ByVal value As Decimal)
            cldmLongitud = value
        End Set
    End Property
    Public ReadOnly Property Domicilio() As String
        Get
            Dim vDom As String
            vDom = cldmCalle
            If cldmAltura > 0 Then vDom = vDom & " " & cldmAltura
            If cldmPiso <> "" And cldmPiso <> "0" Then vDom = vDom & " " & cldmPiso
            If cldmDepto <> "" Then vDom = vDom & " " & cldmDepto
            Return vDom
        End Get
    End Property
    Public Sub CleanProperties(ByVal pObj As usrDomicilio)
        pObj.dmCalle = ""
        pObj.dmAltura = 0
        pObj.dmPiso = 0
        pObj.dmDepto = ""
        pObj.dmEntreCalle1 = ""
        pObj.dmEntreCalle2 = ""
        pObj.dmReferencia = ""
        pObj.dmLatitud = 0
        pObj.dmLongitud = 0
    End Sub
End Class
