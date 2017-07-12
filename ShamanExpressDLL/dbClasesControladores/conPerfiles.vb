Imports System.Data
Imports System.Data.SqlClient
Public Class conPerfiles
    Inherits typPerfiles
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetPerfilAdministrador() As Int64
        GetPerfilAdministrador = 0
        Try
            Dim SQL As String
            SQL = "SELECT TOP 1 ID FROM Perfiles WHERE flgAdministrador = 1"
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetPerfilAdministrador = CType(vOutVal, Int64)
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetPerfilAdministrador", ex)
        End Try
    End Function

    Public Function GetFirstPerfilId(ByVal pUsr As Int64) As Int64
        GetFirstPerfilId = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 PerfilId FROM UsuariosPerfiles WHERE UsuarioId = " & pUsr & " ORDER BY ID"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetFirstPerfilId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetFirstPerfilId", ex)
        End Try

    End Function

    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, Jerarquia, Descripcion FROM Perfiles "
            SQL = SQL & " ORDER BY Jerarquia"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.Jerarquia = 0 Then vRdo = "Debe establecer la jerarquia del perfil"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del perfil"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
