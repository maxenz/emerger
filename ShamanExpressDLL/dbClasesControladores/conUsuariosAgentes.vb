Imports System.Data
Imports System.Data.SqlClient
Public Class conUsuariosAgentes
    Inherits typUsuariosAgentes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetByUsuario(ByVal pUsr As Int64) As DataTable

        GetByUsuario = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, AgenteId FROM UsuariosAgentes "
            SQL = SQL & "WHERE (UsuarioId = " & pUsr & ") "
            SQL = SQL & " ORDER BY AgenteId"

            Dim cmdBus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBus.ExecuteReader)

            GetByUsuario = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByUsuario", ex)
        End Try
    End Function

    Public Function GetFirstAgenteId(ByVal pUsr As Int64) As String
        GetFirstAgenteId = ""
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 AgenteId FROM UsuariosAgentes WHERE UsuarioId = " & pUsr & " ORDER BY ID"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetFirstAgenteId = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetFirstAgenteId", ex)
        End Try

    End Function

    Public Function GetUsuarioByAgenteId(ByVal pAge As String) As Int64

        GetUsuarioByAgenteId = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 UsuarioId FROM UsuariosAgentes WHERE AgenteId = '" & pAge & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetUsuarioByAgenteId = Val(vOutVal)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetUsuarioByAgenteId", ex)
        End Try

    End Function

End Class
