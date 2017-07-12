Public Class typGenericoContacto
    Inherits typAll
    Private clContacto As usrContacto
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Property Contacto() As usrContacto
        Get
            Return clContacto
        End Get
        Set(ByVal value As usrContacto)
            clContacto = value
        End Set
    End Property
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try

            Dim vRdo As String = ""
            Validar = True
            If Me.Contacto.Nombre = "" Then vRdo = "Debe especificar el nombre del contacto"
            If Not Me.Contacto.IsMailValid And vRdo = "" Then vRdo = "La dirección de e-mail es incorrecta"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class
