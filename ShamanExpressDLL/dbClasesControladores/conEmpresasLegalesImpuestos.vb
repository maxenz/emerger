Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'-----> XML
Imports System.Xml
Imports System.Xml.Serialization

Public Class conEmpresasLegalesImpuestos
    Inherits typEmpresasLegalesImpuestos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIdByIndex(ByVal pEmp As Int64, pIto As itoImpuestos) As Int64
        GetIdByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM EmpresasLegalesImpuestos WHERE (EmpresaLegalId = " & pEmp & ") AND (ImpuestoId = " & pIto & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIdByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIdByIndex", ex)
        End Try

    End Function

    Public Function GetByEmpresa(ByVal pEmp As Int64) As DataTable

        GetByEmpresa = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT emp.ID, emp.ImpuestoId, ito.Descripcion "
            SQL = SQL & "FROM EmpresasLegalesImpuestos emp "
            SQL = SQL & "INNER JOIN Impuestos ito ON (emp.ImpuestoId = ito.ID) "
            SQL = SQL & "WHERE (emp.EmpresaLegalId = " & pEmp & ") "
            SQL = SQL & "ORDER BY ito.Descripcion"

            Dim cmdIto As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdIto.ExecuteReader)

            GetByEmpresa = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByEmpresa", ex)
        End Try
    End Function

    Public Sub EliminarByEmpresa(ByVal pEmp As Int64)

        Try

            Dim SQL As String = "DELETE FROM EmpresasLegalesImpuestos WHERE EmpresaLegalId = " & pEmp
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "EliminarByEmpresa", ex)
        End Try

    End Sub


End Class

