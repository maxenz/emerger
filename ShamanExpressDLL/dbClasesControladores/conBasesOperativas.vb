Imports System.Data
Imports System.Data.SqlClient
Public Class conBasesOperativas
    Inherits typBasesOperativas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Overrides Function CanDelete(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean
        CanDelete = False

        Try
            Dim SQL As String = "SELECT TOP 1 Movil FROM Moviles WHERE BaseOperativaId = " & pId
            Dim cmdCob As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vVal As String = cmdCob.ExecuteScalar

            If Not vVal Is Nothing Then
                If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "La base operativa en cuestión está vinculada al móvil " & vVal, MsgBoxStyle.Critical, "Shaman")
            Else
                SQL = "SELECT TOP 1 mov.FecMovimiento FROM InsumosMovimientos mov "
                SQL = SQL & "INNER JOIN BasesOperativas bas ON (mov.CentroAtencionId = bas.CentroAtencionId) "
                SQL = SQL & "WHERE (bas.ID = " & pId & ") "
                cmdCob.CommandText = SQL
                vVal = cmdCob.ExecuteScalar
                If Not vVal Is Nothing Then
                    If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "La base operativa en cuestión está vinculada a movimientos de insumos de la fecha " & vVal, MsgBoxStyle.Critical, "Shaman")
                Else
                    CanDelete = True
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CanDelete", ex)
        End Try
    End Function

    Public Function GetQueryBase() As DataTable

        GetQueryBase = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT bas.ID, bas.AbreviaturaId, bas.Descripcion, bas.Domicilio, loc.AbreviaturaId AS Localidad "
            SQL = SQL & "FROM BasesOperativas bas "
            SQL = SQL & "INNER JOIN Localidades loc ON (bas.LocalidadId = loc.ID) "
            SQL = SQL & " ORDER BY bas.AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetQueryBase = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryBase", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador de la base operativa"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar el nombre de la base operativa"
            If Me.LocalidadId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe determinar la localidad de la base operativa"
            If Me.Domicilio.dmCalle = "" And vRdo = "" Then vRdo = "Debe determinar el domicilio de la base operativa"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function


End Class
