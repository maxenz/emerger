Imports System.Data
Imports System.Data.SqlClient
Public MustInherit Class conAllGenerico00
    Inherits typAllGenerico00
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Overridable Function GetAll() As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, Descripcion FROM " & Me.Tabla
            If Me.Tabla = "ConfiguracionesRegionales" Then SQL = sqlWhere(SQL) & "(ISNULL(flgInterfaz, 0) = 0)"
            SQL = SQL & " ORDER BY Descripcion"

            Dim cmQry As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmQry.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try

    End Function

    Public Function GetIDByDescripcion(ByVal pVal As String) As Int64
        GetIDByDescripcion = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM " & Me.Tabla & " WHERE Descripcion = '" & pVal & "'"
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try

    End Function

End Class
