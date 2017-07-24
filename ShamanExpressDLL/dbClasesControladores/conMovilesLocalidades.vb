Imports System.Data
Imports System.Data.SqlClient
Public Class conMovilesLocalidades
    Inherits typMovilesLocalidades
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIDByIndex(ByVal pMov As Int64, ByVal pLoc As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM MovilesLocalidades WHERE (MovilId = " & pMov & ") AND (LocalidadId = " & pLoc & ")"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function


    Public Function GetByMovil(ByVal pMov As Int64, Optional pPreCli As Int64 = 0) As DataTable

        GetByMovil = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT mov.ID, loc.ID AS LocalidadId, pai.AbreviaturaId AS Pais, prv.AbreviaturaId AS Provincia, "
            SQL = SQL & "par.Descripcion AS Partido, loc.Descripcion AS Localidad, ISNULL(tmv.AbreviaturaId, '') AS TipoMovil, "
            SQL = SQL & "ISNULL(mov.PrestadorClienteId, 0) AS PrestadorClienteId "

            SQL = SQL & "FROM MovilesLocalidades mov "
            SQL = SQL & "INNER JOIN Moviles cab ON mov.MovilId = cab.ID "
            SQL = SQL & "INNER JOIN Localidades loc ON mov.LocalidadId = loc.ID "
            SQL = SQL & "INNER JOIN Localidades par ON loc.PartidoId = par.ID "
            SQL = SQL & "INNER JOIN Provincias prv ON loc.ProvinciaId = prv.ID "
            SQL = SQL & "INNER JOIN Paises pai ON prv.PaisId = pai.ID "
            SQL = SQL & "LEFT JOIN TiposMoviles tmv ON mov.TipoMovilId = tmv.ID "

            SQL = SQL & "WHERE (cab.ID = " & pMov & ") "

            SQL = SQL & "ORDER BY pai.AbreviaturaId, prv.AbreviaturaId, par.Descripcion, loc.Descripcion "

            Dim cmdPre As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdPre.ExecuteReader)

            dt.Columns.Add("Selected", GetType(Boolean))
            dt.Columns("Selected").ReadOnly = False
            dt.Columns("Selected").DefaultValue = False

            For vIdx = 0 To dt.Rows.Count - 1
                If dt(vIdx)("PrestadorClienteId") > 0 Then
                    dt(vIdx)("Selected") = True
                Else
                    dt(vIdx)("Selected") = False
                End If
            Next

            GetByMovil = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByMovil", ex)
        End Try
    End Function

    Public Function GetMapaCobertura(ByVal pGdo As Int64, Optional ByVal pCli As Int64 = 0, Optional ByVal pLoc As Int64 = 0) As DataView

        GetMapaCobertura = Nothing

        Try
            Dim SQL As String
            Dim dtMap As New DataTable

            SQL = "SELECT 0 AS ID, loc.ID AS LocalidadId, pai.AbreviaturaId AS Pais, prv.AbreviaturaId AS Provincia, "
            SQL = SQL & "par.Descripcion AS Partido, loc.Descripcion AS Localidad, 999 AS NroPrioridad, 999 AS PrioridadAuto, "
            SQL = SQL & "SPACE(10) AS Codigo, SPACE(100) AS Empresa, 0 AS PrestadorId, 0 AS ModoCobertura, 0.00 AS ValorReferencia, "
            SQL = SQL & "loc.Latitud, loc.Longitud, SPACE(100) AS TipoCobertura "

            SQL = SQL & "FROM Localidades loc "
            SQL = SQL & "INNER JOIN Localidades par ON loc.PartidoId = par.ID "
            SQL = SQL & "INNER JOIN Provincias prv ON loc.ProvinciaId = prv.ID "
            SQL = SQL & "INNER JOIN Paises pai ON prv.PaisId = pai.ID "

            If pLoc > 0 Then SQL = SQL & "WHERE (loc.ID = " & pLoc & ") "

            SQL = SQL & "ORDER BY pai.AbreviaturaId, prv.AbreviaturaId, par.Descripcion, loc.Descripcion "

            Dim cmdPre As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdPre.ExecuteReader)

            dtMap = dt.Clone()

            dtMap.Columns("NroPrioridad").ReadOnly = False
            dtMap.Columns("NroPrioridad").AllowDBNull = True
            dtMap.Columns("PrioridadAuto").ReadOnly = False

            For vIdx = 0 To dt.Rows.Count - 1

                Dim vAddLoc As Boolean = False

                '-----> Algoritmo (unificar con MovilesSugeridos)

                '-----> Prioridad 1 - Prestador Capita
                '-----> Variante 1
                Dim vCapVar As Integer

                For vCapVar = 0 To 1

                    If vCapVar = 0 Then

                        SQL = "SELECT mvl.ID, 1 AS NroPrioridad, 1 AS PrioridadAuto, pre.AbreviaturaId AS Codigo, pre.RazonSocial AS Empresa, 1 AS ModoCobertura, pre.ID AS PrestadorId, "
                        SQL = SQL & "tcb.Descripcion AS TipoCobertura "

                        SQL = SQL & "FROM MovilesLocalidades mvl "
                        SQL = SQL & "INNER JOIN Moviles mov ON mvl.MovilId = mov.ID "
                        SQL = SQL & "INNER JOIN Prestadores pre ON mov.PrestadorId = pre.ID "
                        SQL = SQL & "INNER JOIN PrestadoresClientes cli ON pre.ID = cli.PrestadorId "
                        SQL = SQL & "INNER JOIN TiposMovilesGrados tmv ON CASE ISNULL(cli.TipoMovilId, 0) WHEN 0 THEN mov.TipoMovilId ELSE cli.TipoMovilId END = tmv.TipoMovilId "
                        SQL = SQL & "INNER JOIN TiposMoviles tcb ON CASE ISNULL(cli.TipoMovilId, 0) WHEN 0 THEN mov.TipoMovilId ELSE cli.TipoMovilId END = tcb.ID "

                        SQL = SQL & "WHERE (mvl.LocalidadId = " & dt(vIdx)(1) & ") "
                        SQL = SQL & "AND (tmv.GradoOperativoId = " & pGdo & ") "
                        SQL = SQL & "AND ((cli.ClienteId = " & pCli & ") OR (cli.ClienteId = 0)) "
                        SQL = SQL & "AND (cli.flgCoberturaPropia = 0) "

                    Else

                        SQL = "SELECT mvl.ID, 1 AS NroPrioridad, 1 AS PrioridadAuto, pre.AbreviaturaId AS Codigo, pre.RazonSocial AS Empresa, 2 AS ModoCobertura, pre.ID AS PrestadorId, "
                        SQL = SQL & "tcb.Descripcion AS TipoCobertura "

                        SQL = SQL & "FROM MovilesLocalidades mvl "
                        SQL = SQL & "INNER JOIN Moviles mov ON mvl.MovilId = mov.ID "
                        SQL = SQL & "INNER JOIN Prestadores pre ON mov.PrestadorId = pre.ID "
                        SQL = SQL & "INNER JOIN PrestadoresClientes cli ON pre.ID = cli.PrestadorId AND mvl.PrestadorClienteId = cli.ID "
                        SQL = SQL & "INNER JOIN TiposMovilesGrados tmv ON CASE ISNULL(cli.TipoMovilId, 0) WHEN 0 THEN mov.TipoMovilId ELSE cli.TipoMovilId END = tmv.TipoMovilId "
                        SQL = SQL & "INNER JOIN TiposMoviles tcb ON CASE ISNULL(cli.TipoMovilId, 0) WHEN 0 THEN mov.TipoMovilId ELSE cli.TipoMovilId END = tcb.ID "

                        SQL = SQL & "WHERE (mvl.LocalidadId = " & dt(vIdx)(1) & ") "
                        SQL = SQL & "AND (tmv.GradoOperativoId = " & pGdo & ") "
                        SQL = SQL & "AND ((cli.ClienteId = " & pCli & ") OR (cli.ClienteId = 0)) "
                        SQL = SQL & "AND (cli.flgCoberturaPropia = 1) "

                    End If

                    If Me.addMapa(pGdo, dtMap, dt, vIdx, SQL, True) Then
                        vAddLoc = True
                    End If

                Next

                '-----> Algoritmo (unificar con MovilesSugeridos)

                SQL = "SELECT mvl.ID, 999 NroPrioridad, pre.AbreviaturaId AS Codigo, pre.RazonSocial AS Empresa, 0 AS ModoCobertura, pre.ID AS PrestadorId, "
                SQL = SQL & "tcb.Descripcion AS TipoCobertura "

                SQL = SQL & "FROM MovilesLocalidades mvl "
                SQL = SQL & "INNER JOIN Moviles mov ON mvl.MovilId = mov.ID "
                SQL = SQL & "INNER JOIN Prestadores pre ON mov.PrestadorId = pre.ID "
                SQL = SQL & "INNER JOIN TiposMovilesGrados tmv ON CASE ISNULL(mvl.TipoMovilId, 0) WHEN 0 THEN mov.TipoMovilId ELSE mvl.TipoMovilId END = tmv.TipoMovilId "
                SQL = SQL & "INNER JOIN TiposMoviles tcb ON CASE ISNULL(mvl.TipoMovilId, 0) WHEN 0 THEN mov.TipoMovilId ELSE mvl.TipoMovilId END = tcb.ID "

                SQL = SQL & "WHERE (mvl.LocalidadId = " & dt(vIdx)(1) & ") "
                SQL = SQL & "AND (tmv.GradoOperativoId = " & pGdo & ") "

                If Me.addMapa(pGdo, dtMap, dt, vIdx, SQL) Then
                    vAddLoc = True
                End If

                If Not vAddLoc Then

                    Dim rowNew As DataRow = dtMap.NewRow
                    Dim vCol As Integer

                    For vCol = 0 To dtMap.Columns.Count - 1
                        rowNew(vCol) = dt(vIdx)(vCol)
                    Next vCol

                    rowNew("Empresa") = "No hay prestador establecido"

                    dtMap.Rows.Add(rowNew)

                End If

            Next

            '-----> Aplicar el valor auto x tarifa...
            Dim dtView As New DataView(dtMap)
            Dim vPaiLst As String = ""
            Dim vPrvLst As String = ""
            Dim vParLst As String = ""
            Dim vLocLst As String = ""
            Dim vModAnt As Integer = 0
            Dim vRefLst As Double = 0
            Dim vNroPri As Integer = 1

            dtView.Sort = "Pais ASC, Provincia ASC, Partido ASC, Localidad ASC, NroPrioridad ASC, ValorReferencia ASC"

            For Each row As DataRowView In dtView

                If Trim(row("Codigo")) <> "" Then

                    If vPaiLst <> row("Pais") Or vPrvLst <> row("Provincia") Or vParLst <> row("Partido") Or vLocLst <> row("Localidad") Then
                        vPaiLst = row("Pais")
                        vPrvLst = row("Provincia")
                        vParLst = row("Partido")
                        vLocLst = row("Localidad")
                        vModAnt = row("ModoCobertura")
                        vNroPri = 1
                        row("PrioridadAuto") = vNroPri
                    Else
                        If row("ValorReferencia") > vRefLst Or vModAnt > 0 Then
                            vNroPri = vNroPri + 1
                        End If
                        row("PrioridadAuto") = vNroPri
                    End If
                    vRefLst = row("ValorReferencia")

                End If
            Next

            dtView.Sort = "Pais ASC, Provincia ASC, Partido ASC, Localidad ASC, NroPrioridad ASC, PrioridadAuto ASC"

            GetMapaCobertura = dtView

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMapaCobertura", ex)
        End Try
    End Function

    Private Function addMapa(pGdo As Int64, dtMap As DataTable, dt As DataTable, pIdx As Integer, SQL As String, Optional pIsCap As Boolean = False) As Boolean

        addMapa = False

        Try

            Dim cmdPre As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dtPre As New DataTable
            Dim vRowPre As Integer
            dtPre.Load(cmdPre.ExecuteReader)

            dtPre.Columns("NroPrioridad").ReadOnly = False

            For vRowPre = 0 To dtPre.Rows.Count - 1

                Dim vRowMatch() As DataRow = dtMap.Select("LocalidadId = " & dt(pIdx)("LocalidadId") & " AND Codigo = '" & dtPre(vRowPre)("Codigo") & "'")

                If vRowMatch.Length = 0 Then

                    If Not pIsCap Then

                        '---> Obtengo el nro. de prioridad
                        Dim objPrioridad As New conMovilesLocalidadesPrioridades(Me.myCnnName)

                        If objPrioridad.Abrir(objPrioridad.GetIdByIndex(dtPre(vRowPre)("ID"), pGdo)) Then
                            dtPre(vRowPre)("NroPrioridad") = objPrioridad.NroPrioridad
                        End If

                    End If

                    Dim rowNew As DataRow = dtMap.NewRow
                    Dim vCol As Integer

                    For vCol = 0 To dtMap.Columns.Count - 1
                        rowNew(vCol) = dt(pIdx)(vCol)
                    Next vCol

                    For vCol = 0 To dtPre.Columns.Count - 1
                        rowNew(dtPre.Columns(vCol).ColumnName) = dtPre(vRowPre)(vCol)
                    Next vCol

                    '----> Debo obtener el valor de referencia

                    If Not pIsCap Then
                        Dim vValRef As Double = 999999

                        SQL = "SELECT TOP 1 pre.Importe "
                        SQL = SQL & "FROM TarifasPrestaciones pre "
                        SQL = SQL & "INNER JOIN TarifasVigencias vig ON pre.TarifaVigenciaId = vig.ID "
                        SQL = SQL & "INNER JOIN PrestadoresTarifas tar ON vig.TarifaId = tar.TarifaId "
                        SQL = SQL & "INNER JOIN GradosOperativos gdo ON pre.ConceptoFacturacionId = gdo.ConceptoFacturacion1Id "
                        SQL = SQL & "WHERE (tar.PrestadorId = " & dtPre(vRowPre)("PrestadorId") & ") "
                        SQL = SQL & "AND (gdo.ID = " & pGdo & ") "
                        SQL = SQL & "AND ('" & DateToSql(Now.Date) & "' BETWEEN vig.VigenciaDesde AND vig.VigenciaHasta) "
                        SQL = SQL & "ORDER BY KmDesde"

                        Dim cmdImporte As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                        Dim vOutVal As String = CType(cmdImporte.ExecuteScalar, String)
                        If Not vOutVal Is Nothing Then vValRef = CType(vOutVal, Double)

                        rowNew("ValorReferencia") = vValRef
                    Else
                        rowNew("ValorReferencia") = 0
                    End If

                    '----> Agrego

                    dtMap.Rows.Add(rowNew)

                End If

                addMapa = True

            Next

        Catch ex As Exception
            HandleError(Me.GetType.Name, "addMapa", ex)
        End Try
    End Function

    Public Function GetLastLocalidad(ByVal pMov As Int64) As Int64

        GetLastLocalidad = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 loc.ID "

            SQL = SQL & "FROM MovilesLocalidades mov "
            SQL = SQL & "INNER JOIN Moviles cab ON mov.MovilId = cab.ID "
            SQL = SQL & "INNER JOIN Localidades loc ON mov.LocalidadId = loc.ID "

            SQL = SQL & "WHERE (cab.id = " & pMov & ") "

            SQL = SQL & "ORDER BY mov.regFechaHora DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetLastLocalidad = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetLastLocalidad", ex)
        End Try

    End Function


End Class
