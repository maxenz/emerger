Imports System.Data
Imports System.Data.SqlClient
Public Class conTarifas
    Inherits typTarifas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll(Optional pLiq As Boolean = False, Optional ByVal pAct As Integer = 1) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion FROM Tarifas "
            SQL = SQL & "WHERE (flgLiquidacion = " & setBoolToInt(pLiq) & ") "
            If pAct = 1 Then SQL = SQL & "AND (Activo = 1) "
            If pAct = 2 Then SQL = SQL & "AND (Activo = 0) "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function GetList(Optional pLiq As Boolean = False) As DataTable

        GetList = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId + ' - ' + Descripcion FROM Tarifas "
            SQL = SQL & "WHERE (flgLiquidacion = " & setBoolToInt(pLiq) & ") "
            SQL = SQL & "AND (Activo = 1) "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetList = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetList", ex)
        End Try
    End Function

    Public Function GetConceptoId(ByVal pTarVigId As Int64, ByVal pGdo As Int64) As Int64
        GetConceptoId = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 tar.ConceptoFacturacionId FROM TarifasPrestaciones tar INNER JOIN GradosOperativos gdo ON (tar.ConceptoFacturacionId = gdo.ConceptoFacturacion1Id) "
            SQL = SQL & "WHERE (tar.TarifaVigenciaId = " & pTarVigId & ") AND (gdo.ID = " & pGdo & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            If vOutVal Is Nothing Then

                SQL = "SELECT TOP 1 tar.ConceptoFacturacionId FROM TarifasPrestaciones tar INNER JOIN GradosOperativos gdo ON (tar.ConceptoFacturacionId = gdo.ConceptoFacturacion2Id) "
                SQL = SQL & "WHERE (tar.TarifaVigenciaId = " & pTarVigId & ") AND (gdo.ID = " & pGdo & ") "

                cmFind.CommandText = SQL
                vOutVal = CType(cmFind.ExecuteScalar, String)

                If vOutVal Is Nothing Then

                    SQL = "SELECT ConceptoFacturacion1Id FROM GradosOperativos WHERE (ID = " & pGdo & ")"

                    cmFind.CommandText = SQL
                    vOutVal = CType(cmFind.ExecuteScalar, String)

                    If vOutVal Is Nothing Then
                        GetConceptoId = 0
                    Else
                        GetConceptoId = CType(vOutVal, Int64)
                    End If
                Else
                    GetConceptoId = CType(vOutVal, Int64)
                End If
            Else
                GetConceptoId = CType(vOutVal, Int64)
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetConceptoId", ex)
        End Try
    End Function

    Public Function Valorizar(ByVal pTarVigId As Int64, ByVal pCon As Long, ByVal pKmt As Long, Optional ByVal pNoc As Boolean = False, Optional ByVal pPed As Boolean = False, Optional ByVal pDer As Boolean = False, Optional ByVal pDem As Long = 0, Optional pVijAbr As String = "IDA", Optional pLiqIncId As Int64 = 0) As Double
        Valorizar = 0
        Try
            Dim SQL As String, vId As Int64, objValores As New conTarifasPrestaciones(Me.myCnnName), vVal As Double = 0

            Valorizar = 0

            SQL = "SELECT ID FROM TarifasPrestaciones WHERE (TarifaVigenciaId = " & pTarVigId & ") AND (ConceptoFacturacionId = " & pCon & ") "
            SQL = SQL & "AND (KmDesde <= " & pKmt & ") AND (KmHasta >= " & pKmt & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            If vOutVal Is Nothing Then

                SQL = "SELECT ID FROM TarifasPrestaciones WHERE (TarifaVigenciaId = " & pTarVigId & ") AND (ConceptoFacturacionId = " & pCon & ") "
                SQL = SQL & "AND (KmDesde < " & pKmt & ") "
                SQL = SQL & "ORDER BY KmDesde DESC"

                cmFind.CommandText = SQL
                vOutVal = CType(cmFind.ExecuteScalar, String)

                If vOutVal Is Nothing Then
                    Exit Function
                End If

            End If

            vId = CType(vOutVal, Int64)

            If objValores.Abrir(vId) Then
                '--------> Valor Base
                vVal = objValores.Importe
                addLogLiq(pLiqIncId, "BAS", 1, objValores.Importe)
                '--------> Km Excedente
                If pKmt > objValores.KmHasta Then
                    vVal = vVal + ((pKmt - objValores.KmHasta) * objValores.ImpKmExcedente)
                    addLogLiq(pLiqIncId, "KEX", pKmt - objValores.KmHasta, objValores.ImpKmExcedente)
                End If
                '--------> Espera
                Dim vHor As Integer = Int(pDem / 60), vMed As Integer = pDem - (vHor * 60), vFraMed As Boolean = False
                If vMed >= 46 Then
                    vHor = vHor + 1
                ElseIf vMed > 16 Then
                    vFraMed = True
                End If
                vVal = vVal + (objValores.ImpDemora * vHor)
                addLogLiq(pLiqIncId, "ESH", vHor, objValores.ImpDemora)
                If vFraMed Then
                    vVal = vVal + (objValores.ImpDemora / 2)
                    addLogLiq(pLiqIncId, "ESM", 1, objValores.ImpDemora / 2)
                End If
                '--------> Recargos
                If pNoc Then
                    vVal = vVal + ((objValores.Importe * objValores.RecNocturno) / 100)
                    addLogLiq(pLiqIncId, "NOC", 1, ((objValores.Importe * objValores.RecNocturno) / 100))
                End If

                If pPed Then
                    vVal = vVal + ((objValores.Importe * objValores.RecPediatrico) / 100)
                    addLogLiq(pLiqIncId, "PED", 1, ((objValores.Importe * objValores.RecPediatrico) / 100))
                End If

                If pDer Then
                    vVal = vVal + ((objValores.Importe * objValores.RecDerivacion) / 100)
                    addLogLiq(pLiqIncId, "DER", 1, ((objValores.Importe * objValores.RecDerivacion) / 100))
                End If

                '--------> Descuento retorno
                If pVijAbr = "VUE" And objValores.ImpDtoRetorno > 0 Then
                    vVal = vVal - objValores.ImpDtoRetorno
                    addLogLiq(pLiqIncId, "DRT", 1, objValores.ImpDtoRetorno)
                End If

                '-------> Resultado
                Valorizar = Math.Round(vVal, 2)
            End If
            objValores = Nothing
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Valorizar", ex)
        End Try
    End Function

    Private Function addLogLiq(pLiqIncId As Int64, pSub As String, pCnt As Integer, pImp As Decimal) As Boolean

        addLogLiq = False

        Try

            If pLiqIncId > 0 And pImp > 0 And pCnt > 0 Then

                Dim objLog As New typLiqPrestadoresIncidentesConceptos
                Dim objConcepto As New conConceptosFacturacion

                objLog.CleanProperties(objLog)
                objLog.LiqPrestadorIncidenteId.SetObjectId(pLiqIncId)
                objLog.ConceptoFacturacionId.SetObjectId(objConcepto.GetSubConceptoId(pSub))
                objLog.Cantidad = pCnt
                objLog.Importe = pImp

                If objLog.Salvar(objLog) Then

                    addLogLiq = True

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "addLogLiq", ex)
        End Try
    End Function

    Public Function GetAlias(ByVal pTarVigId As Int64, ByVal pCon As Long, ByVal pKmt As Long, Optional ByVal pRel As Boolean = True) As String
        GetAlias = ""
        Try
            Dim SQL As String, vId As Int64, objValores As New conTarifasPrestaciones(Me.myCnnName), vVal As Double = 0

            SQL = "SELECT ID FROM TarifasPrestaciones WHERE (TarifaVigenciaId = " & pTarVigId & ") AND (ConceptoFacturacionId = " & pCon & ") "
            SQL = SQL & "AND (KmDesde <= " & pKmt & ") AND (KmHasta >= " & pKmt & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            If vOutVal Is Nothing Then

                SQL = "SELECT ID FROM TarifasPrestaciones WHERE (TarifaVigenciaId = " & pTarVigId & ") AND (ConceptoFacturacionId = " & pCon & ") "
                SQL = SQL & "ORDER BY KmDesde DESC"

                cmFind.CommandText = SQL
                vOutVal = CType(cmFind.ExecuteScalar, String)

                If vOutVal Is Nothing Then
                    Exit Function
                End If

            End If

            vId = CType(vOutVal, Int64)

            If objValores.Abrir(vId) Then

                If pRel Then
                    GetAlias = objValores.Alias1
                Else
                    GetAlias = objValores.Alias2
                End If
            End If
            objValores = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAlias", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código de la tarifa"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción de la tarifa"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Sub GetKilometros(ByVal pTarVigId As Int64, ByVal pInc As Long, ByVal pVij As String, ByRef pLocOri As Int64, ByRef pLocDst As Int64, ByRef pDis As Long, Optional ByVal pMode As Integer = 0)

        '---> pMode
        '---> 0: Kilometraje a facturar del viaje
        '---> 1: Solo kilometraje DB
        '---> 2: Solo kilometraje Google

        Try

            Dim vDesTip As Integer = 0, vDesAnx As Integer = 0
            Dim vHasTip As Integer = 0, vHasAnx As Integer = 0

            pDis = 0

            Select Case pVij
                Case "APE"
                    vDesTip = 3
                    vHasTip = 0
                Case "IDA"
                    vHasTip = 1
                Case "VUE"
                    vDesTip = 1
                    vHasTip = 0
                Case "AN1"
                    vHasTip = 2
                    vHasAnx = 1
                Case Else
                    vDesTip = 2
                    vDesAnx = Val(pVij.Substring(3, 1)) - 1
                    vHasTip = 2
                    vHasAnx = Val(pVij.Substring(3, 1))
            End Select

            '---------> Obtengo localidades
            pLocOri = GetLocalidadId(pInc, vDesTip, vDesAnx)
            pLocDst = GetLocalidadId(pInc, vHasTip, vHasAnx)


            '---------> Obtengo KMT
            Select Case pMode

                Case 0, 1

                    Select Case shamanConfig.kmtObtencion

                        Case 0
                            pDis = Me.getKilometrosDB(pTarVigId, pLocOri, pLocDst)
                        Case 1
                            pDis = Me.getKilometrosGoogle(pInc, vDesTip, vDesAnx, vHasTip, vHasAnx)
                        Case 2
                            Dim vDisDB As Decimal = Me.getKilometrosDB(pTarVigId, pLocOri, pLocDst)
                            Dim vDisGoo As Decimal = Me.getKilometrosGoogle(pInc, vDesTip, vDesAnx, vHasTip, vHasAnx)

                            If vDisDB > vDisGoo Then
                                pDis = vDisDB
                            Else
                                pDis = vDisGoo
                            End If

                    End Select

                Case 1

                    pDis = Me.getKilometrosDB(pTarVigId, pLocOri, pLocDst)

                Case 2

                    pDis = Me.getKilometrosGoogle(pInc, vDesTip, vDesAnx, vHasTip, vHasAnx)


            End Select

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetKilometros", ex)
        End Try
    End Sub

    Private Function getKilometrosDB(pTarVigId As Int64, pLocOri As Int64, pLocDst As Int64) As Decimal
        getKilometrosDB = 0
        Try
            Dim objDistancia As New conDistancias(Me.myCnnName)
            Dim objTarifa As New conTarifasVigencias(Me.myCnnName)
            If objTarifa.Abrir(pTarVigId) Then
                getKilometrosDB = objDistancia.GetDistancia(pLocOri, pLocDst, , objTarifa.LocalidadOrigenId.ID)
            Else
                getKilometrosDB = objDistancia.GetDistancia(pLocOri, pLocDst)
            End If
            objTarifa = Nothing
            objDistancia = Nothing
        Catch ex As Exception

        End Try
    End Function

    Private Function getKilometrosGoogle(pInc As Int64, pDesTip As Integer, pDesAnx As Integer, pHasTip As Integer, pHasAnx As Integer) As Decimal

        getKilometrosGoogle = 0

        Try
            '---------> Obtengo KMT Google
            Dim wsGPS As New GPShamanWS.ServiceSoapClient

            Dim objOrigen As New conIncidentesDomicilios
            Dim objDestino As New conIncidentesDomicilios

            If objOrigen.Abrir(objOrigen.GetIDByIndex(pInc, pDesTip, pDesAnx)) Then

                If objDestino.Abrir(objDestino.GetIDByIndex(pInc, pHasTip, pHasAnx)) Then

                    Dim vDevWeb As String = wsGPS.GetDistanciaTiempo(objOrigen.Domicilio.dmLatitud.ToString.Replace(",", "."), objOrigen.Domicilio.dmLongitud.ToString.Replace(",", "."), _
                                                                     objDestino.Domicilio.dmLatitud.ToString.Replace(",", "."), objDestino.Domicilio.dmLongitud.ToString.Replace(",", "."))
                    If vDevWeb.Contains("/") And vDevWeb.Length > 1 Then

                        Dim vDis As Decimal = GetDouble(Parcer(Parcer(vDevWeb, "/", 0), " ", 0))

                        Dim vDisExp As String = Parcer(vDevWeb, "/", 0)
                        vDisExp = vDisExp.Substring(vDisExp.Length - 2, 2).ToLower

                        If vDisExp <> "km" Then
                            vDis = vDis / 1000
                        End If

                        getKilometrosGoogle = vDis

                    End If

                End If

            End If
        Catch ex As Exception

        End Try
    End Function

    Private Function GetLocalidadId(ByVal pInc As Long, ByVal pTdm As Long, ByVal pAnx As Long) As Long
        GetLocalidadId = 0
        Try
            Dim objDomicilio As New conIncidentesDomicilios(Me.myCnnName)
            If objDomicilio.Abrir(objDomicilio.GetIDByIndex(pInc, pTdm, pAnx)) Then
                GetLocalidadId = objDomicilio.LocalidadId.ID
            End If
            objDomicilio = Nothing
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetLocalidadId", ex)
        End Try
    End Function
    Public Function IsNocturno(ByVal pFhr As DateTime) As Boolean
        IsNocturno = False
        Try
            If pFhr.Year > 1940 Then
                Dim vNocDes As DateTime = pFhr.Date & " " & shamanConfig.hsNocDesde & ":00"
                Dim vNocHas As DateTime = pFhr.Date & " " & shamanConfig.hsNocHasta & ":59"
                If pFhr >= vNocDes Or pFhr <= vNocHas Then
                    IsNocturno = True
                End If
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "IsNocturno", ex)
        End Try
    End Function

    Public Function IsDerivado(ByVal pInc As Long) As Boolean

        IsDerivado = False

        Try
            Dim SQL As String
            SQL = "SELECT dom.ID FROM IncidentesDomicilios dom "
            SQL = SQL & "INNER JOIN Incidentes inc ON (dom.IncidenteId = inc.ID) "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
            SQL = SQL & "WHERE (inc.ID = " & pInc & ") AND (gdo.ClasificacionId = 0) "
            SQL = SQL & "AND (dom.TipoDomicilio = 1) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmFind.ExecuteScalar > 0 Then IsDerivado = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "IsDerivado", ex)
        End Try
    End Function
End Class
