Imports System.Data
Imports System.Data.SqlClient
Public Class conSucesosIncidentes
    Inherits typSucesosIncidentes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion "
            SQL = SQL & "FROM SucesosIncidentes "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function GetIDByAbreviaturaId(ByVal pVal As String) As Int64

        GetIDByAbreviaturaId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM SucesosIncidentes WHERE (AbreviaturaId = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAbreviaturaId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbreviaturaId", ex)
        End Try

    End Function

    Public Function GetIDByDescripcion(ByVal pVal As String) As Int64

        GetIDByDescripcion = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM SucesosIncidentes WHERE (Descripcion = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try

    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador del suceso"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del suceso"
            If Me.TipoAplicacion = "" And vRdo = "" Then vRdo = "Debe determinar a quien se aplica el suceso"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class
