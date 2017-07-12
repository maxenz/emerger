Imports System.Data
Imports System.Data.SqlClient
Public Class contmpClientesIntegrantes
    Inherits typtmpClientesIntegrantes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Sub InitTemp(ByVal pPid As Int64)
        Try
            Dim SQL As String = "DELETE FROM tmpClientesIntegrantes WHERE PID = " & pPid
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()
        Catch ex As Exception
            HandleError(Me.GetType.Name, "InitTemp", ex)
        End Try
    End Sub
    Public Function GetQueryBaseByPID(ByVal pPid As Int64, Optional ByVal pGrp As Int64 = 0, Optional pQry As Integer = 1) As DataTable

        GetQueryBaseByPID = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT itg.ID, itg.NroAfiliado, itg.TipoIntegrante, itg.Apellido, itg.Nombre, itg.NroDocumento, itg.virActivo AS Activo "
            SQL = SQL & "FROM tmpClientesIntegrantes itg "
            SQL = SQL & "WHERE (itg.PID = " & pPid & ") "

            If pQry = 1 Then SQL = SQL & "AND (itg.virActivo = 1) "
            If pQry = 2 Then SQL = SQL & "AND (itg.virActivo = 0) "
            If pGrp > 0 Then SQL = SQL & "AND ((itg.CliIntegranteSubGrupoId = " & pGrp & ") OR (itg.ID = " & pGrp & ")) "

            SQL = SQL & "ORDER BY itg.TipoIntegrante, itg.Apellido, itg.Nombre"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetQueryBaseByPID = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryBaseByPID", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
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

                        SQL = "SELECT Apellido + ' ' + Nombre FROM tmpClientesIntegrantes "
                        SQL = SQL & "WHERE (PID = " & Me.PID & ") AND (IntegranteClasificacionId = " & objClasificacion.ID & ") "
                        SQL = SQL & "AND (ID <> " & Me.ID & ") "

                        Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                        Dim vApn As String = CType(cmFind.ExecuteScalar, String)
                        If Not vApn Is Nothing Then
                            vRdo = "El integrante " & vApn & vbCrLf & " Ya se encuentra clasificado como " & objClasificacion.Descripcion
                        End If

                    End If
                End If
            End If
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
    Public Sub SaveToProduccion(ByVal pCli As Int64, ByVal pPid As Int64)
        Try
            Dim SQL As String
            Dim objTemporal As New typtmpClientesIntegrantes(Me.myCnnName)

            SQL = "SELECT ID FROM tmpClientesIntegrantes WHERE PID = " & pPid

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdGrl.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                If objTemporal.Abrir(dt(vIdx).Item(0)) Then

                    Dim objClienteIntegrante As New typClientesIntegrantes(Me.myCnnName)

                    objClienteIntegrante.CleanProperties(objClienteIntegrante)
                    objClienteIntegrante.ClienteId.SetObjectId(pCli)
                    objClienteIntegrante.NroAfiliado = objTemporal.NroAfiliado
                    objClienteIntegrante.TipoIntegrante = objTemporal.TipoIntegrante
                    objClienteIntegrante.IntegranteClasificacionId.SetObjectId(objTemporal.IntegranteClasificacionId.ID)
                    objClienteIntegrante.Apellido = objTemporal.Apellido
                    objClienteIntegrante.Nombre = objTemporal.Nombre
                    objClienteIntegrante.LocalidadId.SetObjectId(objTemporal.LocalidadId.ID)
                    objClienteIntegrante.Domicilio = objTemporal.Domicilio
                    objClienteIntegrante.CodigoPostal = objTemporal.CodigoPostal
                    objClienteIntegrante.TipoDocumentoId.SetObjectId(objTemporal.TipoDocumentoId.ID)
                    objClienteIntegrante.NroDocumento = objTemporal.NroDocumento
                    objClienteIntegrante.Sexo = objTemporal.Sexo
                    objClienteIntegrante.FecNacimiento = objTemporal.FecNacimiento
                    objClienteIntegrante.Telefono01 = objTemporal.Telefono01
                    objClienteIntegrante.Telefono02 = objTemporal.Telefono02
                    objClienteIntegrante.Telefono01Fix = objTemporal.Telefono01Fix
                    objClienteIntegrante.Telefono02Fix = objTemporal.Telefono02Fix
                    objClienteIntegrante.FecIngreso = objTemporal.FecIngreso
                    objClienteIntegrante.FecEgreso = objTemporal.FecEgreso
                    objClienteIntegrante.eduIntegranteCursoId.SetObjectId(objTemporal.eduIntegranteCursoId.ID)
                    objClienteIntegrante.eduDivision = objTemporal.eduDivision
                    objClienteIntegrante.eduTurno = objTemporal.eduTurno
                    objClienteIntegrante.barBanderaBarcoId.SetObjectId(objTemporal.barBanderaBarcoId.ID)
                    objClienteIntegrante.barNroIMO = objTemporal.barNroIMO
                    objClienteIntegrante.barNroOficial = objTemporal.barNroOficial
                    objClienteIntegrante.barTipoBarcoId.SetObjectId(objTemporal.barTipoBarcoId.ID)
                    objClienteIntegrante.flgIvaGravado = objTemporal.flgIvaGravado
                    objClienteIntegrante.CoberturaIndividualId.SetObjectId(objTemporal.CoberturaIndividualId.ID)
                    objClienteIntegrante.Observaciones = objTemporal.Observaciones

                    If objClienteIntegrante.Salvar(objClienteIntegrante) Then

                        '----> Atributos Dinámicos

                        SQL = "SELECT ID FROM tmpClientesIntegrantesAtributos WHERE tmpClienteIntegranteId = " & objTemporal.ID

                        Dim cmdAtr As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                        Dim dtAtr As New DataTable
                        Dim vIdxAtr As Integer
                        dtAtr.Load(cmdAtr.ExecuteReader)

                        For vIdxAtr = 0 To dtAtr.Rows.Count - 1

                            Dim objTemporalAtributo As New typtmpClientesIntegrantesAtributos(Me.myCnnName)

                            If objTemporalAtributo.Abrir(dtAtr(vIdxAtr)(0)) Then

                                Dim objClienteIntegranteAtributo As New typClientesIntegrantesAtributos(Me.myCnnName)

                                objClienteIntegranteAtributo.CleanProperties(objClienteIntegranteAtributo)
                                objClienteIntegranteAtributo.ClienteIntegranteId.SetObjectId(objClienteIntegrante.ID)
                                objClienteIntegranteAtributo.AtributoDinamicoId.SetObjectId(objTemporalAtributo.AtributoDinamicoId.ID)
                                objClienteIntegranteAtributo.Valor1 = objTemporalAtributo.Valor1
                                objClienteIntegranteAtributo.Valor2 = objTemporalAtributo.Valor2

                                objClienteIntegranteAtributo.Salvar(objClienteIntegranteAtributo)

                                objClienteIntegranteAtributo = Nothing

                            End If

                        Next vIdxAtr

                        '----> InfoPaciente

                        SQL = "SELECT ID FROM tmpClientesIntegrantesInfoPaciente WHERE tmpClienteIntegranteId = " & objTemporal.ID

                        Dim cmdInf As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                        Dim dtInf As New DataTable
                        Dim vIdxInf As Integer
                        dtInf.Load(cmdInf.ExecuteReader)

                        For vIdxInf = 0 To dtInf.Rows.Count - 1

                            Dim objTemporalInfo As New typtmpClientesIntegrantesInfoPaciente(Me.myCnnName)

                            If objTemporalInfo.Abrir(dtInf(vIdxInf)(0)) Then

                                Dim objClienteIntegranteInfoPaciente As New typClientesIntegrantesInfoPaciente(Me.myCnnName)

                                objClienteIntegranteInfoPaciente.CleanProperties(objClienteIntegranteInfoPaciente)
                                objClienteIntegranteInfoPaciente.ClienteIntegranteId.SetObjectId(objClienteIntegrante.ID)
                                objClienteIntegranteInfoPaciente.InfoPacienteItemId.SetObjectId(objTemporalInfo.InfoPacienteItemId.ID)
                                objClienteIntegranteInfoPaciente.Observaciones = objTemporalInfo.Observaciones

                                objClienteIntegranteInfoPaciente.Salvar(objClienteIntegranteInfoPaciente)

                                objClienteIntegranteInfoPaciente = Nothing

                            End If

                        Next vIdxInf


                    End If

                    objClienteIntegrante = Nothing

                End If

            Next vIdx

            '---------> SubGrupos...
            SQL = "SELECT son.ID, itg.ID FROM tmpClientesIntegrantes tmp "
            SQL = SQL & "INNER JOIN tmpClientesIntegrantes pad ON (tmp.CliIntegranteSubGrupoId = pad.ID) "
            SQL = SQL & "INNER JOIN ClientesIntegrantes itg ON (pad.NroAfiliado = itg.NroAfiliado) "
            SQL = SQL & "INNER JOIN ClientesIntegrantes son ON (tmp.NroAfiliado = son.NroAfiliado) "
            SQL = SQL & "WHERE (tmp.PID = " & pPid & ") AND (itg.ClienteId = " & pCli & ") AND (son.ClienteId = " & pCli & ") "
            SQL = SQL & "AND (tmp.CliIntegranteSubGrupoId > 0) "


            Dim cmdSub As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dtSub As New DataTable
            dtSub.Load(cmdSub.ExecuteReader)

            For vIdx = 0 To dtSub.Rows.Count - 1

                Dim objClienteIntegrante As New typClientesIntegrantes(Me.myCnnName)

                If objClienteIntegrante.Abrir(dtSub(vIdx)(0)) Then
                    objClienteIntegrante.CliIntegranteSubGrupoId.SetObjectId(dtSub(vIdx)(1))
                    objClienteIntegrante.Salvar(objClienteIntegrante)
                End If

            Next

            objTemporal = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SaveToProduccion", ex)
        End Try
    End Sub

    Public Function GetCantidad(ByVal pPid As Int64) As Double
        GetCantidad = 0
        Try
            Dim SQL As String
            '--------> Query
            SQL = "SELECT ISNULL(COUNT(ID), 0) FROM tmpClientesIntegrantes WHERE PID = " & pPid

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetCantidad = CType(vOutVal, Double)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetCantidad", ex)
        End Try
    End Function

#Region "InfoPaciente"

    Public Function SetInfoPaciente(ByVal pId As Int64, ByVal dt As DataTable) As Boolean

        SetInfoPaciente = False

        Try

            SetInfoPaciente = True

            Dim SQL As String = "UPDATE tmpClientesIntegrantesInfoPaciente SET flgPurge = 1 WHERE tmpClienteIntegranteId = " & pId
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            Dim objClienteInfoPaciente As New typtmpClientesIntegrantesInfoPaciente(Me.myCnnName)
            Dim vIdx As Integer

            For vIdx = 0 To dt.Rows.Count - 1

                If dt(vIdx)("Checked") Then

                    If Not objClienteInfoPaciente.Abrir(dt(vIdx)("ID")) Then

                        objClienteInfoPaciente.tmpClienteIntegranteId.SetObjectId(pId)
                        objClienteInfoPaciente.InfoPacienteItemId.SetObjectId(dt(vIdx)("InfoPacienteItemId"))

                    End If

                    objClienteInfoPaciente.Observaciones = dt(vIdx)("Observaciones")
                    objClienteInfoPaciente.flgPurge = 0

                    If Not objClienteInfoPaciente.Salvar(objClienteInfoPaciente) Then
                        SetInfoPaciente = False
                    End If

                End If

            Next

            SQL = "DELETE FROM tmpClientesIntegrantesInfoPaciente WHERE tmpClienteIntegranteId = " & pId & " AND flgPurge = 1"
            cmDel.CommandText = SQL
            cmDel.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetInfoPaciente", ex)
        End Try
    End Function

    Public Function GetInfoPacienteByIntegrantePID(ByVal pId As Int64) As DataTable

        GetInfoPacienteByIntegrantePID = Nothing

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
                SQL = SQL & "FROM tmpClientesIntegrantesInfoPaciente "
                SQL = SQL & "WHERE (tmpClienteIntegranteId = " & pId & ") AND (InfoPacienteItemId = " & dt(vIdx)(0) & ")"

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

            GetInfoPacienteByIntegrantePID = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetInfoPacienteByIntegrantePID", ex)
        End Try
    End Function

#End Region


End Class
