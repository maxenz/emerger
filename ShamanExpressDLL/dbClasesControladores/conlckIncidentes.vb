Imports System.Data
Imports System.Data.SqlClient
Public Class conlckIncidentes
    Inherits typlckIncidentes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    '---------> Métodos
    Public Function getNewIncidente(ByVal pFec As Date) As String

        getNewIncidente = ""

        Try

            If shamanConfig.modNumeracion = 0 Then

                Dim SQL As String, vGet As Boolean = False
                '----> Verifico si hay que crear
                SQL = "SELECT TOP 1 ID FROM lckIncidentes WHERE FecIncidente = '" & DateToSql(pFec) & "' AND NroIncidente <> '' AND NroIncidente IS NOT NULL"
                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                If cmFind.ExecuteScalar Is Nothing Then
                    makeNroIncidentes(pFec)
                End If
                '----> Obtengo
                SQL = "SELECT * FROM lckIncidentes WHERE FecIncidente = '" & DateToSql(pFec) & "' AND flgStatus = 0 ORDER BY NroIncidente"
                Dim cmdLck As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dt As New DataTable
                Dim vIdx As Integer = 0
                dt.Load(cmdLck.ExecuteReader)
                Do Until vGet Or vIdx = dt.Rows.Count
                    '-----> Chequeo
                    SQL = "SELECT TOP 1 ID FROM Incidentes WHERE FecIncidente = '" & DateToSql(pFec) & "' AND NroIncidente = '" & dt(vIdx).Item("NroIncidente") & "'"
                    cmdLck.CommandText = SQL
                    If cmdLck.ExecuteScalar Is Nothing Then vGet = True
                    If Not vGet Then vIdx = vIdx + 1
                Loop
                '----> Lockeo
                If vGet Then
                    If Me.Abrir(dt(vIdx).Item("ID")) Then
                        Me.flgStatus = 1
                        If Me.Salvar(Me) Then
                            getNewIncidente = Me.NroIncidente
                        End If
                    End If
                End If
            Else

                Dim objTraslados As New conlckTraslados(Me.myCnnName)

                Dim vTra As Long = objTraslados.getNewTraslado

                getNewIncidente = vTra.ToString

                objTraslados = Nothing

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getNewIncidente", ex)
        End Try
    End Function
    Public Function lockIncidente(ByVal pFec As Date, ByVal pNro As String, Optional ByVal pMsg As Boolean = True) As Boolean

        lockIncidente = False

        Try

            If shamanConfig.modNumeracion = 0 Then

                Dim SQL As String
                SQL = "SELECT B.Nombre FROM lckIncidentes A INNER JOIN Usuarios B ON (A.regUsuarioId = B.ID) "
                SQL = SQL & "WHERE (A.FecIncidente = '" & DateToSql(pFec) & "') AND (A.NroIncidente = '" & pNro & "') "
                SQL = SQL & "AND (A.flgStatus = 1) "

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vUsrLck As String = CType(cmFind.ExecuteScalar, String)

                If Not vUsrLck Is Nothing Then
                    If pMsg Then MsgBox("El servicio se encuentra utilizado por " & vUsrLck, MsgBoxStyle.Exclamation)
                Else
                    Dim objAdd As New conlckIncidentes
                    If objAdd.Abrir(getIDByIndex(pFec, pNro)) Then
                        objAdd.flgStatus = 1
                        If objAdd.Salvar(objAdd) Then
                            lockIncidente = True
                        End If
                        objAdd = Nothing
                    Else
                        objAdd.FecIncidente = pFec
                        objAdd.NroIncidente = pNro
                        objAdd.flgStatus = 1
                        If objAdd.Salvar(objAdd) Then
                            lockIncidente = True
                        End If
                        objAdd = Nothing
                    End If
                End If

            Else

                Dim objTraslados As New conlckTraslados

                If objTraslados.lockTraslado(Val(pNro), pMsg) Then
                    lockIncidente = True
                End If

                objTraslados = Nothing

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "lockIncidente", ex)
        End Try
    End Function
    Public Function unlockIncidente(ByVal pFec As Date, ByVal pNro As String, Optional ByVal pMsg As Boolean = True) As Boolean

        unlockIncidente = False

        Try

            If pNro <> "" Then

                If shamanConfig.modNumeracion = 0 Then

                    Dim SQL As String

                    SQL = "SELECT ID FROM lckIncidentes "
                    SQL = SQL & "WHERE (FecIncidente = '" & DateToSql(pFec) & "') AND (NroIncidente = '" & pNro & "') "
                    SQL = SQL & "AND (regUsuarioId = " & logUsuario & ") "

                    Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    Dim vUsrLck As String = CType(cmFind.ExecuteScalar, String)

                    If vUsrLck Is Nothing Then
                        If pMsg Then MsgBox("El servicio no se encontraba bloquedado o pertenecía a otro usuario", MsgBoxStyle.Exclamation)
                    Else
                        If Me.ClearLock(Me.getIDByIndex(pFec, pNro)) Then
                            unlockIncidente = True
                        End If
                    End If

                Else

                    Dim objTraslados As New conlckTraslados

                    If objTraslados.unlockTraslado(Val(pNro), pMsg) Then
                        unlockIncidente = True
                    End If

                    objTraslados = Nothing

                End If

            Else

                unlockIncidente = True

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "unlockIncidente", ex)
        End Try
    End Function
    Public Function ClearLock(ByVal pId As Int64) As Boolean
        ClearLock = False

        Try

            Dim objUnlock As New conlckIncidentes
            Dim objIncidente As New conIncidentes

            If objUnlock.Abrir(pId) Then

                If objIncidente.GetIDByIndex(objUnlock.FecIncidente, objUnlock.NroIncidente) > 0 Then
                    objUnlock.flgStatus = 2
                Else
                    objUnlock.flgStatus = 0
                End If
                If objUnlock.Salvar(objUnlock) Then
                    ClearLock = True
                End If

            End If

            objUnlock = Nothing
            objIncidente = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "ClearLock", ex)
        End Try
    End Function
    Private Function getIDByIndex(ByVal pFec As Date, ByVal pNro As String) As Int64
        getIDByIndex = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM lckIncidentes WHERE (FecIncidente = '" & DateToSql(pFec) & "') AND (NroIncidente = '" & pNro & "') "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getIDByIndex", ex)
        End Try
    End Function
    Private Sub makeNroIncidentes(Optional ByVal pFec As String = "")
        Try
            Dim vTop As Integer, vSum As Integer, vIzq As Integer = 0, vDer As Integer = 0, vIdx As Integer = 0, vGen As String, vTot As Integer = 0
            Dim SQL As String, objAdd As New conlckIncidentes(Me.myCnnName)
            '----> Fecha
            Dim vFec As Date = Now.Date
            If pFec <> "" Then vFec = CDate(pFec)
            '----> Inicial del día
            objAdd.CleanProperties(objAdd)
            objAdd.FecIncidente = vFec
            objAdd.NroIncidente = "001"
            objAdd.Salvar(objAdd)
            '----> inicializo Random
            Randomize()
            '----> Parametros del día
            vTop = getMaxDay()
            Do Until vIzq = 35 Or vTop = vTot   '----> 0..Z
                vDer = 0
                Do Until vDer = 35 Or vTop = vTot
                    '------> Del 0 a la Z, agrego tantas cantidads
                    vSum = 0
                    Do Until vSum = topCantidad(vTop) Or vTop = vTot
                        '----> Random
                        vIdx = CInt(Int((99 * Rnd()) + 1))
                        '----> Verfico secuencia OK
                        If (vIdx >= 48 And vIdx <= 57) Or (vIdx >= 65 And vIdx <= 90) Then
                            vGen = catVariables(vIzq, vDer, vIdx)
                            If vGen <> "000" Then
                                '-----> Verifico si existe
                                SQL = "SELECT ID FROM lckIncidentes WHERE FecIncidente = '" & DateToSql(vFec) & "' AND NroIncidente = '" & vGen & "'"
                                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                                If cmFind.ExecuteScalar Is Nothing Then
                                    '---> Agrego
                                    objAdd.CleanProperties(objAdd)
                                    objAdd.FecIncidente = vFec
                                    objAdd.NroIncidente = vGen
                                    If objAdd.Salvar(objAdd) Then
                                        vSum = vSum + 1
                                        vTot = vTot + 1
                                    End If
                                End If
                            End If
                        End If
                    Loop
                    vDer = vDer + 1
                Loop
                vIzq = vIzq + 1
            Loop
        Catch ex As Exception
            HandleError(Me.GetType.Name, "makeNroIncidentes", ex)
        End Try
    End Sub
    Private Function getMaxDay() As Integer
        getMaxDay = 400
    End Function
    Private Function topCantidad(ByVal pCnt As Integer) As Integer
        Select Case pCnt
            Case 1 To 1000 : topCantidad = 4
            Case 1001 To 2000 : topCantidad = 6
            Case 2001 To 3000 : topCantidad = 8
            Case 3001 To 5000 : topCantidad = 18
            Case Else : topCantidad = 36
        End Select
    End Function
    Private Function catVariables(ByVal pIzq As Integer, ByVal pDer As Integer, ByVal pIdx As Integer) As String
        Dim vGen As String
        '-----> Armo
        If pIzq < 10 Then
            vGen = pIzq
        Else
            vGen = Chr(pIzq + 55)
        End If
        If pDer < 10 Then
            vGen = vGen & pDer
        Else
            vGen = vGen & Chr(pDer + 55)
        End If
        vGen = vGen & Chr(pIdx)
        catVariables = vGen
    End Function
    Public Function GetByFechas(ByVal pDes As Date, ByVal pHas As Date) As DataTable

        GetByFechas = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT lck.ID, inc.FecIncidente, inc.NroIncidente, gdo.AbreviaturaId AS Grado, cli.AbreviaturaId AS Cliente, inc.NroAfiliado, "
            SQL = SQL & "inc.Paciente, dom.Domicilio, loc.AbreviaturaId AS Localidad "
            SQL = SQL & "FROM lckIncidentes lck "
            SQL = SQL & "INNER JOIN Incidentes inc ON (inc.FecIncidente = lck.FecIncidente) AND (inc.NroIncidente = lck.NroIncidente) "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (inc.ID = dom.IncidenteId) "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
            SQL = SQL & "INNER JOIN Clientes cli ON (inc.ClienteId = cli.ID) "
            SQL = SQL & "LEFT JOIN Localidades loc ON (dom.LocalidadId = loc.ID) "
            SQL = SQL & "WHERE (lck.FecIncidente BETWEEN '" & DateToSql(pDes) & "' AND '" & DateToSql(pHas) & "') "
            SQL = SQL & "AND (lck.flgStatus = 1) AND (dom.TipoDomicilio = 0) "
            SQL = SQL & " ORDER BY inc.FecIncidente, inc.NroIncidente"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByFechas = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByFechas", ex)
        End Try
    End Function

End Class
