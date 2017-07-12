Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Public Class conImpuestosImportaciones
    Inherits typImpuestosImportaciones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetAll(ByVal pIto As itoImpuestos) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT SUBSTRING(CAST(ipt.Periodo as varchar), 5, 2) + '/' + SUBSTRING(CAST(ipt.Periodo as varchar), 1, 4) AS Periodo, usr.Identificacion FROM ImpuestosImportaciones ipt "
            SQL = SQL & "INNER JOIN Usuarios usr ON ipt.regUsuarioId = usr.ID "
            SQL = SQL & "WHERE (ImpuestoId = " & pIto & ") "
            SQL = SQL & "ORDER BY ipt.Periodo DESC"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function


End Class
