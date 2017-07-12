Imports System.Data
Imports System.Data.SqlClient
Public Class conlckTraslados
    Inherits typlckTraslados
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function getNewTraslado() As Int64
        getNewTraslado = 0
        Try
            Dim vMax As Int64 = 0, SQL As String

            SQL = "SELECT ISNULL(MAX(TrasladoId), 0) + 1 FROM Incidentes "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            If Not vOutVal Is Nothing Then
                vMax = CType(vOutVal, Int64)
                SQL = "SELECT ISNULL(MAX(TrasladoId), 0) + 1 FROM lckTraslados "

                cmFind.CommandText = SQL
                vOutVal = CType(cmFind.ExecuteScalar, String)

                If Not vOutVal Is Nothing Then
                    If vMax < CType(vOutVal, Int64) Then vMax = CType(vOutVal, Int64)
                End If

            End If

            '-----> Lockeo
            If lockTraslado(vMax) Then
                '-----> Devuelvo
                getNewTraslado = vMax
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getNewTraslado", ex)
        End Try
    End Function
    Public Function lockTraslado(ByVal pTra As Int64, Optional ByVal pMsg As Boolean = True) As Boolean
        lockTraslado = False
        Try
            Dim SQL As String
            SQL = "SELECT B.Nombre FROM lckTraslados A INNER JOIN Usuarios B ON (A.regUsuarioId = B.ID) "
            SQL = SQL & "WHERE (A.TrasladoId = " & pTra & ") "
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vUsrLck As String = CType(cmFind.ExecuteScalar, String)

            If Not vUsrLck Is Nothing Then
                If pMsg Then MsgBox("El servicio se encuentra utilizado por " & vUsrLck, MsgBoxStyle.Exclamation)
            Else
                Dim objAdd As New conlckTraslados
                If Not objAdd.Abrir(getIDByIndex(pTra)) Then
                    objAdd.TrasladoId = pTra
                    If objAdd.Salvar(objAdd) Then
                        lockTraslado = True
                    End If
                    objAdd = Nothing
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "lockTraslado", ex)
        End Try
    End Function
    Public Function unlockTraslado(ByVal pTra As Int64, Optional ByVal pMsg As Boolean = True) As Boolean
        unlockTraslado = False
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM lckTraslados WHERE (TrasladoId = " & pTra & ") "
            SQL = SQL & "AND (regUsuarioId = " & logUsuario & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vUsrLck As String = CType(cmFind.ExecuteScalar, String)

            If vUsrLck Is Nothing Then
                If pMsg Then MsgBox("El servicio no se encontraba bloquedado o pertenecía a otro usuario", MsgBoxStyle.Exclamation)
            Else
                If Me.ClearLock(Me.getIDByIndex(pTra)) Then
                    unlockTraslado = True
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "lockTraslado", ex)
        End Try
    End Function
    Public Function ClearLock(ByVal pId As Int64) As Boolean
        ClearLock = False

        Try
            Dim objUnlock As New conlckTraslados(Me.myCnnName)

            If objUnlock.Eliminar(pId) Then
                ClearLock = True
            End If

            objUnlock = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "ClearLock", ex)
        End Try
    End Function
    Private Function getIDByIndex(ByVal pTra As Int64) As Int64
        getIDByIndex = 0
        Try

            Dim SQL As String

            SQL = "SELECT ID FROM lckTraslados WHERE (TrasladoId = " & pTra & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getIDByIndex", ex)
        End Try
    End Function

    Public Function GetByFechas(ByVal pDes As Date, ByVal pHas As Date) As DataTable

        GetByFechas = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT lck.ID, inc.FecIncidente, '' AS NroIncidente, gdo.AbreviaturaId AS Grado, cli.AbreviaturaId AS Cliente, inc.TrasladoId AS NroAfiliado, "
            SQL = SQL & "inc.Paciente, dom.Domicilio, loc.AbreviaturaId AS Localidad "
            SQL = SQL & "FROM lckTraslados lck "
            SQL = SQL & "INNER JOIN Incidentes inc ON (inc.TrasladoId = lck.TrasladoId) "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (inc.ID = dom.IncidenteId) "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
            SQL = SQL & "INNER JOIN Clientes cli ON (inc.ClienteId = cli.ID) "
            SQL = SQL & "LEFT JOIN Localidades loc ON (dom.LocalidadId = loc.ID) "
            SQL = SQL & "WHERE (inc.FecIncidente BETWEEN '" & DateToSql(pDes) & "' AND '" & DateToSql(pHas) & "') "
            SQL = SQL & "AND (dom.TipoDomicilio = 0) "
            SQL = SQL & " ORDER BY inc.TrasladoId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByFechas = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByFechas", ex)
        End Try
    End Function

End Class

