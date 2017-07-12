Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'-----> XML
Imports System.Xml
Imports System.Xml.Serialization

Public Class conEmpresasLegalesCMS
    Inherits typEmpresasLegalesCMS
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetCurrentId(ByVal pEmp As Int64) As Int64
        GetCurrentId = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM EmpresasLegalesCMS WHERE (EmpresaLegalId = " & pEmp & ") "
            SQL = SQL & "AND (ExpirationTime > '" & DateTimeToSql(Now, True) & "') "
            SQL = SQL & "ORDER BY GenerationTime DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetCurrentId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetCurrentId", ex)
        End Try

    End Function
    Public Function GetByEmpresa(ByVal pEmp As Int64) As DataTable

        GetByEmpresa = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, UniqueId, GenerationTime, ExpirationTime "
            SQL = SQL & "FROM EmpresasLegalesCMS "
            SQL = SQL & "WHERE (EmpresaLegalId = " & pEmp & ") "
            SQL = SQL & "ORDER BY ExpirationTime DESC"

            Dim cmdVto As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdVto.ExecuteReader)

            GetByEmpresa = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByEmpresa", ex)
        End Try
    End Function

End Class

