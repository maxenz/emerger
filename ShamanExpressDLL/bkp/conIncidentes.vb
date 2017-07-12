Imports System.Data
Imports System.Data.SqlClient

Public Class conIncidentes
    Inherits typIncidentes

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetOperativaDX(Optional pScrOpe As scrOperativa = scrOperativa.EnCurso, Optional pVisCpl As Boolean = False, Optional pSelPrf As Int64 = 0, Optional pChkZon As Collection = Nothing) As DataTable

        GetOperativaDX = Nothing

        Try
            Dim SQL As String
            Dim vIdx As Integer = 0

            Dim vRowSel As Integer = -1, vColSel As Integer = -1

            '--------> Verifico si hay conexión
            If cnnsNET.Count > 1 Then Exit Function

            '----------> Obtengo nro de página

            SQL = "SELECT vij.ID, inc.ID AS IncidenteId, ISNULL(gdo.ColorHexa, '') AS GradoColor, gdo.AbreviaturaId AS Grado, "

            Select Case shamanConfig.opeColumnaCliente
                Case 0 : SQL = SQL & "cli.AbreviaturaId AS Cliente, "
                Case 1 : SQL = SQL & "CASE ISNULL(inc.PlanId, '') WHEN '' THEN cli.AbreviaturaId ELSE inc.PlanId END AS Cliente, "
                Case 2 : SQL = SQL & "CASE ISNULL(inc.NroInterno, '') WHEN '' THEN cli.AbreviaturaId ELSE inc.NroInterno END AS Cliente, "
            End Select

            If pScrOpe <> scrOperativa.Programados Then
                SQL = SQL & "inc.NroIncidente AS NroIncidente, "
            Else
                SQL = SQL & "inc.TrasladoId AS NroIncidente, "
            End If

            SQL = SQL & "CAST(ISNULL((SELECT TOP 1 flgReclamo FROM IncidentesObservaciones WHERE IncidenteId = inc.ID ORDER BY flgReclamo DESC), 0) AS VARCHAR(1)) AS flgReclamo, "
            SQL = SQL & "ISNULL((SELECT TOP 1 Observaciones FROM IncidentesObservaciones WHERE IncidenteId = inc.ID AND flgReclamo = 1 ORDER BY flgReclamo DESC), '') AS Reclamo, "
            SQL = SQL & "CASE LEN(ISNULL(inc.Aviso, '')) WHEN 0 THEN '0' ELSE '1' END AS flgAviso, "

            If Not pVisCpl Then

                SQL = SQL & "CASE vij.ViajeId WHEN 'IDA' THEN dom.Domicilio ELSE dom.Domicilio + ' (' + vij.ViajeId + ')' END AS Domicilio, "

            Else

                SQL = SQL & "CASE vij.ViajeId WHEN 'IDA' THEN dom.Domicilio ELSE dom.Domicilio + ' (' + vij.ViajeId + ')' END + CHAR(10) + CHAR(13) + "
                SQL = SQL & "ISNULL(CASE vij.ViajeId WHEN 'IDA' THEN (SELECT domder.Domicilio FROM IncidentesDomicilios domder WHERE domder.IncidenteId = dom.IncidenteId AND domder.TipoDomicilio = 1) "
                SQL = SQL & "ELSE (SELECT domder.Domicilio FROM IncidentesDomicilios domder WHERE domder.IncidenteId = dom.IncidenteId AND domder.TipoDomicilio = 0) END, '') AS Domicilio, "

            End If

            If Not pVisCpl Then
                SQL = SQL & "inc.Sintomas, "
            Else
                SQL = SQL & "inc.Sintomas + CHAR(10) + CHAR(13) + inc.Paciente AS Sintomas, "
            End If

            SQL = SQL & "ISNULL(zon.ColorHexa, '') AS ZonaColor, "

            If Not pVisCpl Then
                SQL = SQL & "loc.AbreviaturaId AS Localidad, "
            Else
                SQL = SQL & "loc.AbreviaturaId + CHAR(10) + CHAR(13) + "
                SQL = SQL & "ISNULL(CASE vij.ViajeId WHEN 'IDA' THEN (SELECT locder.AbreviaturaId FROM IncidentesDomicilios domder INNER JOIN Localidades locder ON domder.LocalidadId = locder.ID WHERE domder.IncidenteId = dom.IncidenteId AND domder.TipoDomicilio = 1) "
                SQL = SQL & "ELSE (SELECT locder.AbreviaturaId FROM IncidentesDomicilios domder INNER JOIN Localidades locder ON domder.LocalidadId = locder.ID WHERE domder.IncidenteId = dom.IncidenteId AND domder.TipoDomicilio = 0) END, '') AS Localidad, "
            End If

            SQL = SQL & "REPLACE(inc.Sexo + CAST(inc.Edad AS VARCHAR(10)), '.00', '') AS SexoEdad, "
            SQL = SQL & "CASE ISNULL(mov.Movil, '') WHEN '' THEN pre.Movil ELSE mov.Movil END AS Movil, "
            SQL = SQL & "CASE ISNULL(mov.Movil, '') WHEN '' THEN 1 ELSE 0 END AS flgMovilPreasignado, "

            SQL = SQL & "CAST(ISNULL((SELECT TOP 1 CAST(flgEnviado AS INT) FROM IncidentesViajesMensajes WHERE IncidenteViajeId = vij.ID ORDER BY regFechaHora DESC), -1) + 1 AS VARCHAR(1)) AS flgEnviado, "
            SQL = SQL & "ISNULL((SELECT TOP 1 MensajeError FROM IncidentesViajesMensajes WHERE IncidenteViajeId = vij.ID ORDER BY regFechaHora DESC), '') AS ErrorEnvio, "

            If pScrOpe <> scrOperativa.Programados Then
                If shamanConfig.opeColumnaLlamTraslado = 0 Then
                    SQL = SQL & "vij.horLlamada, "
                Else
                    SQL = SQL & "CASE gdo.ClasificacionId WHEN 1 THEN CASE vij.ViajeId WHEN 'IDA' THEN vij.reqHorInternacion ELSE vij.horLlamada END ELSE vij.horLlamada END AS horLlamada, "
                End If
            Else
                If shamanConfig.opeColumnaLlamTraslado = 0 Then
                    SQL = SQL & "vij.reqHorLlegada AS horLlamada, "
                Else
                    SQL = SQL & "CASE gdo.ClasificacionId WHEN 1 THEN CASE vij.ViajeId WHEN 'IDA' THEN vij.reqHorInternacion ELSE vij.horLlamada END ELSE vij.horLlamada END AS horLlamada, "
                End If
            End If

            If pScrOpe <> scrOperativa.Programados Then
                SQL = SQL & "inc.Paciente, dom.dmReferencia, pre.Movil AS MovilPreasignado, san.AbreviaturaId AS Sanatorio, vij.ViajeId, "
                SQL = SQL & "vij.virTpoDespacho AS TpoDespacho, vij.virTpoSalida AS TpoSalida, vij.virTpoDesplazamiento AS TpoDesplazamiento, vij.virTpoAtencion AS TpoAtencion"
            Else
                SQL = SQL & "inc.Paciente, dom.dmReferencia, pre.Movil AS MovilPreasignado, san.AbreviaturaId AS Sanatorio, vij.ViajeId, "
                SQL = SQL & "0 AS TpoDespacho, 0 AS TpoSalida, 0 AS TpoDesplazamiento, 0 AS TpoAtencion"
            End If
            SQL = SQL & ", inc.Aviso, dom.dmLatitud, dom.dmLongitud, "
            SQL = SQL & "inc.GradoOperativoId, loc.ZonaGeograficaId, "
            SQL = SQL & "ISNULL((SELECT TOP 1 sus.AbreviaturaId FROM IncidentesSucesos ics INNER JOIN SucesosIncidentes sus ON ics.SucesoIncidenteId = sus.ID WHERE ics.IncidenteViajeId = vij.ID ORDER BY sus.Orden DESC), 0) AS UltimoSuceso, "

            If pScrOpe <> scrOperativa.Programados Then
                SQL = SQL & "ISNULL((SELECT TOP 1 valRefNumeric FROM GradosOperativosConfig WHERE GradoOperativoId = inc.GradoOperativoId AND TipoConfiguracion = 'DSP' AND vij.virTpoDespacho BETWEEN valDesde AND valHasta), 0) AS clrDespacho, "
                SQL = SQL & "ISNULL((SELECT TOP 1 valRefNumeric FROM GradosOperativosConfig WHERE GradoOperativoId = inc.GradoOperativoId AND TipoConfiguracion = 'SAL' AND vij.virTpoSalida BETWEEN valDesde AND valHasta), 0) AS clrSalida, "
                SQL = SQL & "ISNULL((SELECT TOP 1 valRefNumeric FROM GradosOperativosConfig WHERE GradoOperativoId = inc.GradoOperativoId AND TipoConfiguracion = 'DPL' AND vij.virTpoDesplazamiento BETWEEN valDesde AND valHasta), 0) AS clrDesplazamiento, "
                SQL = SQL & "ISNULL((SELECT TOP 1 valRefNumeric FROM GradosOperativosConfig WHERE GradoOperativoId = inc.GradoOperativoId AND TipoConfiguracion = 'ATE' AND vij.virTpoAtencion BETWEEN valDesde AND valHasta), 0) AS clrAtencion, "

                SQL = SQL & "vij.virTpoDerivacion, vij.virTpoInternacion, "
                SQL = SQL & "ISNULL((SELECT TOP 1 valRefNumeric FROM GradosOperativosConfig WHERE GradoOperativoId = inc.GradoOperativoId AND TipoConfiguracion = 'DRV' AND vij.virTpoDerivacion BETWEEN valDesde AND valHasta), 0) AS clrDerivacion, "
                SQL = SQL & "ISNULL((SELECT TOP 1 valRefNumeric FROM GradosOperativosConfig WHERE GradoOperativoId = inc.GradoOperativoId AND TipoConfiguracion = 'ITE' AND vij.virTpoInternacion BETWEEN valDesde AND valHasta), 0) AS clrInternacion, "

            Else

                SQL = SQL & "0 AS clrDespacho, 0 AS clrSalida, 0 AS clrDesplazamiento, 0 AS clrAtencion, "
                SQL = SQL & "0 AS virTpoDerivacion, 0 AS virTpoInternacion, "
                SQL = SQL & "0 AS clrDerivacion, 0 AS clrInternacion, "

            End If

            If Not pVisCpl Then
                SQL = SQL & "'' AS DomicilioDestino, '' AS LocalidadDestino, '' AS ZonaDestinoColor "
            Else
                SQL = SQL & "ISNULL(CASE vij.ViajeId WHEN 'IDA' THEN (SELECT domder.Domicilio FROM IncidentesDomicilios domder WHERE domder.IncidenteId = dom.IncidenteId AND domder.TipoDomicilio = 1) "
                SQL = SQL & "ELSE (SELECT domder.Domicilio FROM IncidentesDomicilios domder WHERE domder.IncidenteId = dom.IncidenteId AND domder.TipoDomicilio = 0) END, '') AS DomicilioDestino, "
                SQL = SQL & "ISNULL(CASE vij.ViajeId WHEN 'IDA' THEN (SELECT locder.AbreviaturaId FROM IncidentesDomicilios domder INNER JOIN Localidades locder ON domder.LocalidadId = locder.ID WHERE domder.IncidenteId = dom.IncidenteId AND domder.TipoDomicilio = 1) "
                SQL = SQL & "ELSE (SELECT locder.AbreviaturaId FROM IncidentesDomicilios domder INNER JOIN Localidades locder ON domder.LocalidadId = locder.ID WHERE domder.IncidenteId = dom.IncidenteId AND domder.TipoDomicilio = 0) END, '') AS LocalidadDestino, "
                SQL = SQL & "ISNULL(CASE vij.ViajeId WHEN 'IDA' THEN (SELECT zonder.ColorHexa FROM IncidentesDomicilios domder INNER JOIN Localidades locder ON domder.LocalidadId = locder.ID INNER JOIN ZonasGeograficas zonder ON locder.ZonaGeograficaId = zonder.ID WHERE domder.IncidenteId = dom.IncidenteId AND domder.TipoDomicilio = 1) "
                SQL = SQL & "ELSE (SELECT zonder.ColorHexa FROM IncidentesDomicilios domder INNER JOIN Localidades locder ON domder.LocalidadId = locder.ID INNER JOIN ZonasGeograficas zonder ON locder.ZonaGeograficaId = zonder.ID WHERE domder.IncidenteId = dom.IncidenteId AND domder.TipoDomicilio = 1) END, '') AS ZonaDestinoColor "
            End If

            SQL = SQL & "FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "INNER JOIN Incidentes inc ON (dom.IncidenteId = inc.ID) "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
            SQL = SQL & "INNER JOIN Clientes cli ON (inc.ClienteId = cli.ID) "
            SQL = SQL & "INNER JOIN Localidades loc ON (dom.LocalidadId = loc.ID) "
            SQL = SQL & "INNER JOIN ZonasGeograficas zon ON (loc.ZonaGeograficaId = zon.ID) "
            SQL = SQL & "LEFT JOIN Moviles mov ON (vij.MovilId = mov.ID) "
            SQL = SQL & "LEFT JOIN Moviles pre ON (vij.MovilPreasignadoId = pre.ID) "
            SQL = SQL & "LEFT JOIN Sanatorios san ON (dom.SanatorioId = san.ID) "

            If pScrOpe <> scrOperativa.Programados Then
                SQL = SQL & "WHERE (vij.flgStatus = 0) "
            Else
                SQL = SQL & "WHERE (vij.flgStatus = 2) "
            End If
            SQL = SQL & "AND (gdo.ClasificacionId BETWEEN 0 AND 49) "

            If pScrOpe = scrOperativa.Pendientes Then
                SQL = SQL & "AND (mov.Movil IS NULL) "
            End If
            If pScrOpe = scrOperativa.Asignados Then
                SQL = SQL & "AND (mov.Movil IS NOT NULL) "
            End If

            If Not pChkZon Is Nothing Then

                If pChkZon.Count > 0 Then
                    SQL = SQL & "AND ("
                    For vIdx = 1 To pChkZon.Count
                        If vIdx = 1 Then
                            SQL = SQL & "(loc.ZonaGeograficaId = " & pChkZon.Item(vIdx) & ")"
                        Else
                            SQL = SQL & "OR (loc.ZonaGeograficaId = " & pChkZon.Item(vIdx) & ")"
                        End If
                    Next vIdx
                    SQL = SQL & ")"
                End If

            End If

            If pScrOpe <> scrOperativa.Programados Then
                SQL = SQL & " ORDER BY gdo.Orden, vij.ViajeId"
            Else
                SQL = SQL & " ORDER BY vij.reqHorLlegada, gdo.Orden, vij.ViajeId"
            End If

            Dim cmdOpe As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vIncTbl As New DataTable
            Dim vIncIdx As Integer = 0
            vIncTbl.Load(cmdOpe.ExecuteReader)

            '-------> Aplico seguridad de perfiles
            SetPerfiles(vIncTbl, pSelPrf)

            GetOperativaDX = vIncTbl

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetOperativaDX", ex)
        End Try

    End Function

    Public Function GetOperativaFoot(Optional pScrOpe As scrOperativa = scrOperativa.EnCurso) As DataTable

        GetOperativaFoot = Nothing

        Try
            Dim SQL As String
            Dim vIdx As Integer = 0

            SQL = "SELECT gdo.VisualColor, gdo.AbreviaturaId, gdo.Orden, ISNULL(COUNT(inc.ID), 0) "
            SQL = SQL & "FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "INNER JOIN Incidentes inc ON (dom.IncidenteId = inc.ID) "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
            SQL = SQL & "INNER JOIN Clientes cli ON (inc.ClienteId = cli.ID) "
            SQL = SQL & "INNER JOIN Localidades loc ON (dom.LocalidadId = loc.ID) "
            SQL = SQL & "INNER JOIN ZonasGeograficas zon ON (loc.ZonaGeograficaId = zon.ID) "
            SQL = SQL & "LEFT JOIN Moviles mov ON (vij.MovilId = mov.ID) "
            SQL = SQL & "LEFT JOIN Moviles pre ON (vij.MovilPreasignadoId = pre.ID) "
            SQL = SQL & "LEFT JOIN Sanatorios san ON (dom.SanatorioId = san.ID) "

            If pScrOpe <> scrOperativa.Programados Then
                SQL = SQL & "WHERE (vij.flgStatus = 0) "
            Else
                SQL = SQL & "WHERE (vij.flgStatus = 2) "
            End If
            SQL = SQL & "AND (gdo.ClasificacionId BETWEEN 0 AND 49) "

            SQL = SQL & "GROUP BY gdo.VisualColor, gdo.AbreviaturaId, gdo.Orden "
            SQL = SQL & "ORDER BY gdo.Orden"

            Dim cmdFoo As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vFooTbl As New DataTable
            Dim vFooIdx As Integer = 0

            vFooTbl.Load(cmdFoo.ExecuteReader)

            GetOperativaFoot = vFooTbl

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetOperativaFoot", ex)
        End Try

    End Function

    Public Function GetChartCantidades(ByVal pFec As Date) As DataTable

        GetChartCantidades = Nothing

        Try

            '-----> Armo Data Table
            Dim dtTabla As DataTable = makeChartCantidadesDataView()
            Dim dtRegistro As DataRow
            Dim SQL As String

            SQL = "SELECT gdo.Descripcion, gdo.Orden, gdo.ColorHexa, ISNULL(COUNT(vij.ID), 0) "
            SQL = SQL & "FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "INNER JOIN Incidentes inc ON (dom.IncidenteId = inc.ID) "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
            SQL = SQL & "WHERE (inc.FecIncidente = '" & DateToSql(pFec) & "') "
            SQL = SQL & "GROUP BY gdo.Descripcion, gdo.ColorHexa, gdo.Orden "
            SQL = SQL & "HAVING COUNT(vij.ID) > 0"
            SQL = SQL & "ORDER BY gdo.Orden"

            Dim cmdTot As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vTotTbl As New DataTable
            Dim vTotIdx As Integer = 0

            vTotTbl.Load(cmdTot.ExecuteReader)

            For vTotIdx = 0 To vTotTbl.Rows.Count - 1

                dtRegistro = dtTabla.NewRow
                dtRegistro("Descripcion") = vTotTbl.Rows(vTotIdx).Item(0)
                dtRegistro("ColorHexa") = vTotTbl.Rows(vTotIdx).Item(2)
                dtRegistro("Cantidad") = vTotTbl.Rows(vTotIdx).Item(3)
                dtTabla.Rows.Add(dtRegistro)

            Next vTotIdx

            '-----> Paso a DataView
            GetChartCantidades = dtTabla

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetChartCantidades", ex)
        End Try

    End Function

    Public Function GetChartTiempos(ByVal pFec As Date) As DataTable

        GetChartTiempos = Nothing

        Try

            '-----> Armo Data Table
            Dim dtTabla As DataTable = makeChartTiemposDataView()
            Dim SQL As String
            Dim dtRegistro As DataRow

            SQL = "SELECT gdo.Descripcion, gdo.Orden, gdo.ColorHexa, ISNULL(AVG(ISNULL(vij.virTpoDespacho, 0)), 0), "
            SQL = SQL & "ISNULL(AVG(ISNULL(vij.virTpoSalida, 0)), 0), ISNULL(AVG(ISNULL(vij.virTpoDesplazamiento, 0)), 0), "
            SQL = SQL & "ISNULL(AVG(ISNULL(vij.virTpoAtencion, 0)), 0) "
            SQL = SQL & "FROM Incidentes inc "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (inc.ID = dom.IncidenteId) "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON (dom.ID = vij.IncidenteDomicilioId) "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
            SQL = SQL & "WHERE (inc.FecIncidente = '" & DateToSql(pFec) & "') AND (gdo.ClasificacionId = 0) "
            SQL = SQL & "GROUP BY gdo.Descripcion, gdo.Orden, gdo.ColorHexa "
            SQL = SQL & "ORDER BY gdo.Orden"

            Dim cmdTot As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vTotTbl As New DataTable
            Dim vTotIdx As Integer = 0

            vTotTbl.Load(cmdTot.ExecuteReader)

            For vTotIdx = 0 To vTotTbl.Rows.Count - 1

                dtRegistro = dtTabla.NewRow
                dtRegistro("Descripcion") = vTotTbl.Rows(vTotIdx).Item(0)
                dtRegistro("ColorHexa") = vTotTbl.Rows(vTotIdx).Item(2)
                dtRegistro("Despacho") = vTotTbl.Rows(vTotIdx).Item(3)
                dtRegistro("Salida") = vTotTbl.Rows(vTotIdx).Item(4)
                dtRegistro("Desplazamiento") = vTotTbl.Rows(vTotIdx).Item(5)
                dtRegistro("Atencion") = vTotTbl.Rows(vTotIdx).Item(6)
                dtTabla.Rows.Add(dtRegistro)

            Next vTotIdx

            '-----> Paso a DataView
            GetChartTiempos = dtTabla

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetChartTiempos", ex)
        End Try

    End Function

    Public Sub SetPerfiles(ByRef dt As DataTable, Optional pSelPrf As Int64 = 0)
        Try

            If CBool(shamanConfig.flgDespachoPerfiles) Then

                Dim vDespacha As Boolean = False

                If pSelPrf > 0 Then

                    Dim objPerfil As New conPerfiles

                    If objPerfil.Abrir(pSelPrf) Then

                        If CBool(objPerfil.flgDespacha) And Not CBool(objPerfil.flgAdministrador) Then
                            vDespacha = True
                        End If

                    End If

                    objPerfil = Nothing

                Else

                    pSelPrf = logPerfilId
                    vDespacha = logDespacha

                End If

                '----> Verifico si es de mi grado...

                If vDespacha Then

                    Dim vIdx As Integer = 0

                    Do Until vIdx = dt.Rows.Count

                        Dim SQL As String

                        SQL = "SELECT cab.ID FROM Perfiles cab "
                        SQL = SQL & "INNER JOIN PerfilesGradosOperativos gdo ON (cab.ID = gdo.PerfilId) "
                        SQL = SQL & "INNER JOIN PerfilesZonasGeograficas zon ON (cab.ID = zon.PerfilId) "
                        SQL = SQL & "WHERE (cab.ID = " & pSelPrf & ") AND (gdo.GradoOperativoId = " & dt(vIdx)("GradoOperativoId") & ") "
                        SQL = SQL & "AND (zon.ZonaGeograficaId = " & dt(vIdx)("ZonaGeograficaId") & ") "

                        Dim cmdPrf As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                        Dim vRowId As String = CType(cmdPrf.ExecuteScalar, String)
                        If vRowId Is Nothing Then
                            dt.Rows.RemoveAt(vIdx)
                        Else
                            vIdx = vIdx + 1
                        End If
                    Loop

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPerfiles", ex)
        End Try
    End Sub

    Private Function makeChartCantidadesDataView() As DataTable
        makeChartCantidadesDataView = Nothing

        Try
            '-----> Armo Data Table
            Dim dtTabla As New DataTable
            Dim dtColumna As DataColumn
            Dim dtKey(1) As DataColumn

            dtColumna = New DataColumn("Descripcion", GetType(String))
            dtTabla.Columns.Add(dtColumna)
            dtKey(0) = dtColumna
            dtTabla.PrimaryKey = dtKey
            dtColumna = New DataColumn("ColorHexa", GetType(String))
            dtTabla.Columns.Add(dtColumna)
            dtColumna = New DataColumn("Cantidad", GetType(Double))
            dtTabla.Columns.Add(dtColumna)

            makeChartCantidadesDataView = dtTabla

        Catch ex As Exception
            HandleError(Me.GetType.Name, "makeChartCantidadesDataView", ex)
        End Try

    End Function

    Private Function makeChartTiemposDataView() As DataTable

        makeChartTiemposDataView = Nothing

        Try
            '-----> Armo Data Table
            Dim dtTabla As New DataTable
            Dim dtColumna As DataColumn
            Dim dtKey(1) As DataColumn

            dtColumna = New DataColumn("Descripcion", GetType(String))
            dtTabla.Columns.Add(dtColumna)
            dtKey(0) = dtColumna
            dtTabla.PrimaryKey = dtKey
            dtColumna = New DataColumn("ColorHexa", GetType(String))
            dtTabla.Columns.Add(dtColumna)
            dtColumna = New DataColumn("Despacho", GetType(Double))
            dtTabla.Columns.Add(dtColumna)
            dtColumna = New DataColumn("Salida", GetType(Double))
            dtTabla.Columns.Add(dtColumna)
            dtColumna = New DataColumn("Desplazamiento", GetType(Double))
            dtTabla.Columns.Add(dtColumna)
            dtColumna = New DataColumn("Atencion", GetType(Double))
            dtTabla.Columns.Add(dtColumna)

            makeChartTiemposDataView = dtTabla

        Catch ex As Exception
            HandleError(Me.GetType.Name, "makeChartTiemposDataView", ex)
        End Try

    End Function


    Public Overrides Function CanDelete(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean
        CanDelete = False

        Try
            Dim SQL As String

            SQL = "SELECT mov.Movil FROM IncidentesSucesos suc "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON (suc.IncidenteViajeId = vij.ID) "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "INNER JOIN Moviles mov ON (suc.MovilId = mov.ID) "
            SQL = SQL & "WHERE (dom.IncidenteId = " & pId & ") "

            Dim cmAli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vVal As String = cmAli.ExecuteScalar

            If Not vVal Is Nothing Then
                If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "El incidente ha sido despachado al móvil " & vVal, MsgBoxStyle.Critical, "Shaman")
            Else
                CanDelete = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CanDelete", ex)
        End Try
    End Function

    Public Function ValidarIncidente(ByVal pFec As Date, ByVal pNic As String, ByVal pCliAbr As String, ByVal pCli As Int64, ByVal pAfl As String, ByVal pGdo As Int64, ByVal pDom As String, ByVal pLoc As Int64, ByVal pPac As String, ByRef pAddPac As Boolean, Optional ByVal pClf As gdoClasificacion = gdoClasificacion.gdoIncidente, Optional ByVal pDerDom As String = "", Optional ByVal pDerLoc As Int64 = 0, Optional ByVal pIncTiempoCarga As incTiempoCarga = incTiempoCarga.Presente, Optional pPla As Int64 = 0) As Boolean
        ValidarIncidente = False
        Try
            pAddPac = False
            Dim vMsgErr As String = ""
            Dim vValAfl As Boolean = True

            If pCli = 0 Then
                vMsgErr = "Debe establecer el código de cliente"
            ElseIf pAfl <> "" Then
                Dim objIntegrante As New conClientesIntegrantes
                If objIntegrante.GetIDByNroAfiliado(pCli, pAfl) = 0 Then vValAfl = False
            Else
                vValAfl = False
            End If
            If Not vValAfl And pCli <> shamanConfig.ClienteDefaultId.ID Then
                If MsgBox("El afiliado no se encuentra en padrón de " & pCliAbr & vbCrLf & "Confirma ?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Afiliados") = MsgBoxResult.No Then
                    Exit Function
                Else
                    pAddPac = True
                End If
            End If
            If vMsgErr = "" Then

                If pClf = gdoClasificacion.gdoIntDomiciliaria And shamanConfig.flgCargaHistorica = 0 And pFec < Now.Date Then
                    vMsgErr = "La fecha del servicio no puede ser inferior al día de hoy"
                End If

                If pGdo = 0 Then vMsgErr = "Debe establecer el grado operativo del servicio"

                If pDom = "" And vMsgErr = "" Then vMsgErr = "Debe establecer el domicilio del servicio"
                If pLoc = 0 And vMsgErr = "" Then vMsgErr = "Debe establecer la localidad del domicilio del servicio"
                If pPac = "" And vMsgErr = "" Then vMsgErr = "Debe establecer el nombre del paciente"
            End If
            If vMsgErr = "" And pClf = gdoClasificacion.gdoTraslado Then
                If pDerDom = "" Then vMsgErr = "Debe establecer el domicilio de destino"
                If pDerLoc = 0 And vMsgErr = "" Then vMsgErr = "Debe establecer la localidad del domicilio de destino"
            End If
            '---> Valido Morosidad
            If vMsgErr = "" And shamanConfig.modMorosidad = 2 Then
                Dim objClientes As New conClientes
                vMsgErr = objClientes.GetEstadoMorosidad(pCli)
                objClientes = Nothing
            End If
            '---> Valido Cobertura
            If vMsgErr = "" Then
                Me.flgCubierto = 1
                If shamanConfig.modSinCobertura = 2 Then
                    HaveCoberturaGrado(pCli, pGdo, 0, vMsgErr, False, pClf, pPla)
                ElseIf shamanConfig.modSinCobertura = 1 Then
                    HaveCoberturaGrado(pCli, pGdo, 0, vMsgErr, False, pClf, pPla)
                    If vMsgErr <> "" Then
                        If MsgBox(vMsgErr & vbCrLf & "¿ Desea tomar el servicio de todas maneras ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2, "Aprobación") = MsgBoxResult.Yes Then
                            vMsgErr = ""
                            Me.flgCubierto = 0
                        Else
                            Exit Function
                        End If
                    End If
                End If
            End If

            If vMsgErr <> "" Then
                MsgBox(vMsgErr, MsgBoxStyle.Critical, "Recepción")
            Else
                ValidarIncidente = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "ValidarIncidente", ex)
        End Try
    End Function

    Public Function SetIncidente(ByVal objIncidente As conIncidentes, ByVal objDomicilio As conIncidentesDomicilios, ByVal objObservacion As conIncidentesObservaciones, ByVal pFhr As DateTime, Optional ByVal pDig As Int64 = 0, Optional ByVal pMot As Int64 = 0, Optional ByVal pIncTiempoCarga As incTiempoCarga = incTiempoCarga.Presente) As Boolean

        Dim cnnKey As String = "Incidentes"

        SetIncidente = False

        Try
            Dim vSav As Boolean = False, objViaje As New conIncidentesViajes(cnnKey)
            Dim objLock As New conlckIncidentes(cnnKey)

            If shamanStartUp.AbrirConexion(cnnKey) Then
                objIncidente.myCnnName = cnnKey

                cnnsTransNET.Add(cnnKey, cnnsNET(cnnKey).BeginTransaction)

                If objIncidente.NroIncidente = "" Then
                    objIncidente.NroIncidente = objLock.getNewIncidente(objIncidente.FecIncidente)
                    If shamanConfig.modNumeracion = 1 Then objIncidente.TrasladoId = Val(objIncidente.NroIncidente)
                End If

                If objIncidente.Salvar(objIncidente) Then

                    vSav = True
                    '----> Verifico Observaciones
                    If objObservacion.Observaciones <> "" Then
                        objObservacion.myCnnName = cnnKey
                        objObservacion.IncidenteId.SetObjectId(objIncidente.ID)
                        If objObservacion.Observaciones.Length >= 3 Then
                            If objObservacion.Observaciones.Substring(0, 3) = "RCL" Then
                                objObservacion.flgReclamo = 1
                            End If
                        End If
                        vSav = objObservacion.Salvar(objObservacion)
                        If vSav Then
                            '----> Resconstruyo Observaciones
                            objIncidente.Observaciones = objObservacion.GetByIncidenteId(objIncidente.ID)
                            vSav = objIncidente.Salvar(objIncidente)
                        End If
                    End If

                    If vSav Then
                        vSav = False
                        '----> Verifico Domicilio
                        objDomicilio.myCnnName = cnnKey
                        objDomicilio.ID = objDomicilio.GetIDByIndex(objIncidente.ID)
                        '----> Establezco cabecera domicilio
                        objDomicilio.IncidenteId.SetObjectId(objIncidente.ID)
                        '----> Actualizo Domicilio
                        If objDomicilio.Salvar(objDomicilio) Then

                            If Not objViaje.Abrir(objViaje.GetIDByIndex(objDomicilio.IncidenteId.ID)) Then
                                '----> Agrego el Viaje
                                objViaje.CleanProperties(objViaje)
                                objViaje.IncidenteDomicilioId.SetObjectId(objDomicilio.ID)
                                objViaje.ViajeId = "IDA"
                                If pFhr = NullDateTime Then
                                    Select Case pIncTiempoCarga
                                        Case incTiempoCarga.Presente
                                            objViaje.flgStatus = 0
                                        Case incTiempoCarga.Historico
                                            objViaje.flgStatus = 1
                                            If pDig > 0 Or pMot > 0 Then
                                                objViaje.DiagnosticoId.SetObjectId(pDig)
                                                objViaje.MotivoNoRealizacionId.SetObjectId(pMot)
                                            End If
                                    End Select
                                Else
                                    Select Case pIncTiempoCarga
                                        Case incTiempoCarga.Presente, incTiempoCarga.Programado
                                            objViaje.flgStatus = 2
                                            objViaje.reqHorLlegada = pFhr
                                        Case incTiempoCarga.Historico
                                            objViaje.flgStatus = 1
                                            If pDig > 0 Or pMot > 0 Then
                                                objViaje.DiagnosticoId.SetObjectId(pDig)
                                                objViaje.MotivoNoRealizacionId.SetObjectId(pMot)
                                            End If
                                    End Select
                                End If

                                If objViaje.Salvar(objViaje) Then

                                    '----> Agrego los sucesos de horarios
                                    Dim objSuceso As New conIncidentesSucesos(cnnKey)
                                    Dim objSucesoId As New conSucesosIncidentes(cnnKey)

                                    objSuceso.CleanProperties(objSuceso)
                                    objSuceso.IncidenteViajeId.SetObjectId(objViaje.ID)

                                    Select Case pIncTiempoCarga
                                        Case incTiempoCarga.Presente
                                            objSuceso.FechaHoraSuceso = GetCurrentTime()
                                        Case incTiempoCarga.Historico, incTiempoCarga.Programado
                                            objSuceso.FechaHoraSuceso = pFhr
                                    End Select

                                    objSuceso.SucesoIncidenteId.SetObjectId(objSucesoId.GetIDByAbreviaturaId("G"))
                                    vSav = objSuceso.addSuceso(objSuceso, , , , True)

                                    objSuceso = Nothing
                                    objSucesoId = Nothing

                                Else
                                    vSav = False
                                End If
                                objViaje = Nothing
                            Else
                                If pDig > 0 Or pMot > 0 Then

                                    objViaje.DiagnosticoId.SetObjectId(pDig)
                                    objViaje.MotivoNoRealizacionId.SetObjectId(pMot)
                                    vSav = objViaje.Salvar(objViaje)

                                ElseIf pIncTiempoCarga <> incTiempoCarga.Presente And objViaje.reqHorLlegada <> pFhr Then

                                    objViaje.reqHorLlegada = pFhr

                                    If objViaje.Salvar(objViaje) Then

                                        Dim objSuceso As New conIncidentesSucesos(cnnKey)
                                        Dim objSucesoId As New conSucesosIncidentes(cnnKey)

                                        If objSuceso.Abrir(objSuceso.GetIDBySuceso(objViaje.ID, objSucesoId.GetIDByAbreviaturaId("G"))) Then

                                            objSuceso.FechaHoraSuceso = pFhr
                                            vSav = objSuceso.Salvar(objSuceso)

                                        End If

                                    End If
                                Else
                                    vSav = True
                                End If
                            End If
                        End If
                    End If
                End If
                '----> Síntomas
                If vSav And objIncidente.GradoOperativoId.ClasificacionId <> gdoClasificacion.gdoIntDomiciliaria Then
                    SetSintomas(objIncidente.ID, cnnKey)
                End If

                If vSav Then
                    cnnsTransNET(cnnKey).Commit()
                Else
                    cnnsTransNET(cnnKey).Rollback()
                End If
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)

                '----> Restauro Valores
                objIncidente.myCnnName = cnnDefault
                objObservacion.myCnnName = cnnDefault
                objDomicilio.myCnnName = cnnDefault
                '----> Resultado
                SetIncidente = vSav
            End If

        Catch ex As Exception

            If cnnsNET.Contains(cnnKey) Then
                cnnsTransNET(cnnKey).Rollback()
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)
            End If
            '----> Restauro Valores
            objIncidente.myCnnName = cnnDefault
            objObservacion.myCnnName = cnnDefault
            objDomicilio.myCnnName = cnnDefault

            HandleError(Me.GetType.Name, "SetIncidente", ex)

        End Try

    End Function

    Public Function SetSintomas(ByVal pInc As Int64, ByVal pcnnKey As String, Optional pLst As Boolean = False) As Boolean

        SetSintomas = False

        Try

            Dim SQL As String
            Dim objIncidenteSintoma As New typIncidentesSintomas(pcnnKey)
            Dim objIncidenteSintomaPregs As New typIncidentesSintomasPregs(pcnnKey)
            Dim vSav As Boolean = True

            SQL = "SELECT ID, SintomaId, Descripcion, GradoOperativoId, regUsuarioId, regFechaHora, regTerminalId "
            SQL = SQL & "FROM tmpIncidentesSintomas WHERE PID = " & logPID
            If Not pLst Then
                SQL = SQL & " ORDER BY regFechaHora"
            Else
                SQL = SQL & " ORDER BY regFechaHora DESC"
            End If

            Dim cmdTmp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vTmpTbl As New DataTable
            Dim vTmpIdx As Integer = 0

            vTmpTbl.Load(cmdTmp.ExecuteReader)

            For vTmpIdx = 0 To vTmpTbl.Rows.Count - 1

                objIncidenteSintoma.CleanProperties(objIncidenteSintoma)

                objIncidenteSintoma.IncidenteId.SetObjectId(pInc)
                objIncidenteSintoma.SintomaId.SetObjectId(vTmpTbl(vTmpIdx).Item(1))
                objIncidenteSintoma.Descripcion = vTmpTbl(vTmpIdx).Item(2)
                objIncidenteSintoma.GradoOperativoId.SetObjectId(vTmpTbl(vTmpIdx).Item(3))
                objIncidenteSintoma.regUsuarioId.SetObjectId(vTmpTbl(vTmpIdx).Item(4))
                objIncidenteSintoma.regFechaHora = vTmpTbl(vTmpIdx).Item(5)
                objIncidenteSintoma.regTerminalId.SetObjectId(vTmpTbl(vTmpIdx).Item(6))

                If objIncidenteSintoma.Salvar(objIncidenteSintoma, False) Then

                    SQL = "SELECT tmp.SintomaPreguntaId, tmp.PreguntaId, tmp.TipoFrase, tmp.Frase, tmp.Respuesta, tmp.Puntaje, tmp.regUsuarioId, tmp.regFechaHora, tmp.regTerminalId, "
                    SQL = SQL & "ISNULL(prg.flgPediatrico, 0) AS flgPediatrico "
                    SQL = SQL & "FROM tmpIncidentesSintomasPregs tmp "
                    SQL = SQL & "LEFT JOIN SintomasPreguntas prg ON tmp.SintomaPreguntaId = prg.ID "
                    SQL = SQL & "WHERE tmp.tmpIncidenteSintomaId = " & vTmpTbl(vTmpIdx).Item(0) & " ORDER BY tmp.PreguntaId"

                    Dim cmdDet As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    Dim vDetTbl As New DataTable
                    Dim vDetIdx As Integer = 0

                    vDetTbl.Load(cmdDet.ExecuteReader)

                    For vDetIdx = 0 To vDetTbl.Rows.Count - 1

                        objIncidenteSintomaPregs.CleanProperties(objIncidenteSintomaPregs)
                        objIncidenteSintomaPregs.IncidenteSintomaId.SetObjectId(objIncidenteSintoma.ID)
                        objIncidenteSintomaPregs.SintomaPreguntaId.SetObjectId(vDetTbl(vDetIdx).Item(0))
                        objIncidenteSintomaPregs.flgPediatrico = vDetTbl(vDetIdx).Item(9)
                        objIncidenteSintomaPregs.PreguntaId = vDetTbl(vDetIdx).Item(1)
                        objIncidenteSintomaPregs.TipoFrase = vDetTbl(vDetIdx).Item(2)
                        objIncidenteSintomaPregs.Frase = vDetTbl(vDetIdx).Item(3)
                        objIncidenteSintomaPregs.Respuesta = vDetTbl(vDetIdx).Item(4)
                        objIncidenteSintomaPregs.Puntaje = vDetTbl(vDetIdx).Item(5)
                        objIncidenteSintomaPregs.regUsuarioId.SetObjectId(vDetTbl(vDetIdx).Item(6))
                        objIncidenteSintomaPregs.regFechaHora = vDetTbl(vDetIdx).Item(7)
                        objIncidenteSintomaPregs.regTerminalId.SetObjectId(vDetTbl(vDetIdx).Item(8))

                        If Not objIncidenteSintomaPregs.Salvar(objIncidenteSintomaPregs, False) Then
                            vSav = False
                        End If

                    Next vDetIdx

                Else

                    vSav = False

                End If

            Next vTmpIdx

            SetSintomas = vSav

        Catch ex As Exception

            HandleError(Me.GetType.Name, "SetSintomas", ex)

        End Try

    End Function

    Public Function ValidarTraslado(ByVal pGdo As Double, ByVal pIdv As Boolean, ByVal pFhrOri As String, ByVal pFhrDst As String, ByVal pFhrRet As String, Optional ByVal pInc As Int64 = 0, Optional pTpoCarga As incTiempoCarga = modDeclares.incTiempoCarga.Programado) As Boolean
        ValidarTraslado = False
        Try
            Dim vMsgErr As String = ""
            If pGdo = 0 Then vMsgErr = "Debe establecer el grado operativo del traslado"
            If vMsgErr = "" Then
                If pTpoCarga = incTiempoCarga.Programado And CDate(pFhrOri).Date < Now.Date And Me.ID = 0 Then
                    vMsgErr = "La fecha del traslado no puede ser inferior al día de hoy"
                End If
                If vMsgErr = "" Then
                    If CDate(pFhrOri) > CDate(pFhrDst) Then vMsgErr = "La fecha/hora de llegada a destino debe ser inferior a la fecha/hora de llegada a origen"
                    If vMsgErr = "" And pIdv Then
                        If CDate(pFhrDst) > CDate(pFhrRet) Then vMsgErr = "La fecha/hora de retorno debe ser inferior a la fecha/hora de llegada a destino en la ida"
                    End If
                End If
            End If
            If vMsgErr = "" And Not pIdv Then

                '-----> Verifico si había un móvil ligado al retorno...
                Dim SQL As String

                SQL = "SELECT mov.Movil FROM IncidentesSucesos suc "
                SQL = SQL & "INNER JOIN IncidentesViajes vij ON (suc.IncidenteViajeId = vij.ID) "
                SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
                SQL = SQL & "INNER JOIN Moviles mov ON (suc.MovilId = mov.ID) "
                SQL = SQL & "INNER JOIN SucesosIncidentes sus ON (suc.SucesoIncidenteId = sus.ID) "
                SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ") AND (vij.ViajeId = 'VUE') AND (sus.AbreviaturaId = 'A') "

                Dim cmdRetorno As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vMovRet As String = CType(cmdRetorno.ExecuteScalar, String)
                If Not vMovRet Is Nothing Then
                    vMsgErr = "No se puede quitar el retorno" & vbCrLf & "El móvil " & vMovRet & " se encuentra ligado al mismo"
                End If

            End If

            If vMsgErr <> "" Then
                MsgBox(vMsgErr, MsgBoxStyle.Critical, "Traslado")
            Else
                ValidarTraslado = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "ValidarTraslado", ex)
        End Try
    End Function

    Public Function SetTraslado(ByVal objIncidente As conIncidentes, ByVal objDomicilio As conIncidentesDomicilios, ByVal objDstDomicilio As conIncidentesDomicilios, ByVal objAn1Domicilio As conIncidentesDomicilios, ByVal objAn2Domicilio As conIncidentesDomicilios, ByVal objObservacion As conIncidentesObservaciones, ByVal pFhrOri As String, ByVal pFhrDst As String, ByVal pFhrRet As String, ByVal pDig As Int64, Optional ByVal pIncTiempoCarga As incTiempoCarga = incTiempoCarga.Presente) As Boolean

        Dim cnnKey As String = "Traslados"
        SetTraslado = False

        Try
            Dim vSav As Boolean = False, vReg As Boolean = False
            Dim objViaje As New conIncidentesViajes(cnnKey)

            If shamanStartUp.AbrirConexion(cnnKey) Then
                objIncidente.myCnnName = cnnKey
                cnnsTransNET.Add(cnnKey, cnnsNET(cnnKey).BeginTransaction)

                If pIncTiempoCarga = incTiempoCarga.Historico Then
                    If objIncidente.NroIncidente = "" Then
                        Dim objLock As New conlckIncidentes(cnnKey)
                        If shamanConfig.modNumeracion = 1 Then
                            objIncidente.NroIncidente = objIncidente.TrasladoId
                        Else
                            objIncidente.NroIncidente = objLock.getNewIncidente(objIncidente.FecIncidente)
                        End If
                        objLock = Nothing
                    End If
                End If

                If objIncidente.Salvar(objIncidente) Then
                    vSav = True
                    '----> Verifico Observaciones
                    If objObservacion.Observaciones <> "" Then
                        objObservacion.myCnnName = cnnKey
                        objObservacion.IncidenteId.SetObjectId(objIncidente.ID)
                        If objObservacion.Observaciones.Length >= 3 Then
                            If objObservacion.Observaciones.Substring(0, 3) = "RCL" Then
                                objObservacion.flgReclamo = 1
                            End If
                        End If
                        vSav = objObservacion.Salvar(objObservacion)
                        If vSav Then
                            '----> Resconstruyo Observaciones
                            objIncidente.Observaciones = objObservacion.GetByIncidenteId(objIncidente.ID)
                            vSav = objIncidente.Salvar(objIncidente)
                        End If
                    End If
                    If vSav Then
                        vSav = False
                        objDomicilio.myCnnName = cnnKey
                        '----> Verifico Domicilio Origen
                        objDomicilio.ID = objDomicilio.GetIDByIndex(objIncidente.ID)
                        objDomicilio.IncidenteId.SetObjectId(objIncidente.ID)
                        objDstDomicilio.TipoDomicilio = 0
                        '----> Actualizo Domicilio Origen
                        If objDomicilio.Salvar(objDomicilio) Then
                            objDstDomicilio.myCnnName = cnnKey
                            '----> Verifico Domicilio Destino
                            objDstDomicilio.ID = objDstDomicilio.GetIDByIndex(objIncidente.ID, 1)
                            objDstDomicilio.IncidenteId.SetObjectId(objIncidente.ID)
                            objDstDomicilio.TipoDomicilio = 1
                            '----> Actualizo Domicilio Destino
                            If objDstDomicilio.Salvar(objDstDomicilio) Then
                                '------> Anexo I
                                If objAn1Domicilio.LocalidadId.GetObjectId = 0 Then
                                    If objAn1Domicilio.Eliminar(objAn1Domicilio.GetIDByIndex(objIncidente.ID, 2, 1)) Then
                                    End If
                                Else
                                    '----> Actualizo Domicilio Anexo 1
                                    objAn1Domicilio.myCnnName = cnnKey
                                    '----> Verifico Domicilio Destino
                                    objAn1Domicilio.ID = objAn1Domicilio.GetIDByIndex(objIncidente.ID, 2, 1)
                                    objAn1Domicilio.IncidenteId.SetObjectId(objIncidente.ID)
                                    objAn1Domicilio.TipoDomicilio = 2
                                    objAn1Domicilio.NroAnexo = 1
                                    If objAn1Domicilio.Salvar(objAn1Domicilio) Then
                                    End If
                                End If
                                '------> Anexo II
                                If objAn2Domicilio.LocalidadId.GetObjectId = 0 Then
                                    If objAn2Domicilio.Eliminar(objAn2Domicilio.GetIDByIndex(objIncidente.ID, 2, 2)) Then
                                    End If
                                Else
                                    '----> Actualizo Domicilio Anexo 1
                                    objAn2Domicilio.myCnnName = cnnKey
                                    '----> Verifico Domicilio Destino
                                    objAn2Domicilio.ID = objAn2Domicilio.GetIDByIndex(objIncidente.ID, 2, 2)
                                    objAn2Domicilio.IncidenteId.SetObjectId(objIncidente.ID)
                                    objAn2Domicilio.TipoDomicilio = 2
                                    objAn2Domicilio.NroAnexo = 2
                                    If objAn2Domicilio.Salvar(objAn2Domicilio) Then
                                    End If
                                End If
                                '----> Chequeo Viaje de IDA
                                If Not objViaje.Abrir(objViaje.GetIDByIndex(objDomicilio.IncidenteId.ID)) Then
                                    '----> Agrego el Viaje
                                    objViaje.CleanProperties(objViaje)
                                    objViaje.IncidenteDomicilioId.SetObjectId(objDomicilio.ID)
                                    objViaje.ViajeId = "IDA"
                                    If pIncTiempoCarga = incTiempoCarga.Programado Then
                                        objViaje.flgStatus = 2
                                    Else
                                        objViaje.flgStatus = 1
                                    End If
                                Else
                                    If objViaje.flgStatus = 0 Then
                                        objViaje.HorarioOperativo.horLlamada = pFhrOri
                                    End If
                                End If
                                objViaje.DiagnosticoId.SetObjectId(pDig)
                                objViaje.reqHorLlegada = pFhrOri
                                objViaje.reqHorInternacion = pFhrDst
                                '-----> Salvo el Viaje
                                If objViaje.Salvar(objViaje) Then
                                    '----> Agrego los sucesos de horarios
                                    Dim objSuceso As New conIncidentesSucesos(cnnKey)
                                    Dim objSucesoId As New conSucesosIncidentes(cnnKey)
                                    Dim vGo As Boolean = True

                                    If objSuceso.GetIDBySuceso(objViaje.ID, objSucesoId.GetIDByAbreviaturaId("G")) = 0 Then
                                        objSuceso.CleanProperties(objSuceso)
                                        objSuceso.IncidenteViajeId.SetObjectId(objViaje.ID)
                                        objSuceso.FechaHoraSuceso = pFhrOri
                                        objSuceso.SucesoIncidenteId.SetObjectId(objSucesoId.GetIDByAbreviaturaId("G"))
                                        If Not objSuceso.addSuceso(objSuceso, , , , True) Then vGo = False
                                    End If

                                    If vGo Then
                                        '----> Chequeo Viaje de VUE
                                        If objIncidente.trsIdaVuelta = 1 Then
                                            If Not objViaje.Abrir(objViaje.GetIDByIndex(objDstDomicilio.IncidenteId.ID, "VUE")) Then
                                                '----> Agrego el Viaje
                                                objViaje.CleanProperties(objViaje)
                                                objViaje.IncidenteDomicilioId.SetObjectId(objDstDomicilio.ID)
                                                objViaje.ViajeId = "VUE"
                                                If pIncTiempoCarga = incTiempoCarga.Programado Then
                                                    objViaje.flgStatus = 2
                                                Else
                                                    objViaje.flgStatus = 1
                                                End If
                                            Else
                                                If objViaje.flgStatus = 0 Then
                                                    objViaje.HorarioOperativo.horLlamada = pFhrRet
                                                End If
                                            End If
                                            objViaje.reqHorLlegada = pFhrRet
                                            If objViaje.Salvar(objViaje) Then
                                                If objSuceso.GetIDBySuceso(objViaje.ID, objSucesoId.GetIDByAbreviaturaId("G")) = 0 Then
                                                    objSuceso.CleanProperties(objSuceso)
                                                    objSuceso.IncidenteViajeId.SetObjectId(objViaje.ID)
                                                    objSuceso.FechaHoraSuceso = pFhrRet
                                                    objSuceso.SucesoIncidenteId.SetObjectId(objSucesoId.GetIDByAbreviaturaId("G"))
                                                    vSav = objSuceso.addSuceso(objSuceso, , , , True)
                                                Else
                                                    vSav = True
                                                End If
                                            End If
                                        Else
                                            vSav = objViaje.Eliminar(objViaje.GetIDByIndex(objDstDomicilio.IncidenteId.ID, "VUE"))
                                        End If
                                    End If

                                    objSuceso = Nothing
                                    objSucesoId = Nothing

                                End If
                                objViaje = Nothing
                            End If
                        End If
                    End If
                End If

                If vSav Then
                    cnnsTransNET(cnnKey).Commit()
                Else
                    cnnsTransNET(cnnKey).Rollback()
                End If
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)

                '----> Restauro Valores
                objIncidente.myCnnName = cnnDefault
                objObservacion.myCnnName = cnnDefault
                objDomicilio.myCnnName = cnnDefault
                objDstDomicilio.myCnnName = cnnDefault
                '----> Resultado
                SetTraslado = vSav
            End If

        Catch ex As Exception

            If cnnsNET.Contains(cnnKey) Then
                cnnsTransNET(cnnKey).Rollback()
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)
            End If
            '----> Restauro Valores
            objIncidente.myCnnName = cnnDefault
            objObservacion.myCnnName = cnnDefault
            objDomicilio.myCnnName = cnnDefault
            objDstDomicilio.myCnnName = cnnDefault

            HandleError(Me.GetType.Name, "SetTraslado", ex)

        End Try

    End Function

    Public Function CrearSimultaneos(ByVal pInc As Int64, ByVal pTblView As DataView) As Boolean

        CrearSimultaneos = False

        Dim cnnKey As String = "Simultaneo"

        Try

            Dim objOriIncidente As New conIncidentes(cnnKey)
            Dim objDstIncidente As conIncidentes
            Dim objOriIncidenteDomicilio As New conIncidentesDomicilios(cnnKey)
            Dim objDstIncidenteDomicilio As conIncidentesDomicilios
            Dim objOriIncidenteViaje As New conIncidentesViajes(cnnKey)
            Dim objDstIncidenteViaje As conIncidentesViajes
            Dim objOriIncidenteSuceso As New conIncidentesSucesos(cnnKey)
            Dim objDstIncidenteSuceso As conIncidentesSucesos
            Dim objOriMovilSuceso As New conMovilesSucesos(cnnKey)
            Dim objDstMovilSuceso As conMovilesSucesos

            Dim objProgramacion As New conIncidentesProgramaciones(cnnKey)
            Dim objlckIncidentes As New conlckIncidentes(cnnKey)

            Dim vIdx As Integer = 0
            Dim vSav As Boolean = True
            Dim SQL As String

            If shamanStartUp.AbrirConexion(cnnKey) Then

                If objOriIncidente.Abrir(pInc) Then

                    cnnsTransNET.Add(cnnKey, cnnsNET(cnnKey).BeginTransaction)
                    Me.myCnnName = cnnKey

                    Dim vTblSim As DataTable = pTblView.Table

                    Do Until vIdx = vTblSim.Rows.Count Or Not vSav

                        Dim vIncId As Int64 = 0
                        If Not IsDBNull(vTblSim(vIdx)("ID")) Then vIncId = vTblSim(vIdx)("ID")

                        vSav = False
                        '-----------> Creo objeto destino
                        objDstIncidente = objOriIncidente.CloneMe()
                        objDstIncidente.ID = 0
                        '-----------> Inicializo
                        If vIncId > 0 Then
                            Dim objAuxiliar As New conIncidentes(Me.myCnnName)
                            If objAuxiliar.Abrir(vIncId) Then
                                objDstIncidente.ID = objAuxiliar.ID
                                objDstIncidente.NroIncidente = objAuxiliar.NroIncidente
                                objDstIncidente.TrasladoId = objAuxiliar.TrasladoId
                            End If
                            objAuxiliar = Nothing
                        End If
                        If objDstIncidente.ID = 0 Then
                            objDstIncidente.NroIncidente = objlckIncidentes.getNewIncidente(Now.Date)
                            If shamanConfig.modNumeracion = 1 Then
                                objDstIncidente.TrasladoId = objDstIncidente.NroIncidente
                            End If
                        End If
                        objDstIncidente.NroAfiliado = vTblSim(vIdx)("NroAfiliado")
                        objDstIncidente.Paciente = vTblSim(vIdx)("Paciente")
                        objDstIncidente.Sexo = vTblSim(vIdx)("Sexo")
                        If Not IsDBNull(vTblSim(vIdx)("Edad")) Then
                            objDstIncidente.Edad = Val(vTblSim(vIdx)("Edad"))
                        Else
                            objDstIncidente.Edad = 0
                        End If
                        objDstIncidente.NroInterno = vTblSim(vIdx)("NroInterno")
                        objDstIncidente.CoPago = vTblSim(vIdx)("CoPago")
                        objDstIncidente.Sintomas = vTblSim(vIdx)("Sintomas")

                        '------------> Salvamos...
                        If objDstIncidente.Salvar(objDstIncidente) Then
                            vSav = True

                            '-----------> Obtengo domicilios
                            SQL = "SELECT ID FROM IncidentesDomicilios WHERE IncidenteId = " & objOriIncidente.ID
                            Dim cmdDom As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                            Dim vDomTbl As New DataTable
                            Dim vDomIdx As Integer = 0
                            vDomTbl.Load(cmdDom.ExecuteReader)

                            Do Until vDomIdx = vDomTbl.Rows.Count Or Not vSav

                                '-------> Copio objetos domicilio
                                If objOriIncidenteDomicilio.Abrir(vDomTbl.Rows(vDomIdx)(0)) Then

                                    objDstIncidenteDomicilio = DirectCast(objOriIncidenteDomicilio.CloneMe, conIncidentesDomicilios)
                                    objDstIncidenteDomicilio.ID = 0

                                    '----------> Domicilio
                                    If vIncId > 0 Then
                                        '----------> Ajusto
                                        Dim objAuxiliar As New conIncidentesDomicilios
                                        If objAuxiliar.Abrir(objAuxiliar.GetIDByIndex(vIncId, objOriIncidenteDomicilio.TipoDomicilio, objOriIncidenteDomicilio.NroAnexo)) Then
                                            objDstIncidenteDomicilio.ID = objAuxiliar.ID
                                        End If
                                        objAuxiliar = Nothing
                                    End If

                                    objDstIncidenteDomicilio.IncidenteId.SetObjectId(objDstIncidente.ID)

                                    '----------> Salvo
                                    If Not objDstIncidenteDomicilio.Salvar(objDstIncidenteDomicilio) Then
                                        vSav = False
                                    End If
                                End If

                                vDomIdx = vDomIdx + 1

                            Loop

                            '----------------> Viajes
                            If vSav Then
                                '-----------> Obtengo Viajes
                                SQL = "SELECT vij.ID FROM IncidentesViajes vij "
                                SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
                                SQL = SQL & "WHERE (dom.IncidenteId = " & objOriIncidente.ID & ") "

                                Dim cmdVijs As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                                Dim vVijTbl As New DataTable
                                Dim vVijIdx As Integer = 0
                                vVijTbl.Load(cmdVijs.ExecuteReader)

                                Do Until vVijIdx = vVijTbl.Rows.Count Or Not vSav

                                    If objOriIncidenteViaje.Abrir(vVijTbl.Rows(vVijIdx)(0)) Then

                                        objDstIncidenteViaje = DirectCast(objOriIncidenteViaje.CloneMe, conIncidentesViajes)
                                        objDstIncidenteViaje.ID = 0

                                        If vIncId > 0 Then
                                            '----------> Ajusto
                                            Dim objAuxiliar As New conIncidentesViajes(Me.myCnnName)
                                            If objAuxiliar.Abrir(objAuxiliar.GetIDByIndex(vIncId, objOriIncidenteViaje.ViajeId)) Then
                                                objDstIncidenteViaje.ID = objAuxiliar.ID
                                                If objAuxiliar.DiagnosticoId.ID > 0 Then objDstIncidenteViaje.DiagnosticoId.SetObjectId(objAuxiliar.DiagnosticoId.ID)
                                                If objAuxiliar.MotivoNoRealizacionId.ID > 0 Then objDstIncidenteViaje.MotivoNoRealizacionId.SetObjectId(objAuxiliar.MotivoNoRealizacionId.ID)
                                            End If
                                            objAuxiliar = Nothing
                                        End If

                                        objDstIncidenteViaje.IncidenteDomicilioId.SetObjectId(objOriIncidenteDomicilio.GetIDByIndex(objDstIncidente.ID, objOriIncidenteViaje.IncidenteDomicilioId.TipoDomicilio, objOriIncidenteViaje.IncidenteDomicilioId.NroAnexo))
                                        objDstIncidenteViaje.flgStatus = 1

                                        vSav = objDstIncidenteViaje.Salvar(objDstIncidenteViaje)
                                    Else
                                        vSav = False
                                    End If
                                    vVijIdx = vVijIdx + 1
                                Loop
                            End If

                            '-----------------> Sucesos...
                            If vSav Then

                                If vIncId > 0 Then

                                    '--------> Elimino sucesos anteriores

                                    SQL = "DELETE mov FROM MovilesSucesos mov "
                                    SQL = SQL & "INNER JOIN IncidentesSucesos sus ON (mov.IncidenteSucesoId = sus.ID) "
                                    SQL = SQL & "INNER JOIN IncidentesViajes vij ON (sus.IncidenteViajeId = vij.ID) "
                                    SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
                                    SQL = SQL & "WHERE dom.IncidenteId = " & vIncId

                                    Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                                    cmDel.ExecuteNonQuery()

                                    SQL = "DELETE sus FROM IncidentesSucesos sus "
                                    SQL = SQL & "INNER JOIN IncidentesViajes vij ON (sus.IncidenteViajeId = vij.ID) "
                                    SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
                                    SQL = SQL & "WHERE dom.IncidenteId = " & vIncId

                                    cmDel.CommandText = SQL
                                    cmDel.ExecuteNonQuery()

                                End If

                                '-----------> Obtengo Sucesos
                                SQL = "SELECT sus.ID FROM IncidentesSucesos sus "
                                SQL = SQL & "INNER JOIN IncidentesViajes vij ON (sus.IncidenteViajeId = vij.ID) "
                                SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
                                SQL = SQL & "WHERE (dom.IncidenteId = " & objOriIncidente.ID & ") "

                                Dim cmdSuce As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                                Dim vSucTbl As New DataTable
                                Dim vSucIdx As Integer = 0
                                vSucTbl.Load(cmdSuce.ExecuteReader)

                                Do Until vSucIdx = vSucTbl.Rows.Count Or Not vSav

                                    If objOriIncidenteSuceso.Abrir(vSucTbl.Rows(vSucIdx)(0)) Then

                                        objDstIncidenteSuceso = DirectCast(objOriIncidenteSuceso.CloneMe, conIncidentesSucesos)
                                        objDstIncidenteSuceso.ID = 0
                                        objDstIncidenteSuceso.IncidenteViajeId.SetObjectId(objOriIncidenteViaje.GetIDByIndex(objDstIncidente.ID, objDstIncidenteSuceso.IncidenteViajeId.ViajeId))

                                        vSav = objDstIncidenteSuceso.Salvar(objDstIncidenteSuceso)

                                        If vSav Then

                                            '--------> Suceso de Móvil
                                            SQL = "SELECT ID FROM MovilesSucesos WHERE IncidenteSucesoId = " & vSucTbl.Rows(vSucIdx)(0)

                                            Dim cmdMovil As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                                            Dim vMovTbl As New DataTable
                                            Dim vMovIdx As Integer = 0
                                            vMovTbl.Load(cmdMovil.ExecuteReader)

                                            Do Until vMovIdx = vMovTbl.Rows.Count Or Not vSav

                                                If objOriMovilSuceso.Abrir(vMovTbl.Rows(vMovIdx)(0)) Then

                                                    objDstMovilSuceso = DirectCast(objOriMovilSuceso.CloneMe, conMovilesSucesos)
                                                    objDstMovilSuceso.ID = 0
                                                    objDstMovilSuceso.IncidenteSucesoId.SetObjectId(objDstIncidenteSuceso.ID)

                                                    vSav = objDstMovilSuceso.Salvar(objDstMovilSuceso)

                                                Else

                                                    vSav = False

                                                End If

                                                vMovIdx = vMovIdx + 1

                                            Loop

                                        End If

                                    Else
                                        vSav = False
                                    End If

                                    vSucIdx = vSucIdx + 1
                                Loop

                            End If


                            '-----------------> Resultante
                            If vSav Then
                                If Not objProgramacion.Abrir(objProgramacion.GetIDByIndex(objDstIncidente.ID)) Then
                                    objProgramacion.CleanProperties(objProgramacion)
                                    objProgramacion.IncidenteId.SetObjectId(objOriIncidente.ID)
                                    objProgramacion.IncidentePrgId.SetObjectId(objDstIncidente.ID)
                                    objProgramacion.flgSimultaneo = 1
                                    vSav = objProgramacion.Salvar(objProgramacion)
                                End If
                            End If

                        End If

                        objlckIncidentes.unlockIncidente(objDstIncidente.FecIncidente, objDstIncidente.NroIncidente, False)

                        vIdx = vIdx + 1

                    Loop

                    '-----------> Salvo resultante
                    If vSav Then
                        cnnsTransNET(cnnKey).Commit()
                        CrearSimultaneos = True
                    Else
                        cnnsTransNET(cnnKey).Rollback()
                    End If
                    cnnsTransNET.Remove(cnnKey)
                    cnnsNET.Remove(cnnKey)

                End If

            End If

        Catch ex As Exception

            If cnnsNET.Contains(cnnKey) Then
                cnnsTransNET(cnnKey).Rollback()
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)
            End If

            HandleError(Me.GetType.Name, "CrearSimultaneos", ex)

        End Try

    End Function



    Public Function GetIDByIndex(ByVal pFec As Date, ByVal pNro As String, Optional ByVal pTra As Int64 = 0) As Int64
        GetIDByIndex = 0

        Try
            Dim SQL As String

            If pTra = 0 Then
                SQL = "SELECT ID FROM Incidentes WHERE (FecIncidente = '" & DateToSql(pFec) & "') AND (NroIncidente = '" & pNro & "') "
            Else
                SQL = "SELECT ID FROM Incidentes WHERE (TrasladoId = " & pTra & ") "
            End If

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try

    End Function
    Public Function MoveFirst(ByVal pFec As Date, Optional ByVal pCla As gdoClasificacion = gdoClasificacion.gdoIncidente) As Int64
        MoveFirst = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 A.ID FROM Incidentes A INNER JOIN GradosOperativos B ON (A.GradoOperativoId = B.ID) "
            SQL = SQL & "WHERE (A.FecIncidente = '" & DateToSql(pFec) & "') "
            SQL = SQL & "AND (B.ClasificacionId = " & pCla & ") "
            Select Case pCla
                Case gdoClasificacion.gdoIncidente : SQL = SQL & "ORDER BY A.NroIncidente"
                Case gdoClasificacion.gdoIntDomiciliaria : SQL = SQL & "ORDER BY A.TrasladoId"
                Case gdoClasificacion.gdoTraslado : SQL = SQL & "ORDER BY A.TrasladoId"
            End Select

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then MoveFirst = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "MoveFirst", ex)
        End Try

    End Function
    Public Function MovePrevious(ByVal pFec As Date, Optional ByVal pIncRef As Long = 0, Optional ByVal pCla As gdoClasificacion = gdoClasificacion.gdoIncidente) As Int64
        MovePrevious = 0

        Try
            Dim SQL As String

            If pCla = gdoClasificacion.gdoIncidente Then

                SQL = "SELECT TOP 1 A.ID FROM Incidentes A INNER JOIN GradosOperativos B ON (A.GradoOperativoId = B.ID) "
                SQL = SQL & " WHERE (A.FecIncidente = '" & DateToSql(pFec) & "') AND (B.ClasificacionId = " & pCla & ") "
                If pIncRef > 0 Then SQL = SQL & "AND A.NroIncidente < (SELECT NroIncidente FROM Incidentes WHERE ID = " & pIncRef & ") "
                SQL = SQL & "ORDER BY A.NroIncidente DESC"

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then MovePrevious = CType(vOutVal, Int64)

            Else
                Dim vTraIdx As Int64 = 0, vIncPrv As Int64 = 0

                SQL = "SELECT TrasladoId FROM Incidentes WHERE ID = " & pIncRef

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

                If Not vOutVal Is Nothing Then
                    vTraIdx = CType(vOutVal, Int64)

                    Do Until vIncPrv > 0 Or vTraIdx = 1

                        vTraIdx = vTraIdx - 1

                        SQL = "SELECT A.ID FROM Incidentes A INNER JOIN GradosOperativos B ON (A.GradoOperativoId = B.ID) "
                        SQL = SQL & "WHERE (A.TrasladoId = " & vTraIdx & ") AND (B.ClasificacionId = " & pCla & ") "

                        cmFind.CommandText = SQL
                        vOutVal = CType(cmFind.ExecuteScalar, String)
                        If Not vOutVal Is Nothing Then vIncPrv = CType(vOutVal, Int64)

                    Loop

                    If vIncPrv > 0 Then MovePrevious = vIncPrv

                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "MovePrevious", ex)
        End Try

    End Function
    Public Function MoveNext(ByVal pFec As Date, Optional ByVal pIncRef As Long = 0, Optional ByVal pCla As gdoClasificacion = gdoClasificacion.gdoIncidente) As Int64
        MoveNext = 0

        Try
            Dim SQL As String

            If pCla = gdoClasificacion.gdoIncidente Then
                SQL = "SELECT TOP 1 A.ID FROM Incidentes A INNER JOIN GradosOperativos B ON (A.GradoOperativoId = B.ID) "
                SQL = SQL & " WHERE (A.FecIncidente = '" & DateToSql(pFec) & "') AND (B.ClasificacionId = " & pCla & ") "
                If pIncRef > 0 Then SQL = SQL & "AND A.NroIncidente > (SELECT NroIncidente FROM Incidentes WHERE ID = " & pIncRef & ") "
                SQL = SQL & "ORDER BY A.NroIncidente"

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then MoveNext = CType(vOutVal, Int64)

            Else
                Dim vTraIdx As Int64 = 0, vSeq As Integer = 0, vIncNxt As Int64 = 0

                SQL = "SELECT TrasladoId FROM Incidentes WHERE ID = " & pIncRef
                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

                If Not vOutVal Is Nothing Then

                    vTraIdx = CType(vOutVal, Int64)

                    Do Until vIncNxt > 0 Or vSeq = 500

                        vTraIdx = vTraIdx + 1
                        vSeq = vSeq + 1

                        SQL = "SELECT A.ID FROM Incidentes A INNER JOIN GradosOperativos B ON (A.GradoOperativoId = B.ID) "
                        SQL = SQL & "WHERE (A.TrasladoId = " & vTraIdx & ") AND (B.ClasificacionId = " & pCla & ") "

                        cmFind.CommandText = SQL
                        vOutVal = CType(cmFind.ExecuteScalar, String)
                        If Not vOutVal Is Nothing Then vIncNxt = CType(vOutVal, Int64)

                    Loop

                    If vIncNxt > 0 Then MoveNext = vIncNxt

                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "MoveNext", ex)
        End Try

    End Function
    Public Function MoveLast(ByVal pFec As Date, Optional ByVal pCla As gdoClasificacion = gdoClasificacion.gdoIncidente) As Int64
        MoveLast = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 A.ID FROM Incidentes A INNER JOIN GradosOperativos B ON (A.GradoOperativoId = B.ID) "
            SQL = SQL & "WHERE (A.FecIncidente = '" & DateToSql(pFec) & "') "
            SQL = SQL & "AND (B.ClasificacionId = " & pCla & ") "
            Select Case pCla
                Case gdoClasificacion.gdoIncidente : SQL = SQL & "ORDER BY A.NroIncidente DESC"
                Case gdoClasificacion.gdoIntDomiciliaria : SQL = SQL & "ORDER BY A.TrasladoId DESC"
                Case gdoClasificacion.gdoTraslado : SQL = SQL & "ORDER BY A.TrasladoId DESC"
            End Select

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then MoveLast = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "MoveLast", ex)
        End Try

    End Function
    Public Function getDtHistoriaClinica(ByVal pDes As Date, ByVal pHas As Date, Optional ByVal pCliIteId As Int64 = 0, Optional ByVal pCli As Int64 = 0, Optional ByVal pAfl As String = "") As DataTable

        getDtHistoriaClinica = Nothing

        Try
            If (pCliIteId > 0) Or (pCli > 0 And pAfl <> "") Then

                Dim SQL As String

                SQL = "SELECT inc.ID, inc.FecIncidente, inc.NroIncidente, gdo.VisualColor, gdo.AbreviaturaId, inc.Paciente, "
                SQL = SQL & "inc.Sintomas, dig.Descripcion, mov.Movil "
                SQL = SQL & "FROM Incidentes inc "
                SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (inc.ID = dom.IncidenteId) "
                SQL = SQL & "INNER JOIN IncidentesViajes vij ON (dom.ID = vij.IncidenteDomicilioId) "
                SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
                SQL = SQL & "INNER JOIN Clientes cli ON (inc.ClienteId = cli.ID) "
                SQL = SQL & "LEFT JOIN Diagnosticos dig ON (vij.DiagnosticoId = dig.ID) "
                SQL = SQL & "LEFT JOIN Moviles mov ON (vij.MovilId = mov.ID) "
                SQL = SQL & "WHERE (inc.FecIncidente BETWEEN '" & DateToSql(pDes) & "' AND '" & DateToSql(pHas) & "') "
                SQL = SQL & "AND (vij.ViajeId <> 'VUE') "
                If pCliIteId > 0 Then
                    SQL = SQL & "AND (inc.ClienteIntegranteId = " & pCliIteId & ") "
                Else
                    If pCli > 0 And pAfl <> "" Then
                        SQL = SQL & "AND (inc.ClienteId = " & pCli & ") AND (inc.NroAfiliado = '" & pAfl & "') "
                    End If
                End If
                SQL = SQL & " ORDER BY inc.FecIncidente DESC, inc.NroIncidente DESC"

                Dim cmdHis As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vHisTbl As New DataTable

                vHisTbl.Load(cmdHis.ExecuteReader)

                getDtHistoriaClinica = vHisTbl

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getRsHistoriaClinica", ex)
        End Try

    End Function

    Public Function GetSimultaneosByIncidente(ByVal pInc As Int64) As DataTable

        GetSimultaneosByIncidente = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT prg.IncidentePrgId AS ID, inc.NroAfiliado, inc.Paciente, inc.Sexo, REPLACE(CAST(inc.Edad AS VARCHAR), '.00', '') AS Edad, "
            SQL = SQL & "inc.NroInterno, inc.CoPago, inc.Sintomas "
            SQL = SQL & "FROM IncidentesProgramaciones prg "
            SQL = SQL & "INNER JOIN Incidentes inc ON (prg.IncidentePrgId = inc.ID) "
            SQL = SQL & "WHERE (prg.IncidenteId = " & pInc & ") AND (prg.flgSimultaneo = 1) "
            SQL = SQL & "ORDER BY inc.NroAfiliado, inc.Paciente"

            Dim cmdSim As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vSimTbl As New DataTable

            vSimTbl.Load(cmdSim.ExecuteReader)

            Dim objIncidentes As New conIncidentes(Me.myCnnName)

            If objIncidentes.Abrir(pInc) Then

                vSimTbl.Columns("ID").DefaultValue = 0
                vSimTbl.Columns("NroAfiliado").DefaultValue = ""
                vSimTbl.Columns("Paciente").DefaultValue = "NN"
                vSimTbl.Columns("Sexo").DefaultValue = "M"
                vSimTbl.Columns("Edad").ReadOnly = False
                vSimTbl.Columns("NroInterno").DefaultValue = objIncidentes.NroInterno
                vSimTbl.Columns("CoPago").DefaultValue = objIncidentes.CoPago
                vSimTbl.Columns("Sintomas").DefaultValue = objIncidentes.Sintomas

            End If

            GetSimultaneosByIncidente = vSimTbl

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetSimultaneosByIncidente", ex)
        End Try

    End Function

    Public Function GetSimultaneosCierreByIncidente(ByVal pInc As Int64) As DataTable

        GetSimultaneosCierreByIncidente = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT vij.ID, inc.Paciente, dig.AbreviaturaId, dig.Descripcion "

            SQL = SQL & "FROM IncidentesProgramaciones prg "
            SQL = SQL & "INNER JOIN Incidentes inc ON (prg.IncidentePrgId = inc.ID) "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (inc.ID = dom.IncidenteId) "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON (dom.ID = vij.IncidenteDomicilioId) "
            SQL = SQL & "LEFT JOIN Diagnosticos dig ON (vij.DiagnosticoId = dig.ID) "

            SQL = SQL & "WHERE (prg.IncidenteId = " & pInc & ") AND (prg.flgSimultaneo = 1) "

            SQL = SQL & "ORDER BY inc.Paciente"

            Dim cmdSim As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vSimTbl As New DataTable

            vSimTbl.Load(cmdSim.ExecuteReader)

            GetSimultaneosCierreByIncidente = vSimTbl

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetSimultaneosCierreByIncidente", ex)
        End Try

    End Function

    Public Function GetSimultaneoInfo(ByVal pInc As Int64) As String
        GetSimultaneoInfo = ""
        Try
            Dim SQL As String

            '-----> Traigo simultáneo hijo
            SQL = "SELECT CAST(sim.IncidenteId AS VARCHAR) + '^' + inc.NroIncidente "
            SQL = SQL & "FROM IncidentesProgramaciones sim "
            SQL = SQL & "INNER JOIN Incidentes inc ON (sim.IncidenteId = inc.ID) "
            SQL = SQL & "WHERE (sim.IncidentePrgId = " & pInc & ") AND (sim.flgSimultaneo = 1) "

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetSimultaneoInfo = vOutVal

            '-----> Traigo simultáneo
            SQL = "SELECT sim.ID "
            SQL = SQL & "FROM IncidentesProgramaciones sim "
            SQL = SQL & "INNER JOIN Incidentes inc ON (sim.IncidenteId = inc.ID) "
            SQL = SQL & "WHERE (sim.IncidenteId = " & pInc & ") AND (sim.flgSimultaneo = 1) "

            cmdFind.CommandText = SQL
            vOutVal = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetSimultaneoInfo = "Tiene Simultáneos"

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetSimultaneoInfo", ex)
        End Try
    End Function

    Public Function GetLastLugarDerivacion(ByVal pInc As Int64) As String
        GetLastLugarDerivacion = ""
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 CASE dom.SanatorioId WHEN 0 THEN dom.dmReferencia ELSE san.Descripcion END "
            SQL = SQL & "FROM IncidentesDomicilios dom  "
            SQL = SQL & "LEFT JOIN Sanatorios san ON (dom.SanatorioId = san.ID) "
            SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ") AND (dom.TipoDomicilio = 1) "
            SQL = SQL & "ORDER BY dom.ID DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetLastLugarDerivacion = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetLastLugarDerivacion", ex)
        End Try
    End Function

    Public Function GetByPhone(ByVal pTel As String) As Int64
        GetByPhone = 0
        Try

            If GetNumericValue(pTel) > 0 Then

                Dim SQL As String
                SQL = "SELECT TOP 1 inc.ID FROM Incidentes inc "
                SQL = SQL & "INNER JOIN Clientes cli ON (inc.ClienteId = cli.ID) "
                SQL = SQL & "WHERE (inc.TelefonoFix = " & GetNumericValue(pTel) & ") AND (cli.virActivo = 1) "
                SQL = SQL & "ORDER BY inc.ID DESC"

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then GetByPhone = CType(vOutVal, Int64)

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByPhone", ex)
        End Try

    End Function

    Public Function GetQuerySearchBase(ByVal pDes As Date, ByVal pHas As Date, Optional ByVal pAtrId As Int64 = 0, Optional ByVal pTipAtr As Integer = 0) As DataTable

        GetQuerySearchBase = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT inc.ID, inc.FecIncidente, inc.NroIncidente, gdo.AbreviaturaId AS Grado, cli.AbreviaturaId AS Cliente, inc.NroInterno, "
            SQL = SQL & "inc.NroAfiliado, inc.Paciente, dom.Domicilio, ISNULL(loc.AbreviaturaId, '') AS LocalidadId "
            SQL = SQL & "FROM Incidentes inc "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (inc.ID = dom.IncidenteId) "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON (dom.ID = vij.IncidenteDomicilioId) "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
            SQL = SQL & "INNER JOIN Clientes cli ON (inc.ClienteId = cli.ID) "
            SQL = SQL & "LEFT JOIN Localidades loc ON (dom.LocalidadId = loc.ID) "
            SQL = SQL & "WHERE (inc.FecIncidente BETWEEN '" & DateToSql(pDes) & "' AND '" & DateToSql(pHas) & "') "
            SQL = SQL & "ORDER BY inc.FecIncidente, inc.NroIncidente"

            Dim cmdBus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdBus.ExecuteReader)

            dt.Columns("LocalidadId").ReadOnly = False

            For vIdx = 0 To dt.Rows.Count - 1

                Dim vDynVal As String = dt(vIdx)(9)
                Dim vAdd As Boolean = True

                If pAtrId > 0 Then
                    vDynVal = GetDynamicValue(dt(vIdx)(0), pAtrId, pTipAtr)
                    dt(vIdx)(9) = vDynVal
                End If

            Next vIdx

            GetQuerySearchBase = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQuerySearchBase", ex)
        End Try
    End Function

    Public Function GetDynamicValue(ByVal pInc As Int64, ByVal pAtrId As Int64, Optional ByVal pTipAtr As Integer = -1) As String
        GetDynamicValue = ""
        Try
            Dim SQL As String

            If pTipAtr >= 0 Then
                SQL = "SELECT Valor1 FROM IncidentesAtributos "
                SQL = SQL & "WHERE (IncidenteId = " & pInc & ") AND (AtributoDinamicoId = " & pAtrId & ") "
            Else
                SQL = "SELECT inc.Valor1, atr.TipoDatoSalida FROM IncidentesAtributos inc "
                SQL = SQL & "INNER JOIN AtributosDinamicos atr ON (inc.AtributoDinamicoId = atr.ID) "
                SQL = SQL & "WHERE (inc.IncidenteId = " & pInc & ") AND (inc.AtributoDinamicoId = " & pAtrId & ") "
            End If

            Dim cmdDyn As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dtAtr As New DataTable
            dtAtr.Load(cmdDyn.ExecuteReader)

            If dtAtr.Rows.Count > 0 Then

                If pTipAtr = -1 Then pTipAtr = dtAtr(0)(1)

                If pTipAtr = 3 Then
                    GetDynamicValue = getSINO(dtAtr(0)(0), True)
                Else
                    GetDynamicValue = dtAtr(0)(0)
                End If

            Else

                If pTipAtr = -1 Then pTipAtr = dtAtr(0)(1)

                If pTipAtr = 3 Then
                    GetDynamicValue = "No"
                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDynamicValue", ex)
        End Try

    End Function

    Public Function GetDataView(Optional ByVal pGdoClf As gdoClasificacion = gdoClasificacion.gdoIncidente, Optional ByVal pHavSel As Boolean = True) As DataView
        GetDataView = Nothing
        Try

            Dim SQL As String

            If Not pHavSel Then
                Me.setSelPrn(, pGdoClf)
            End If

            SQL = "SELECT inc.ID, inc.FecIncidente, inc.NroIncidente, inc.Telefono, cli.AbreviaturaId AS Cliente, cli.RazonSocial, inc.NroAfiliado, "
            SQL = SQL & "dom.dmCalle, dom.dmAltura, dom.dmPiso, dom.dmDepto, dom.dmEntreCalle1, dom.dmEntreCalle2, "
            SQL = SQL & "dom.dmReferencia, dom.Domicilio, loc.Descripcion AS Localidad, par.Descripcion AS Partido, "
            SQL = SQL & "inc.Sintomas, inc.Sexo, inc.Edad, gdo.AbreviaturaId AS Grado, "
            SQL = SQL & "inc.Paciente, CASE inc.flgIvaGravado WHEN 1 THEN 'GRAVADO' ELSE 'EXENTO' END AS Iva, "
            SQL = SQL & "inc.PlanId, inc.NroInterno, inc.Aviso, inc.Observaciones, dig.Descripcion AS Diagnostico, mot.Descripcion AS MotivoAnulacion, "
            SQL = SQL & "sus.FechaHoraSuceso, vij.ViajeId, sin.Descripcion AS Suceso, mov.Movil AS MovilSuceso, usr.Nombre AS UsuarioSuceso "

            SQL = SQL & "FROM Incidentes inc "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (inc.ID = dom.IncidenteId) "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON (dom.ID = vij.IncidenteDomicilioId) "
            SQL = SQL & "INNER JOIN IncidentesSucesos sus ON (vij.ID = sus.IncidenteViajeId) "
            SQL = SQL & "INNER JOIN Clientes cli ON (inc.ClienteId = cli.ID) "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
            SQL = SQL & "INNER JOIN Localidades loc ON (dom.LocalidadId = loc.ID) "
            SQL = SQL & "LEFT JOIN Localidades par ON (loc.PartidoId = par.ID) "
            SQL = SQL & "INNER JOIN SucesosIncidentes sin ON (sus.SucesoIncidenteId = sin.ID) "
            SQL = SQL & "LEFT JOIN Moviles mov ON (sus.MovilId = mov.ID) "
            SQL = SQL & "LEFT JOIN Usuarios usr ON (sus.regUsuarioId = usr.ID) "
            SQL = SQL & "LEFT JOIN Diagnosticos dig ON (vij.DiagnosticoId = dig.ID) "
            SQL = SQL & "LEFT JOIN MotivosNoRealizacion mot ON (vij.MotivoNoRealizacionId = mot.ID) "

            SQL = SQL & "INNER JOIN selIncidentes sel ON (inc.ID = sel.IncidenteId) "
            SQL = SQL & "WHERE (sel.PID = " & logPID & ") "

            SQL = SQL & "ORDER BY inc.FecIncidente, inc.NroIncidente, dom.TipoDomicilio "

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdGrl.ExecuteReader)

            GetDataView = New DataView(dt)

        Catch ex As Exception

            HandleError(Me.GetType.Name, "GetDataView", ex)

        End Try
    End Function

    Private Sub setSelPrn(Optional ByVal pInc As Int64 = 0, Optional ByVal pGdoClf As gdoClasificacion = gdoClasificacion.gdoIncidente)

        Try

            Dim SQL As String
            Dim objSelInc As New conselIncidentes(Me.myCnnName)

            objSelInc.DelByPID(logPID)

            SQL = "SELECT TOP 10 inc.ID FROM Incidentes inc "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON (inc.GradoOperativoId = gdo.ID) "
            If pInc > 0 Then
                SQL = SQL & "WHERE (inc.ID = " & pInc & ") "
            Else
                SQL = SQL & "WHERE (gdo.ClasificacionId = " & pGdoClf & ") "
            End If

            Dim cmdCreate As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            dt.Load(cmdCreate.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                objSelInc.CleanProperties(objSelInc)
                objSelInc.PID = logPID
                objSelInc.IncidenteId.SetObjectId(dt(vIdx)(0))

                objSelInc.Salvar(objSelInc)

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "setSelPrn", ex)
        End Try
    End Sub

    Public Function GetDTTripulantes(ByVal pInc As Int64, Optional pVij As Decimal = 0, Optional pPue As String = "") As DataTable

        GetDTTripulantes = Nothing

        Try

            Dim dt As New DataTable

            dt.Columns.Add("ID", GetType(Long))
            dt.Columns.Add("IncidenteViajeId", GetType(Long))
            dt.Columns.Add("ViajeId", GetType(String))
            dt.Columns.Add("MovilId", GetType(Long))
            dt.Columns.Add("Movil", GetType(String))
            dt.Columns.Add("PersonalId", GetType(Long))
            dt.Columns.Add("Legajo", GetType(String))
            dt.Columns.Add("Nombre", GetType(String))
            dt.Columns.Add("PuestoGrilla", GetType(String))

            Dim SQL As String

            SQL = "SELECT vij.ID, vij.ViajeId, vij.horDespacho FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ") "
            If pVij > 0 Then SQL = SQL & "AND (vij.ID = " & pVij & ")"

            Dim cmdVij As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dtVij As New DataTable
            Dim vIdx As Integer
            dtVij.Load(cmdVij.ExecuteReader)

            For vIdx = 0 To dtVij.Rows.Count - 1

                SQL = "SELECT DISTINCT inc.MovilId, mov.Movil FROM IncidentesSucesos inc "
                SQL = SQL & "INNER JOIN Moviles mov ON (inc.MovilId = mov.ID) "
                SQL = SQL & "WHERE (inc.IncidenteViajeId = " & dtVij(vIdx)(0) & ")"

                cmdVij.CommandText = SQL
                Dim dtMov As New DataTable
                Dim vMov As Integer
                dtMov.Load(cmdVij.ExecuteReader)

                '----> Verifico que el móvil no se encuentre interrumpido
                For vMov = 0 To dtMov.Rows.Count - 1

                    SQL = "SELECT inc.ID FROM IncidentesSucesos inc "
                    SQL = SQL & "INNER JOIN SucesosIncidentes sus ON (inc.SucesoIncidenteId = sus.ID) "
                    SQL = SQL & "WHERE (inc.IncidenteViajeId = " & dtVij(vIdx)(0) & ") AND (inc.MovilId = " & dtMov(vMov)(0) & ") "
                    SQL = SQL & "AND (sus.AbreviaturaId = 'T') "

                    cmdVij.CommandText = SQL
                    Dim vRowInc As Int64 = 0
                    Dim vRowStr As String = cmdVij.ExecuteScalar()
                    If Not vRowStr Is Nothing Then vRowInc = CType(vRowStr, Int64)

                    If vRowInc = 0 Then

                        '----> Obtengo dotación fija adjunta

                        SQL = "SELECT grl.ID, grl.PersonalId, per.Legajo, per.Apellido, per.Nombre, grl.PuestoGrilla "
                        SQL = SQL & "FROM IncidentesPersonal grl "
                        SQL = SQL & "INNER JOIN Personal per ON (grl.PersonalId = per.ID) "
                        SQL = SQL & "WHERE (grl.IncidenteViajeId = " & dtVij(vIdx)(0) & ") AND (grl.MovilId = " & dtMov(vMov)(0) & ") "
                        If pPue <> "" Then SQL = SQL & "AND (grl.PuestoGrilla = '" & pPue & "')"

                        cmdVij.CommandText = SQL
                        Dim dtFij As New DataTable
                        Dim vFij As Integer
                        dtFij.Load(cmdVij.ExecuteReader)

                        For vFij = 0 To dtFij.Rows.Count - 1

                            Dim dtRow As DataRow = dt.NewRow

                            dtRow("ID") = dtFij(vFij)(0)
                            dtRow("IncidenteViajeId") = dtVij(vIdx)(0)
                            dtRow("ViajeId") = dtVij(vIdx)(1)
                            dtRow("MovilId") = dtMov(vMov)(0)
                            dtRow("Movil") = dtMov(vMov)(1)
                            dtRow("PersonalId") = dtFij(vFij)(1)
                            dtRow("Legajo") = dtFij(vFij)(2)
                            dtRow("Nombre") = dtFij(vFij)(3) & ", " & dtFij(vFij)(4)
                            dtRow("PuestoGrilla") = dtFij(vFij)(5)

                            dt.Rows.Add(dtRow)

                        Next vFij

                        If dtFij.Rows.Count = 0 Then

                            '----> Obtengo dotación del móvil
                            SQL = "SELECT grl.PersonalId, per.Legajo, per.Apellido, per.Nombre, grl.PuestoGrilla "
                            SQL = SQL & "FROM GrillaOperativaTripulantes grl "
                            SQL = SQL & "INNER JOIN Personal per ON (grl.PersonalId = per.ID) "

                            SQL = SQL & "WHERE (grl.MovilId = " & dtMov(vMov)(0) & ") "
                            SQL = SQL & "AND ('" & DateTimeToSql(dtVij(vIdx)(2)) & "' BETWEEN "
                            SQL = SQL & "CASE grl.FicEntrada WHEN '" & DateTimeToSql(NullDateMin) & "' THEN grl.PacEntrada ELSE grl.FicEntrada END "
                            SQL = SQL & "AND "
                            SQL = SQL & "CASE grl.FicSalida WHEN '" & DateTimeToSql(NullDateMin) & "' THEN grl.PacSalida ELSE grl.FicSalida END) "

                            If pPue <> "" Then SQL = SQL & "AND (grl.PuestoGrilla = '" & pPue & "')"

                            cmdVij.CommandText = SQL
                            Dim dtGrl As New DataTable
                            Dim vGrl As Integer
                            dtGrl.Load(cmdVij.ExecuteReader)

                            For vGrl = 0 To dtGrl.Rows.Count - 1

                                Dim dtMatch() As DataRow = dt.Select("IncidenteViajeId=" & dtVij(vIdx)(0) & " AND MovilId=" & dtMov(vMov)(0) & " AND PuestoGrilla='" & dtGrl(vGrl)(4) & "'")

                                If dtMatch.Length = 0 Then

                                    Dim dtRow As DataRow = dt.NewRow

                                    dtRow("ID") = 0
                                    dtRow("IncidenteViajeId") = dtVij(vIdx)(0)
                                    dtRow("ViajeId") = dtVij(vIdx)(1)
                                    dtRow("MovilId") = dtMov(vMov)(0)
                                    dtRow("Movil") = dtMov(vMov)(1)
                                    dtRow("PersonalId") = dtGrl(vGrl)(0)
                                    dtRow("Legajo") = dtGrl(vGrl)(1)
                                    dtRow("Nombre") = dtGrl(vGrl)(2) & ", " & dtGrl(vGrl)(3)
                                    dtRow("PuestoGrilla") = dtGrl(vGrl)(4)

                                    dt.Rows.Add(dtRow)

                                End If

                            Next vGrl

                        End If

                    End If

                Next

            Next vIdx

            GetDTTripulantes = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDTTripulantes", ex)
        End Try

    End Function

    Public Sub SetTripulantes(ByVal pInc As Int64, ByVal dt As DataTable)
        Try
            Dim SQL As String

            SQL = "DELETE per FROM IncidentesPersonal per "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON per.IncidenteViajeId = vij.ID "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON vij.IncidenteDomicilioId = dom.ID "
            SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ") "

            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            Dim vIdx As Integer
            Dim objIncPersonal As New typIncidentesPersonal

            For vIdx = 0 To dt.Rows.Count - 1

                objIncPersonal.CleanProperties(objIncPersonal)
                objIncPersonal.IncidenteViajeId.SetObjectId(dt(vIdx)("IncidenteViajeId"))
                objIncPersonal.MovilId.SetObjectId(dt(vIdx)("MovilId"))
                objIncPersonal.PersonalId.SetObjectId(dt(vIdx)("PersonalId"))
                objIncPersonal.PuestoGrilla = dt(vIdx)("PuestoGrilla")

                objIncPersonal.Salvar(objIncPersonal)

            Next vIdx

            objIncPersonal = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetTripulantes", ex)
        End Try
    End Sub

    Public Function GetDemorasByIncidente(ByVal pInc As Int64) As DataTable

        GetDemorasByIncidente = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT vij.ID, vij.ViajeId, dom.Domicilio, loc.Descripcion AS Localidad, vij.Demora AS DemoraMin, SPACE(5) AS Demora "
            SQL = SQL & "FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "INNER JOIN Localidades loc ON (dom.LocalidadId = loc.ID) "
            SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ")"
            SQL = SQL & "ORDER BY vij.ViajeId"

            Dim cmdDem As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            dt.Load(cmdDem.ExecuteReader)

            dt.Columns("Demora").ReadOnly = False

            For vIdx = 0 To dt.Rows.Count - 1
                dt(vIdx)("Demora") = MinutesToTime(dt(vIdx)("DemoraMin")).ToString
            Next vIdx

            GetDemorasByIncidente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDemorasByIncidente", ex)
        End Try

    End Function

    Public Sub SetDemoras(ByVal dt As DataTable)
        Try

            Dim vIdx As Integer
            Dim objIncidentesViajes As New typIncidentesViajes

            For vIdx = 0 To dt.Rows.Count - 1

                If objIncidentesViajes.Abrir(dt.Rows(vIdx)("ID")) Then

                    objIncidentesViajes.Demora = GetMinutes(dt.Rows(vIdx)("Demora"))

                    objIncidentesViajes.Salvar(objIncidentesViajes)

                End If

            Next vIdx

            objIncidentesViajes = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetTripulantes", ex)
        End Try
    End Sub

    Public Function CanPreArribo(ByVal pId As Int64, pSin As Int64) As Boolean
        CanPreArribo = False

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 cab.ID FROM IncidentesSintomas cab "
            SQL = SQL & "INNER JOIN IncidentesSintomasPregs prg ON (cab.ID = prg.IncidenteSintomaId) AND (prg.flgPediatrico < 2) "
            SQL = SQL & "INNER JOIN SintomasPreguntas spg ON (cab.SintomaId = spg.SintomaId) AND (spg.flgPediatrico = 2) "
            SQL = SQL & "LEFT JOIN IncidentesSintomasPregs pre ON (cab.ID = pre.IncidenteSintomaId) AND (pre.flgPediatrico = 2) "
            SQL = SQL & "WHERE (cab.IncidenteId = " & pId & ") AND (cab.SintomaId = " & pSin & ") AND (pre.ID IS NULL) "

            Dim cmAli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vVal As String = cmAli.ExecuteScalar

            If Not vVal Is Nothing Then
                CanPreArribo = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CanPreArribo", ex)
        End Try
    End Function

    Public Function GetTimeLine(pInc As Int64) As DataTable

        GetTimeLine = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ics.ID, ics.FechaHoraSuceso, CASE ics.MovilId WHEN 0 THEN 50 ELSE 51 END AS GrupoSuceso, sus.Descripcion AS Suceso, "
            SQL = SQL & "CASE ics.Condicion WHEN 'TIT' THEN 'TITULAR' WHEN 'APO' THEN 'EN APOYO' WHEN 'REE' THEN 'REEMPLAZO' ELSE '' END AS ToolTip, "
            SQL = SQL & "CASE ics.MovilId WHEN 0 THEN '' ELSE CASE mov.PrestadorId WHEN 0 THEN 'Móvil ' + mov.Movil ELSE 'Empresa ' + mov.Movil END END AS Referente, "
            SQL = SQL & "tmv.Descripcion AS ReferenteNombre, usr.Nombre AS Usuario "
            SQL = SQL & "FROM IncidentesSucesos ics "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON ics.IncidenteViajeId = vij.ID "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON vij.IncidenteDomicilioId = dom.ID "
            SQL = SQL & "INNER JOIN Incidentes inc ON dom.IncidenteId = inc.ID "
            SQL = SQL & "INNER JOIN SucesosIncidentes sus ON ics.SucesoIncidenteId = sus.ID "
            SQL = SQL & "LEFT JOIN Moviles mov ON ics.MovilId = mov.ID "
            SQL = SQL & "LEFT JOIN TiposMoviles tmv ON mov.TipoMovilId = tmv.ID "
            SQL = SQL & "INNER JOIN Usuarios usr ON ics.regUsuarioId = usr.ID "
            SQL = SQL & "WHERE (inc.ID = " & pInc & ") "
            SQL = SQL & "UNION "
            SQL = SQL & "SELECT obs.ID, obs.regFechaHora, obs.flgReclamo AS GrupoSuceso, "
            SQL = SQL & "CASE obs.flgReclamo WHEN 3 THEN 'RECHAZO INSTITUCION' WHEN 4 THEN 'RECHAZO PRESTADOR' WHEN 5 THEN 'RECHAZO MOVIL' ELSE '' END AS Suceso, "
            SQL = SQL & "CONVERT(VARCHAR(100), obs.Observaciones) AS ToolTip, "
            SQL = SQL & "CASE obs.flgReclamo WHEN 3 THEN CASE obs.SanatorioId WHEN 0 THEN obs.SanatorioNombre ELSE san.AbreviaturaId END ELSE mov.Movil END AS Referente, "
            SQL = SQL & "CASE obs.flgReclamo WHEN 3 THEN san.Descripcion WHEN 4 THEN pre.RazonSocial WHEN 5 THEN tmv.Descripcion ELSE '' END AS ReferenteNombre, "
            SQL = SQL & "usr.Nombre AS Usuario "
            SQL = SQL & "FROM IncidentesObservaciones obs "
            SQL = SQL & "INNER JOIN Incidentes inc ON obs.IncidenteId = inc.ID "
            SQL = SQL & "LEFT JOIN Sanatorios san ON obs.SanatorioId = san.ID "
            SQL = SQL & "LEFT JOIN Moviles mov ON obs.MovilId = mov.ID "
            SQL = SQL & "LEFT JOIN Prestadores pre ON mov.PrestadorId = pre.ID "
            SQL = SQL & "LEFT JOIN TiposMoviles tmv ON mov.TipoMovilId = tmv.ID "
            SQL = SQL & "INNER JOIN Usuarios usr ON obs.regUsuarioId = usr.ID "
            SQL = SQL & "WHERE (inc.ID = " & pInc & ") AND (obs.flgReclamo >= 3) "

            SQL = SQL & "ORDER BY 2"

            Dim cmdTim As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            dt.Load(cmdTim.ExecuteReader)

            GetTimeLine = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetTimeLine", ex)
        End Try

    End Function

    Public Function Programar(ByVal pInc As Int64, ByVal dtFechas As DataTable) As Boolean

        Programar = False

        Dim cnnKey As String = "Programar"

        Try

            Dim objOriIncidente As New conIncidentes(cnnKey)
            Dim objDstIncidente As conIncidentes
            Dim objOriIncidenteDomicilio As New conIncidentesDomicilios(cnnKey)
            Dim objDstIncidenteDomicilio As conIncidentesDomicilios
            Dim objDstIncidenteViaje As conIncidentesViajes
            Dim objDstIncidenteSuceso As conIncidentesSucesos
            Dim objOriIncidenteObservacion As New conIncidentesObservaciones(cnnKey)
            Dim objDstIncidenteObservacion As conIncidentesObservaciones
            Dim objProgramacion As New conIncidentesProgramaciones(cnnKey)
            Dim objlckTraslados As New conlckTraslados(cnnKey)
            Dim objSucesoIncidente As New conSucesosIncidentes

            Dim vIdx As Integer = 0, vSav As Boolean = True
            Dim SQL As String


            If shamanStartUp.AbrirConexion(cnnKey) Then

                If objOriIncidente.Abrir(pInc) Then

                    cnnsTransNET.Add(cnnKey, cnnsNET(cnnKey).BeginTransaction)
                    Me.myCnnName = cnnKey

                    Do Until vIdx = dtFechas.Rows.Count Or Not vSav

                        vSav = False
                        '-----------> Creo objeto destino
                        objDstIncidente = objOriIncidente.MemberwiseClone()
                        '-----------> Limpio
                        objDstIncidente.ID = 0
                        objDstIncidente.FecIncidente = dtFechas(vIdx)(0)
                        objDstIncidente.NroIncidente = ""
                        objDstIncidente.HorarioOperativo.horDerivacion = GetCurrentTime.Date
                        objDstIncidente.HorarioOperativo.horDespacho = GetCurrentTime.Date
                        objDstIncidente.HorarioOperativo.horFinalizacion = GetCurrentTime.Date
                        objDstIncidente.HorarioOperativo.horInicial = GetCurrentTime.Date
                        objDstIncidente.HorarioOperativo.horInternacion = GetCurrentTime.Date
                        objDstIncidente.HorarioOperativo.horLlamada = GetCurrentTime.Date
                        objDstIncidente.HorarioOperativo.horLlegada = GetCurrentTime.Date
                        objDstIncidente.HorarioOperativo.horSalida = GetCurrentTime.Date
                        objDstIncidente.HorarioOperativo.horSolDerivacion = GetCurrentTime.Date
                        '------------> Nro Traslado
                        objDstIncidente.TrasladoId = objlckTraslados.getNewTraslado()
                        If objDstIncidente.Salvar(objDstIncidente) Then
                            vSav = True
                            '-----------> Obtengo domicilios
                            SQL = "SELECT ID FROM IncidentesDomicilios WHERE IncidenteId = " & objOriIncidente.ID
                            Dim cmdDom As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                            Dim vDomTbl As New DataTable
                            Dim vDomIdx As Integer = 0
                            vDomTbl.Load(cmdDom.ExecuteReader)

                            Do Until vDomIdx = vDomTbl.Rows.Count Or Not vSav

                                '-------> Copio objetos domicilio
                                If objOriIncidenteDomicilio.Abrir(vDomTbl.Rows(vDomIdx).Item(0)) Then
                                    objDstIncidenteDomicilio = DirectCast(objOriIncidenteDomicilio.CloneMe, conIncidentesDomicilios)
                                    '----------> Limpio
                                    objDstIncidenteDomicilio.ID = 0
                                    objDstIncidenteDomicilio.IncidenteId.SetObjectId(objDstIncidente.ID)
                                    '----------> Salvo
                                    If objDstIncidenteDomicilio.Salvar(objDstIncidenteDomicilio) Then
                                        '--------> Viaje IDA
                                        If objDstIncidenteDomicilio.TipoDomicilio = 0 Then
                                            objDstIncidenteViaje = New conIncidentesViajes(cnnKey)
                                            objDstIncidenteViaje.CleanProperties(objDstIncidenteViaje)
                                            objDstIncidenteViaje.IncidenteDomicilioId.SetObjectId(objDstIncidenteDomicilio.ID)
                                            objDstIncidenteViaje.ViajeId = "IDA"
                                            objDstIncidenteViaje.flgStatus = 2
                                            objDstIncidenteViaje.reqHorLlegada = dtFechas(vIdx)(0) & " " & dtFechas(vIdx)(1)
                                            If objOriIncidente.GradoOperativoId.ClasificacionId = gdoClasificacion.gdoTraslado Then
                                                objDstIncidenteViaje.reqHorInternacion = Me.calHorInternacion(objOriIncidente.ID, "IDA", CDate(dtFechas(vIdx)(0) & " " & dtFechas(vIdx)(1)))
                                            End If
                                            If objDstIncidenteViaje.Salvar(objDstIncidenteViaje) Then
                                                '-------> Suceso IDA
                                                objDstIncidenteSuceso = New conIncidentesSucesos(cnnKey)
                                                objDstIncidenteSuceso.CleanProperties(objDstIncidenteSuceso)
                                                objDstIncidenteSuceso.IncidenteViajeId.SetObjectId(objDstIncidenteViaje.ID)
                                                objDstIncidenteSuceso.FechaHoraSuceso = dtFechas(vIdx)(0) & " " & dtFechas(vIdx)(1)
                                                objDstIncidenteSuceso.SucesoIncidenteId.SetObjectId(objSucesoIncidente.GetIDByAbreviaturaId("G"))
                                                vSav = objDstIncidenteSuceso.addSuceso(objDstIncidenteSuceso, , , , True)
                                            End If
                                        ElseIf objDstIncidenteDomicilio.TipoDomicilio = 1 Then
                                            '--------> Viaje VUE
                                            If objOriIncidente.trsIdaVuelta = 1 Then
                                                objDstIncidenteViaje = New conIncidentesViajes(cnnKey)
                                                objDstIncidenteViaje.CleanProperties(objDstIncidenteViaje)
                                                objDstIncidenteViaje.IncidenteDomicilioId.SetObjectId(objDstIncidenteDomicilio.ID)
                                                objDstIncidenteViaje.ViajeId = "VUE"
                                                objDstIncidenteViaje.flgStatus = 2
                                                objDstIncidenteViaje.reqHorLlegada = calHorRetorno(objOriIncidente.ID, dtFechas(vIdx)(0) & " " & dtFechas(vIdx)(1))
                                                If objDstIncidenteViaje.Salvar(objDstIncidenteViaje) Then
                                                    '-------> Suceso VUE
                                                    objDstIncidenteSuceso = New conIncidentesSucesos(cnnKey)
                                                    objDstIncidenteSuceso.CleanProperties(objDstIncidenteSuceso)
                                                    objDstIncidenteSuceso.IncidenteViajeId.SetObjectId(objDstIncidenteViaje.ID)
                                                    objDstIncidenteSuceso.FechaHoraSuceso = objDstIncidenteViaje.reqHorLlegada
                                                    objDstIncidenteSuceso.SucesoIncidenteId.SetObjectId(objSucesoIncidente.GetIDByAbreviaturaId("G"))
                                                    vSav = objDstIncidenteSuceso.addSuceso(objDstIncidenteSuceso, , , , True)
                                                Else
                                                    vSav = False
                                                End If
                                            End If
                                        End If
                                    End If
                                End If

                                vDomIdx = vDomIdx + 1

                            Loop

                            '----------------> Observaciones
                            If vSav Then
                                '-----------> Obtengo Observaciones
                                SQL = "SELECT ID FROM IncidentesObservaciones WHERE (IncidenteId = " & objOriIncidente.ID & ") "
                                SQL = SQL & "AND (flgReclamo = 0)"

                                Dim cmdObs As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                                Dim vObsTbl As New DataTable
                                Dim vObsIdx As Integer = 0
                                vObsTbl.Load(cmdObs.ExecuteReader)

                                Do Until vObsIdx = vObsTbl.Rows.Count Or Not vSav
                                    If objOriIncidenteObservacion.Abrir(vObsTbl.Rows(vObsIdx).Item(0)) Then
                                        objDstIncidenteObservacion = DirectCast(objOriIncidenteObservacion.CloneMe, conIncidentesObservaciones)
                                        objDstIncidenteObservacion.ID = 0
                                        objDstIncidenteObservacion.IncidenteId.SetObjectId(objDstIncidente.ID)
                                        objDstIncidenteObservacion.Observaciones = objOriIncidenteObservacion.Observaciones
                                        vSav = objDstIncidenteObservacion.Salvar(objDstIncidenteObservacion)
                                    Else
                                        vSav = False
                                    End If
                                    vObsIdx = vObsIdx + 1
                                Loop
                            End If
                            '-----------------> Resultante
                            If vSav Then
                                objProgramacion.CleanProperties(objProgramacion)
                                objProgramacion.IncidenteId.SetObjectId(objOriIncidente.ID)
                                objProgramacion.IncidentePrgId.SetObjectId(objDstIncidente.ID)
                                objProgramacion.estFechaHora = dtFechas(vIdx)(0) & " " & dtFechas(vIdx)(1)
                                vSav = objProgramacion.Salvar(objProgramacion)
                            End If

                        End If

                        objlckTraslados.unlockTraslado(objDstIncidente.TrasladoId)

                        vIdx = vIdx + 1

                    Loop

                    '-----------> Salvo resultante
                    If vSav Then
                        cnnsTransNET(cnnKey).Commit()
                        Programar = True
                    Else
                        cnnsTransNET(cnnKey).Rollback()
                    End If
                    cnnsTransNET.Remove(cnnKey)
                    cnnsNET.Remove(cnnKey)

                End If

            End If

        Catch ex As Exception

            If cnnsNET.Contains(cnnKey) Then
                cnnsTransNET(cnnKey).Rollback()
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)
            End If

            HandleError(Me.GetType.Name, "Programar", ex)

        End Try

    End Function

    Private Function calHorRetorno(ByVal pInc As Int64, ByVal pFecHor As DateTime) As DateTime
        calHorRetorno = pFecHor
        Try
            Dim SQL As String
            Dim vIdaFhr As DateTime = NullDateTime
            Dim vVueFhr As DateTime = NullDateTime

            SQL = "SELECT vij.ViajeId, vij.reqHorLlegada FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ") AND ((vij.ViajeId = 'IDA') OR (vij.ViajeId = 'VUE')) "

            Dim cmdHor As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vHorTbl As New DataTable
            Dim vHorIdx As Integer = 0
            vHorTbl.Load(cmdHor.ExecuteReader)

            For vHorIdx = 0 To vHorTbl.Rows.Count - 1

                If vHorTbl.Rows(vHorIdx).Item(0) = "IDA" Then
                    vIdaFhr = vHorTbl.Rows(vHorIdx).Item(1)
                Else
                    vVueFhr = vHorTbl.Rows(vHorIdx).Item(1)
                End If

            Next vHorIdx

            If vIdaFhr <> NullDateTime And vVueFhr <> NullDateTime And vIdaFhr <= vVueFhr Then
                Dim vDif As Integer = DateDiff(DateInterval.Minute, vIdaFhr, vVueFhr)
                pFecHor = pFecHor.AddMinutes(vDif)
                calHorRetorno = pFecHor
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "calHorRetorno", ex)
        End Try

    End Function

    Private Function calHorInternacion(ByVal pInc As Int64, ByVal pVij As String, ByVal pFecHor As DateTime) As DateTime
        calHorInternacion = pFecHor
        Try
            Dim SQL As String

            SQL = "SELECT vij.reqHorLlegada, vij.reqHorInternacion FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ") AND (vij.ViajeId = '" & pVij & "') "

            Dim cmdHor As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vHorTbl As New DataTable
            Dim vHorIdx As Integer = 0
            vHorTbl.Load(cmdHor.ExecuteReader)

            If vHorTbl.Rows.Count > 0 Then
                Dim vHor As DataRow = vHorTbl.Rows(0)
                If vHor.Item(0) <> NullDateTime And vHor.Item(1) <> NullDateTime And vHor.Item(0) <= vHor.Item(1) Then
                    Dim vDif As Integer = DateDiff(DateInterval.Minute, vHor.Item(0), vHor.Item(1))
                    pFecHor = pFecHor.AddMinutes(vDif)
                    calHorInternacion = pFecHor
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "calHorInternacion", ex)
        End Try

    End Function


End Class
