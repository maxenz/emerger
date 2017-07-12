Imports System.Reflection
Imports System.Data.SqlClient

Public MustInherit Class typAll
    Private clTabla As String
    '----> ID + Id de referencia de INSERT / UPDATE
    Private clID As Int64
    Private clrefId As Int64
    '----> Seguridad
    Private flgOpen As Boolean = False
    Private flgSaving As Boolean = False
    '----> Seguridad
    Private clregUsuarioId As typUsuarios
    Private clregFechaHora As DateTime
    Private clregTerminalId As typTerminales
    '----> Conexion ID (Name)
    Private clmyCnnName As String
    '----> LastExec
    Public MyLastExec As LastExec

    Public Overridable ReadOnly Property Tabla() As String
        Get
            If Left(Me.GetType.Name, 3) = "cls" Or Left(Me.GetType.Name, 3) = "typ" Or Left(Me.GetType.Name, 3) = "con" Then
                Return Me.GetType.Name.Substring(3, Me.GetType.Name.Length - 3)
            Else
                Return Me.GetType.Name
            End If
        End Get
    End Property
    Public Property ID() As Int64
        Get
            Return clID
        End Get
        Set(ByVal value As Int64)
            clID = value
        End Set
    End Property
    Public Property regUsuarioId() As typUsuarios
        Get
            Return Me.GetTypProperty(clregUsuarioId)
        End Get
        Set(ByVal value As typUsuarios)
            clregUsuarioId = value
        End Set
    End Property
    Public Property regFechaHora() As DateTime
        Get
            Return clregFechaHora
        End Get
        Set(ByVal value As DateTime)
            clregFechaHora = value
        End Set
    End Property
    Public Property regTerminalId() As typTerminales
        Get
            Return Me.GetTypProperty(clregTerminalId)
        End Get
        Set(ByVal value As typTerminales)
            clregTerminalId = value
        End Set
    End Property
    Public Property myCnnName() As String
        Get
            Return clmyCnnName
        End Get
        Set(ByVal value As String)
            clmyCnnName = value
        End Set
    End Property
    Public Sub New(Optional ByVal pCnnName As String = "")
        If pCnnName <> "" Then
            Me.myCnnName = pCnnName
        Else
            Me.myCnnName = cnnDefault
        End If
    End Sub
    Public Sub CleanProperties(ByVal pObj As Object)
        Dim myProperties() As PropertyInfo
        Dim PropertyItem As PropertyInfo
        MyLastExec = New LastExec
        myProperties = Me.GetType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
        For Each PropertyItem In myProperties
            With PropertyItem
                If PropertyItem.CanWrite And PropertyItem.Name <> "myCnnName" Then
                    Select Case PropertyItem.PropertyType.Name
                        Case "Int32" : PropertyItem.SetValue(pObj, 0, Nothing)
                        Case "Int64" : PropertyItem.SetValue(pObj, 0, Nothing)
                        Case "Decimal" : PropertyItem.SetValue(pObj, CDec(0), Nothing)
                        Case "String" : PropertyItem.SetValue(pObj, "", Nothing)
                        Case Else
                            If Left(PropertyItem.PropertyType.Name, 3) = "typ" Or Left(PropertyItem.PropertyType.Name, 3) = "usr" Then
                                Dim ty As Type = Type.GetType(PropertyItem.PropertyType.FullName), o As Object
                                If Left(PropertyItem.PropertyType.Name, 3) = "typ" Then
                                    o = System.Activator.CreateInstance(ty, Me.myCnnName)
                                    o.SetObjectId(0)
                                Else
                                    o = System.Activator.CreateInstance(ty)
                                    o.CleanProperties(o)
                                End If
                                PropertyItem.SetValue(Me, o, Nothing)
                                o = Nothing
                                ty = Nothing
                            Else
                                PropertyItem.SetValue(pObj, Nothing, Nothing)
                            End If
                    End Select
                End If
            End With
        Next
    End Sub
    Public Function Abrir(ByVal pId As String) As Boolean
        Dim SQL As String
        Dim cmOpe As New SqlCommand

        Dim myProperties() As PropertyInfo
        Dim PropertyItem As PropertyInfo

        MyLastExec = New LastExec

        flgOpen = False

        Abrir = False

        Try

            Me.CleanProperties(Me)
            '--------> Abro el RecordSet
            If pId <> "0" And pId <> "" Then

                SQL = "SELECT * FROM " & Me.Tabla & " WHERE ID = " & pId

                cmOpe.Connection = cnnsNET.Item(Me.myCnnName)
                cmOpe.Transaction = cnnsTransNET.Item(Me.myCnnName)
                cmOpe.CommandText = SQL

                Dim dt As New DataTable
                Dim vIdx As Integer = 0
                dt.Load(cmOpe.ExecuteReader)

                If dt.Rows.Count > 0 Then

                    myProperties = Me.GetType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                    For Each PropertyItem In myProperties
                        If PropertyItem.CanWrite Then
                            Select Case Left(PropertyItem.PropertyType.Name, 3)
                                Case "usr"
                                    Dim usrProperties() As PropertyInfo
                                    Dim usrPropertyItem As PropertyInfo
                                    Dim ty As Type = Type.GetType(PropertyItem.PropertyType.FullName), o As Object

                                    o = System.Activator.CreateInstance(ty)
                                    o.CleanProperties(o)
                                    usrProperties = o.GetType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)

                                    For Each usrPropertyItem In usrProperties
                                        If usrPropertyItem.CanWrite Then
                                            If Not IsDBNull(dt.Rows(0).Item(usrPropertyItem.Name)) Then
                                                usrPropertyItem.SetValue(o, dt.Rows(0).Item(usrPropertyItem.Name), Nothing)
                                            End If
                                        End If
                                    Next
                                    PropertyItem.SetValue(Me, o, Nothing)

                                    o = Nothing
                                    ty = Nothing

                                Case "typ"
                                    If Not IsDBNull(dt.Rows(0).Item(PropertyItem.Name)) Then
                                        Dim ty As Type = Type.GetType(PropertyItem.PropertyType.FullName), o As Object

                                        o = System.Activator.CreateInstance(ty, Me.myCnnName)
                                        o.ID = dt.Rows(0).Item(PropertyItem.Name)
                                        o.SetObjectId(o.ID)
                                        PropertyItem.SetValue(Me, o, Nothing)
                                        o = Nothing
                                        ty = Nothing
                                    End If

                                Case Else
                                    If PropertyItem.Name <> "myCnnName" Then
                                        If Not IsDBNull(dt.Rows(0).Item(PropertyItem.Name)) Then
                                            If PropertyItem.Name <> "ID" Then
                                                PropertyItem.SetValue(Me, dt.Rows(0).Item(PropertyItem.Name), Nothing)
                                            Else
                                                PropertyItem.SetValue(Me, dt.Rows(0).Item(PropertyItem.Name), Nothing)
                                                Me.SetObjectId(dt.Rows(0).Item(PropertyItem.Name))
                                            End If
                                        End If
                                    End If
                            End Select
                        End If
                    Next

                    Abrir = True
                    flgOpen = True
                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Abrir", ex, True, Me.Tabla, , MyLastExec)
        End Try

    End Function
    Public Function Salvar(ByVal pObj As Object, Optional ByVal pAutoDef As Boolean = True, Optional ByVal pMsg As Boolean = True) As Boolean

        Salvar = False

        Dim SQL As String
        Dim cmOpe As New SqlCommand
        Dim vAddNew As Boolean = False

        Dim myProperties() As PropertyInfo
        Dim PropertyItem As PropertyInfo

        MyLastExec = New LastExec

        Try

            flgSaving = True

            Salvar = False
            '--------> Abro el RecordSet
            SQL = "SELECT ID FROM " & Me.Tabla & " WHERE ID = " & Me.GetType.GetProperty("ID").GetValue(pObj, Nothing)

            cmOpe.Connection = cnnsNET.Item(Me.myCnnName)
            cmOpe.CommandText = SQL
            cmOpe.Transaction = cnnsTransNET.Item(Me.myCnnName)
            If cmOpe.ExecuteScalar = 0 Then vAddNew = True

            myProperties = Me.GetType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)

            If vAddNew Then

                SQL = "INSERT INTO " & Me.Tabla & " ("

                '--------------> INSERT INTO Columnas
                For Each PropertyItem In myProperties
                    If Left(PropertyItem.PropertyType.Name, 3) = "usr" Then
                        Dim usrProperties() As PropertyInfo
                        Dim usrPropertyItem As PropertyInfo
                        Dim o As Object = PropertyItem.GetValue(pObj, Nothing)

                        usrProperties = o.GetType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)

                        For Each usrPropertyItem In usrProperties
                            SQL = SQL & usrPropertyItem.Name & ", "
                        Next

                        o = Nothing
                    ElseIf PropertyItem.CanWrite And PropertyItem.Name <> "ID" And PropertyItem.Name <> "myCnnName" Then
                        SQL = SQL & PropertyItem.Name & ", "
                    End If
                Next

                '--------------> Medio
                SQL = SQL.Substring(0, SQL.Length - 2) & ") VALUES("

                '--------------> Valores
                For Each PropertyItem In myProperties
                    If PropertyItem.Name = "regUsuarioId" And pAutoDef Then
                        SQL = SQL & logUsuario & ", "
                    ElseIf PropertyItem.Name = "regFechaHora" And pAutoDef Then
                        SQL = SQL & " '" & DateTimeToSql(Now) & "', "
                    ElseIf PropertyItem.Name = "regTerminalId" And pAutoDef Then
                        SQL = SQL & logTerminal & ", "
                    ElseIf Left(PropertyItem.PropertyType.Name, 3) = "usr" Then
                        Dim usrProperties() As PropertyInfo
                        Dim usrPropertyItem As PropertyInfo
                        Dim o As Object = PropertyItem.GetValue(pObj, Nothing)

                        usrProperties = o.GetType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)

                        For Each usrPropertyItem In usrProperties
                            insertAddValue(o, usrPropertyItem, SQL)
                        Next

                        o = Nothing
                    ElseIf Left(PropertyItem.PropertyType.Name, 3) = "typ" Then
                        Dim o As Object = PropertyItem.GetValue(pObj, Nothing)
                        SQL = SQL & o.GetObjectId & ", "
                    ElseIf PropertyItem.CanWrite And PropertyItem.Name <> "ID" And PropertyItem.Name <> "myCnnName" Then
                        insertAddValue(pObj, PropertyItem, SQL)
                    End If
                Next

                SQL = SQL.Substring(0, SQL.Length - 2) & ")"

            Else
                SQL = "UPDATE " & Me.Tabla & " SET "

                For Each PropertyItem In myProperties
                    If PropertyItem.Name = "regUsuarioId" And pAutoDef Then
                        SQL = SQL & PropertyItem.Name & " = " & logUsuario & ", "
                    ElseIf PropertyItem.Name = "regFechaHora" And pAutoDef Then
                        SQL = SQL & PropertyItem.Name & " = '" & DateTimeToSql(Now) & "', "
                    ElseIf PropertyItem.Name = "regTerminalId" And pAutoDef Then
                        SQL = SQL & PropertyItem.Name & " = " & logTerminal & ", "
                    ElseIf Left(PropertyItem.PropertyType.Name, 3) = "usr" Then
                        Dim usrProperties() As PropertyInfo
                        Dim usrPropertyItem As PropertyInfo
                        Dim o As Object = PropertyItem.GetValue(pObj, Nothing)

                        usrProperties = o.GetType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)

                        For Each usrPropertyItem In usrProperties
                            updateAddValue(o, usrPropertyItem, SQL)
                        Next

                        o = Nothing
                    ElseIf Left(PropertyItem.PropertyType.Name, 3) = "typ" Then
                        Dim o As Object = PropertyItem.GetValue(pObj, Nothing)
                        SQL = SQL & PropertyItem.Name & " = " & o.GetObjectId & ", "
                    ElseIf PropertyItem.CanWrite And PropertyItem.Name <> "ID" And PropertyItem.Name <> "myCnnName" Then
                        updateAddValue(pObj, PropertyItem, SQL)
                    End If
                Next

                SQL = SQL.Substring(0, SQL.Length - 2) & " WHERE ID = " & Me.GetType.GetProperty("ID").GetValue(pObj, Nothing)

            End If

            cmOpe.Connection = cnnsNET(Me.myCnnName)
            cmOpe.Transaction = cnnsTransNET.Item(Me.myCnnName)
            cmOpe.CommandText = SQL

            If cmOpe.ExecuteNonQuery() = 0 Then
                If pMsg Then MsgBox("No se pudo completar su operación", MsgBoxStyle.Critical, "Shaman")
            Else
                If vAddNew Then

                    SQL = "SELECT @@IDENTITY"
                    cmOpe.CommandText = SQL
                    Dim vOutVal As String = CType(cmOpe.ExecuteScalar, String)

                    If Not vOutVal Is Nothing Then
                        Me.GetType.GetProperty("ID").SetValue(Me, CType(vOutVal, Int64), Nothing)
                    End If
                End If

                If Me.Abrir(Me.GetType.GetProperty("ID").GetValue(pObj, Nothing)) Then
                    Salvar = True
                End If

            End If

            flgSaving = False
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Salvar", ex, pMsg, Me.Tabla, , MyLastExec)
        End Try

    End Function
    Private Sub insertAddValue(ByVal pObj As Object, ByVal PropertyItem As PropertyInfo, ByRef SQL As String)
        Select Case PropertyItem.PropertyType.Name
            Case "String"
                Dim vVal As String = PropertyItem.GetValue(pObj, Nothing).ToString
                vVal = vVal.Replace("'", "''")
                'vVal = vVal.Replace("^", " ")
                'vVal = vVal.Replace("’", " ")
                SQL = SQL & "N'" & vVal & "', "
            Case "DateTime"
                Dim vVal As DateTime = CType(PropertyItem.GetValue(pObj, Nothing), DateTime)
                SQL = SQL & "'" & DateTimeToSql(vVal) & "', "
            Case "Decimal"
                SQL = SQL & Replace(PropertyItem.GetValue(pObj, Nothing), ",", ".") & ", "
            Case Else
                SQL = SQL & PropertyItem.GetValue(pObj, Nothing) & ", "
        End Select
    End Sub
    Private Sub updateAddValue(ByVal pObj As Object, ByVal PropertyItem As PropertyInfo, ByRef SQL As String)
        SQL = SQL & PropertyItem.Name & " = "
        insertAddValue(pObj, PropertyItem, SQL)
    End Sub
    Public Overridable Function CanDelete(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean
        CanDelete = True
    End Function
    Public Overridable Function Eliminar(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean
        Dim SQL As String

        Eliminar = False

        Try

            MyLastExec = New LastExec

            If Me.CanDelete(pId, pMsg) Then
                Dim cmOpe As New SqlCommand
                '--------> QUERY el RecordSet
                SQL = "DELETE FROM " & Me.Tabla & " WHERE ID = " & pId
                cmOpe.Connection = cnnsNET(Me.myCnnName)
                cmOpe.Transaction = cnnsTransNET(Me.myCnnName)
                cmOpe.CommandText = SQL
                cmOpe.ExecuteNonQuery()
                Eliminar = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Eliminar", ex, pMsg, Me.Tabla, , MyLastExec)
        End Try
    End Function
    Public Sub SetObjectId(ByVal pId As String)
        Me.clrefId = pId
    End Sub
    Public Function GetObjectId() As String
        GetObjectId = Me.clrefId
    End Function
    Public Function IsOpen() As Boolean
        IsOpen = flgOpen
    End Function
    Public Function GetTypProperty(ByVal pObj As Object) As Object
        Dim vOpn As Boolean = False
        GetTypProperty = Nothing
        If Not pObj Is Nothing And Not pObj.IsOpen And Not flgSaving Then
            vOpn = pObj.Abrir(pObj.ID)
        End If
        Return pObj
    End Function
    Public Function CloneMe() As Object
        CloneMe = Me.MemberwiseClone()
    End Function
End Class
