Imports System.Data
Imports System.Data.SqlClient
Public Class conMetodosMensajeria
    Inherits typMetodosMensajeria
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pMet As msgMetodos) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM MetodosMensajeria WHERE MetodoId = " & pMet

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function GetAll(Optional ByVal pAct As Integer = 1, Optional ByVal pTab As Integer = 0) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, Descripcion "
            SQL = SQL & "FROM MetodosMensajeria "
            If pAct = 1 Then SQL = sqlWhere(SQL) & "(Activo = 1)"
            If pAct = 2 Then SQL = sqlWhere(SQL) & "(Activo = 0)"
            SQL = SQL & "ORDER BY Descripcion"

            Dim cmdBus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBus.ExecuteReader)

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
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar él nombre del método"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
