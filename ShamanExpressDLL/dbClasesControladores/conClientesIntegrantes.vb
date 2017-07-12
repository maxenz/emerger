Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesIntegrantes
    Inherits typClientesIntegrantes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function EliminarTodos(Optional ByVal pMsg As Boolean = True) As Boolean
        Dim SQL As String
        EliminarTodos = False
        Try
            Dim cmOpe As New SqlCommand

            SQL = "DELETE FROM ClientesIntegrantes"
            cmOpe.Connection = cnnsNET(Me.myCnnName)
            cmOpe.Transaction = cnnsTransNET(Me.myCnnName)
            cmOpe.CommandText = SQL
            cmOpe.ExecuteNonQuery()

            EliminarTodos = True
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Eliminar", ex, pMsg, Me.Tabla)
        End Try
    End Function
    Public Sub SetPurge(ByVal pCli As Int64)
        Dim SQL As String
        Try
            Dim cmOpe As New SqlCommand

            SQL = "UPDATE ClientesIntegrantes SET flgPurge = 1 WHERE ClienteId = " & pCli
            cmOpe.Connection = cnnsNET(Me.myCnnName)
            cmOpe.Transaction = cnnsTransNET(Me.myCnnName)
            cmOpe.CommandText = SQL
            cmOpe.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPurge", ex)
        End Try
    End Sub
    Public Sub DeletePurge(ByVal pCli As Int64)
        Dim SQL As String
        Try
            Dim cmOpe As New SqlCommand

            SQL = "DELETE FROM ClientesIntegrantes WHERE ClienteId = " & pCli & " AND flgPurge = 1"
            cmOpe.Connection = cnnsNET(Me.myCnnName)
            cmOpe.Transaction = cnnsTransNET(Me.myCnnName)
            cmOpe.CommandText = SQL
            cmOpe.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPurge", ex)
        End Try
    End Sub
    Public Function GetQueryBaseByCliente(ByVal pCli As Int64, Optional ByVal pGrp As Int64 = 0, Optional pQry As Integer = 1, Optional pLimit As Boolean = False) As DataTable

        GetQueryBaseByCliente = Nothing

        Try
            Dim SQL As String

            If Not pLimit Then
                SQL = "SELECT "
            Else
                SQL = "SELECT TOP 300 "
            End If

            SQL = SQL & "itg.ID, itg.NroAfiliado, itg.TipoIntegrante, itg.Apellido, itg.Nombre, itg.NroDocumento, itg.virActivo AS Activo "
            SQL = SQL & "FROM ClientesIntegrantes itg "
            SQL = SQL & "WHERE (itg.ClienteId = " & pCli & ") "

            If pQry = 1 Then SQL = SQL & "AND (itg.virActivo = 1) "
            If pQry = 2 Then SQL = SQL & "AND (itg.virActivo = 0) "
            If pGrp > 0 Then SQL = SQL & "AND ((itg.CliIntegranteSubGrupoId = " & pGrp & ") OR (itg.ID = " & pGrp & ")) "

            SQL = SQL & "ORDER BY itg.TipoIntegrante, itg.Apellido, itg.Nombre"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetQueryBaseByCliente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryBaseByCliente", ex)
        End Try
    End Function

    Public Function GetQueryAP(ByVal pCli As Int64) As DataTable

        GetQueryAP = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, NroDocumento, Apellido + ' ' + Nombre AS Nombre  "
            SQL = SQL & "FROM ClientesIntegrantes "
            SQL = SQL & "WHERE (ClienteId = " & pCli & ") "
            SQL = SQL & "ORDER BY NroDocumento"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetQueryAP = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryAP", ex)
        End Try
    End Function

    Public Sub EliminarTodos(ByVal pCli As Int64)
        Try
            Dim SQL As String = "DELETE FROM ClientesIntegrantes WHERE ClienteId = " & pCli
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()
        Catch ex As Exception
            HandleError(Me.GetType.Name, "EliminarTodos", ex)
        End Try
    End Sub

    Public Function GetIDByNroAfiliado(ByVal pCli As Long, ByVal pVal As String) As Int64
        GetIDByNroAfiliado = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM ClientesIntegrantes WHERE ClienteId = " & pCli & " AND NroAfiliado = '" & pVal & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByNroAfiliado = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByNroAfiliado", ex)
        End Try
    End Function

    Public Function GetIDByNroDocumento(ByVal pNdc As Int64, Optional ByVal pTdc As Int64 = 0, Optional pCli As Int64 = 0) As Int64
        GetIDByNroDocumento = 0
        Try
            If pNdc > 100000 Or pCli > 0 Then

                Dim SQL As String

                SQL = "SELECT ID FROM ClientesIntegrantes WHERE (NroDocumento = " & pNdc & ") "
                If pTdc > 0 Then SQL = SQL & "AND (TipoDocumentoId = " & pTdc & ")"
                If pCli > 0 Then SQL = SQL & "AND (ClienteId = " & pCli & ")"

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then GetIDByNroDocumento = CType(vOutVal, Int64)

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByNroDocumento", ex)
        End Try
    End Function

    Public Function Validar(ByRef pErrStr As String, Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        pErrStr = ""
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.NroAfiliado = "" Then vRdo = "El número de afiliado es incorrecto"
            If Me.TipoIntegrante = "" Then vRdo = "El tipo de integrante es incorrecto"
            If Me.IntegranteClasificacionId.GetObjectId = 0 Then vRdo = "Debe especificar la clasificación del integrante"
            If Me.Apellido = "" And vRdo = "" Then vRdo = "Debe especificar el apellido del integrante"
            If Me.Nombre = "" And vRdo = "" Then vRdo = "Debe especificar el nombre del integrante"
            If vRdo = "" Then
                Dim objClasificacion As New conIntegrantesClasificaciones

                If objClasificacion.Abrir(Me.IntegranteClasificacionId.GetObjectId) Then

                    If objClasificacion.flgIngresoMultiple = 0 Then
                        Dim SQL As String

                        SQL = "SELECT Apellido + ' ' + Nombre FROM ClientesIntegrantes "
                        SQL = SQL & "WHERE (ClienteId = " & Me.ClienteId.GetObjectId & ") AND (IntegranteClasificacionId = " & objClasificacion.ID & ") "
                        SQL = SQL & "AND (ID <> " & Me.ID & ") "

                        Dim cmVal As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                        Dim vApn As String = cmVal.ExecuteScalar

                        If Not vApn Is Nothing Then
                            vRdo = "El integrante " & vApn & vbCrLf & " Ya se encuentra clasificado como " & objClasificacion.Descripcion
                        End If

                    End If
                End If
            End If

            pErrStr = vRdo

            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Function GetCantidad(ByVal pCli As Int64) As Double
        GetCantidad = 0
        Try
            Dim SQL As String
            SQL = "SELECT ISNULL(COUNT(ID), 0) FROM ClientesIntegrantes WHERE ClienteId = " & pCli

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetCantidad = Val(vOutVal)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetCantidad", ex)
        End Try
    End Function

    Public Function GetByPhone(ByVal pTel As String) As Int64

        GetByPhone = 0

        Try

            If GetNumericValue(pTel) > 0 Then

                Dim SQL As String
                SQL = "SELECT TOP 1 ite.ID FROM ClientesIntegrantes ite "
                SQL = SQL & "INNER JOIN Clientes cli ON (ite.ClienteId = cli.ID) "
                SQL = SQL & "WHERE ((ite.Telefono01Fix = " & GetNumericValue(pTel) & ") OR (ite.Telefono02Fix = " & GetNumericValue(pTel) & ")) "
                SQL = SQL & "AND (cli.virActivo = 1) ORDER BY ite.ID DESC"

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then GetByPhone = CType(vOutVal, Int64)

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByPhone", ex)
        End Try

    End Function

    Public Function GetPlanAfiliado(ByVal pIte As Int64) As Int64
        GetPlanAfiliado = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 pli.PlanId FROM ClientesIntegrantesPlanes pli "
            SQL = SQL & "INNER JOIN ClientesIntegrantes itg ON pli.ClienteIntegranteId = itg.ID "
            SQL = SQL & "INNER JOIN Planes pla ON pli.PlanId = pla.ID AND itg.ClienteId = pla.ClienteId "
            SQL = SQL & "WHERE (pli.ClienteIntegranteId = " & pIte & ") "

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then

                GetPlanAfiliado = CType(vOutVal, Int64)

            Else

                SQL = "SELECT TOP 1 cli.PlanId FROM ClientesIntegrantes itg "
                SQL = SQL & "INNER JOIN Clientes cli ON itg.ClienteId = cli.ID "
                SQL = SQL & "WHERE (itg.ID = " & pIte & ") "

                cmdFind.CommandText = SQL
                vOutVal = CType(cmdFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then GetPlanAfiliado = CType(vOutVal, Int64)

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetPlanAfiliado", ex)
        End Try
    End Function

    ''' <summary>
    ''' Metodo para combos de shamanweb
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT * FROM viewIntegrantes ORDER BY [AbreviaturaId] DESC "

            Dim cmdCli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdCli.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

#Region "InfoPaciente"

    Public Function SetInfoPaciente(ByVal pId As Int64, ByVal dt As DataTable) As Boolean

        SetInfoPaciente = False

        Try

            SetInfoPaciente = True

            Dim SQL As String = "UPDATE ClientesIntegrantesInfoPaciente SET flgPurge = 1 WHERE ClienteIntegranteId = " & pId
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            Dim objClienteInfoPaciente As New typClientesIntegrantesInfoPaciente(Me.myCnnName)
            Dim vIdx As Integer

            For vIdx = 0 To dt.Rows.Count - 1

                If dt(vIdx)("Checked") Then

                    If Not objClienteInfoPaciente.Abrir(dt(vIdx)("ID")) Then

                        objClienteInfoPaciente.ClienteIntegranteId.SetObjectId(pId)
                        objClienteInfoPaciente.InfoPacienteItemId.SetObjectId(dt(vIdx)("InfoPacienteItemId"))

                    End If

                    objClienteInfoPaciente.Observaciones = dt(vIdx)("Observaciones")
                    objClienteInfoPaciente.flgPurge = 0

                    If Not objClienteInfoPaciente.Salvar(objClienteInfoPaciente) Then
                        SetInfoPaciente = False
                    End If

                End If

            Next

            SQL = "DELETE FROM ClientesIntegrantesInfoPaciente WHERE ClienteIntegranteId = " & pId & " AND flgPurge = 1"
            cmDel.CommandText = SQL
            cmDel.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetInfoPaciente", ex)
        End Try
    End Function

    Public Function GetInfoPacienteByIntegrante(ByVal pId As Int64) As DataTable

        GetInfoPacienteByIntegrante = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT itm.ID AS InfoPacienteItemId, grp.Descripcion AS InfoPacienteGrupo, itm.Descripcion "
            SQL = SQL & "FROM InfoPacienteItems itm "
            SQL = SQL & "INNER JOIN InfoPacienteGrupos grp ON (itm.InfoPacienteGrupoId = grp.ID) "
            SQL = SQL & "ORDER BY grp.Descripcion, itm.Descripcion"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdBas.ExecuteReader)

            dt.Columns.Add("ID", GetType(Int64))
            dt.Columns.Add("Checked", GetType(Boolean))
            dt.Columns.Add("Observaciones", GetType(String))

            For vIdx = 0 To dt.Rows.Count - 1

                SQL = "SELECT ID, Observaciones "
                SQL = SQL & "FROM ClientesIntegrantesInfoPaciente "
                SQL = SQL & "WHERE (ClienteIntegranteId = " & pId & ") AND (InfoPacienteItemId = " & dt(vIdx)(0) & ")"

                cmdBas.CommandText = SQL
                Dim dtCli As New DataTable
                dtCli.Load(cmdBas.ExecuteReader)

                If dtCli.Rows.Count > 0 Then
                    dt(vIdx)("ID") = dtCli(0)(0)
                    dt(vIdx)("Checked") = True
                    dt(vIdx)("Observaciones") = dtCli(0)(1)
                Else
                    dt(vIdx)("ID") = 0
                    dt(vIdx)("Checked") = False
                    dt(vIdx)("Observaciones") = ""
                End If

            Next vIdx

            GetInfoPacienteByIntegrante = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetInfoPacienteByIntegrante", ex)
        End Try
    End Function

#End Region

End Class
