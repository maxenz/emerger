Imports System.Data
Imports System.Data.SqlClient
Public Class conMovilesActuales
    Inherits typMovilesActuales
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    'Public Function GetMovilesOperativos() As DataTable

    '    GetMovilesOperativos = Nothing

    '    Try
    '        Dim SQL As String, vRowSel As Int16 = -1, vColSel As Int16 = -1

    '        If cnnsNET.Count > 1 Then Exit Function

    '        SQL = "SELECT act.ID, mov.Movil, zon.ColorHexa AS ColorZona, sus.ValorGrilla, sus.ColorHexa AS ColorSuceso, "
    '        SQL = SQL & "CASE ISNULL(act.MotivoCondicionalId, 0) WHEN 0 THEN loc.AbreviaturaId ELSE mot.AbreviaturaId END AS Localidad, "
    '        SQL = SQL & "act.TipoMovilId, blc.ZonaGeograficaId "
    '        SQL = SQL & "FROM MovilesActuales act "
    '        SQL = SQL & "INNER JOIN Moviles mov ON (act.MovilId = mov.ID) "
    '        SQL = SQL & "INNER JOIN BasesOperativas bas ON (act.BaseOperativaId = bas.ID) "
    '        SQL = SQL & "INNER JOIN Localidades blc ON (bas.LocalidadId = blc.ID) "
    '        SQL = SQL & "INNER JOIN ZonasGeograficas zon ON (blc.ZonaGeograficaId = zon.ID) "
    '        SQL = SQL & "INNER JOIN SucesosIncidentes sus ON (act.SucesoIncidenteId = sus.ID) "
    '        SQL = SQL & "LEFT JOIN Localidades loc ON (act.LocalidadId = loc.ID) "
    '        SQL = SQL & "LEFT JOIN MotivosCondicionales mot ON (act.MotivoCondicionalId = mot.ID) "
    '        SQL = SQL & "ORDER BY mov.Movil"

    '        Dim cmdMov As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
    '        Dim dt As New DataTable
    '        Dim vIdx As Integer = 0
    '        dt.Load(cmdMov.ExecuteReader)

    '        '-------> Aplico seguridad de perfiles
    '        SetPerfiles(dt)

    '        GetMovilesOperativos = dt

    '    Catch ex As Exception
    '        HandleError(Me.GetType.Name, "GetMovilesOperativos", ex)
    '    End Try
    'End Function

    Public Function GetMovilesOperativos(pSelMoviles As scrMoviles, Optional pSelPrf As Int64 = 0, Optional pVisCpl As Boolean = False) As DataTable

        GetMovilesOperativos = Nothing

        Try
            Dim SQL As String, vRowSel As Int16 = -1, vColSel As Int16 = -1

            If cnnsNET.Count > 1 Then Exit Function

            SQL = "SELECT act.ID, mov.Movil, zon.ColorHexa AS ColorZona, "

            If Not pVisCpl Then
                SQL = SQL & "sus.ValorGrilla, "
            Else
                SQL = SQL & "sus.Descripcion + CHAR(10) + CHAR(13) + CASE ISNULL(act.MotivoCondicionalId, 0) WHEN 0 THEN loc.Descripcion ELSE mot.Descripcion END "
                SQL = SQL & "AS ValorGrilla, "
            End If

            SQL = SQL & "sus.ColorHexa AS ColorSuceso, "

            If Not pVisCpl Then
                SQL = SQL & "CASE ISNULL(act.MotivoCondicionalId, 0) WHEN 0 THEN loc.AbreviaturaId ELSE mot.AbreviaturaId END AS Localidad, "
            Else
                SQL = SQL & "(SELECT ISNULL(COUNT(mvs.ID), 0) FROM MovilesSucesos mvs "
                SQL = SQL & "INNER JOIN IncidentesSucesos ics ON mvs.IncidenteSucesoId = ics.ID "
                SQL = SQL & "INNER JOIN SucesosIncidentes sus ON ics.SucesoIncidenteId = sus.ID "
                SQL = SQL & "WHERE (mvs.MovilId = mov.ID) AND (sus.AbreviaturaId = 'A') "
                SQL = SQL & "AND (mvs.FechaHoraSuceso BETWEEN '" & DateToSql(Now.Date, , True) & "' AND '" & DateToSql(Now.Date, , True, False) & "')) AS Localidad, "
            End If

            SQL = SQL & "act.TipoMovilId, blc.ZonaGeograficaId "

            SQL = SQL & "FROM MovilesActuales act "
            SQL = SQL & "INNER JOIN Moviles mov ON (act.MovilId = mov.ID) "
            SQL = SQL & "INNER JOIN BasesOperativas bas ON (act.BaseOperativaId = bas.ID) "
            SQL = SQL & "INNER JOIN Localidades blc ON (bas.LocalidadId = blc.ID) "
            SQL = SQL & "INNER JOIN ZonasGeograficas zon ON (blc.ZonaGeograficaId = zon.ID) "
            SQL = SQL & "INNER JOIN SucesosIncidentes sus ON (act.SucesoIncidenteId = sus.ID) "
            SQL = SQL & "LEFT JOIN Localidades loc ON (act.LocalidadId = loc.ID) "
            SQL = SQL & "LEFT JOIN MotivosCondicionales mot ON (act.MotivoCondicionalId = mot.ID) "
            If pSelMoviles = scrMoviles.Disponibles Then SQL = SQL & "WHERE ((sus.AbreviaturaId = 'L') OR (sus.AbreviaturaId = 'R') OR (sus.AbreviaturaId = 'T') OR (sus.AbreviaturaId = 'C')) "
            If pSelMoviles = scrMoviles.Ocupados Then SQL = SQL & "WHERE (sus.AbreviaturaId <> 'L') AND (sus.AbreviaturaId <> 'R') AND (sus.AbreviaturaId <> 'T') AND (sus.AbreviaturaId <> 'C') "

            SQL = SQL & "ORDER BY mov.Movil"

            Dim cmdMov As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdMov.ExecuteReader)

            '-------> Aplico seguridad de perfiles
            SetPerfiles(dt, pSelPrf)

            GetMovilesOperativos = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMovilesOperativos", ex)
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

                If vDespacha Then

                    Dim vIdx As Integer = 0

                    Do Until vIdx = dt.Rows.Count
                        '----> Verifico si es de mi tipo móvil y zona
                        Dim SQL As String

                        SQL = "SELECT cab.ID FROM Perfiles cab "
                        SQL = SQL & "INNER JOIN PerfilesTiposMoviles tmv ON (cab.ID = tmv.PerfilId) "
                        SQL = SQL & "INNER JOIN PerfilesZonasGeograficas zon ON (cab.ID = zon.PerfilId) "
                        SQL = SQL & "WHERE (cab.ID = " & pSelPrf & ") AND (tmv.TipoMovilId = " & dt(vIdx)("TipoMovilId") & ") "
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

    Public Function GetIDByIndex(ByVal pVal As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM MovilesActuales WHERE MovilId = " & pVal
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetIDByVehiculo(ByVal pVal As Int64) As Int64
        GetIDByVehiculo = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM MovilesActuales WHERE VehiculoId = " & pVal
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByVehiculo = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByVehiculo", ex)
        End Try
    End Function

    Public Function GetIDAndValidation(Optional ByVal pMov As Long = 0, Optional ByVal pAbr As String = "", Optional ByVal pMsg As Boolean = True) As Long
        GetIDAndValidation = 0
        Try
            Dim objMovil As New conMoviles
            If pMov = 0 And pAbr = "" Then
                '--------> Por ahora sin rutina
            Else
                '--------> Valido si existe móvil
                If pMov = 0 Then pMov = objMovil.GetIDByMovil(pAbr)
                If objMovil.Abrir(pMov) Then
                    '-----> Valido si esta de alta operativa
                    If Me.Abrir(Me.GetIDByIndex(pMov)) Then
                        '-----> Valido x perfil
                        GetIDAndValidation = Me.ID
                    Else
                        If pMsg Then MsgBox("El móvil ingresado no se encuentra operativo", MsgBoxStyle.Critical, "Móviles")
                    End If
                Else
                    If pMsg Then MsgBox("El móvil ingresado es inexistente", MsgBoxStyle.Critical, "Móviles")
                End If
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDAndValidation", ex)
        End Try
    End Function

    Public Function GetDTSugerenciaDespacho(ByVal pSel As Integer, Optional ByVal pGdo As Long = 0, Optional ByVal pLoc As Long = 0, Optional pCli As Int64 = 0, Optional dtPolys As DataTable = Nothing, Optional pMovCob As Integer = 0, Optional pGeoEmp As Boolean = False) As DataTable

        GetDTSugerenciaDespacho = Nothing

        Try

            Dim SQL As String = ""
            Dim dt As New DataTable

            Select Case pSel

                Case 0
                    '-----> Móviles
                    SQL = "SELECT A.ID, B.Movil, C.Descripcion AS TipoMovil, D.Descripcion AS Estado, 0 AS Sel, "
                    SQL = SQL & "A.gpsFecHorTransmision, A.gpsLatitud, A.gpsLongitud, "
                    SQL = SQL & "9999 AS Distancia, 9999 AS Tiempo, SPACE(100) AS DistanciaTiempo, SPACE(10) AS Link "
                    SQL = SQL & "FROM MovilesActuales A "
                    SQL = SQL & "INNER JOIN Moviles B ON (A.MovilId = B.ID) "
                    SQL = SQL & "INNER JOIN TiposMoviles C ON (A.TipoMovilId = C.ID) "
                    SQL = SQL & "INNER JOIN SucesosIncidentes D ON (A.SucesoIncidenteId = D.ID) "
                    If pLoc > 0 Then
                        If pMovCob = 0 Then
                            SQL = sqlWhere(SQL) & "(((B.flgGeoCobPropia = 1) AND (A.MovilId IN(SELECT MovilId FROM MovilesLocalidades WHERE (LocalidadId =  " & pLoc & ")))) "
                            SQL = SQL & "OR (((B.flgGeoCobPropia = 0) AND (B.BaseOperativaId IN(SELECT BaseOperativaId FROM BasesOperativasLocalidades WHERE (LocalidadId = " & pLoc & ")))))) "
                        Else
                            If Not dtPolys Is Nothing Then

                                Dim vPolIdx As Integer

                                SQL = sqlWhere(SQL) & "(((B.flgGeoCobPropia = 1) AND (A.MovilId IN(SELECT MovilId FROM MovilesPoligonos "

                                For vPolIdx = 0 To dtPolys.Rows.Count - 1

                                    If vPolIdx = 0 Then
                                        SQL = SQL & "WHERE (PoligonoId =  " & dtPolys(vPolIdx)("ID") & ") "
                                    Else
                                        SQL = SQL & "OR (PoligonoId =  " & dtPolys(vPolIdx)("ID") & ") "
                                    End If

                                Next

                                SQL = SQL & "))) "

                                SQL = SQL & "OR (((B.flgGeoCobPropia = 0) AND (B.BaseOperativaId IN(SELECT BaseOperativaId FROM BasesOperativasPoligonos "

                                For vPolIdx = 0 To dtPolys.Rows.Count - 1

                                    If vPolIdx = 0 Then
                                        SQL = SQL & "WHERE (PoligonoId =  " & dtPolys(vPolIdx)("ID") & ") "
                                    Else
                                        SQL = SQL & "OR (PoligonoId =  " & dtPolys(vPolIdx)("ID") & ") "
                                    End If

                                Next

                                SQL = SQL & "))))) "

                            End If
                        End If
                    End If
                    If pGdo > 0 Then SQL = sqlWhere(SQL) & "(A.TipoMovilId IN(SELECT TipoMovilId FROM TiposMovilesGrados WHERE (GradoOperativoId =  " & pGdo & "))) "
                    SQL = SQL & "ORDER BY D.Orden"

                    Dim cmdSug As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    dt.Load(cmdSug.ExecuteReader)

                Case 1

                    If Not pGeoEmp Then

                        If pLoc > 0 And pGdo > 0 Then

                            Dim objMovilesLocalidades As New conMovilesLocalidades

                            Dim dtMap As DataTable = objMovilesLocalidades.GetMapaCobertura(pGdo, pCli, pLoc).Table
                            Dim vIdx As Integer

                            dt.Columns.Add("ID", GetType(Int64))
                            dt.Columns.Add("Movil", GetType(String))
                            dt.Columns.Add("TipoMovil", GetType(String))
                            dt.Columns.Add("Estado", GetType(String))
                            dt.Columns.Add("DistanciaTiempo", GetType(String))
                            dt.Columns.Add("NroPrioridad", GetType(Int64))
                            dt.Columns.Add("ModoCobertura", GetType(Integer))

                            For vIdx = 0 To dtMap.Rows.Count - 1

                                If dtMap(vIdx)("ID") > 0 Then

                                    Dim dtRow As DataRow = dt.NewRow

                                    dtRow("ID") = dtMap(vIdx)("PrestadorId")
                                    dtRow("Movil") = dtMap(vIdx)("Codigo")
                                    dtRow("TipoMovil") = dtMap(vIdx)("Empresa")
                                    dtRow("Estado") = dtMap(vIdx)("TipoCobertura")
                                    dtRow("ModoCobertura") = dtMap(vIdx)("ModoCobertura")
                                    '-----> Debería de traer las observaciones del rechazo
                                    If dtMap(vIdx)("ModoCobertura") > 0 Then
                                        dtRow("DistanciaTiempo") = "Cápita preestablecida con el prestador sobre el cliente"
                                    Else
                                        dtRow("DistanciaTiempo") = ""
                                    End If
                                    If dtMap(vIdx)("NroPrioridad") <> 999 Then
                                        dtRow("NroPrioridad") = dtMap(vIdx)("NroPrioridad")
                                    Else
                                        dtRow("NroPrioridad") = dtMap(vIdx)("PrioridadAuto")
                                    End If


                                    dt.Rows.Add(dtRow)

                                End If

                            Next vIdx

                        Else

                            '-----> Empresas
                            SQL = "SELECT A.ID, B.Movil, A.RazonSocial AS TipoMovil, C.Descripcion AS Estado, 1 AS Sel, "
                            SQL = SQL & "CAST('1980-01-01' AS DATETIME) AS gpsFecHorTransmision, 0 AS gpsLatitud, 0 AS gpsLongitud, "
                            SQL = SQL & "9999 AS Distancia, 9999 AS Tiempo, SPACE(100) AS DistanciaTiempo, SPACE(10) AS Link "

                            SQL = SQL & "FROM Prestadores A "
                            SQL = SQL & "INNER JOIN Moviles B ON (A.ID = B.PrestadorId) "
                            SQL = SQL & "INNER JOIN TiposMoviles C ON (B.TipoMovilId = C.ID) "
                            If pLoc > 0 And pGdo = 0 Then SQL = sqlWhere(SQL) & "B.ID IN(SELECT MovilId FROM MovilesLocalidades WHERE (LocalidadId =  " & pLoc & ")) "
                            If pGdo > 0 And pLoc = 0 Then SQL = sqlWhere(SQL) & "B.TipoMovilId IN(SELECT TipoMovilId FROM TiposMovilesGrados WHERE (GradoOperativoId =  " & pGdo & ")) "

                            SQL = SQL & "ORDER BY B.Movil"

                            Dim cmdSug As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                            dt.Load(cmdSug.ExecuteReader)

                        End If

                    Else

                        '-----> Geo Empresas (solo por distancia, sin importar grado)

                        SQL = "SELECT A.ID, B.Movil, A.RazonSocial AS TipoMovil, C.Descripcion AS Estado, 1 AS Sel, "
                        SQL = SQL & "CAST('1980-01-01' AS DATETIME) AS gpsFecHorTransmision, 0 AS gpsLatitud, 0 AS gpsLongitud, "
                        SQL = SQL & "9999 AS Distancia, 9999 AS Tiempo, 'Empresa domiciliada a ' + CAST(dbo.fn_CoordinateDistance(D.Latitud, D.Longitud, E.Latitud, E.Longitud) AS VARCHAR) + ' km. del servicio' DistanciaTiempo, SPACE(10) AS Link, "
                        SQL = SQL & "dbo.fn_CoordinateDistance(D.Latitud, D.Longitud, E.Latitud, E.Longitud) AS DistanciaPrestador "

                        SQL = SQL & "FROM Prestadores A "
                        SQL = SQL & "INNER JOIN Moviles B ON (A.ID = B.PrestadorId) "
                        SQL = SQL & "INNER JOIN TiposMoviles C ON (B.TipoMovilId = C.ID) "
                        SQL = SQL & "INNER JOIN Localidades D ON (A.LocalidadId = D.ID) "
                        SQL = SQL & "INNER JOIN Localidades E ON (E.ID = " & pLoc & ") "

                        SQL = SQL & "ORDER BY DistanciaPrestador, B.Movil"

                        Dim cmdSug As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                        dt.Load(cmdSug.ExecuteReader)


                    End If

                Case 2
                    '-----> Domiciliario
                    SQL = "SELECT A.ID, B.Movil, A.Apellido + ' ' + A.Nombre AS TipoMovil, C.Descripcion AS Estado, 1 AS Sel, "
                    SQL = SQL & "CAST('1980-01-01' AS DATETIME) AS gpsFecHorTransmision, 0 AS gpsLatitud, 0 AS gpsLongitud, "
                    SQL = SQL & "9999 AS Distancia, 9999 AS Tiempo, SPACE(100) AS DistanciaTiempo, SPACE(10) AS Link "
                    SQL = SQL & "FROM Personal A "
                    SQL = SQL & "INNER JOIN Moviles B ON (A.ID = B.PersonalId) "
                    SQL = SQL & "INNER JOIN TiposMoviles C ON (B.TipoMovilId = C.ID) "
                    If pLoc > 0 Then
                        SQL = sqlWhere(SQL) & "(((B.flgGeoCobPropia = 1) AND (B.ID IN(SELECT MovilId FROM MovilesLocalidades WHERE (LocalidadId =  " & pLoc & ")))) "
                        SQL = SQL & "OR (((B.flgGeoCobPropia = 0) AND (B.BaseOperativaId IN(SELECT BaseOperativaId FROM BasesOperativasLocalidades WHERE (LocalidadId = " & pLoc & ")))))) "
                    End If
                    If pGdo > 0 Then SQL = sqlWhere(SQL) & "(B.TipoMovilId IN(SELECT TipoMovilId FROM TiposMovilesGrados WHERE (GradoOperativoId =  " & pGdo & "))) "
                    SQL = SQL & "ORDER BY B.Movil"

                    Dim cmdSug As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    dt.Load(cmdSug.ExecuteReader)

            End Select

            GetDTSugerenciaDespacho = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDTSugerenciaDespacho", ex)
        End Try
    End Function

End Class
