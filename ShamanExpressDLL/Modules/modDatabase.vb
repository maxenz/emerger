Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Module modDatabase
    Public cnnsNET As New NETCnns
    Public cnnsTransNET As New NETTrans
    '-----> Variables de conexón
    Public cnnDataSource As String
    Public cnnCatalog As String
    Public cnnUser As String
    Public cnnPassword As String

    'Public Function AbrirConexion(ByVal pName As String) As Boolean
    '    Dim myCnnNET As New SqlConnection
    '    AbrirConexion = False
    '    If NETConnect(myCnnNET) Then
    '        If Not cnnsNET.Contains(pName) Then
    '            cnnsNET.Add(pName, myCnnNET)
    '        End If
    '        AbrirConexion = True
    '    End If
    'End Function

    'Private Function NETConnect(ByRef pCnn As SqlConnection) As Boolean
    '    NETConnect = False
    '    Try

    '        If Not pCnn Is Nothing Then
    '            If pCnn.State = 1 Then pCnn.Close()
    '            pCnn = Nothing
    '        End If

    '        pCnn = New SqlConnection
    '        pCnn.ConnectionString = GetConnectionString()

    '        TraceLogInit("Conectando a: " & pCnn.ConnectionString)
    '        pCnn.Open()

    '        If pCnn.State = 1 Then
    '            NETConnect = True
    '        Else
    '            MsgBox("La base de datos no se encuentra disponible", MsgBoxStyle.Critical, "Shaman")
    '        End If

    '    Catch ex As Exception
    '        HandleError("modDatabase", GetConnectionString(), ex)
    '    End Try
    'End Function

    Public Function GetConnectionString(Optional pSql As Boolean = True) As String
        Dim vCnnStr As String

        'cnnDataSource = "JAVI\SQLEXPRESS"
        'cnnDataSource = "JAVI-PC\SQLEXPRESS"
        'cnnCatalog = "Cruz"
        'cnnCatalog = "Corpus"
        'cnnCatalog = "Guatemala"
        'cnnCatalog = "Duarte"
        'cnnCatalog = "SurSalud"
        'cnnCatalog = "ShamanAymed"
        'cnnCatalog = "ShamanMutual"
        'cnnCatalog = "ShamanWeb"
        'cnnCatalog = "MutualPrd"
        'cnnDataSource = "168.226.219.190"
        'cnnCatalog = "Shaman"
        'cnnUser = "sa"
        'cnnPassword = "Password1!"


        If pSql Then
            vCnnStr = "Data Source=" & cnnDataSource & ";multipleactiveresultsets=true;Initial Catalog=" & cnnCatalog & ";User Id=" & cnnUser & "; Password=" & cnnPassword & ";"
        Else
            vCnnStr = "Provider=sqloledb;Data Source=" & cnnDataSource & ";multipleactiveresultsets=true;Initial Catalog=" & cnnCatalog & ";User Id=" & cnnUser & "; Password=" & cnnPassword & ";"
        End If
        GetConnectionString = vCnnStr
    End Function

    Public Function sqlWhere(ByVal pSql As String) As String
        If InStr(UCase(pSql), "WHERE") > 0 Then
            sqlWhere = pSql & " AND "
        Else
            sqlWhere = pSql & " WHERE "
        End If
    End Function

    Public Function likeTinyInt(ByVal pVal As String) As String
        likeTinyInt = ""
        If pVal.ToUpper Like "S" Then
            likeTinyInt = "1"
        ElseIf pVal.ToUpper Like "N" Then
            likeTinyInt = "0"
        End If
    End Function

    Public Function GetCurrentTime() As DateTime
        GetCurrentTime = Now
        Try

            Dim cmFind As New SqlCommand("SELECT CURRENT_TIMESTAMP", cnnsNET(cnnDefault))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetCurrentTime = CType(vOutVal, DateTime)

        Catch ex As Exception
            HandleError("modDatabase", "GetCurrentTime", ex)
        End Try
    End Function


End Module
