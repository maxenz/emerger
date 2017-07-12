Imports System.Data
Imports System.Data.SqlClient
Public Class conUsuariosRelaciones
    Inherits typUsuariosRelaciones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByUsuario(pUsr As Int64) As DataTable
        GetByUsuario = Nothing
        Try
            Dim SQL As String

            SQL = "SELECT rel.ID, "
            SQL = SQL & "CASE ISNULL(rel.PrestadorId, 0) WHEN 0 THEN CASE ISNULL(rel.PersonalId, 0) WHEN 0 THEN 2 ELSE 1 END ELSE 0 END AS TipoRelacion, "
            SQL = SQL & "CASE ISNULL(rel.PrestadorId, 0) WHEN 0 THEN CASE ISNULL(rel.PersonalId, 0) WHEN 0 THEN cli.ID ELSE per.ID END ELSE pre.ID END AS RelacionId, "
            SQL = SQL & "CASE ISNULL(rel.PrestadorId, 0) WHEN 0 THEN CASE ISNULL(rel.PersonalId, 0) WHEN 0 THEN cli.AbreviaturaId ELSE per.Legajo END ELSE pre.AbreviaturaId END AS IdentificacionId, "
            SQL = SQL & "CASE ISNULL(rel.PrestadorId, 0) WHEN 0 THEN CASE ISNULL(rel.PersonalId, 0) WHEN 0 THEN cli.RazonSocial ELSE per.Apellido + ' ' + per.Nombre END ELSE pre.RazonSocial END AS Nombre "

            SQL = SQL & "FROM UsuariosRelaciones rel "
            SQL = SQL & "LEFT JOIN Prestadores pre ON rel.PrestadorId = pre.ID "
            SQL = SQL & "LEFT JOIN Personal per ON rel.PersonalId = per.ID "
            SQL = SQL & "LEFT JOIN Clientes cli ON rel.ClienteId = cli.ID "

            SQL = SQL & "WHERE (rel.UsuarioId = " & pUsr & ") "
            SQL = SQL & "ORDER BY 1, 3"

            Dim cmdRel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdRel.ExecuteReader)

            GetByUsuario = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByUsuario", ex)
        End Try
    End Function

    Public Sub SetRelaciones(pUsr As Int64, dt As DataTable)
        Try
            Dim vIdx As Integer
            Dim objRelacion As New typUsuariosRelaciones

            Dim SQL As String = "UPDATE UsuariosRelaciones SET flgPurge = 1 WHERE UsuarioId = " & pUsr
            Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdUpd.ExecuteNonQuery()

            For vIdx = 0 To dt.Rows.Count - 1

                objRelacion = New typUsuariosRelaciones(Me.myCnnName)

                If Not objRelacion.Abrir(dt(vIdx)("ID")) Then
                    objRelacion.UsuarioId.SetObjectId(pUsr)
                End If

                Select Case dt(vIdx)("TipoRelacion")
                    Case 0
                        objRelacion.PrestadorId.SetObjectId(dt(vIdx)("RelacionId"))
                        objRelacion.PersonalId.SetObjectId(0)
                        objRelacion.ClienteId.SetObjectId(0)
                    Case 1
                        objRelacion.PersonalId.SetObjectId(dt(vIdx)("RelacionId"))
                        objRelacion.PrestadorId.SetObjectId(0)
                        objRelacion.ClienteId.SetObjectId(0)
                    Case 2
                        objRelacion.ClienteId.SetObjectId(dt(vIdx)("RelacionId"))
                        objRelacion.PrestadorId.SetObjectId(0)
                        objRelacion.PrestadorId.SetObjectId(0)
                End Select

                objRelacion.flgPurge = 0

                objRelacion.Salvar(objRelacion)
                objRelacion = Nothing

            Next vIdx

            SQL = "DELETE FROM UsuariosRelaciones WHERE UsuarioId = " & pUsr & " AND flgPurge = 1"
            cmdUpd.CommandText = SQL
            cmdUpd.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetRelaciones", ex)
        End Try
    End Sub

End Class
