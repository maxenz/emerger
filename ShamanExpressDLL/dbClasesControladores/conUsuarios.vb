Imports System.Data
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class conUsuarios
    Inherits typUsuarios
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pIde As String) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Usuarios WHERE Identificacion = '" & qyVal(pIde) & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetAll(Optional ByVal pAct As Integer = 1) As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT DISTINCT usr.ID, usr.Identificacion, usr.Nombre FROM Usuarios usr "
            SQL = SQL & "LEFT JOIN UsuariosNodos und ON usr.ID = und.UsuarioId AND und.TipoRelacion = 2 "
            SQL = SQL & "LEFT JOIN sysNodos nod ON und.sysNodoId = nod.ID AND nod.ProductoId = 200 "

            SQL = SQL & "WHERE (nod.ID IS NULL) "
            If pAct = 1 Then SQL = SQL & "AND (usr.Activo = 1) "
            If pAct = 2 Then SQL = SQL & "AND (usr.Activo = 0) "

            SQL = SQL & "ORDER BY usr.Identificacion"

            Dim cmdBus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBus.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function GetExtranetAll(Optional ByVal pAct As Integer = 1) As DataTable

        GetExtranetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT DISTINCT usr.ID, usr.Identificacion, usr.Nombre FROM Usuarios usr "
            SQL = SQL & "LEFT JOIN UsuariosNodos und ON usr.ID = und.UsuarioId AND und.TipoRelacion = 2 "
            SQL = SQL & "LEFT JOIN sysNodos nod ON und.sysNodoId = nod.ID AND nod.ProductoId = 200 "

            SQL = SQL & "WHERE (nod.ID IS NOT NULL) "
            If pAct = 1 Then SQL = SQL & "AND (usr.Activo = 1) "
            If pAct = 2 Then SQL = SQL & "AND (usr.Activo = 0) "

            SQL = SQL & "ORDER BY usr.Identificacion"

            Dim cmdBus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBus.ExecuteReader)

            GetExtranetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.Identificacion = "" Then vRdo = "Debe determinar la identificación del usuario"
            If Me.Nombre = "" Then vRdo = "Debe determinar el nombre del usuario"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Function Autenticar(ByVal pIde As String, ByVal pPsw As String, ByRef pNchg As Boolean, Optional ByVal pMode As modLogin = modLogin.lShamanExpress) As Int64

        Autenticar = 0

        Try

            If shamanConfig.segModoLogin = segModoLogin.Shaman Or pMode = modLogin.lExtranet Then

                Dim objTry As New conUsuarios(Me.myCnnName)

                Dim vUsr As Int64 = Me.GetIDByIndex(pIde)

                If objTry.Abrir(vUsr) Then

                    If Not (objTry.tryFecha = Now.Date And objTry.tryCantidad >= 5) Then

                        '-----> Password
                        If checkPassword(vUsr, pPsw) Then

                            pNchg = True
                            '--------> Verfico si necesita cambiar
                            If Not IsDBNull(objTry.FecCambioPassword) Then
                                If DateToSql(objTry.FecCambioPassword) <> SQLNullDate Then
                                    If DateDiff(DateInterval.Day, objTry.FecCambioPassword, Now.Date) <= 60 Then
                                        pNchg = False
                                    End If
                                End If
                            End If

                            '--------> Verfico acceso
                            Select Case pMode
                                Case modLogin.lShamanExpress
                                    Autenticar = vUsr
                                Case modLogin.lExtranet
                                    Dim objExtranetAcceso As New conUsuariosNodos
                                    If objExtranetAcceso.GetExtranetByUsuario(vUsr).Rows.Count > 0 Then
                                        Autenticar = vUsr
                                    Else
                                        Autenticar = 0
                                    End If
                            End Select

                        Else
                            MsgBox("Usuario / Contraseña incorrectos", MsgBoxStyle.Critical, "Login")

                            If objTry.tryFecha = Now.Date Then
                                objTry.tryCantidad = objTry.tryCantidad + 1
                            Else
                                objTry.tryFecha = Now.Date
                                objTry.tryCantidad = 1
                            End If
                            If objTry.Salvar(objTry) Then
                                If objTry.tryCantidad = 5 Then
                                    MsgBox("El usuario " & pIde & " ha sido temporalmente bloqueado", MsgBoxStyle.Critical, "Login")
                                End If
                            End If
                        End If
                    Else
                        MsgBox("Usuario temporalmente bloqueado", MsgBoxStyle.Critical, "Login")
                    End If
                Else
                    MsgBox("Usuario / Contraseña incorrectos", MsgBoxStyle.Critical, "Login")
                End If

            Else

                Dim Entry As New System.DirectoryServices.DirectoryEntry("LDAP://" & shamanConfig.segServidorDominio, pIde, pPsw)
                Dim Searcher As New System.DirectoryServices.DirectorySearcher(Entry)

                Try
                    Dim vNomEvl As String = Entry.Name

                    '-----> Ingreso OK
                    Dim vUsr As Int64 = Me.GetIDByIndex(pIde)

                    If vUsr = 0 Then

                        '---> Verifico que exista al menos un perfil que no se Administrador
                        Dim SQL As String = "SELECT ID FROM Perfiles WHERE flgAdministrador = 0"
                        Dim cmdPerfiles As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                        Dim dtPerfiles As New DataTable
                        Dim vIdx As Integer
                        dtPerfiles.Load(cmdPerfiles.ExecuteReader)

                        If dtPerfiles.Rows.Count > 0 Then

                            Dim vNom As String = ""
                            Dim vMail As String = ""

                            '----> Creo el usuario con todos los menos el perfil supervisor...
                            Dim objNewUser As New conUsuarios(Me.myCnnName)

                            objNewUser.CleanProperties(objNewUser)
                            objNewUser.Identificacion = Entry.Username.ToUpper
                            objNewUser.Nombre = Entry.Username.ToUpper
                            objNewUser.viewArbolOpciones = 1
                            objNewUser.viewFavoritos = 1
                            objNewUser.viewReciente = 1

                            '-----> Información de Active Directory
                            Me.getActiveDirectoryUserInfo(pIde, vNom, vMail)
                            If vNom <> "" Then objNewUser.Nombre = vNom
                            objNewUser.Email = vMail

                            '-----> Salvo
                            If objNewUser.Salvar(objNewUser) Then

                                vUsr = objNewUser.ID

                                '----> Agrego los permisos
                                For vIdx = 0 To dtPerfiles.Rows.Count - 1

                                    Dim objUsuarioPerfil As New typUsuariosPerfiles

                                    objUsuarioPerfil.CleanProperties(objUsuarioPerfil)
                                    objUsuarioPerfil.UsuarioId.SetObjectId(vUsr)
                                    objUsuarioPerfil.PerfilId.SetObjectId(dtPerfiles(vIdx)(0))
                                    objUsuarioPerfil.Salvar(objUsuarioPerfil)

                                    objUsuarioPerfil = Nothing

                                Next vIdx

                            End If

                        Else

                            MsgBox("Debe existir al menos un perfil de tipo No Administrador", MsgBoxStyle.Critical, "Login")

                        End If

                    End If

                    Autenticar = vUsr

                Catch ex As Exception
                    MsgBox("Usuario / Contraseña incorrectos en dominio " & shamanConfig.segServidorDominio, MsgBoxStyle.Critical, "Login")
                End Try

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Autenticar", ex)
        End Try

    End Function
    Private Sub getActiveDirectoryUserInfo(ByVal pIde As String, ByRef pNom As String, ByRef pMail As String)

        pNom = ""
        pMail = ""

        Try
            Dim dirEntry As New System.DirectoryServices.DirectoryEntry("LDAP://" & shamanConfig.segServidorDominio)
            Dim dirSearcher As New System.DirectoryServices.DirectorySearcher(dirEntry)

            dirSearcher.Filter = "(samAccountName=" & pIde & ")"

            dirSearcher.PropertiesToLoad.Add("GivenName")
            'Users First Name 
            dirSearcher.PropertiesToLoad.Add("Mail")
            'Users e-mail address 
            dirSearcher.PropertiesToLoad.Add("sn")

            'Users last name 
            Dim sr As System.DirectoryServices.SearchResult = dirSearcher.FindOne()

            If Not sr Is Nothing Then

                Dim de As System.DirectoryServices.DirectoryEntry = sr.GetDirectoryEntry()

                If Not de.Properties("LastName").Value Is Nothing Then
                    pNom = de.Properties("LastName").Value.ToString()
                End If

                If Not de.Properties("FirstName").Value Is Nothing Then
                    If pNom = "" Then
                        pNom = de.Properties("FirstName").Value.ToString()
                    Else
                        pNom = pNom & " " & de.Properties("FirstName").Value.ToString()
                    End If
                End If

                If Not de.Properties("Mail").Value Is Nothing Then
                    pMail = de.Properties("Mail").Value.ToString()
                End If

            End If

        Catch ex As Exception
            'HandleError(Me.GetType.Name, "getActiveDirectoryUserInfo", ex)
        End Try

    End Sub
    Public Function CambiarPassword(ByVal pUsr As Int64, ByVal pPsw As String, ByVal pNew As String, ByVal pCnf As String) As Boolean
        CambiarPassword = False
        Try
            Dim SQL As String

            If checkPassword(pUsr, pPsw) Then
                '-----> Password Actual
                If pPsw <> pNew Then
                    '---------------------> Verifico fortaleza de password
                    If checkForcePassword(pNew) Then
                        If pNew = pCnf Then

                            SQL = "UPDATE Usuarios SET Password = PWDENCRYPT('" & qyVal(pNew).ToLower & "') WHERE ID = " & pUsr
                            Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))

                            If cmdUpd.ExecuteNonQuery() > 0 Then
                                MsgBox("Su nueva contraseña fue modifica con éxito", MsgBoxStyle.Information, "Cambiar Contraseña")

                                Me.FecCambioPassword = Now.Date
                                Me.tryCantidad = 0
                                Me.Salvar(Me)

                                CambiarPassword = True
                            Else
                                MsgBox("Su nueva contraseña no pudo ser almacenada", MsgBoxStyle.Critical, "Cambiar Contraseña")
                            End If
                        Else
                            MsgBox("La contraseña nueva no coincide con la de confirmación", MsgBoxStyle.Critical, "Cambiar Contraseña")
                        End If
                    End If
                Else
                    MsgBox("La contraseña nueva debe ser diferente a la anterior", MsgBoxStyle.Critical, "Cambiar Contraseña")
                End If
            Else
                MsgBox("La contraseña actual establecida es incorrecta", MsgBoxStyle.Critical, "Cambiar Contraseña")
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CambiarPassword", ex)
        End Try
    End Function

    Private Function checkPassword(ByVal pUsr As Int64, ByVal pPsw As String) As Boolean
        checkPassword = False

        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Usuarios WHERE ID = " & pUsr & " AND PwdCompare('" & qyVal(pPsw).ToLower & "',Password) = 1"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))

            If cmFind.ExecuteScalar Is Nothing Then

                SQL = "SELECT ID FROM Usuarios WHERE ID = " & pUsr & " AND PwdCompare('" & qyVal(pPsw) & "',Password) = 1"

                cmFind.CommandText = SQL

                If cmFind.ExecuteScalar > 0 Then
                    checkPassword = True
                End If

            Else

                checkPassword = True

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "checkPassword", ex)
        End Try


    End Function

    Private Function checkForcePassword(ByVal pVal As String) As Boolean

        checkForcePassword = False

        If Not Regex.IsMatch(pVal, "(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{6,15})$") Then
            MsgBox("La nueva contraseña no es lo suficiente segura" & vbCrLf & "Su longitud debe ser igual o superior a 6 y debe contener números y letras", MsgBoxStyle.Critical, "Cambiar Contraseña")
        Else
            checkForcePassword = True
        End If

    End Function

    Public Function Desbloquear(ByVal pId As Int64) As Boolean
        Desbloquear = False
        Try
            If Me.Abrir(pId) Then
                If Not (Me.tryFecha = Now.Date And Me.tryCantidad = 5) Then
                    MsgBox("El usuario " & Me.Identificacion & " no se encontraba bloqueado", MsgBoxStyle.Critical, "Desbloquear")
                Else
                    Me.tryCantidad = 0
                    If Me.Salvar(Me) Then
                        MsgBox("El usuario " & Me.Identificacion & " fue desbloqueado con éxito", MsgBoxStyle.Information, "Desbloquear")
                        Desbloquear = True
                    End If
                End If
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Desbloquear", ex)
        End Try
    End Function

    Public Function BlanquearPassword(ByVal pId As Int64, Optional ByVal pMsg As Boolean = True) As Boolean
        BlanquearPassword = False
        Try
            Dim SQL As String
            Dim objUserBlank As New conUsuarios(Me.myCnnName)

            If objUserBlank.Abrir(pId) Then

                SQL = "UPDATE Usuarios SET Password = PWDENCRYPT('') WHERE ID = " & pId
                Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))

                If cmdUpd.ExecuteNonQuery > 0 Then
                    If pMsg Then
                        MsgBox("La contraseña fue blanqueada con éxito", MsgBoxStyle.Information, "Blanquear Password")
                    End If

                    If pId <> shamanSession.ID Then
                        objUserBlank.FecCambioPassword = NullDateTime
                        objUserBlank.tryCantidad = 0
                        BlanquearPassword = Me.Salvar(objUserBlank)
                    Else
                        shamanSession.FecCambioPassword = NullDateTime
                        shamanSession.tryCantidad = 0
                        BlanquearPassword = Me.Salvar(shamanSession)
                    End If

                Else
                    If pMsg Then
                        MsgBox("El blanqueo no pudo ser almacenado", MsgBoxStyle.Critical, "Blanquear Password")
                    End If
                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "BlanquearPassword", ex)
        End Try
    End Function

    Public Function getNodeAccess(ByVal pUsr As Int64, ByVal pPro As shamanProductos, ByVal pNodeName As String) As nodAccess

        getNodeAccess = nodAccess.sSinAcceso

        Try

            If Not Me.IsAdmin(pUsr) Then
                Dim objNodo As New consysNodos
                Dim vNode As Int64 = objNodo.GetIDByClave(pPro, pNodeName)

                If vNode > 0 Then

                    Dim SQL As String

                    SQL = "SELECT TOP 1 prf.Acceso FROM PerfilesNodos prf "
                    SQL = SQL & "INNER JOIN UsuariosPerfiles usr ON (prf.PerfilId = usr.PerfilId) "
                    SQL = SQL & "WHERE (usr.UsuarioId = " & pUsr & ") AND (prf.sysNodoId = " & vNode & ") "
                    SQL = SQL & "ORDER BY prf.Acceso DESC"

                    Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                    If Not vOutVal Is Nothing Then getNodeAccess = CType(vOutVal, nodAccess)

                Else

                    getNodeAccess = nodAccess.sEscritura

                End If

                objNodo = Nothing

            Else

                getNodeAccess = nodAccess.sAdministracion

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getNodeAccess", ex)
        End Try

    End Function

    Public Function IsAdmin(ByVal pUsr As Int64) As Boolean
        IsAdmin = False
        Try

            Dim SQL As String

            SQL = "SELECT TOP 1 prf.ID FROM UsuariosPerfiles usr "
            SQL = SQL & "INNER JOIN Perfiles prf ON (usr.PerfilId = prf.ID) "
            SQL = SQL & "WHERE (usr.UsuarioId = " & pUsr & ") AND (prf.flgAdministrador = 1) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmFind.ExecuteScalar > 0 Then IsAdmin = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "IsAdmin", ex)
        End Try
    End Function

End Class
