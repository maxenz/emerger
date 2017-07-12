Imports System.Data
Imports System.Data.SqlClient
Public Class conPrestadoresClientes
    Inherits typPrestadoresClientes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByPrestador(ByVal pPre As Int64) As DataTable

        GetByPrestador = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT pre.ID, pre.ClienteId, cli.AbreviaturaId AS Cliente, ISNULL(cli.RazonSocial, 'A todos los clientes') AS RazonSocial, ISNULL(tmv.Descripcion, '') AS Cobertura, "
            SQL = SQL & "pre.Cantidad, pre.Cantidad * pre.Importe AS Importe "
            SQL = SQL & "FROM PrestadoresClientes pre "
            SQL = SQL & "LEFT JOIN Clientes cli ON pre.ClienteId = cli.ID "
            SQL = SQL & "LEFT JOIN TiposMoviles tmv ON pre.TipoMovilId = tmv.ID "
            SQL = SQL & "WHERE (pre.PrestadorId = " & pPre & ") ORDER BY cli.AbreviaturaId"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetByPrestador = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByPrestador", ex)
        End Try
    End Function
    Public Sub DeleteLocalidades(ByVal pMov As Int64, ByVal pPreCli As Int64)

        Try

            Dim cmOpe As New SqlCommand

            Dim SQL As String

            SQL = "UPDATE MovilesLocalidades SET PrestadorClienteId = 0 WHERE MovilId = " & pMov & " AND PrestadorClienteId = " & pPreCli

            cmOpe.Connection = cnnsNET(Me.myCnnName)
            cmOpe.Transaction = cnnsTransNET(Me.myCnnName)
            cmOpe.CommandText = SQL
            cmOpe.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "DeleteLocalidades", ex)
        End Try
    End Sub

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            'If Me.ClienteId.GetObjectId = 0 Then vRdo = "Debe especificar el cliente de la capita del prestador"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class
