Imports System.Data
Imports System.Data.SqlClient
Public Class conUsuariosNodos
    Inherits typUsuariosNodos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pUsr As Int64, ByVal pTip As tipUsoRelacion, ByVal pNod As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM UsuariosNodos WHERE UsuarioId = " & pUsr & " AND TipoRelacion = " & pTip & " AND sysNodoId = " & pNod

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function GetExtranetByUsuario(pUsr As Int64) As DataTable
        GetExtranetByUsuario = Nothing
        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(und.ID, 0) AS ID, nod.ID AS sysNodoId, nod.Jerarquia, nod.Descripcion, ISNULL(und.Acceso, 0) AS Acceso "

            SQL = SQL & "FROM sysNodos nod "
            SQL = SQL & "LEFT JOIN UsuariosNodos und ON (und.sysNodoId = nod.ID) AND (und.UsuarioId = " & pUsr & ") "

            SQL = SQL & "WHERE (nod.ProductoId = 200) "
            SQL = SQL & "ORDER BY sysNodoId"

            Dim cmdRel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdRel.ExecuteReader)

            GetExtranetByUsuario = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByUsuario", ex)
        End Try
    End Function
    Public Sub SetNodosExtranet(pUsr As Int64, dt As DataTable)
        Try
            Dim vIdx As Integer
            Dim objUsuarioNodo As New conUsuariosNodos

            For vIdx = 0 To dt.Rows.Count - 1

                If dt(vIdx)("Acceso") > 0 Then

                    If Not objUsuarioNodo.Abrir(dt(vIdx)("ID")) Then

                        objUsuarioNodo.UsuarioId.SetObjectId(pUsr)
                        objUsuarioNodo.sysNodoId.SetObjectId(dt(vIdx)("sysNodoId"))

                    End If

                    objUsuarioNodo.TipoRelacion = tipUsoRelacion.tExtranet
                    objUsuarioNodo.Acceso = dt(vIdx)("Acceso")

                    objUsuarioNodo.Salvar(objUsuarioNodo)

                Else

                    If objUsuarioNodo.Eliminar(dt(vIdx)("ID")) Then
                    End If

                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetRelaciones", ex)
        End Try
    End Sub


End Class
