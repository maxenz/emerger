Imports System.Data
Imports System.Data.SqlClient
Public Class conConfiguracion
    Inherits typConfiguracion
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function UpConfig() As Boolean

        UpConfig = False

        Try
            Dim SQL As String
            SQL = "SELECT ID FROM Configuracion"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then
                If Me.Abrir(CType(vOutVal, Int64)) Then
                    UpConfig = True
                End If
            Else
                Me.CleanProperties(Me)
                Me.prmLimitePediatrico = 14
                Me.hsNocDesde = "22:00"
                Me.hsNocHasta = "06:00"
                If Me.Salvar(Me) Then
                    UpConfig = Me.Abrir(Me.ID)
                End If
            End If

            '-----> Checkeo primer uso
            SQL = "SELECT TOP 1 ID FROM Usuarios"
            cmFind.CommandText = SQL

            vOutVal = CType(cmFind.ExecuteScalar, String)

            If vOutVal Is Nothing Then
                '-----> Agrego usuario SUPERVISOR
                Dim objUsuario As New conUsuarios(Me.myCnnName)

                objUsuario.CleanProperties(objUsuario)
                objUsuario.Identificacion = "SUPERVISOR"
                objUsuario.Nombre = "SUPERVISOR"
                objUsuario.Activo = 1

                If objUsuario.Salvar(objUsuario) Then
                    '-----> Le blanqueo el password
                    objUsuario.BlanquearPassword(objUsuario.ID, False)

                    '-----> Agrego perfil SUPERVISOR
                    Dim objPerfil As New conPerfiles(Me.myCnnName)

                    If objPerfil.GetPerfilAdministrador = 0 Then
                        objPerfil.Jerarquia = 1
                        objPerfil.CleanProperties(objPerfil)
                        objPerfil.Descripcion = "SUPERVISOR"
                        objPerfil.flgAdministrador = 1

                        If objPerfil.Salvar(objPerfil) Then
                            '-----> Relación
                            Dim objRelacion As New typUsuariosPerfiles(Me.myCnnName)

                            objRelacion.CleanProperties(objRelacion)
                            objRelacion.UsuarioId.SetObjectId(objUsuario.ID)
                            objRelacion.PerfilId.SetObjectId(objPerfil.ID)

                            objRelacion.Salvar(objRelacion)

                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "UpConfig", ex)
        End Try
    End Function
    Public Sub CheckDomicilioStruct(pDomMod As Integer)
        Try
            If pDomMod = 1 Then

                Dim SQL As String

                '----> Ajusto Calles

                SQL = "SELECT TABLE_NAME, COLUMN_NAME, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'dmCalle' AND CHARACTER_MAXIMUM_LENGTH = 70 "
                SQL = SQL & "AND TABLE_NAME <> 'viewClientes'"

                Dim cmdCal As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dtCal As New DataTable
                Dim vIdx As Integer
                dtCal.Load(cmdCal.ExecuteReader)

                For vIdx = 0 To dtCal.Rows.Count - 1

                    If dtCal(vIdx)(2) = "YES" Then
                        SQL = "ALTER TABLE " & dtCal(vIdx)(0) & " ALTER COLUMN " & dtCal(vIdx)(1) & " VARCHAR(200) "
                    Else
                        SQL = "ALTER TABLE " & dtCal(vIdx)(0) & " ALTER COLUMN " & dtCal(vIdx)(1) & " VARCHAR(200) NOT NULL "
                    End If

                    Dim cmdAlter As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    cmdAlter.ExecuteNonQuery()

                Next vIdx

                '----> Ajusto Domicilios

                SQL = "SELECT TABLE_NAME, COLUMN_NAME, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = 'Domicilio' AND CHARACTER_MAXIMUM_LENGTH = 100 "
                SQL = SQL & "AND TABLE_NAME <> 'viewClientes'"

                Dim cmdDom As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dtDom As New DataTable
                dtDom.Load(cmdDom.ExecuteReader)

                For vIdx = 0 To dtDom.Rows.Count - 1

                    If dtDom(vIdx)(2) = "YES" Then
                        SQL = "ALTER TABLE " & dtDom(vIdx)(0) & " ALTER COLUMN " & dtDom(vIdx)(1) & " VARCHAR(230) "
                    Else
                        SQL = "ALTER TABLE " & dtDom(vIdx)(0) & " ALTER COLUMN " & dtDom(vIdx)(1) & " VARCHAR(230) NOT NULL"
                    End If

                    Dim cmdAlter As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    cmdAlter.ExecuteNonQuery()

                Next vIdx

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "CheckDomicilioStruct", ex)
        End Try
    End Sub
    Public Sub CheckReporteadores()
        Try
            If Me.flgLaboratorio = 1 Then

                Dim SQL As String = "SELECT ID FROM ReporteadoresFields WHERE ReporteadorId = 1 AND Campo = 'labNroProtocolo'"

                Dim cmdChk As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmdChk.ExecuteScalar, String)
                If vOutVal Is Nothing Then

                    '-------------> labNroProtocolo

                    Dim objFields As New typReporteadoresFields

                    objFields.CleanProperties(objFields)

                    objFields.ReporteadorId.SetObjectId(rptReporteadores.Incidentes)
                    objFields.Campo = "labNroProtocolo"
                    objFields.Descripcion = "Nro. de Protocolo"
                    objFields.AliasTabla = "inc"

                    objFields.Salvar(objFields)

                    '-------------> labTestRapido

                    objFields.CleanProperties(objFields)

                    objFields.ReporteadorId.SetObjectId(rptReporteadores.Incidentes)
                    objFields.Campo = "labTestRapido"
                    objFields.Descripcion = "Test Laboratorio"
                    objFields.AliasTabla = "inc"

                    objFields.Salvar(objFields)

                    SQL = "UPDATE ReporteadoresFields SET Campo = 'CASE ISNULL(labTestRapido, 0) WHEN 0 THEN ' + CHAR(39) + CHAR(39) + ' WHEN 1 THEN ' + CHAR(39) + 'Negativo' + CHAR(39) + '  WHEN 2 THEN ' + CHAR(39) + 'Positivo' + CHAR(39) + ' END' WHERE Campo = 'labTestRapido'"

                    Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    cmdUpd.ExecuteNonQuery()

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CheckReporteadores", ex)
        End Try
    End Sub

    Public Function isPediatrico(ByVal pEda As Integer) As Boolean
        isPediatrico = False
        Try
            If pEda <= Me.prmLimitePediatrico Then
                isPediatrico = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "isPediatrico", ex)
        End Try
    End Function

    Public Function haveReposicionCierre() As Boolean
        haveReposicionCierre = False
        Try
            If Me.incReposicionCierre = 1 Then
                If haveProductoSub(shamanProductos.Express, "09") Then
                    Dim objTiposMovimientos As New conTiposMovimientosInsumos
                    If objTiposMovimientos.GetDefault > 0 Then
                        haveReposicionCierre = True
                    End If
                End If
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "haveReposicionCierre", ex)
        End Try
    End Function

    Public Function havePracticasCierre() As Boolean
        havePracticasCierre = False
        Try
            If Me.incPracticasCierre = 1 Then
                If haveProductoSub(shamanProductos.Express, "20") Then
                    havePracticasCierre = True
                End If
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "haveReposicionCierre", ex)
        End Try
    End Function

    Public Function haveEncuestasCierre() As Boolean
        haveEncuestasCierre = False
        Try
            If Me.incEncuestaCierre = 1 Then
                If haveProductoSub(shamanProductos.Express, "13") Then
                    haveEncuestasCierre = True
                End If
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "haveEncuestasCierre", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Not valTime(Me.hsNocDesde, False, False) Then vRdo = "La hora desde nocturna establecida es incorrecta"
            If Not valTime(Me.hsNocHasta, False, False) And vRdo = "" Then vRdo = "La hora hasta nocturna establecida es incorrecta"
            If Me.opeRefresh < 3 Then vRdo = "Debe establecer un mínimo de 3 segundos para el refresco operativo"
            If Me.segModoLogin = 1 And Me.segServidorDominio = "" Then vRdo = "Debe establecer el servidor de dominio para el método de login especificado"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Function GetReglaEtiqueta(pVal As String) As String
        GetReglaEtiqueta = pVal
        Try

            If Me.ConfiguracionRegionalId.ID > 0 Then

                Dim objRegla As New conConfiguracionesRegionalesReglas(Me.myCnnName)

                Dim vTxt As String = objRegla.GetValor2(Me.ConfiguracionRegionalId.ID, pVal)

                If vTxt <> "" Then GetReglaEtiqueta = vTxt

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetReglaEtiqueta", ex)
        End Try
    End Function


End Class
