Imports System.Data
Imports System.Data.SqlClient

Module modFuncionesGenerales

    Public Function haveProducto(ByVal pPrd As shamanProductos) As Boolean
        Dim vFnd As Boolean = False
        Dim vIdx As Integer = 0

        If Not sysProductos Is Nothing Then

            Do Until vIdx > UBound(sysProductos) Or vFnd
                If Val(sysProductos(vIdx)) = pPrd Then
                    vFnd = True
                Else
                    vIdx = vIdx + 1
                End If
            Loop

        End If

        haveProducto = vFnd

    End Function

    Public Function haveProductoSub(ByVal pPrd As shamanProductos, pNod As String) As Boolean

        Dim vFnd As Boolean = False
        Dim vIdx As Integer = 0

        If pNod = "X" Then

            If Debugger.IsAttached Then
                haveProductoSub = True
            Else
                haveProductoSub = False
            End If

        ElseIf pNod = "12" Or pNod = "34" Then

            If Debugger.IsAttached Then
                haveProductoSub = True
            Else
                haveProductoSub = False
            End If

        ElseIf pNod = "22" Then

            If Debugger.IsAttached Then
                haveProductoSub = True
            ElseIf sysHardKey = "10147 41472" Then
                haveProductoSub = True
            Else
                haveProductoSub = False
            End If

        Else

            If Not sysSubExclude Is Nothing Then

                Do Until vIdx > UBound(sysSubExclude) Or vFnd
                    If sysSubExclude(vIdx) = Format(Val(pPrd), "000") & pNod Then
                        vFnd = True
                    Else
                        vIdx = vIdx + 1
                    End If
                Loop

            End If

            haveProductoSub = Not (vFnd)

        End If

    End Function

    Public Function GetEdad(ByVal pFnc As Date) As Integer

        GetEdad = 0

        Try
            Dim vEda As Integer = 0

            If pFnc < Now.Date Then
                vEda = DateDiff(DateInterval.Year, pFnc, Now.Date)
                Try
                    pFnc = CDate(pFnc.Day & "/" & pFnc.Month & "/" & Now.Date.Year)
                    If DateDiff(DateInterval.Day, pFnc, Now.Date) < 0 Then
                        vEda = vEda - 1
                    End If
                Catch ex As Exception
                    GetEdad = vEda
                End Try
            End If

            GetEdad = vEda

        Catch ex As Exception
            HandleError("modFunciones", "GetEdad", ex)
        End Try

    End Function

    Public Function qyVal(ByVal pVal As String) As String
        If pVal.Length > 0 Then
            qyVal = Replace(pVal, "'", "")
        Else
            qyVal = pVal
        End If
    End Function

    Public Function getSINO(pVal As Integer, Optional pUppLow As Boolean = False) As String
        If Not pUppLow Then
            If pVal = 1 Then
                getSINO = "SI"
            Else
                getSINO = "NO"
            End If
        Else
            If pVal = 1 Then
                getSINO = "Si"
            Else
                getSINO = "No"
            End If
        End If
    End Function

    Public Function HaveCoberturaGrado(ByVal pCli As Int64, ByVal pGdo As Int64, ByRef pCos As Double, ByRef pMsgErr As String, Optional ByVal pMsg As Boolean = True, Optional ByVal pClf As gdoClasificacion = gdoClasificacion.gdoIncidente, Optional ByVal pPla As Int64 = 0, Optional pInc As Int64 = 0, Optional pIsPed As Boolean = False, Optional pLocOri As Int64 = 0, Optional pLocDst As Int64 = 0) As Boolean

        HaveCoberturaGrado = False
        pCos = 0
        pMsgErr = ""

        Try
            Dim objCliente As New conClientes
            Dim objCobertura As conClientesGradosOperativos
            Dim objGrado As New conGradosOperativos
            Dim objCoberturaPlan As conClientesPlanes
            Dim objPlan As conPlanesGradosOperativos
            Dim vCub As Boolean = False
            Dim vErrPla As String = ""
            Dim vTar As Int64 = 0

            If Not IsFinalizado(pInc) Then

                If objCliente.Abrir(pCli) Then

                    '------> Seteo Grado
                    If objGrado.Abrir(pGdo) Then

                        If pPla = 0 Then

                            objCobertura = New conClientesGradosOperativos

                            If objCobertura.HaveCoberturaPropia(objCliente.ID) Then

                                If objCobertura.Abrir(objCobertura.GetIDByIndex(objCliente.ID, objGrado.GradoAgrupacionId.ID)) Then

                                    If objCobertura.flgCubierto = 1 Then

                                        pCos = objCobertura.CoPago
                                        vCub = True

                                        If objCliente.facForma >= 4 Then

                                            '------> Verifico si tiene tope....
                                            Dim objClienteTarifa As New conClientesTarifas

                                            If objClienteTarifa.Abrir(objClienteTarifa.GetIDByIndex(objCliente.ID, objClienteTarifa.GetTarifaPrincipal(objCliente.ID))) Then

                                                Dim objTarifa As New conTarifas
                                                Dim objVigencia As New conTarifasVigencias
                                                Dim vTarVigId As Int64 = objVigencia.GetVigenciaIdByTarifaFecha(objClienteTarifa.TarifaId.ID, Now.Date)

                                                Dim vCon As Int64 = objTarifa.GetConceptoId(vTarVigId, objGrado.ID)

                                                If SuperaTopeCorriente(objCliente.ID, vTarVigId, objClienteTarifa.aplExcedente, vCon, pInc) Then

                                                    Dim vCosPre As Decimal = ValorPrestacion(pInc, vTarVigId, vCon, pIsPed, pLocOri, pLocDst)

                                                    If vCosPre > 0 And pMsg Then
                                                        MsgBox("Se incrementaron $ " & vCosPre & " al copago por excedente de módulo", MsgBoxStyle.Exclamation, "CoPago")
                                                    End If

                                                    pCos = pCos + vCosPre

                                                End If

                                                objTarifa = Nothing

                                            End If

                                            objClienteTarifa = Nothing

                                        End If

                                    End If

                                End If

                            Else

                                Dim vExcApl As Integer = 0

                                objCoberturaPlan = New conClientesPlanes
                                objCoberturaPlan.GetCobertura(objCliente.ID, objGrado.GradoAgrupacionId.ID, vCub, pCos, vExcApl, vTar)

                                If vCub Then

                                    Dim objTarifa As New conTarifas
                                    Dim objVigencia As New conTarifasVigencias
                                    Dim vTarVigId As Int64 = objVigencia.GetVigenciaIdByTarifaFecha(vTar, Now.Date)

                                    Dim vCon As Int64 = objTarifa.GetConceptoId(vTarVigId, objGrado.ID)

                                    If SuperaTopeCorriente(objCliente.ID, vTarVigId, vExcApl, vCon, pInc, True) Then
                                        Dim vCosPre As Decimal = ValorPrestacion(pInc, vTarVigId, vCon, pIsPed, pLocOri, pLocDst)
                                        If vCosPre > 0 And pMsg Then
                                            MsgBox("Se incrementaron $ " & vCosPre & " al copago por excedente de módulo", MsgBoxStyle.Exclamation, "CoPago")
                                        End If
                                        pCos = pCos + vCosPre
                                    End If

                                    objTarifa = Nothing

                                End If

                            End If

                        Else

                            objPlan = New conPlanesGradosOperativos

                            If objPlan.Abrir(objPlan.GetIDByIndex(pPla, pGdo)) Then

                                If objPlan.flgCubierto = 1 Then

                                    pCos = objPlan.CoPago
                                    vCub = True

                                    If vCub Then

                                        Dim objTarifa As New conTarifas
                                        Debug.Print(objPlan.PlanId.Descripcion) '---> Para destrabar el error en desencadenado
                                        Dim objVigencia As New conTarifasVigencias
                                        Dim vTarVigId As Int64 = objVigencia.GetVigenciaIdByTarifaFecha(vTar, Now.Date)

                                        Dim vCon As Int64 = objTarifa.GetConceptoId(vTarVigId, objGrado.ID)

                                        If SuperaTopeCorriente(objCliente.ID, vTarVigId, objPlan.PlanId.aplExcedente, vCon, pInc, True, pPla) Then
                                            Dim vCosPre As Decimal = ValorPrestacion(pInc, vTarVigId, vCon, pIsPed, pLocOri, pLocDst)
                                            If vCosPre > 0 And pMsg Then
                                                MsgBox("Se incrementaron $ " & vCosPre & " al copago por excedente de módulo", MsgBoxStyle.Exclamation, "CoPago")
                                            End If
                                            pCos = pCos + vCosPre
                                        End If

                                        objTarifa = Nothing

                                    End If

                                Else
                                    vErrPla = " en plan " & objPlan.PlanId.AbreviaturaId
                                End If
                            End If

                        End If
                    End If
                End If

                If vCub Then

                    HaveCoberturaGrado = True

                Else

                    Select Case pClf
                        Case gdoClasificacion.gdoTraslado : pMsgErr = "El cliente " & objCliente.AbreviaturaId & " no tiene cubierta dicha modalidad de traslado" & vErrPla
                        Case gdoClasificacion.gdoIntDomiciliaria : pMsgErr = "El cliente " & objCliente.AbreviaturaId & " no tiene cubierta dicha prestación" & vErrPla
                        Case Else : pMsgErr = "El cliente " & objCliente.AbreviaturaId & " no tiene cubierto dicho grado" & vErrPla
                    End Select

                    If pMsg Then
                        MsgBox(pMsgErr, MsgBoxStyle.Exclamation, "Cobertura")
                    End If

                End If

            End If

        Catch ex As Exception
            HandleError("modFunciones", "HaveCoberturaGrado", ex)
        End Try

    End Function

    Private Function IsFinalizado(pInc As Int64) As Boolean

        IsFinalizado = False

        Try

            Dim objViaje As New conIncidentesViajes

            If objViaje.Abrir(objViaje.GetIDByIndex(pInc)) Then
                If objViaje.flgStatus = 1 Then
                    IsFinalizado = True
                End If
            End If

            objViaje = Nothing

        Catch ex As Exception
            HandleError("modFunciones", "IsFinalizado", ex)
        End Try
    End Function

    Private Function SuperaTopeCorriente(ByVal pCli As Int64, ByVal pTarVigId As Int64, ByVal pExcApl As Integer, ByVal pCon As Int64, Optional pInc As Int64 = 0, Optional ByVal pByPlanesCliente As Boolean = False, Optional ByVal pPla As Int64 = 0) As Boolean
        SuperaTopeCorriente = False

        Try

            If pTarVigId > 0 And pExcApl > 0 And pCli > 0 Then

                Dim SQL As String
                Dim vIdx As Integer = 0
                Dim vSup As Boolean = False
                Dim dtModulos As DataTable

                If Not pByPlanesCliente Then

                    Dim objClientesModulos As New conClientesModulos
                    dtModulos = objClientesModulos.GetByCliente(pCli, pCon)

                    Do Until vIdx = dtModulos.Rows.Count Or vSup

                        SQL = "SELECT ISNULL(COUNT(vij.ID), 0) "

                        SQL = SQL & "FROM IncidentesViajes vij "
                        SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON vij.IncidenteDomicilioId = dom.ID "
                        SQL = SQL & "INNER JOIN Incidentes inc ON dom.IncidenteId = inc.ID "
                        SQL = SQL & "INNER JOIN GradosOperativos gdo ON inc.GradoOperativoId = gdo.ID "
                        SQL = SQL & "INNER JOIN ClientesModulos mdl ON inc.ClienteId = mdl.ClienteId "
                        SQL = SQL & "INNER JOIN ClientesModulosConceptos con ON mdl.ID = con.ClienteModuloId AND gdo.ConceptoFacturacion1Id = con.ConceptoFacturacionId "

                        SQL = SQL & "WHERE (inc.FecIncidente BETWEEN '" & DateToSql(CDate("01/" & Now.Month & "/" & Now.Year)) & "' AND '" & DateToSql(Now.Date) & "') "
                        SQL = SQL & "AND (inc.ClienteId = " & pCli & ") AND (mdl.ID = " & dtModulos(vIdx)("ID") & ") "
                        SQL = SQL & "AND (vij.MotivoNoRealizacionId = 0) "
                        SQL = SQL & "AND (inc.ID <> " & pInc & ") "

                        SQL = SQL & "HAVING COUNT(vij.ID) >= " & dtModulos(vIdx)("Cantidad") - 1

                        Dim cmdCantidad As New SqlCommand(SQL, cnnsNET(cnnDefault), cnnsTransNET(cnnDefault))
                        Dim vCnt As Integer = cmdCantidad.ExecuteScalar

                        If vCnt > 0 Then
                            vSup = True
                        Else
                            vIdx = vIdx + 1
                        End If

                    Loop

                    objClientesModulos = Nothing

                Else

                    If pPla = 0 Then

                        Dim objPlanesModulos As New conPlanesModulos
                        dtModulos = objPlanesModulos.GetByCliente(pCli, pCon)

                    Else

                        Dim objPlanesModulos As New conPlanesModulos
                        dtModulos = objPlanesModulos.GetByPlan(pPla, pCon)

                    End If


                    Do Until vIdx = dtModulos.Rows.Count Or vSup

                        SQL = "SELECT ISNULL(COUNT(vij.ID), 0) FROM IncidentesViajes vij "
                        SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON vij.IncidenteDomicilioId = dom.ID "
                        SQL = SQL & "INNER JOIN Incidentes inc ON dom.IncidenteId = inc.ID "
                        SQL = SQL & "INNER JOIN GradosOperativos gdo ON inc.GradoOperativoId = gdo.ID "
                        SQL = SQL & "INNER JOIN ClientesPlanes pla ON inc.ClienteId = pla.ClienteId "
                        SQL = SQL & "INNER JOIN PlanesModulos mdl ON pla.PlanId = mdl.PlanId "
                        SQL = SQL & "INNER JOIN PlanesModulosConceptos con ON mdl.ID = con.PlanModuloId AND gdo.ConceptoFacturacion1Id = con.ConceptoFacturacionId "
                        SQL = SQL & "WHERE (inc.FecIncidente BETWEEN '" & DateToSql(CDate("01/" & Now.Month & "/" & Now.Year)) & "' AND '" & DateToSql(Now.Date) & "') "
                        SQL = SQL & "AND (mdl.ID = " & dtModulos(vIdx)("ID") & ") "
                        SQL = SQL & "AND (vij.MotivoNoRealizacionId = 0) "
                        SQL = SQL & "HAVING COUNT(vij.ID) >= " & dtModulos(vIdx)("Cantidad") - 1

                        Dim cmdCantidad As New SqlCommand(SQL, cnnsNET(cnnDefault), cnnsTransNET(cnnDefault))
                        Dim vCnt As Integer = cmdCantidad.ExecuteScalar

                        If vCnt > 0 Then
                            vSup = True
                        Else
                            vIdx = vIdx + 1
                        End If

                    Loop


                End If

                SuperaTopeCorriente = vSup

            End If
        Catch ex As Exception
            HandleError("modFunciones", "SuperaTopeCorriente", ex)
        End Try
    End Function

    Private Function ValorPrestacion(ByVal pInc As Int64, ByVal pTarVigId As Int64, ByVal pCon As Int64, Optional ByVal pIsPed As Boolean = False, Optional pLocOri As Int64 = 0, Optional pLocDst As Int64 = 0) As Decimal

        ValorPrestacion = 0

        Try

            Dim objTarifa As New conTarifas
            Dim objVigencia As New conTarifasVigencias
            Dim objDistancia As New conDistancias

            Dim vVal As Double = 0

            '---------> Obtengo KMT
            If objVigencia.Abrir(pTarVigId) Then

                Dim vKmt As Long = objDistancia.GetDistancia(pLocOri, pLocDst, , objVigencia.LocalidadOrigenId.ID)

                If pInc = 0 Then
                    vVal = objTarifa.Valorizar(pTarVigId, pCon, vKmt, objTarifa.IsNocturno(Now), pIsPed)
                Else
                    Dim objViaje As New conIncidentesViajes
                    If objViaje.Abrir(objViaje.GetIDByIndex(pInc)) Then
                        vVal = objTarifa.Valorizar(pTarVigId, pCon, vKmt, objTarifa.IsNocturno(objViaje.reqHorInternacion), pIsPed)
                    End If
                    objViaje = Nothing
                End If

            End If

            ValorPrestacion = vVal

            objTarifa = Nothing
            objDistancia = Nothing

        Catch ex As Exception
            HandleError("modFunciones", "ValorPrestacion", ex)
        End Try
    End Function

    Public Function HaveCoberturaPractica(ByVal pCli As Int64, pPra As Int64, pPla As Int64, pPlaCod As String, ByRef pCos As Double, ByRef pMsgErr As String, Optional pMsg As Boolean = False) As Boolean

        HaveCoberturaPractica = False
        pCos = 0
        pMsgErr = ""

        Try

            Dim objCliente As New conClientes
            Dim vCub As Boolean = False

            If objCliente.Abrir(pCli) Then

                Dim objClientesPracticas As New conClientesPracticas
                Dim vOpn As Boolean = objClientesPracticas.Abrir(objClientesPracticas.GetIDByIndex(objCliente.ID, pPra, pPla))

                If Not vOpn And pPla > 0 Then
                    vOpn = objClientesPracticas.Abrir(objClientesPracticas.GetIDByIndex(objCliente.ID, pPra))
                End If

                If vOpn Then
                    If objClientesPracticas.flgCubierto = 1 Then
                        pCos = objClientesPracticas.CoPago
                        vCub = True
                    Else
                        If pPla = 0 Then
                            pMsgErr = "El cliente " & objCliente.AbreviaturaId & " no tiene cubierta dicha práctica"
                        Else
                            pMsgErr = "El plan " & pPlaCod & " del cliente " & objCliente.AbreviaturaId & vbCrLf & "No tiene cubierta dicha práctica"
                        End If
                    End If
                End If

            End If

            If vCub Then
                HaveCoberturaPractica = True
            Else
                If pMsg And pMsgErr <> "" Then
                    MsgBox(pMsgErr, MsgBoxStyle.Critical, "Cobertura")
                End If
            End If

            objCliente = Nothing

        Catch ex As Exception
            HandleError("modFunciones", "HaveCoberturaPractica", ex)
        End Try
    End Function

    Public Sub RunShamanGestion(Optional pUsr As String = "", Optional pPsw As String = "")

        If pUsr = "" Then pUsr = shamanSession.webUser
        If pPsw = "" Then pPsw = shamanSession.webPassword

        Dim vUrl As String = "http://200.49.156.125:57771/ExternalLogin/?user=" & pUsr & "&pass=" & pPsw & "&llave=" & sysHardKey.Replace(" ", "") & "&shex_id=" & shamanSession.ID
        Process.Start(vUrl)

    End Sub

    Public Sub SetGrabacion(ByVal pInc As Decimal, ByRef pObj As conIncidentesLlamados, Optional pAddNew As Boolean = True)
        Try
            If pObj.AgenteId <> "" Then
                pObj.IncidenteId.SetObjectId(pInc)
                pObj.Salvar(pObj)
            Else
                If shamanRing.GrabacionId <> "" And pAddNew Then
                    '-----> Pregunto si quiere linkear el llamado...
                    Dim objLlamado As New conIncidentesLlamados

                    If objLlamado.GetIDByIndex(shamanRing.GrabacionId, shamanRing.AgenteId, shamanRing.NroInterno) = 0 Then

                        If Math.Abs(DateDiff(DateInterval.Minute, shamanRing.regFechaHora, Now)) <= 5 Then
                            If MsgBox("¿ Desea vincular la última llamada recibida a este servicio ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1, "Grabaciones") = MsgBoxResult.Yes Then

                                pObj.CleanProperties(pObj)
                                pObj.IncidenteId.SetObjectId(pInc)
                                pObj.AgenteId = shamanRing.AgenteId
                                pObj.DNIS = shamanRing.DNIS
                                pObj.Campania = shamanRing.Campania
                                pObj.ANI = shamanRing.ANI
                                pObj.NroInterno = shamanRing.NroInterno
                                pObj.GrabacionId = shamanRing.GrabacionId

                                pObj.Salvar(pObj)

                            End If
                        End If

                    End If
                End If
            End If
        Catch ex As Exception
            HandleError("modFunciones", "SetGrabacion", ex)
        End Try
    End Sub

    Public Function CloneDT(dtClone As DataTable) As DataTable

        CloneDT = Nothing

        Try
            Dim dtNew As DataTable = dtClone.Clone()

            For vPerIdx = 0 To dtClone.Rows.Count - 1

                Dim desRow As DataRow = dtNew.NewRow()
                Dim sourceRow As DataRow = dtClone.Rows(vPerIdx)

                desRow.ItemArray = sourceRow.ItemArray.Clone()

                dtNew.Rows.Add(desRow)

            Next

            CloneDT = dtNew

        Catch ex As Exception
            HandleError("modFunciones", "SetGrabacion", ex)
        End Try

    End Function


End Module