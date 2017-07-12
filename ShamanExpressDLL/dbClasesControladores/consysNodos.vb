Imports System.Data
Imports System.Data.SqlClient
Public Class consysNodos
    Inherits typsysNodos

    Public Function GetExtranet() As DataTable
        GetExtranet = Nothing
        Try
            Dim SQL As String

            SQL = "SELECT ID, Jerarquia, Descripcion "
            SQL = SQL & "FROM sysNodos "
            SQL = SQL & "WHERE (ProductoId = 200) "
            SQL = SQL & "ORDER BY Jerarquia"

            Dim cmdAli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdAli.ExecuteReader)

            GetExtranet = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetExtranet", ex)
        End Try
    End Function

    Public Function GetIDByClave(ByVal pPro As shamanProductos, ByVal pCla As String) As Int64
        GetIDByClave = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM sysNodos WHERE (ProductoId = " & pPro & ") AND (Clave = '" & pCla & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByClave = CType(vOutVal, Int64)
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByClave", ex)
        End Try
    End Function

    Public Function GetIDByJerarquia(ByVal pPro As shamanProductos, ByVal pJer As String) As Int64
        GetIDByJerarquia = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM sysNodos WHERE (ProductoId = " & pPro & ") AND (Jerarquia = '" & pJer & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByJerarquia = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByJerarquia", ex)
        End Try
    End Function

    Private Sub SetPurge(Optional ByVal pPro As shamanProductos = shamanProductos.Express)
        Try
            Dim SQL As String = "UPDATE sysNodos SET flgPurge = 1 WHERE (ProductoId = " & pPro & ") AND (flgOperativo = 0)"
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPurge", ex)
        End Try
    End Sub

    Private Sub DelPurge(Optional ByVal pPro As shamanProductos = shamanProductos.Express)
        Try
            Dim SQL As String = "DELETE FROM sysNodos WHERE (ProductoId = " & pPro & ") AND (flgPurge = 1)"
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()
        Catch ex As Exception
            HandleError(Me.GetType.Name, "DelPurge", ex)
        End Try
    End Sub

End Class
