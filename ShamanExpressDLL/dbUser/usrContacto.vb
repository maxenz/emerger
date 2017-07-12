Public Class usrContacto
    Private clNombre As String
    Private clEmail As String
    Private clTelefono As String
    Public Property Nombre() As String
        Get
            Return clNombre
        End Get
        Set(ByVal value As String)
            clNombre = value
        End Set
    End Property
    Public Property Email() As String
        Get
            Return clEmail
        End Get
        Set(ByVal value As String)
            clEmail = value
        End Set
    End Property
    Public Property Telefono() As String
        Get
            Return clTelefono
        End Get
        Set(ByVal value As String)
            clTelefono = value
        End Set
    End Property
    Public Sub CleanProperties(ByVal pObj As usrContacto)
        pObj.Nombre = ""
        pObj.Email = ""
        pObj.Telefono = ""
    End Sub
    Public Function IsMailValid() As Boolean
        IsMailValid = True
        If InStr(Me.Email, "@") = 0 And Me.Email <> "" Then
            IsMailValid = False
        End If
    End Function
End Class
