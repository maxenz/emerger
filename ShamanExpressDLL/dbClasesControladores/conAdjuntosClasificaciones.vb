Imports System.Data
Imports System.Data.SqlClient
Public Class conAdjuntosClasificaciones
    Inherits typAdjuntosClasificaciones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByDescripcion(ByVal pVal As String) As Int64
        GetIDByDescripcion = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM AdjuntosClasificaciones WHERE Descripcion = '" & pVal & "'"
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try

    End Function
    Public Overrides Function CanDelete(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean
        CanDelete = False

        Try
            Dim SQL As String = "SELECT TOP 1 inc.FecIncidente FROM IncidentesAdjuntos adj INNER JOIN Incidentes inc ON adj.IncidenteId = inc.ID WHERE adj.AdjuntoClasificacionId = " & pId
            Dim cmAli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vVal As String = cmAli.ExecuteScalar

            If Not vVal Is Nothing Then
                If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "La clasificación de adjuntos en cuestión está vinculada a un servicio de la fecha " & vVal, MsgBoxStyle.Critical, "Shaman")
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

            SQL = "SELECT ID, Descripcion, RutaRemota, Extensiones "
            SQL = SQL & "FROM AdjuntosClasificaciones "
            SQL = SQL & "ORDER BY Descripcion"

            Dim cmdAli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdAli.ExecuteReader)

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
            If Me.Descripcion = "" Then vRdo = "Debe determinar la descripción de la clasificación de adjunto"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class
