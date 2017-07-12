Imports System.Data
Imports System.Data.SqlClient
Public Class conZonasGeograficas
    Inherits typZonasGeograficas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Overrides Function CanDelete(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean
        CanDelete = False

        Try
            Dim SQL As String = "SELECT TOP 1 Descripcion FROM Localidades WHERE ZonaGeograficaId = " & pId
            Dim cmAli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vVal As String = cmAli.ExecuteScalar

            If Not vVal Is Nothing Then
                If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "La zona en cuestión está vinculada a la localidad " & vVal, MsgBoxStyle.Critical, "Shaman")
            Else
                CanDelete = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CanDelete", ex)
        End Try
    End Function
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion, VisualColor FROM ZonasGeograficas "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdZon As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdZon.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function
    Public Function GetDTContextMenu() As DataTable
        GetDTContextMenu = Nothing
        Try
            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion FROM ZonasGeograficas "
            SQL = SQL & " ORDER BY AbreviaturaId"

            Dim cmdZon As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdZon.ExecuteReader)

            GetDTContextMenu = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDTContextMenu", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador de la zona geográfica"
            If Me.Descripcion = "" Then vRdo = "Debe determinar la descripción de la zona geográfica"
            If vRdo <> "" Then
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            Else
                Validar = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "RefreshGrid", ex)
        End Try
    End Function
End Class
