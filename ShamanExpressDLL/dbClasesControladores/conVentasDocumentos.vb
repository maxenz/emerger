Imports System.Data
Imports System.Data.SqlClient

Public Class conVentasDocumentos
    Inherits typVentasDocumentos
    Private vHavLog As Boolean
    Private vCopies As Integer
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

#Region "Visores"

    Public Function GetAll(ByVal pFecDes As Date, ByVal pFecHas As Date, Optional ByVal pChkFac As Boolean = True, Optional ByVal pChkCob As Boolean = False) As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT vta.ID, tdc.AbreviaturaId AS Tipo, vta.NroComprobante, vta.FecComprobante, cli.AbreviaturaId AS Cliente, vta.RazonSocial, "
            SQL = SQL & "vta.Importe - vta.ImpIva1 - vta.ImpIva2 - vta.ImpImpuesto AS SubTotal, vta.Importe AS Total, "
            SQL = SQL & "CASE vta.flgStatus WHEN 1 THEN 'DEF' ELSE 'ANU' END AS Estado "
            SQL = SQL & "FROM VentasDocumentos vta "
            SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (vta.TipoComprobanteId = tdc.ID) "
            SQL = SQL & "INNER JOIN Clientes cli ON (vta.ClienteId = cli.ID) "
            SQL = SQL & "WHERE (vta.FecComprobante BETWEEN '" & DateToSql(pFecDes) & "' AND '" & DateToSql(pFecHas) & "') "
            SQL = SQL & "AND (vta.flgStatus > 0) "
            If pChkFac And Not pChkCob Then
                SQL = SQL & "AND (tdc.flgSubDiario = 1) "
            ElseIf Not pChkFac And pChkCob Then
                SQL = SQL & "AND (tdc.flgSubDiario = 0) "
            End If
            SQL = SQL & " ORDER BY tdc.AbreviaturaId, vta.NroComprobante"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function GetSaldos(Optional ByVal pRub As Int64 = 0, Optional ByVal pAct As Byte = 1) As DataTable

        GetSaldos = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, RazonSocial, Saldo "
            SQL = SQL & "FROM Clientes "
            If pRub > 0 Then SQL = sqlWhere(SQL) & "(RubroClienteId = " & pRub & ")"
            If pAct = 1 Then SQL = sqlWhere(SQL) & "(virActivo = 1)"
            If pAct = 2 Then SQL = sqlWhere(SQL) & "(virActivo = 0)"
            SQL = SQL & " ORDER BY AbreviaturaId"

            Dim cmdBus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBus.ExecuteReader)

            GetSaldos = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetSaldos", ex)
        End Try
    End Function

    Public Function GetCuentaCorrienteByCliente(ByVal pCli As Int64, Optional ByVal pDes As Date = Nothing, Optional ByVal pHas As Date = Nothing) As DataTable

        GetCuentaCorrienteByCliente = Nothing

        Try

            Dim objCliente As New typClientes(Me.myCnnName)

            If objCliente.Abrir(pCli) Then

                Dim vSal As Double = 0

                vSal = objCliente.SaldoInicial

                Dim SQL As String

                SQL = "SELECT 0 AS ID, '1900-01-01' AS FecComprobante, '' AS Tipo, 'SALDO ANTERIOR' AS NroComprobante, 0.00 AS Debe, 0.00 AS Haber,  "
                SQL = SQL & objCliente.SaldoInicial.ToString.Replace(",", ".") & " AS Saldo "

                SQL = SQL & "UNION "

                SQL = SQL & "SELECT doc.ID, doc.FecComprobante, tdc.AbreviaturaId AS Tipo, doc.NroComprobante, "
                SQL = SQL & "CASE tdc.flgCobranza WHEN 0 THEN doc.Importe ELSE 0 END AS Debe, "
                SQL = SQL & "CASE tdc.flgCobranza WHEN 1 THEN doc.Importe ELSE 0 END AS Haber, 0.00 AS Saldo "
                SQL = SQL & "FROM VentasDocumentos doc "
                SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (doc.TipoComprobanteId = tdc.ID) "
                SQL = SQL & "WHERE (doc.ClienteId = " & pCli & ") AND (doc.flgStatus = 1) "
                SQL = SQL & "ORDER BY FecComprobante"

                Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dt As New DataTable
                Dim vIdx As Integer
                Dim vDeb As Double = 0
                Dim vHab As Double = 0

                dt.Load(cmdGrl.ExecuteReader)

                dt.Columns("NroComprobante").MaxLength = 50
                dt.Columns("Saldo").ReadOnly = False

                For vIdx = 1 To dt.Rows.Count - 1

                    If dt(vIdx)("Debe") > 0 Then
                        vSal = vSal + dt(vIdx)("Debe")
                    Else
                        vSal = vSal - dt(vIdx)("Haber")
                    End If

                    dt(vIdx)("Saldo") = vSal

                Next vIdx

                Dim dtRow As DataRow = dt.NewRow

                dtRow("ID") = 0
                dtRow("Tipo") = ""
                dtRow("FecComprobante") = CDate("01/01/1900")
                dtRow("NroComprobante") = ""
                dtRow("Debe") = 0
                dtRow("Haber") = 0
                dtRow("Saldo") = 0

                dt.Rows.Add(dtRow)

                dtRow = dt.NewRow

                dtRow("ID") = 0
                dtRow("Tipo") = ""
                dtRow("FecComprobante") = CDate("01/01/1900")
                dtRow("NroComprobante") = Space(10) & "SALDO CONSULTA"
                dtRow("Debe") = 0
                dtRow("Haber") = 0
                dtRow("Saldo") = vSal

                dt.Rows.Add(dtRow)

                dtRow = dt.NewRow

                dtRow("ID") = 0
                dtRow("Tipo") = ""
                dtRow("FecComprobante") = CDate("01/01/1900")
                dtRow("NroComprobante") = Space(10) & "SALDO ACTUAL"
                dtRow("Debe") = 0
                dtRow("Haber") = 0
                dtRow("Saldo") = objCliente.Saldo

                dt.Rows.Add(dtRow)

                GetCuentaCorrienteByCliente = dt

            End If

            objCliente = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetCuentaCorrienteByCliente", ex)
        End Try
    End Function

    Public Function GetComposicionByCliente(ByVal pCli As Int64, Optional ByVal pDes As Date = Nothing, Optional ByVal pHas As Date = Nothing) As DataTable

        GetComposicionByCliente = Nothing

        Try

            Dim dt As New DataTable

            dt.Columns.Add("ID", GetType(Int64))
            dt.Columns.Add("FecComprobante", GetType(Date))
            dt.Columns.Add("Tipo", GetType(String))
            dt.Columns.Add("NroComprobante", GetType(String))
            dt.Columns.Add("Debe", GetType(Decimal))
            dt.Columns.Add("Haber", GetType(Decimal))
            dt.Columns.Add("Saldo", GetType(Decimal))


            Dim objCliente As New typClientes(Me.myCnnName)

            If objCliente.Abrir(pCli) Then

                Dim vSal As Double = 0

                Dim dtRow As DataRow = dt.NewRow

                dtRow("ID") = 0
                dtRow("FecComprobante") = CDate("01/01/1900")
                dtRow("Tipo") = ""
                dtRow("NroComprobante") = "SALDO ANTERIOR"
                dtRow("Debe") = 0
                dtRow("Haber") = 0
                dtRow("Saldo") = objCliente.SaldoInicial

                dt.Rows.Add(dtRow)

                vSal = objCliente.SaldoInicial

                Dim SQL As String

                SQL = "SELECT doc.ID, doc.FecComprobante, tdc.AbreviaturaId, doc.NroComprobante, doc.Importe "
                SQL = SQL & "FROM VentasDocumentos doc "
                SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (doc.TipoComprobanteId = tdc.ID) "
                SQL = SQL & "WHERE (doc.ClienteId = " & pCli & ") AND (doc.flgStatus = 1) AND (tdc.AbreviaturaId = 'FAC') "
                SQL = SQL & "ORDER BY doc.FecComprobante"

                Dim cmdFac As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dtFac As New DataTable
                Dim vIdx As Integer

                dtFac.Load(cmdFac.ExecuteReader)

                For vIdx = 0 To dtFac.Rows.Count - 1

                    vSal = dtFac(vIdx)(4)

                    dtRow = dt.NewRow

                    dtRow("ID") = dtFac(vIdx)(0)
                    dtRow("FecComprobante") = dtFac(vIdx)(1)
                    dtRow("Tipo") = dtFac(vIdx)(2)
                    dtRow("NroComprobante") = dtFac(vIdx)(3)
                    dtRow("Debe") = dtFac(vIdx)(4)
                    dtRow("Haber") = 0
                    dtRow("Saldo") = dtFac(vIdx)(4)

                    dt.Rows.Add(dtRow)

                    '----> Imputados

                    SQL = "SELECT ipt.VentaDocumentoId, doc.FecComprobante, tdc.AbreviaturaId, doc.NroComprobante, tdc.flgCobranza, ipt.Importe "
                    SQL = SQL & "FROM VentasDocImputaciones ipt "
                    SQL = SQL & "INNER JOIN VentasDocumentos doc ON (ipt.VentaDocumentoId = doc.ID) "
                    SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (doc.TipoComprobanteId = tdc.ID) "
                    SQL = SQL & "WHERE (ipt.VentaDocImputadoId = " & dtFac(vIdx).Item(0) & ") AND (doc.flgStatus = 1) "
                    SQL = SQL & "ORDER BY doc.FecComprobante"

                    Dim cmdIpt As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    Dim dtIpt As New DataTable
                    Dim vIpt As Integer

                    dtIpt.Load(cmdIpt.ExecuteReader)

                    For vIpt = 0 To dtIpt.Rows.Count - 1

                        If dtIpt(vIpt).Item(4) = 1 Then
                            vSal = vSal - dtIpt(vIpt).Item(5)
                        Else
                            vSal = vSal + dtIpt(vIpt).Item(5)
                        End If

                        dtRow = dt.NewRow

                        dtRow("ID") = dtIpt(vIpt)(0)
                        dtRow("FecComprobante") = dtIpt(vIpt)(1)
                        dtRow("Tipo") = dtIpt(vIpt)(2)
                        dtRow("NroComprobante") = dtIpt(vIpt)(3)
                        dtRow("Debe") = 0
                        dtRow("Haber") = dtIpt(vIpt)(5)
                        dtRow("Saldo") = vSal

                        dt.Rows.Add(dtRow)

                    Next vIpt

                Next vIdx

                Dim vTitPen As Boolean = False


                '-------> Pendientes de imputación

                SQL = "SELECT doc.ID, doc.FecComprobante, tdc.AbreviaturaId, doc.NroComprobante, doc.Importe, "
                SQL = SQL & "(SELECT ISNULL(SUM(ipt.Importe), 0) FROM VentasDocImputaciones ipt WHERE ipt.VentaDocumentoId = doc.ID) AS Imputado "
                SQL = SQL & "FROM VentasDocumentos doc "
                SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (doc.TipoComprobanteId = tdc.ID) "
                SQL = SQL & "WHERE (doc.ClienteId = " & pCli & ") AND (doc.flgStatus = 1) AND (tdc.AbreviaturaId <> 'FAC') "
                SQL = SQL & "ORDER BY doc.FecComprobante"

                Dim cmdPen As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dtPen As New DataTable

                dtPen.Load(cmdPen.ExecuteReader)

                For vIdx = 0 To dtPen.Rows.Count - 1

                    If dtPen(vIdx)("Imputado") < dtPen(vIdx)("Importe") Then

                        If Not vTitPen Then

                            '-------> Renglón en blanco
                            dtRow = dt.NewRow

                            dtRow("ID") = 0
                            dtRow("Tipo") = ""
                            dtRow("FecComprobante") = CDate("01/01/1900")
                            dtRow("NroComprobante") = "PENDIENTES DE IMPUTACION.."
                            dtRow("Debe") = 0
                            dtRow("Haber") = 0
                            dtRow("Saldo") = 0

                            dt.Rows.Add(dtRow)

                            vTitPen = True

                        End If


                        dtRow = dt.NewRow

                        dtRow("ID") = dtPen(vIdx)(0)
                        dtRow("FecComprobante") = dtPen(vIdx)(1)
                        dtRow("Tipo") = dtPen(vIdx)(2)
                        dtRow("NroComprobante") = dtPen(vIdx)(3)
                        dtRow("Debe") = 0
                        dtRow("Haber") = dtPen(vIdx)("Importe") - dtPen(vIdx)("Imputado")
                        dtRow("Saldo") = dtPen(vIdx)("Importe") - dtPen(vIdx)("Imputado")

                        dt.Rows.Add(dtRow)

                    End If

                Next vIdx


                '-------> Renglón en blanco
                dtRow = dt.NewRow

                dtRow("ID") = 0
                dtRow("Tipo") = ""
                dtRow("FecComprobante") = CDate("01/01/1900")
                dtRow("NroComprobante") = ""
                dtRow("Debe") = 0
                dtRow("Haber") = 0
                dtRow("Saldo") = 0

                dt.Rows.Add(dtRow)

                '-------> Renglón final
                dtRow = dt.NewRow

                dtRow("ID") = 0
                dtRow("Tipo") = ""
                dtRow("FecComprobante") = CDate("01/01/1900")
                dtRow("NroComprobante") = Space(10) & "SALDO ACTUAL"
                dtRow("Debe") = 0
                dtRow("Haber") = 0
                dtRow("Saldo") = objCliente.Saldo

                dt.Rows.Add(dtRow)

                GetComposicionByCliente = dt

            End If

            objCliente = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetComposicionByCliente", ex)
        End Try
    End Function

    Public Function GetPendientesByCliente(ByVal pCli As Int64) As DataTable

        GetPendientesByCliente = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT vta.ID, tdc.AbreviaturaId AS Tipo, vta.NroComprobante, vta.FecComprobante, vta.RazonSocial, "
            SQL = SQL & "vta.Importe AS ImporteComprobante, vta.Importe - vta.ImpSaldado AS Saldo, 0.00 AS ImporteImputado, "
            SQL = SQL & "tal.flgSubDiario "
            SQL = SQL & "FROM VentasDocumentos vta "
            SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (vta.TipoComprobanteId = tdc.ID) "
            SQL = SQL & "INNER JOIN Clientes cli ON (vta.ClienteId = cli.ID) "
            SQL = SQL & "INNER JOIN Talonarios tal ON (vta.TalonarioId = tal.ID) "
            SQL = SQL & "WHERE (vta.ClienteId = " & pCli & ") "
            SQL = SQL & "AND (vta.flgStatus = 1) AND (tdc.flgCobranza = 0) "
            SQL = SQL & "AND (vta.Importe - vta.ImpSaldado > 0) "
            SQL = SQL & "ORDER BY tdc.AbreviaturaId, vta.NroComprobante"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdGrl.ExecuteReader)

            dt.Columns("ImporteImputado").ReadOnly = False

            GetPendientesByCliente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetPendientesCliente", ex)
        End Try
    End Function

    Public Sub SetDTRenglonesByDocumento(ByVal pDoc As Int64, ByRef dtRenglones As DataTable)
        Try
            Dim SQL As String

            dtRenglones.Rows.Clear()

            SQL = "SELECT RenglonId, Descripcion, AlicuotaIvaId, PorcentajeIva, AlicuotaBsAsIIBBId, PorcentajeBsAsIIBB, Cantidad, Importe, "
            SQL = SQL & "CASE Cantidad WHEN 0 THEN 1 ELSE Cantidad END * Importe "
            SQL = SQL & "FROM VentasDocRenglones "
            SQL = SQL & "WHERE (VentaDocumentoId = " & pDoc & ") "
            SQL = SQL & "ORDER BY RenglonId"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer, vCol As Integer
            dt.Load(cmdGrl.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                Dim dtRow As DataRow = dtRenglones.NewRow

                For vCol = 0 To dt.Columns.Count - 1

                    dtRow(vCol) = dt(vIdx)(vCol)

                Next vCol

                dtRenglones.Rows.Add(dtRow)

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDTRenglonesByDocumento", ex)
        End Try
    End Sub

    Public Sub SetDTCobrosByDocumento(ByVal pDoc As Int64, ByRef dtFormasCobro As DataTable)
        Try
            Dim SQL As String

            SQL = "SELECT cob.FormaPagoId, fpg.AbreviaturaId, cob.BancoId, bco.Descripcion, cob.NroCheque, "
            SQL = SQL & "CASE cob.FecCheque WHEN '" & DateToSql(NullDateTime) & "' THEN NULL ELSE cob.FecCheque END, cob.Importe "
            SQL = SQL & "FROM VentasDocCobros cob "
            SQL = SQL & "INNER JOIN FormasPago fpg ON (cob.FormaPagoId = fpg.ID) "
            SQL = SQL & "LEFT JOIN Bancos bco ON (cob.BancoId = bco.ID) "
            SQL = SQL & "WHERE (VentaDocumentoId = " & pDoc & ") "

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer, vCol As Integer
            dt.Load(cmdBas.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1
                Dim dtRow As DataRow = dtFormasCobro.NewRow
                For vCol = 0 To dt.Columns.Count - 1
                    dtRow(vCol) = dt(vIdx)(vCol)
                Next vCol
                dtFormasCobro.Rows.Add(dtRow)
            Next

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDTCobrosByDocumento", ex)
        End Try
    End Sub

    Public Function GetImputacionesByDocumento(ByVal pDoc As Int64, ByVal pCli As Int64, Optional ByVal pFul As Boolean = False) As DataTable

        GetImputacionesByDocumento = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ipt.VentaDocImputadoId AS ID, tdc.AbreviaturaId AS Tipo, vta.NroComprobante, vta.FecComprobante, vta.RazonSocial, "
            SQL = SQL & "vta.Importe AS ImporteComprobante, vta.Importe - vta.ImpSaldado + ipt.Importe AS Saldo, ipt.Importe AS ImporteImputado, "
            SQL = SQL & "tal.flgSubDiario "
            SQL = SQL & "FROM VentasDocImputaciones ipt "
            SQL = SQL & "INNER JOIN VentasDocumentos vta ON (ipt.VentaDocImputadoId = vta.ID) "
            SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (vta.TipoComprobanteId = tdc.ID) "
            SQL = SQL & "INNER JOIN Talonarios tal ON (vta.TalonarioId = tal.ID) "
            SQL = SQL & "WHERE (ipt.VentaDocumentoId = " & pDoc & ") "
            SQL = SQL & "ORDER BY tdc.AbreviaturaId, vta.NroComprobante"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdGrl.ExecuteReader)

            If pFul Then

                SQL = "SELECT vta.ID, tdc.AbreviaturaId AS Tipo, vta.NroComprobante, vta.FecComprobante, vta.RazonSocial, "
                SQL = SQL & "vta.Importe AS ImporteComprobante, vta.Importe - vta.ImpSaldado AS Saldo, 0.00 AS ImporteImputado, "
                SQL = SQL & "tal.flgSubDiario "
                SQL = SQL & "FROM VentasDocumentos vta "
                SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (vta.TipoComprobanteId = tdc.ID) "
                SQL = SQL & "INNER JOIN Clientes cli ON (vta.ClienteId = cli.ID) "
                SQL = SQL & "INNER JOIN Talonarios tal ON (vta.TalonarioId = tal.ID) "
                SQL = SQL & "WHERE (vta.ClienteId = " & pCli & ") AND (vta.ImpSaldado < vta.Importe) "
                SQL = SQL & "AND (vta.flgStatus = 1) AND (tdc.flgCobranza = 0) "
                SQL = SQL & "AND (vta.ID NOT IN(SELECT VentaDocImputadoId FROM VentasDocImputaciones WHERE (VentaDocumentoId = " & pDoc & "))) "
                SQL = SQL & "ORDER BY tdc.AbreviaturaId, vta.NroComprobante"

                cmdGrl.CommandText = SQL
                Dim dtPen As New DataTable
                dtPen.Load(cmdGrl.ExecuteReader)

                For vIdx = 0 To dtPen.Rows.Count - 1

                    Dim dtRow As DataRow = dt.NewRow
                    Dim vCol As Integer

                    For vCol = 0 To dt.Columns.Count - 1
                        dtRow(vCol) = dtPen(vIdx)(vCol)
                    Next vCol

                    dt.Rows.Add(dtRow)

                Next vIdx

            End If

            GetImputacionesByDocumento = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetImputacionesByDocumento", ex)
        End Try
    End Function

#End Region

#Region "Comunes"

    Public Function GetIDByIndex(ByVal pTal As Int64, ByVal pNro As String, Optional ByVal pExcPru As Boolean = True) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM VentasDocumentos WHERE (TalonarioId = " & pTal & ") AND (NroComprobante = '" & pNro & "') "
            If pExcPru Then SQL = SQL & "AND (flgStatus > 0) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetIDByTipoNroComprobante(ByVal pTip As String, ByVal pNro As String) As Int64
        GetIDByTipoNroComprobante = 0
        Try
            Dim SQL As String

            SQL = "SELECT doc.ID FROM VentasDocumentos doc "
            SQL = SQL & "INNER JOIN TiposComprobantes tip ON (doc.TipoComprobanteId = tip.ID) "
            SQL = SQL & "WHERE (tip.AbreviaturaId = '" & pTip & "') AND (doc.NroComprobante = '" & pNro & "') "
            SQL = SQL & "AND (flgStatus > 0) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByTipoNroComprobante = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByTipoNroComprobante", ex)
        End Try

    End Function

    Public Function GetIDByClienteTipoNroComprobante(ByVal pCli As Int64, ByVal pTip As String, ByVal pNro As Int64) As Int64
        GetIDByClienteTipoNroComprobante = 0
        Try
            Dim SQL As String

            SQL = "SELECT doc.ID FROM VentasDocumentos doc "
            SQL = SQL & "INNER JOIN TiposComprobantes tip ON (doc.TipoComprobanteId = tip.ID) "
            SQL = SQL & "WHERE (doc.ClienteId = " & pCli & ") AND (tip.AbreviaturaId = '" & pTip & "') AND (doc.NroComprobante LIKE '%" & pNro & "') "
            SQL = SQL & "AND (doc.flgStatus > 0) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByClienteTipoNroComprobante = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByClienteTipoNroComprobante", ex)
        End Try

    End Function

#End Region

#Region "Impresion"

    Public Sub CreateCopies(ByRef rptDataView As DataView, pCpy As Integer)
        Try

            Dim dt As DataTable = rptDataView.Table
            Dim dtKey(4) As DataColumn

            dtKey(0) = dt.Columns("TipoComprobante")
            dtKey(1) = dt.Columns("NroComprobante")
            dtKey(2) = dt.Columns("NroCopia")
            dtKey(3) = dt.Columns("NroRenglon")

            dt.PrimaryKey = dtKey

            dt.Columns("TituloCopia").MaxLength = 100

            Dim vIdx As Integer
            Dim vCol As Integer
            Dim vCpy As Integer

            For vCpy = 2 To pCpy

                For vIdx = 0 To dt.Rows.Count - 1

                    Dim dtRow As DataRow = dt.NewRow

                    For vCol = 0 To dt.Columns.Count - 1

                        Select Case dt.Columns(vCol).ColumnName
                            Case "NroCopia"
                                dtRow(dt.Columns(vCol)) = vCpy
                            Case "TituloCopia"
                                Select Case vCpy
                                    Case 2 : dtRow(dt.Columns(vCol)) = "DUPLICADO"
                                    Case 3 : dtRow(dt.Columns(vCol)) = "TRIPLICADO"
                                    Case Else : dtRow(dt.Columns(vCol)) = "COPIA " & vCpy
                                End Select
                            Case Else
                                dtRow(dt.Columns(vCol)) = dt(vIdx)(vCol)
                        End Select

                    Next

                    dt.Rows.Add(dtRow)

                Next

            Next

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CreateCopies", ex)
        End Try

    End Sub

    Public Sub CreatePaginacion(ByRef rptDataView As DataView, pPagCnt As Integer)
        Try

            rptDataView.Sort = "TipoComprobante, Numero, NroCopia, NroRenglon"

            Dim vLstTip As String = ""
            Dim vLstNro As String = ""
            Dim vLstCpy As Integer = 0
            Dim vNroCmp As Integer = 0

            rptDataView.Table.Columns("NumeroPagina").ReadOnly = False

            For Each row As DataRowView In rptDataView

                If Not (vLstTip = row("TipoComprobante") And vLstNro = row("NroComprobante") And vLstCpy = row("NroCopia")) Then

                    vNroCmp = vNroCmp + 1

                    vLstTip = row("TipoComprobante")
                    vLstNro = row("NroComprobante")
                    vLstCpy = row("NroCopia")

                End If

                If vNroCmp Mod pPagCnt <> 0 Then
                    row("NumeroPagina") = Int(vNroCmp / pPagCnt) + 1
                Else
                    row("NumeroPagina") = vNroCmp / pPagCnt
                End If

                Debug.Print(row("TipoComprobante") & "-" & row("NroComprobante") & "-" & row("NroCopia") & "-" & row("NumeroPagina"))

            Next

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CreatePaginacion", ex)
        End Try

    End Sub

    Public Function GetDetalleBySel(pPid As Int64) As DataTable

        GetDetalleBySel = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT dic.VentaDocumentoId, tip.AbreviaturaId AS TipoComprobante, doc.NroComprobante, inc.FecIncidente, inc.NroIncidente, "
            SQL = SQL & "inc.Paciente, con.Descripcion AS Concepto, CASE dic.PorcentajeIva WHEN 0 THEN 'EX' ELSE '' END AS Iva, dic.Importe "
            SQL = SQL & "FROM VentasDocIncidentes dic "
            SQL = SQL & "INNER JOIN VentasDocumentos doc ON dic.VentaDocumentoId = doc.ID "
            SQL = SQL & "INNER JOIN TiposComprobantes tip ON doc.TipoComprobanteId = tip.ID "
            SQL = SQL & "INNER JOIN ConceptosFacturacion con ON dic.ConceptoFacturacionId = con.ID "
            SQL = SQL & "INNER JOIN Incidentes inc ON dic.IncidenteId = inc.ID "
            SQL = SQL & "INNER JOIN selVentasDocumentos sel ON (doc.ID = sel.VentaDocumentoId) "
            SQL = SQL & "WHERE (sel.PID = " & logPID & ") "


            Dim cmdInc As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdInc.ExecuteReader)

            GetDetalleBySel = dt

        Catch ex As Exception

            HandleError(Me.GetType.Name, "GetDetalleBySel", ex)

        End Try

    End Function

    Public Function GetDataView(ByVal pRptId As Int64, Optional ByVal pHavSel As Boolean = True, Optional ByVal pDocId As Int64 = 0) As DataView
        GetDataView = Nothing
        Try
            Dim objImagen As New conImagenDocumentos(Me.myCnnName)

            If objImagen.Abrir(pRptId) Then

                Dim SQL As String

                If Not pHavSel Then
                    Me.autoCreateSel(1)
                End If

                SQL = "SELECT tdc.AbreviaturaId AS TipoComprobante, doc.NroComprobante, tal.NroSucursal AS PuntoVenta, tal.Letra, CAST(SUBSTRING(doc.NroComprobante, 6, 8) AS numeric) AS Numero, "
                SQL = SQL & "CASE SUBSTRING(doc.NroComprobante, 1, 1) WHEN 'A' THEN CASE tdc.AbreviaturaId WHEN 'FAC' THEN '01' WHEN 'DEB' THEN '02' ELSE '03' END WHEN 'B' THEN CASE tdc.AbreviaturaId WHEN 'FAC' THEN '06' WHEN 'DEB' THEN '07' ELSE '08' END "
                SQL = SQL & "WHEN 'C' THEN CASE tdc.AbreviaturaId WHEN 'FAC' THEN '11' WHEN 'DEB' THEN '12' ELSE '13' END END AS CodigoComprobante, "
                SQL = SQL & "doc.NroCAE, doc.FecVencimientoCAE, doc.CodigoBarra, 0 AS NroCopia, 'ORIGINAL' AS TituloCopia, 1 AS NumeroPagina, "
                SQL = SQL & "ISNULL(emp.Descripcion, '') AS EmpresaEmisora, ISNULL(emp.CUIT, '') AS EmpresaEmiCUIT, ISNULL(emp.Domicilio, '') AS EmpresaEmiDomicilio, "
                SQL = SQL & "emp.FecInicio AS EmpresaEmiInicioAct, ISNULL(emp.NroIngresosBrutos, '') AS EmpresaEmiNroIngresosBrutos, "
                SQL = SQL & "doc.CUIT, cli.AbreviaturaId AS CodigoCliente, doc.RazonSocial, doc.FecComprobante, doc.FecVencimiento, "
                SQL = SQL & "doc.Importe, doc.ImpExento, doc.ImpGravado, doc.ImpIva1, doc.ImpIva2, doc.ImpImpuesto, "
                SQL = SQL & "iva.Descripcion AS CondicionIva, doc.PorcentajeIva, cli.Domicilio, cli.dmReferencia AS Referencia, cli.CodigoPostal, loc.Descripcion AS Localidad, "
                SQL = SQL & "ren.RenglonId AS NroRenglon, ISNULL(con.AbreviaturaId, '') AS Concepto, ren.Descripcion AS TextoRenglon, "
                SQL = SQL & "ren.PorcentajeIva AS IvaRenglon, ren.Cantidad AS Cantidad, "

                SQL = SQL & "ISNULL((SELECT SUM(Cantidad) FROM VentasDocRenglones WHERE VentaDocumentoId = doc.ID), 0) AS CantidadGrupo, "

                SQL = SQL & "CASE tal.Letra WHEN 'A' THEN ren.Importe ELSE ROUND(ren.Importe + ROUND((ren.Importe * ren.PorcentajeIva) / 100, 2), 2) END AS ImporteRenglon, "
                SQL = SQL & "CASE tal.Letra WHEN 'A' THEN ren.Importe * (CASE ISNULL(ren.Cantidad, 1) WHEN 0 THEN 1 ELSE ren.Cantidad END) ELSE "
                SQL = SQL & "(ROUND(ren.Importe * (CASE ISNULL(ren.Cantidad, 1) WHEN 0 THEN 1 ELSE ren.Cantidad END) + ROUND(((ren.Importe * (CASE ISNULL(ren.Cantidad, 1) WHEN 0 THEN 1 ELSE ren.Cantidad END)) * ren.PorcentajeIva) / 100, 2), 2)) END AS ImporteRenglonIva, "

                SQL = SQL & "CASE WHEN MONTH(doc.FecComprobante) < 10 THEN '0' + CAST(MONTH(doc.FecComprobante) AS varchar) ELSE CAST(MONTH(doc.FecComprobante) AS varchar) END + '/' + CAST(YEAR(FecComprobante) AS varchar) AS Periodo, "
                SQL = SQL & "fpg.Descripcion AS FormaPago, rub.Descripcion AS RubroCliente, cob.Descripcion AS Cobrador, cob.AbreviaturaId AS CobradorCodigo, "
                SQL = SQL & "pla.Descripcion AS PlanCliente, cli.FecIngreso, cli.Observaciones, "
                SQL = SQL & "ISNULL((SELECT vto.Importe FROM VentasDocVencimientos vto WHERE vto.VentaDocumentoId = doc.ID AND vto.NroVencimiento = 1), doc.Importe) AS ImporteVencimiento"

                Dim vVtoIdx As Integer

                For vVtoIdx = 2 To 4

                    SQL = SQL & ", ISNULL((SELECT vto.FecVencimiento FROM VentasDocVencimientos vto WHERE vto.VentaDocumentoId = doc.ID AND vto.NroVencimiento = " & vVtoIdx & "), '1899-01-01') AS FecVencimiento" & vVtoIdx
                    SQL = SQL & ", ISNULL((SELECT DAY(DATEADD(day, 1, vto.FecVencimiento)) FROM VentasDocVencimientos vto WHERE vto.VentaDocumentoId = doc.ID AND vto.NroVencimiento = " & vVtoIdx & "), 0) AS SiguienteDiaVencimiento" & vVtoIdx
                    SQL = SQL & ", ISNULL((SELECT vto.Importe FROM VentasDocVencimientos vto WHERE vto.VentaDocumentoId = doc.ID AND vto.NroVencimiento = " & vVtoIdx & "), 0) AS ImporteVencimiento" & vVtoIdx

                Next


                Dim vFamIdx As Integer

                For vFamIdx = 1 To 5

                    SQL = SQL & ", ISNULL((SELECT ISNULL(SUM(renfam.Cantidad), 0) FROM VentasDocRenglones renfam "
                    SQL = SQL & "INNER JOIN Planes plafam ON renfam.PlanId = plafam.ID "
                    SQL = SQL & "INNER JOIN PlanesFamilias famfam ON plafam.PlanFamiliaId = famfam.ID "
                    SQL = SQL & "WHERE renfam.VentaDocumentoId = doc.ID AND famfam.NroImpresion = " & vFamIdx & "), 0) AS CantidadFamiliaPlan" & vFamIdx

                    SQL = SQL & ", ISNULL((SELECT ISNULL(SUM(ROUND(renfam.Importe + ROUND((renfam.Importe * renfam.PorcentajeIva) / 100, 2), 2)), 0) FROM VentasDocRenglones renfam "
                    SQL = SQL & "INNER JOIN Planes plafam ON renfam.PlanId = plafam.ID "
                    SQL = SQL & "INNER JOIN PlanesFamilias famfam ON plafam.PlanFamiliaId = famfam.ID "
                    SQL = SQL & "WHERE renfam.VentaDocumentoId = doc.ID AND famfam.NroImpresion = " & vFamIdx & "), 0) AS ImporteFamiliaPlan" & vFamIdx

                Next

                SQL = SQL & " FROM VentasDocumentos doc "
                SQL = SQL & "INNER JOIN Talonarios tal ON (doc.TalonarioId = tal.ID) "
                SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (doc.TipoComprobanteId = tdc.ID) "
                SQL = SQL & "INNER JOIN VentasDocRenglones ren ON (doc.ID = ren.VentaDocumentoId) "
                SQL = SQL & "INNER JOIN Clientes cli ON (doc.ClienteId = cli.ID) "
                SQL = SQL & "INNER JOIN SituacionesIva iva ON (doc.SituacionIvaId = iva.ID) "
                SQL = SQL & "LEFT JOIN EmpresasLegales emp ON (tal.EmpresaLegalId = emp.ID) "
                SQL = SQL & "LEFT JOIN ConceptosFacturacion con ON (ren.ConceptoFacturacionId = con.ID) "
                SQL = SQL & "LEFT JOIN Localidades loc ON (cli.LocalidadId = loc.ID) "
                SQL = SQL & "LEFT JOIN FormasPago fpg ON (cli.FormaPagoId = fpg.ID) "
                SQL = SQL & "LEFT JOIN RubrosClientes rub ON (cli.RubroClienteId = rub.ID) "
                SQL = SQL & "LEFT JOIN Cobradores cob ON (cli.CobradorId = cob.ID) "
                SQL = SQL & "LEFT JOIN Planes pla ON (cli.PlanId = pla.ID) "

                SQL = SQL & "INNER JOIN selVentasDocumentos sel ON (doc.ID = sel.VentaDocumentoId) "
                SQL = SQL & "WHERE (sel.PID = " & logPID & ") "
                If pDocId > 0 Then SQL = SQL & "AND (doc.ID = " & pDocId & ") "
                SQL = SQL & "ORDER BY tdc.AbreviaturaId, doc.NroComprobante, ren.RenglonId"


                Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dt As New DataTable
                dt.Load(cmdGrl.ExecuteReader)

                '----> Aplico Número y Letras
                Me.FillNumerosLetras(objImagen, dt)

                '----> Aplico Pago Facil
                Me.FillCodigoBarraPagoFacil(dt)

                If objImagen.CantidadRenglones > 0 Then

                    Dim dtDataSet As DataTable
                    Dim vRow As Integer, vCol As Integer
                    Dim vLstCmp As String = "", vLstRen As Integer = 0

                    dtDataSet = dt.Clone

                    For vRow = 0 To dt.Rows.Count - 1

                        Dim dtRow As DataRow

                        If vLstCmp = "" Then
                            vLstCmp = dt(vRow)("NroComprobante")
                        End If

                        '---> Completo renglones faltantes
                        If vLstCmp <> dt(vRow)("NroComprobante") Then
                            FillDataViewRenglones(vLstRen, objImagen.CantidadRenglones, dt.Rows(vRow - 1), dtDataSet)
                        End If

                        If dt(vRow)("NroRenglon") <= objImagen.CantidadRenglones Then
                            '---> Copio el renglón
                            dtRow = dtDataSet.NewRow
                            For vCol = 0 To dtDataSet.Columns.Count - 1
                                dtRow(vCol) = dt.Rows(vRow)(vCol)
                            Next
                            dtDataSet.Rows.Add(dtRow)

                            vLstCmp = dt(vRow)("NroComprobante")
                            vLstRen = dt(vRow)("NroRenglon")
                        End If

                    Next

                    If dt.Rows.Count > 0 Then
                        FillDataViewRenglones(vLstRen, objImagen.CantidadRenglones, dt.Rows(vRow - 1), dtDataSet)
                    End If

                    GetDataView = New DataView(dtDataSet)

                Else

                    GetDataView = New DataView(dt)

                End If

            End If

            objImagen = Nothing

        Catch ex As Exception

            HandleError(Me.GetType.Name, "GetDataView", ex)

        End Try
    End Function

    Public Function GetDataViewRecibo(ByVal pRptId As Int64, Optional ByVal pHavSel As Boolean = True, Optional ByVal pDocId As Int64 = 0) As DataView

        GetDataViewRecibo = Nothing

        Try
            Dim objImagen As New conImagenDocumentos(Me.myCnnName)

            If objImagen.Abrir(pRptId) Then

                Dim SQL As String

                If Not pHavSel Then
                    Me.autoCreateSel(1, True)
                End If

                SQL = "SELECT tdc.AbreviaturaId AS TipoComprobante, doc.NroComprobante, tal.NroSucursal AS PuntoVenta, tal.Letra, CAST(SUBSTRING(doc.NroComprobante, 6, 8) AS numeric) AS Numero, "
                SQL = SQL & "0 AS NroCopia, 'ORIGINAL' AS TituloCopia, 1 AS NumeroPagina, "
                SQL = SQL & "ISNULL(emp.Descripcion, '') AS EmpresaEmisora, ISNULL(emp.CUIT, '') AS EmpresaEmiCUIT, ISNULL(emp.Domicilio, '') AS EmpresaEmiDomicilio, "
                SQL = SQL & "emp.FecInicio AS EmpresaEmiInicioAct, ISNULL(emp.NroIngresosBrutos, '') AS EmpresaEmiNroIngresosBrutos, "
                SQL = SQL & "doc.CUIT, cli.AbreviaturaId AS CodigoCliente, doc.RazonSocial, doc.FecComprobante, doc.FecVencimiento, "
                SQL = SQL & "doc.Importe, cli.Domicilio, cli.dmReferencia AS Referencia, cli.CodigoPostal, loc.Descripcion AS Localidad, "
                SQL = SQL & "cob.AbreviaturaId AS CobradorCodigo, cob.Descripcion AS Cobrador, "
                SQL = SQL & "0 AS NroRenglon, "
                SQL = SQL & "ISNULL(fpg.AbreviaturaId, '') AS FormaPagoCodigo, fpg.Descripcion AS FormaPago, ISNULL(bco.Descripcion, '') AS Banco, ren.NroCheque, ren.FecCheque, ren.Importe AS ImporteRenglon "

                SQL = SQL & "FROM VentasDocumentos doc "
                SQL = SQL & "INNER JOIN Talonarios tal ON (doc.TalonarioId = tal.ID) "
                SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (doc.TipoComprobanteId = tdc.ID) "
                SQL = SQL & "LEFT JOIN VentasDocCobros ren ON (doc.ID = ren.VentaDocumentoId) "
                SQL = SQL & "INNER JOIN Clientes cli ON (doc.ClienteId = cli.ID) "
                SQL = SQL & "LEFT JOIN EmpresasLegales emp ON (tal.EmpresaLegalId = emp.ID) "
                SQL = SQL & "LEFT JOIN Localidades loc ON (cli.LocalidadId = loc.ID) "
                SQL = SQL & "LEFT JOIN FormasPago fpg ON (ren.FormaPagoId = fpg.ID) "
                SQL = SQL & "LEFT JOIN Bancos bco ON (ren.BancoId = bco.ID) "
                SQL = SQL & "LEFT JOIN RubrosClientes rub ON (cli.RubroClienteId = rub.ID) "
                SQL = SQL & "LEFT JOIN Cobradores cob ON (doc.CobradorId = cob.ID) "

                SQL = SQL & "INNER JOIN selVentasDocumentos sel ON (doc.ID = sel.VentaDocumentoId) "
                SQL = SQL & "WHERE (sel.PID = " & logPID & ") "
                If pDocId > 0 Then SQL = SQL & "AND (doc.ID = " & pDocId & ") "
                SQL = SQL & "ORDER BY tdc.AbreviaturaId, doc.NroComprobante"


                Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dt As New DataTable
                dt.Load(cmdGrl.ExecuteReader)

                '----> Numeración de renglones
                dt.Columns("NroRenglon").ReadOnly = False

                '----> Aplico Número y Letras
                Me.FillNumerosLetras(objImagen, dt)

                If objImagen.CantidadRenglones > 0 Then

                    Dim dtDataSet As DataTable
                    Dim vRow As Integer, vCol As Integer
                    Dim vLstCmp As String = "", vLstRen As Integer = 0

                    dtDataSet = dt.Clone

                    For vRow = 0 To dt.Rows.Count - 1

                        Dim dtRow As DataRow

                        If vLstCmp = "" Then
                            vLstCmp = dt(vRow)("NroComprobante")
                        End If

                        '---> Completo renglones faltantes
                        If vLstCmp <> dt(vRow)("NroComprobante") Then
                            FillDataViewRenglones(vLstRen, objImagen.CantidadRenglones, dt.Rows(vRow - 1), dtDataSet)
                            dt(vRow)("NroRenglon") = 1
                        Else
                            dt(vRow)("NroRenglon") = dt(vRow)("NroRenglon") + 1
                        End If

                        If dt(vRow)("NroRenglon") <= objImagen.CantidadRenglones Then
                            '---> Copio el renglón
                            dtRow = dtDataSet.NewRow
                            For vCol = 0 To dtDataSet.Columns.Count - 1
                                dtRow(vCol) = dt.Rows(vRow)(vCol)
                            Next
                            dtDataSet.Rows.Add(dtRow)

                            vLstCmp = dt(vRow)("NroComprobante")
                            vLstRen = dt(vRow)("NroRenglon")
                        End If

                    Next

                    If dt.Rows.Count > 0 Then
                        FillDataViewRenglones(vLstRen, objImagen.CantidadRenglones, dt.Rows(vRow - 1), dtDataSet)
                    End If

                    GetDataViewRecibo = New DataView(dtDataSet)

                Else

                    GetDataViewRecibo = New DataView(dt)

                End If

            End If

            objImagen = Nothing

        Catch ex As Exception

            HandleError(Me.GetType.Name, "GetDataViewRecibo", ex)

        End Try
    End Function

    Private Sub FillCodigoBarraPagoFacil(ByRef dt As DataTable)
        Try

            '----> Verifico si tiene integración con Pago Fácil
            Dim objIntegracion As New conMediosPagoIntegraciones

            Dim vIteId As Int64 = objIntegracion.GetIDByIntegracionId(iteMediosPago.PagoFacil)

            If vIteId > 0 Then

                Dim vRow As Integer
                Dim vEmp As String = objIntegracion.GetAtributo(vIteId, "NroEmpresa", "9999")

                dt.Columns.Add("CodigoBarrasPagoFacil", GetType(String))
                dt.Columns("CodigoBarrasPagoFacil").MaxLength = 45

                For vRow = 0 To dt.Rows.Count - 1

                    Dim vVto2 As Double = dt(vRow)("ImporteVencimiento2") - dt(vRow)("Importe")
                    Dim vFec2 As Integer = DateDiff(DateInterval.Minute, dt(vRow)("FecVencimiento"), dt(vRow)("FecVencimiento2"))
                    Dim vCic As Integer = 0

                    If vVto2 < 0 Then vVto2 = 0
                    If vFec2 < 0 Then vFec2 = 0

                    Dim vBarCode As String = vEmp & objIntegracion.prnDouble(dt(vRow)("Importe"), 6) & CDate(dt(vRow)("FecVencimiento")).Year.ToString.Substring(2, 2) & Format(CDate(dt(vRow)("FecVencimiento")).DayOfYear, "000") & _
                        Format(dt(vRow)("Numero"), "00000000") & "000000" & "0" & objIntegracion.prnDouble(vVto2, 6) & Format(vFec2, "00")


                    '-----> Digitos verificadores
                    For vCic = 0 To 1

                        Dim vDig As Integer
                        Dim vBarSec As String = "1"
                        Dim vSum As Double = 0

                        For vDig = 1 To vBarCode.Length - 1

                            Select Case (vDig / 4 - Int(vDig / 4))
                                Case 0 : vBarSec = vBarSec & "9"
                                Case 0.25 : vBarSec = vBarSec & "3"
                                Case 0.5 : vBarSec = vBarSec & "5"
                                Case 0.75 : vBarSec = vBarSec & "7"
                            End Select

                        Next vDig

                        For vDig = 0 To vBarCode.Length - 1
                            vSum = vSum + (Val(vBarCode(vDig)) / Val(vBarSec(vDig)))
                        Next vDig

                        Dim vDigVer As Integer = Int(vSum / 2) Mod 10

                        vBarCode = vBarCode & vDigVer

                    Next vCic

                    dt(vRow)("CodigoBarrasPagoFacil") = vBarCode

                Next vRow

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "FillCodigoBarraPagoFacil", ex)
        End Try
    End Sub


    Private Sub FillNumerosLetras(ByVal objImagen As conImagenDocumentos, ByRef dt As DataTable)
        Try
            If objImagen.flgImporteLetras = 1 Then

                Dim vRow As Integer
                Dim objNumerosLetras As New NumerosLetraV2

                dt.Columns.Add("ImporteEnLetras", GetType(String))
                dt.Columns("ImporteEnLetras").MaxLength = 300

                For vRow = 0 To dt.Rows.Count - 1

                    Dim vLetNum As String = objNumerosLetras.Letras(dt(vRow)("Importe")).ToUpper

                    'If vLetNum.Contains("CON") Then

                    '    Dim vParts() As String = vLetNum.Split(" CON ")

                    '    vLetNum = vParts(0) & " PESOS CON " & vParts(1) & " CENTAVOS"

                    'Else

                    '    vLetNum = vLetNum & " PESOS"

                    'End If

                    dt(vRow)("ImporteEnLetras") = vLetNum

                Next vRow

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "FillNumerosLetras", ex)
        End Try
    End Sub

    Private Sub FillDataViewRenglones(ByVal pLstRen As Integer, ByVal pCntRen As Integer, ByVal dtRen As DataRow, ByRef dtDataSet As DataTable)
        Try
            Dim vRowAdd As Integer
            Dim vCol As Integer

            '----> Ajusto renglones...
            If pLstRen + 1 <= pCntRen Then

                For vRowAdd = (pLstRen + 1) To pCntRen

                    Dim dtRow As DataRow = dtDataSet.NewRow

                    For vCol = 0 To dtDataSet.Columns.Count - 1
                        Select Case dtDataSet.Columns(vCol).ColumnName
                            Case "NroRenglon" : dtRow(vCol) = vRowAdd
                            Case "Concepto" : dtRow(vCol) = ""
                            Case "TextoRenglon" : dtRow(vCol) = ""
                            Case "IvaRenglon" : dtRow(vCol) = 0
                            Case "ImporteRenglon" : dtRow(vCol) = 0
                            Case "Cantidad" : dtRow(vCol) = 0
                            Case "PlanCliente" : dtRow(vCol) = ""
                            Case "ImporteRenglonIva" : dtRow(vCol) = 0

                            Case "FormaPago" : dtRow(vCol) = ""
                            Case "FormaPagoCodigo" : dtRow(vCol) = ""
                            Case "Banco" : dtRow(vCol) = ""
                            Case "NroCheque" : dtRow(vCol) = ""
                            Case "FecCheque" : dtRow(vCol) = NullDateMin
                            Case "ImporteRenglon" : dtRow(vCol) = 0

                            Case Else : dtRow(vCol) = dtRen(vCol)
                        End Select
                    Next

                    dtDataSet.Rows.Add(dtRow)

                Next

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDataView", ex)
        End Try
    End Sub

    Private Sub autoCreateSel(ByVal pCntDoc As Integer, Optional pRec As Boolean = False)

        Try

            Dim SQL As String
            Dim objDocumento As New conVentasDocumentos(Me.myCnnName)
            Dim objSelDoc As New conselVentasDocumentos(Me.myCnnName)

            objSelDoc.DelByPID(logPID)

            SQL = "SELECT TOP 10 doc.ID "
            SQL = SQL & "FROM VentasDocumentos doc "
            SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (doc.TipoComprobanteId = tdc.ID) "
            If Not pRec Then
                SQL = SQL & "WHERE (tdc.AbreviaturaId = 'FAC') "
            Else
                SQL = SQL & "WHERE (tdc.AbreviaturaId = 'REC') "
            End If
            SQL = SQL & "ORDER BY tdc.AbreviaturaId, doc.NroComprobante DESC"

            Dim cmdCreate As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            dt.Load(cmdCreate.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                If objDocumento.Abrir(dt(vIdx)(0)) Then
                    Me.setSelImpresion(objDocumento, pCntDoc)
                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "autoCreateSel", ex)
        End Try
    End Sub

    Public Function GetDataViewHorizontal(ByVal pRptId As Int64, Optional ByVal pHavSel As Boolean = True, Optional ByVal pDocId As Int64 = 0) As DataView

        GetDataViewHorizontal = Nothing

        Try
            Dim objImagen As New conImagenDocumentos(Me.myCnnName)

            If objImagen.Abrir(pRptId) Then

                Dim SQL As String

                If Not pHavSel Then
                    Me.autoCreateSel(objImagen.CantidadDocumentos)
                End If

                Dim vIdx As Integer

                SQL = ""

                For vIdx = 1 To objImagen.CantidadDocumentos

                    If vIdx = 1 Then
                        SQL = "SELECT sel.NroCupon, " & getSelectQueryDataSet("cup" & vIdx)
                    Else
                        SQL = SQL & ", " & getSelectQueryDataSet("cup" & vIdx)
                    End If

                Next vIdx

                SQL = SQL & " FROM selVentasDocCupones sel "

                For vIdx = 1 To objImagen.CantidadDocumentos

                    SQL = SQL & "LEFT JOIN VentasDocumentos cup" & vIdx & " ON (sel.VentaDocumento" & vIdx & "Id = cup" & vIdx & ".ID) "
                    SQL = SQL & getJoinQueryDataSet("cup" & vIdx)

                Next vIdx

                SQL = SQL & "WHERE (sel.PID = " & logPID & ") "
                If pDocId > 0 Then SQL = SQL & "AND (doc.ID = " & pDocId & ") "


                Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dt As New DataTable
                dt.Load(cmdGrl.ExecuteReader)

                '----> Aplico Número y Letras
                Me.FillNumerosLetras(objImagen, dt)

                '----> Aplico Pago Facil
                Me.FillCodigoBarraPagoFacil(dt)

                If objImagen.CantidadRenglones > 0 Then

                    Dim dtDataSet As DataTable
                    Dim vRow As Integer, vCol As Integer
                    Dim vLstCmp As String = "", vLstRen As Integer = 0

                    dtDataSet = dt.Clone

                    For vRow = 0 To dt.Rows.Count - 1

                        Dim dtRow As DataRow

                        If vLstCmp = "" Then
                            vLstCmp = dt(vRow)("NroComprobante")
                        End If

                        '---> Completo renglones faltantes
                        If vLstCmp <> dt(vRow)("NroComprobante") Then
                            FillDataViewRenglones(vLstRen, objImagen.CantidadRenglones, dt.Rows(vRow - 1), dtDataSet)
                        End If

                        If dt(vRow)("NroRenglon") <= objImagen.CantidadRenglones Then
                            '---> Copio el renglón
                            dtRow = dtDataSet.NewRow
                            For vCol = 0 To dtDataSet.Columns.Count - 1
                                dtRow(vCol) = dt.Rows(vRow)(vCol)
                            Next
                            dtDataSet.Rows.Add(dtRow)

                            vLstCmp = dt(vRow)("NroComprobante")
                            vLstRen = dt(vRow)("NroRenglon")
                        End If

                    Next

                    If dt.Rows.Count > 0 Then
                        FillDataViewRenglones(vLstRen, objImagen.CantidadRenglones, dt.Rows(vRow - 1), dtDataSet)
                    End If

                    GetDataViewHorizontal = New DataView(dtDataSet)

                Else

                    GetDataViewHorizontal = New DataView(dt)

                End If

            End If

            objImagen = Nothing

        Catch ex As Exception

            HandleError(Me.GetType.Name, "GetDataView", ex)

        End Try

    End Function

    Private Function getSelectQueryDataSet(Optional ByVal pPfx As String = "") As String

        getSelectQueryDataSet = ""

        Try

            Dim SQL As String

            SQL = getField("tdc", "AbreviaturaId", pPfx, "TipoComprobante") & getField("", "NroComprobante", pPfx, "NroComprobante")
            SQL = SQL & getField("", "TalonarioId", pPfx, "Talonario") & getField("", "CUIT", pPfx, "CUIT")
            SQL = SQL & getField("cli", "AbreviaturaId", pPfx, "CodigoCliente") & getField("", "RazonSocial", pPfx, "RazonSocial")
            SQL = SQL & getField("", "FecComprobante", pPfx, "FecComprobante") & getField("", "FecVencimiento", pPfx, "FecVencimiento")
            SQL = SQL & getField("", "Importe", pPfx, "Importe") & getField("", "ImpExento", pPfx, "ImpExento")
            SQL = SQL & getField("", "ImpGravado", pPfx, "ImpGravado") & getField("", "ImpIva1", pPfx, "ImpIva1")
            SQL = SQL & getField("", "ImpIva2", pPfx, "ImpIva2") & getField("", "ImpImpuesto", pPfx, "ImpImpuesto")
            SQL = SQL & getField("iva", "Descripcion", pPfx, "CondicionIva") & getField("", "PorcentajeIva", pPfx, "PorcentajeIva")
            SQL = SQL & getField("cli", "Domicilio", pPfx, "Domicilio") & getField("cli", "CodigoPostal", pPfx, "CodigoPostal")
            SQL = SQL & getField("loc", "Descripcion", pPfx, "Localidad", False)

            'SQL = SQL & getField("ren", "RenglonId", pPfx, "NroRenglon")
            'SQL = SQL & getField("con", "AbreviaturaId", pPfx, "Concepto", , True) & getField("ren", "Descripcion", pPfx, "TextoRenglon")
            'SQL = SQL & getField("ren", "PorcentajeIva", pPfx, "IvaRenglon") & getField("ren", "Importe", pPfx, "ImporteRenglon")
            'SQL = SQL & getField("ren", "Cantidad", pPfx, "Cantidad", False)

            getSelectQueryDataSet = SQL

        Catch ex As Exception
            HandleError(Me.GetType.Name, "autoCreateSel", ex)
        End Try

    End Function

    Private Function getJoinQueryDataSet(Optional ByVal pPfx As String = "") As String

        getJoinQueryDataSet = ""

        Try

            Dim SQL As String

            SQL = "LEFT JOIN TiposComprobantes " & getTableAlias("tdc", pPfx) & " ON (" & getField("", "TipoComprobanteId", pPfx) & " = " & getField("tdc", "ID", pPfx) & ") "
            SQL = SQL & "LEFT JOIN Clientes " & getTableAlias("cli", pPfx) & " ON (" & getField("", "ClienteId", pPfx) & " = " & getField("cli", "ID", pPfx) & ") "
            SQL = SQL & "LEFT JOIN SituacionesIva " & getTableAlias("iva", pPfx) & " ON (" & getField("", "SituacionIvaId", pPfx) & " = " & getField("iva", "ID", pPfx) & ") "
            SQL = SQL & "LEFT JOIN Localidades " & getTableAlias("loc", pPfx) & " ON (" & getField("cli", "LocalidadId", pPfx) & " = " & getField("loc", "ID", pPfx) & ") "

            'SQL = SQL & "LEFT JOIN VentasDocRenglones " & getTableAlias("ren", pPfx) & " ON (" & getField("", "ID", pPfx) & " = " & getField("ren", "VentaDocumentoId", pPfx) & ") "
            'SQL = SQL & "LEFT JOIN ConceptosFacturacion " & getTableAlias("con", pPfx) & " ON (" & getField("ren", "ConceptoFacturacionId", pPfx) & " = " & getField("con", "ID", pPfx) & ") "

            getJoinQueryDataSet = SQL

        Catch ex As Exception
            HandleError(Me.GetType.Name, "autoCreateSel", ex)
        End Try

    End Function

    Private Function getField(ByVal pTbl As String, ByVal pNom As String, ByVal pPfx As String, Optional ByVal pAli As String = "", Optional ByVal pNxt As Boolean = True, Optional ByVal pIsNull As Boolean = False) As String

        Dim vStr As String

        pTbl = getTableAlias(pTbl, pPfx)

        vStr = pTbl & "." & pNom

        If pIsNull Then
            vStr = "ISNULL(" & vStr & ", '')"
        End If

        If pAli <> "" Then
            If pPfx = "" Then
                vStr = vStr & " AS " & pAli
            Else
                vStr = vStr & " AS " & pPfx & pAli
            End If
            If pNxt Then vStr = vStr & ", "
        End If

        getField = vStr

    End Function

    Private Function getTableAlias(ByVal pNom As String, ByVal pPfx As String) As String
        If pPfx = "" Then
            getTableAlias = pNom
        Else
            getTableAlias = pPfx & pNom
        End If
    End Function

    Public Function setSelImpresion(ByVal objDocumento As conVentasDocumentos, Optional ByVal pCntDoc As Integer = 0) As Boolean
        setSelImpresion = False
        Try

            If pCntDoc = 0 Then pCntDoc = objDocumento.TalonarioId.ImagenDocumentoId.CantidadDocumentos

            Dim objSelDoc As New conselVentasDocumentos(Me.myCnnName)

            objSelDoc.CleanProperties(objSelDoc)
            objSelDoc.PID = logPID
            objSelDoc.VentaDocumentoId.SetObjectId(objDocumento.ID)

            If objSelDoc.Salvar(objSelDoc) Then
                setSelImpresion = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "setSelImpresion", ex)
        End Try
    End Function



#End Region

End Class
