Imports System.Data
Imports System.Data.SqlClient
Public Class conConfiguracionesRegionales
    Inherits typConfiguracionesRegionales
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, Descripcion "
            SQL = SQL & "FROM ConfiguracionesRegionales "
            SQL = SQL & "WHERE (ISNULL(flgInterfaz, 0) = 0) "
            SQL = SQL & "ORDER BY Descripcion"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function GetIDByDescripcion(ByVal pVal As String) As Int64

        GetIDByDescripcion = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM ConfiguracionesRegionales WHERE (Descripcion = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try

    End Function

    Public Function GetIDByClienteId(ByVal pVal As Int64) As Int64

        GetIDByClienteId = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM ConfiguracionesRegionales WHERE (ClienteId = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByClienteId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByClienteId", ex)
        End Try

    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar el nombre de la configuración regional"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
