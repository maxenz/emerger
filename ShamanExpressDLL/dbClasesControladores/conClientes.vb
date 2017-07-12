Imports System.Data
Imports System.Data.SqlClient
Public Class conClientes
    Inherits typClientes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function EliminarTodos(Optional ByVal pMsg As Boolean = True) As Boolean
        Dim SQL As String
        EliminarTodos = False
        Try
            Dim cmOpe As New SqlCommand

            SQL = "DELETE FROM Clientes"
            cmOpe.Connection = cnnsNET(Me.myCnnName)
            cmOpe.Transaction = cnnsTransNET(Me.myCnnName)
            cmOpe.CommandText = SQL
            cmOpe.ExecuteNonQuery()

            EliminarTodos = True
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Eliminar", ex, pMsg, Me.Tabla)
        End Try
    End Function

    Public Function GetConvenios() As DataTable

        GetConvenios = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId AS Codigo, RazonSocial "
            SQL = SQL & "FROM Clientes "
            SQL = SQL & "WHERE (TipoConvenio = 2) AND (virActivo = 1) "
            SQL = SQL & " ORDER BY AbreviaturaId"

            Dim cmdCli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdCli.ExecuteReader)

            GetConvenios = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetConvenios", ex)
        End Try
    End Function

    Public Function GetByConvenio(ByVal pCnv As Int64) As DataTable

        GetByConvenio = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT cli.ID, cli.AbreviaturaId, cli.RazonSocial, cli.CUIT "
            SQL = SQL & "FROM Clientes cli "
            SQL = SQL & "WHERE (ConvenioId = " & pCnv & ") "
            SQL = SQL & "ORDER BY cli.AbreviaturaId"

            Dim cmdCli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdCli.ExecuteReader)

            GetByConvenio = dt

        Catch ex As Exception

            HandleError(Me.GetType.Name, "GetByConvenio", ex)

        End Try
    End Function

    Public Function GetMetodosServiciosExternos(Optional ByVal pAct As Integer = 1) As DataTable

        GetMetodosServiciosExternos = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, RazonSocial, CASE rcpMetodoId WHEN 1 THEN 'WS' WHEN 2 THEN 'E-Mail' WHEN 3 THEN 'FTP' WHEN 4 THEN 'Rabbit MQ' ELSE '' END AS Metodo "
            SQL = SQL & "FROM Clientes "
            SQL = SQL & "WHERE (rcpMetodoId > 0) "
            If pAct = 1 Then SQL = sqlWhere(SQL) & "(virActivo = 1)"
            If pAct = 2 Then SQL = sqlWhere(SQL) & "(virActivo = 0)"
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdCli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdCli.ExecuteReader)

            GetMetodosServiciosExternos = dt

        Catch ex As Exception

            HandleError(Me.GetType.Name, "GetMetodosServiciosExternos", ex)

        End Try

    End Function

    Public Function GetEquivalencias(Optional ByVal pAct As Integer = 1) As DataTable

        GetEquivalencias = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, RazonSocial, SPACE(200) AS Equivalencias "
            SQL = SQL & "FROM Clientes "
            SQL = SQL & "WHERE (rcpMetodoId > 0) "
            If pAct = 1 Then SQL = sqlWhere(SQL) & "(virActivo = 1)"
            If pAct = 2 Then SQL = sqlWhere(SQL) & "(virActivo = 0)"
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdCli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdCli.ExecuteReader)

            dt.Columns("Equivalencias").ReadOnly = False

            For vIdx = 0 To dt.Rows.Count - 1

                Dim vCnt As Int64 = 0
                Dim vEqv As String = ""

                SQL = "SELECT ISNULL(COUNT(rgl.ID), 0) FROM ConfiguracionesRegionalesReglas rgl "
                SQL = SQL & "INNER JOIN ConfiguracionesRegionales cab ON rgl.ConfiguracionRegionalId = cab.ID "
                SQL = SQL & "WHERE (cab.flgInterfaz = 1) AND (cab.ClienteId = " & dt(vIdx)("ID") & ") "

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then vCnt = CType(vOutVal, Int64)

                Select Case vCnt
                    Case 0 : vEqv = "No se registran equivalencias"
                    Case 1 : vEqv = "Se encontró 1 equivalencia registrada"
                    Case Else : vEqv = "Se encontraron " & vCnt & " equivalencias registradas"
                End Select

                dt(vIdx)("Equivalencias") = vEqv

            Next

            GetEquivalencias = dt

        Catch ex As Exception

            HandleError(Me.GetType.Name, "GetEquivalencias", ex)

        End Try

    End Function

    Public Function GetIDByAbreviaturaId(ByVal pVal As String, Optional pValAct As Boolean = False) As Int64
        GetIDByAbreviaturaId = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID, virActivo FROM Clientes WHERE (AbreviaturaId = '" & pVal & "') "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dtAbr As New DataTable
            dtAbr.Load(cmFind.ExecuteReader)

            If dtAbr.Rows.Count > 0 Then

                If Not pValAct Then
                    GetIDByAbreviaturaId = dtAbr(0)(0)
                Else
                    If dtAbr(0)(1) = 1 Then
                        GetIDByAbreviaturaId = dtAbr(0)(0)
                    End If
                End If

            Else

                If shamanConfig.cliGeneracionCodigo = 1 And IsNumeric(pVal) Then

                    '----> MAXLEN
                    SQL = "SELECT ISNULL(MAX(LEN(AbreviaturaId)), 0) FROM Clientes"
                    cmFind.CommandText = SQL
                    Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

                    If Not vOutVal Is Nothing Then

                        Dim vZerLen = CType(vOutVal, Integer)
                        Dim vIdx As Integer
                        Dim vZer As String = ""

                        For vIdx = 0 To vZerLen - 1
                            vZer = vZer & "0"
                        Next

                        SQL = "SELECT ID FROM Clientes WHERE (AbreviaturaId = '" & Format(Val(pVal), vZer) & "') "
                        If pValAct Then SQL = SQL & "AND (virActivo = 1) "

                        cmFind.CommandText = SQL
                        vOutVal = CType(cmFind.ExecuteScalar, String)
                        If Not vOutVal Is Nothing Then GetIDByAbreviaturaId = CType(vOutVal, Int64)

                    End If

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbreviaturaId", ex)
        End Try

    End Function
    Public Function GetIDByAbrevituraIdLike(ByVal pVal As String, Optional pValAct As Boolean = False) As Int64
        GetIDByAbrevituraIdLike = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Clientes WHERE (AbreviaturaId LIKE '%" & pVal & "') "
            If pValAct Then SQL = SQL & "AND (virActivo = 1) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAbrevituraIdLike = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbrevituraIdLike", ex)
        End Try

    End Function

    Public Function GetIDByRazonSocial(ByVal pVal As String, Optional pValAct As Boolean = False) As Int64
        GetIDByRazonSocial = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Clientes WHERE (RazonSocial = '" & pVal & "') "
            If pValAct Then SQL = SQL & "AND (virActivo = 1) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByRazonSocial = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByRazonSocial", ex)
        End Try

    End Function

    Public Function GetIDByCUIT(ByVal pVal As String, Optional pValAct As Boolean = False) As Int64
        GetIDByCUIT = 0
        Try
            Dim SQL As String

            If Not pVal.Contains("-") Then
                If pVal.Length = 11 Then
                    pVal = pVal.Substring(0, 2) & "-" & pVal.Substring(2, 8) & "-" & pVal.Substring(10, 1)
                ElseIf pVal.Length = 8 Then
                    pVal = "00-" & pVal & "-0"
                End If
            End If

            SQL = "SELECT ID FROM Clientes WHERE (CUIT = '" & pVal & "') "
            If pValAct Then SQL = SQL & "AND (virActivo = 1) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByCUIT = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByCUIT", ex)
        End Try

    End Function

    Public Function GetNextNroAbreviaturaId(Optional pRubId As Decimal = 0, Optional pRubAbr As String = "") As String
        GetNextNroAbreviaturaId = ""
        Try
            Dim SQL As String

            If pRubAbr = "" Then
                SQL = "SELECT ISNULL(MAX(AbreviaturaId), '') FROM Clientes "
            Else
                SQL = "SELECT ISNULL(MAX(AbreviaturaId), '') FROM Clientes WHERE (RubroClienteId = " & pRubId & ") AND (SUBSTRING(AbreviaturaId, 1, " & pRubAbr.Length & ") = '" & pRubAbr & "')"
            End If

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            If Not vOutVal Is Nothing Then

                If pRubAbr = "" Then

                    GetNextNroAbreviaturaId = Format(Val(vOutVal) + 1, "00000")

                Else
                    Try

                        If vOutVal = "" Then

                            GetNextNroAbreviaturaId = pRubAbr & "00001"

                        Else

                            Dim vLenZer As Integer = vOutVal.Length - pRubAbr.Length

                            If vLenZer > 0 Then

                                Dim vIdx As Integer
                                Dim vZerStr As String = ""

                                For vIdx = 0 To vLenZer - 1
                                    vZerStr = vZerStr & "0"
                                Next

                                Dim vCod As String = pRubAbr & Format(Val(vOutVal.Substring(pRubAbr.Length, vOutVal.Length - pRubAbr.Length)) + 1, vZerStr)

                                GetNextNroAbreviaturaId = vCod

                            End If

                        End If

                    Catch ex As Exception

                    End Try
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNextNroAbreviaturaId", ex)
        End Try
    End Function

    Public Function GetNextNroContrato() As Decimal

        GetNextNroContrato = 0

        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(MAX(CASE ISNUMERIC(NroContrato) WHEN 1 THEN NroContrato ELSE 0 END) + 1, 0) FROM Clientes"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then

                GetNextNroContrato = Val(vOutVal)

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNextNroContrato", ex)
        End Try
    End Function

    Public Function Validar(ByRef pErrStr As String, Optional ByVal pMsg As Boolean = True, Optional pValLoc As Boolean = True, Optional pValIva As Boolean = True) As Boolean

        Validar = False
        pErrStr = ""

        Try

            Dim vRdo As String = ""
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código de cliente"
            If Me.RazonSocial = "" And vRdo = "" Then vRdo = "Debe determinar la razón social del cliente"
            If Me.RubroClienteId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe determinar el rubro del cliente"
            If Me.Domicilio.dmCalle = "" And vRdo = "" Then vRdo = "Debe determinar el domicilio del cliente"
            If Me.LocalidadId.GetObjectId = 0 And pValLoc And vRdo = "" Then vRdo = "Debe determinar la localidad del cliente"
            If Me.SituacionIvaId.GetObjectId = 0 And pValIva And vRdo = "" Then vRdo = "Debe establecer la situación ante el IVA"
            '----> Valido Plan
            If vRdo = "" Then
                vRdo = Me.ValidarPlan(Me.ID, Me.PlanId.GetObjectId)
                If vRdo = "" Then
                    Dim SQL As String
                    If Me.ID > 0 Then
                        '--------> Valido lista de planes...

                        SQL = "SELECT PlanId FROM ClientesPlanes WHERE ClienteId = " & Me.ID & " AND flgPrincipal = 0"
                        Dim cmPlan As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                        Dim vTbl As New DataTable
                        Dim vIdx As Integer

                        vTbl.Load(cmPlan.ExecuteReader)

                        Do Until vIdx = vTbl.Rows.Count Or vRdo <> ""
                            vRdo = Me.ValidarPlan(Me.ID, vTbl.Rows(vIdx).Item(0))
                            vIdx = vIdx + 1
                        Loop

                    End If
                End If
            End If

            pErrStr = vRdo

            If vRdo <> "" Then
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            Else
                Validar = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try

    End Function
    Private Function ValidarPlan(ByVal pCli As Int64, ByVal pPla As Int64) As String
        ValidarPlan = ""
        Try
            Dim vVal As String = ""
            Dim objPlanes As New conPlanes(Me.myCnnName)
            Dim SQL As String

            If objPlanes.Abrir(pPla) Then

                SQL = "SELECT pla.IntegranteClasificacionId, itg.Descripcion "
                SQL = SQL & "FROM PlanesTarifasIntegrantes pla "
                SQL = SQL & "INNER JOIN IntegrantesClasificaciones itg ON (pla.IntegranteClasificacionId = itg.ID) "
                SQL = SQL & "WHERE (pla.PlanId = " & pPla & ") AND (pla.flgRequerido = 1) "

                Dim cmVal As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dt As New DataTable
                Dim vIdx As Integer = 0

                dt.Load(cmVal.ExecuteReader)

                Do Until vIdx = dt.Rows.Count Or vVal <> ""

                    If pCli > 0 Then
                        SQL = "SELECT TOP 1 ID FROM ClientesIntegrantes WHERE ClienteId = " & pCli
                    Else
                        SQL = "SELECT TOP 1 ID FROM tmpClientesIntegrantes WHERE PID = " & logPID
                    End If
                    SQL = SQL & " AND IntegranteClasificacionId = " & dt(vIdx).Item(0)

                    Dim cmItg As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))

                    If cmItg.ExecuteScalar Is Nothing Then
                        vVal = "Debe existir al menos un integrante de tipo " & dt(vIdx).Item(1) & vbCrLf & " para el plan " & objPlanes.Descripcion
                    Else
                        vIdx = vIdx + 1
                    End If

                Loop

            End If

            objPlanes = Nothing

            ValidarPlan = vVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "ValidarPlan", ex)
        End Try
    End Function
    Public Function GetEstadoMorosidad(pCli As Int64) As String

        GetEstadoMorosidad = ""

        Try

            If haveProductoSub(shamanProductos.Express, "08") Then

                If shamanConfig.tpoMorosidad > 0 Then

                    Dim SQL As String

                    SQL = "SELECT ISNULL(MIN(doc.FecVencimiento), '" & DateToSql(NullDateMax) & "') FROM VentasDocumentos doc "
                    SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (doc.TipoComprobanteId = tdc.ID) "
                    SQL = SQL & "WHERE (doc.ClienteId = " & pCli & ") AND (doc.flgStatus = 1) "
                    SQL = SQL & "AND (tdc.flgCobranza = 0) AND (doc.Importe > doc.ImpSaldado) "

                    Dim cmdMor As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    Dim vFecVto As String = CType(cmdMor.ExecuteScalar, String)

                    If Not vFecVto Is Nothing Then

                        Dim vMor As Integer = DateDiff(DateInterval.Day, CDate(vFecVto), Now.Date)

                        If vMor > shamanConfig.tpoMorosidad Then
                            GetEstadoMorosidad = "El cliente seleccionado presenta morosidad de mas de " & shamanConfig.tpoMorosidad & " días"
                        End If

                    End If

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetEstadoMorosidad", ex)
        End Try
    End Function

    Public Function GetForAsistente(pAcc As Integer, pPor As Double, pNewVal As Int64, pIdes As Date, pIHas As Date, pEdes As Date, pEhas As Date, pSit As Integer, pZon As Int64, _
                                  pPai As Int64, pPrv As Int64, pRub As Int64, pPla As Int64, pCli As Int64) As DataTable

        GetForAsistente = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT cli.ID, cli.AbreviaturaId, cli.RazonSocial, "
            Select Case pAcc
                Case 0 : SQL = SQL & "cli.facImporte AS ValorAnterior, ROUND(cli.facImporte + ((cli.facImporte * " & pPor.ToString.Replace(wSepDecimal, ".") & ") / 100), 2) AS NuevoValor, SPACE(10) AS Estado "
                Case 1 : SQL = SQL & "rub.Descripcion AS ValorAnterior, " & pNewVal & " AS NuevoValor, SPACE(10) AS Estado "
                Case 2 : SQL = SQL & "iva.Descripcion AS ValorAnterior, " & pNewVal & " AS NuevoValor, SPACE(10) AS Estado "
                Case 3 : SQL = SQL & "fpg.Descripcion AS ValorAnterior, " & pNewVal & " AS NuevoValor, SPACE(10) AS Estado "
                Case 4 : SQL = SQL & "cob.Descripcion AS ValorAnterior, " & pNewVal & " AS NuevoValor, SPACE(10) AS Estado "
                Case 5 : SQL = SQL & "ven.Descripcion AS ValorAnterior, " & pNewVal & " AS NuevoValor, SPACE(10) AS Estado "
                Case 6 : SQL = SQL & "pla.Descripcion AS ValorAnterior, " & pNewVal & " AS NuevoValor, SPACE(10) AS Estado "
            End Select

            SQL = SQL & "FROM Clientes cli "
            SQL = SQL & "LEFT JOIN RubrosClientes rub ON cli.RubroClienteId = rub.ID "
            SQL = SQL & "LEFT JOIN SituacionesIva iva ON cli.SituacionIvaId = iva.ID "
            SQL = SQL & "LEFT JOIN FormasPago fpg ON cli.FormaPagoId = fpg.ID "
            SQL = SQL & "LEFT JOIN Cobradores cob ON cli.CobradorId = cob.ID "
            SQL = SQL & "LEFT JOIN Cobradores ven ON cli.VendedorId = ven.ID "
            SQL = SQL & "LEFT JOIN Planes pla ON cli.PlanId = pla.ID "
            SQL = SQL & "LEFT JOIN Localidades loc ON cli.LocalidadId = loc.ID "
            SQL = SQL & "LEFT JOIN Provincias prv ON loc.ProvinciaId = prv.ID "

            '-----> Filtros Aplicados
            If (pIdes <> NullDateMax) Then SQL = sqlWhere(SQL) & "(cli.FecIngreso BETWEEN '" & DateToSql(pIdes) & "' AND '" & DateToSql(pIHas) & "')"
            If (pEdes <> NullDateMax) Then SQL = sqlWhere(SQL) & "(cli.FecEgreso BETWEEN '" & DateToSql(pEdes) & "' AND '" & DateToSql(pEhas) & "')"
            If pZon > 0 Then SQL = sqlWhere(SQL) & "(loc.ZonaGeograficaId = " & pZon & ")"
            If pPai > 0 Then SQL = sqlWhere(SQL) & "(prv.PaisId = " & pPai & ")"
            If pPrv > 0 Then SQL = sqlWhere(SQL) & "(loc.ProvinciaId = " & pPrv & ")"
            If pRub > 0 Then SQL = sqlWhere(SQL) & "(cli.RubroClienteId = " & pRub & ")"
            If pPla > 0 Then SQL = sqlWhere(SQL) & "(cli.PlanId = " & pPla & ")"
            If pSit = 1 Then SQL = sqlWhere(SQL) & "(cli.virActivo = 1)"
            If pSit = 2 Then SQL = sqlWhere(SQL) & "(cli.virActivo = 0)"
            If pCli > 0 Then SQL = sqlWhere(SQL) & "(cli.ID = " & pCli & ")"


            Dim cmdCli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdCli.ExecuteReader)

            dt.Columns("NuevoValor").ReadOnly = False

            GetForAsistente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetForAsistente", ex)
        End Try
    End Function

    Public Function GetEquivalenciasByTabla(ByVal pTbl As Integer, Optional pCli As Int64 = 0) As DataTable

        GetEquivalenciasByTabla = Nothing

        Try

            Dim objConfiguraciones As New conConfiguracionesRegionales

            Dim vConfId As Int64 = objConfiguraciones.GetIDByClienteId(pCli)

            Dim SQL As String = ""

            Select Case pTbl
                Case 100
                    SQL = "SELECT ID, '' AS AbreviaturaId, Descripcion FROM EspecialidadesMedicas ORDER BY AbreviaturaId"
                Case 101
                    SQL = "SELECT ID, AbreviaturaId, Descripcion FROM Diagnosticos ORDER BY AbreviaturaId"
                Case 102
                    SQL = "SELECT ID, AbreviaturaId, Descripcion FROM ConceptosFacturacion ORDER BY AbreviaturaId"
                Case 103
                    SQL = "SELECT ID, AbreviaturaId, Descripcion FROM GradosOperativos ORDER BY AbreviaturaId"
                Case 104
                    SQL = "SELECT ID, AbreviaturaId, Descripcion FROM Localidades ORDER BY AbreviaturaId"
                Case 105
                    SQL = "SELECT ID, AbreviaturaId, Descripcion FROM Sanatorios ORDER BY AbreviaturaId"
            End Select

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdGrl.ExecuteReader)

            dt.Columns.Add("Equivalencia", GetType(String))

            For vIdx = 0 To dt.Rows.Count - 1

                Dim objEquivalencia As New conConfiguracionesRegionalesReglas

                dt(vIdx)("Equivalencia") = objEquivalencia.GetValor2(vConfId, dt(vIdx)("ID"), pTbl)

            Next vIdx

            GetEquivalenciasByTabla = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetEquivalenciasByTabla", ex)
        End Try

    End Function

    Public Sub SetEquivalenciasByTabla(ByVal dt As DataTable, ByVal pTbl As Integer, Optional pCli As Int64 = 0)
        Try

            Dim objConfiguraciones As New conConfiguracionesRegionales

            If Not objConfiguraciones.Abrir(objConfiguraciones.GetIDByClienteId(pCli)) Then
                objConfiguraciones.Descripcion = "CLIENTE " & pCli
                objConfiguraciones.flgInterfaz = 1
                objConfiguraciones.ClienteId.SetObjectId(pCli)
                If Not objConfiguraciones.Salvar(objConfiguraciones) Then
                    Exit Sub
                End If
            End If


            Dim vIdx As Integer

            For vIdx = 0 To dt.Rows.Count - 1

                Dim objEquivalencia As New conConfiguracionesRegionalesReglas

                If dt(vIdx)("Equivalencia") <> "" Then

                    If Not objEquivalencia.Abrir(objEquivalencia.GetIDByIndex(objConfiguraciones.ID, dt(vIdx)("ID"), pTbl)) Then
                        objEquivalencia.ConfiguracionRegionalId.SetObjectId(objConfiguraciones.ID)
                        objEquivalencia.TipoConfiguracion = pTbl
                        objEquivalencia.Valor1 = dt(vIdx)("ID")
                    End If
                    objEquivalencia.Valor2 = dt(vIdx)("Equivalencia")

                    objEquivalencia.Salvar(objEquivalencia)

                Else

                    objEquivalencia.Eliminar(objEquivalencia.GetIDByIndex(objConfiguraciones.ID, dt(vIdx)("ID"), pTbl))

                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetEquivalenciasByTabla", ex)
        End Try

    End Sub

    ''' <summary>
    ''' Metodo para combos de shamanweb
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT cli.ID, cli.AbreviaturaId, cli.RazonSocial, rub.Descripcion AS Rubro, loc.AbreviaturaId AS Localidad, "
            SQL = SQL & "cli.Domicilio "
            SQL = SQL & "FROM Clientes cli "
            SQL = SQL & "LEFT JOIN RubrosClientes rub ON cli.RubroClienteId = rub.ID "
            SQL = SQL & "LEFT JOIN Localidades loc ON cli.LocalidadId = loc.ID "
            SQL = SQL & "ORDER BY cli.AbreviaturaId "

            Dim cmdCli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdCli.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function GetIDByAliasRabbit(ByVal pRabbitAlias As String) As Int64
        GetIDByAliasRabbit = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Clientes WHERE rcpRabbitAlias = '" & pRabbitAlias & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAliasRabbit = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAliasRabbit", ex)
        End Try
    End Function

End Class
