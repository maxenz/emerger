Imports System.Data
Imports System.Data.SqlClient

Public Class conEncuestasIncidentes
    Inherits typEncuestasIncidentes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIdByIndex(ByVal pInc As Int64) As Int64
        GetIdByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM EncuestasIncidentes WHERE (IncidenteId = " & pInc & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIdByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIdByIndex", ex)
        End Try

    End Function

    Public Function GetByFechas(pDes As Date, pHas As Date, Optional pFecSel As Integer = 0) As DataTable

        GetByFechas = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT enc.ID, enc.IncidenteId, enc.FecHorEncuesta, inc.FecIncidente, inc.NroIncidente, gdo.AbreviaturaId AS Grado, cli.AbreviaturaId AS Cliente, "
            SQL = SQL & "vij.virTpoLlegada AS Llegada, inc.Paciente, enc.Entrevistado, ISNULL((SELECT TOP 1 1 FROM EncuestasIncidentesItems WHERE EncuestaIncidenteId = enc.ID), 0) AS Estado "
            SQL = SQL & "FROM EncuestasIncidentes enc "
            SQL = SQL & "INNER JOIN Incidentes inc ON enc.IncidenteId = inc.ID "
            SQL = SQL & "INNER JOIN GradosOperativos gdo ON inc.GradoOperativoId = gdo.ID "
            SQL = SQL & "INNER JOIN Clientes cli ON inc.ClienteId = cli.ID "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON inc.ID = dom.IncidenteId "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON dom.ID = vij.IncidenteDomicilioId "
            If pFecSel = 0 Then
                SQL = SQL & "WHERE (inc.FecIncidente BETWEEN '" & DateToSql(pDes, , True) & "' AND '" & DateToSql(pHas, , True, False) & "') "
            Else
                SQL = SQL & "WHERE (enc.FecHorEncuesta BETWEEN '" & DateToSql(pDes, , True) & "' AND '" & DateToSql(pHas, , True, False) & "') "
            End If
            SQL = SQL & "AND (vij.ViajeId = 'IDA') "
            SQL = SQL & "ORDER BY inc.FecIncidente, inc.NroIncidente"

            Dim cmdEmp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdEmp.ExecuteReader)

            dt.Columns.Add("flgEstado", GetType(Boolean))

            For vIdx = 0 To dt.Rows.Count - 1
                dt(vIdx)("flgEstado") = setIntToBool(dt(vIdx)("Estado"))
            Next

            GetByFechas = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByFechas", ex)
        End Try
    End Function

    Public Function GetCantidad(ByVal pDes As Date, pHas As Date, Optional pGdo As Int64 = 0, Optional pCli As Int64 = 0) As Int64
        GetCantidad = 0
        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(COUNT(ID), 0) FROM Incidentes "
            SQL = SQL & "WHERE (FecIncidente BETWEEN '" & DateToSql(pDes) & "' AND '" & DateToSql(pHas) & "') "
            If pGdo > 0 Then SQL = SQL & "AND (GradoOperativoId = " & pGdo & ") "
            If pCli > 0 Then SQL = SQL & "AND (ClienteId = " & pCli & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetCantidad = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetCantidad", ex)
        End Try

    End Function

    Public Sub Recrear(ByVal pCnt As Integer, ByVal pTop As Integer, ByVal pDes As Date, pHas As Date, Optional pGdo As Int64 = 0, Optional pCli As Int64 = 0)

        Try
            Dim SQL As String

            '-----> Armo Temporal

            SQL = "DROP TABLE tmpIncidentesEncuestas"

            Dim cmdDrop As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Try
                cmdDrop.ExecuteNonQuery()
            Catch ex As Exception

            End Try

            SQL = "SELECT row_number() over(ORDER BY ID) AS Registro, ID, 0 AS Used INTO tmpIncidentesEncuestas "
            SQL = SQL & "FROM Incidentes "
            SQL = SQL & "WHERE (FecIncidente BETWEEN '" & DateToSql(pDes) & "' AND '" & DateToSql(pHas) & "') "
            If pGdo > 0 Then SQL = SQL & "AND (GradoOperativoId = " & pGdo & ") "
            If pCli > 0 Then SQL = SQL & "AND (ClienteId = " & pCli & ") "

            Dim cmdTemp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Try
                cmdTemp.ExecuteNonQuery()
            Catch ex As Exception

            End Try

            Randomize()

            '-----> Random
            Dim vCntAdd As Integer = 0
            Dim vTry As Integer = 0

            Do Until vCntAdd = pTop Or vTry = 99999

                Dim vChk As Integer = CInt(Int((pCnt * Rnd()) + 1))

                SQL = "SELECT ID FROM tmpIncidentesEncuestas WHERE Registro = " & vChk & " AND Used = 0"

                cmdTemp.CommandText = SQL
                Dim vOutVal As String = CType(cmdTemp.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then

                    Dim vIncId As Int64 = CType(vOutVal, Int64)

                    Dim objEncuestaIncidente As New conEncuestasIncidentes

                    objEncuestaIncidente.CleanProperties(objEncuestaIncidente)

                    objEncuestaIncidente.IncidenteId.SetObjectId(vIncId)

                    If objEncuestaIncidente.Salvar(objEncuestaIncidente) Then

                        vCntAdd = vCntAdd + 1

                        SQL = "UPDATE tmpIncidentesEncuestas SET Used = 1 WHERE ID = " & vIncId
                        cmdTemp.CommandText = SQL
                        cmdTemp.ExecuteNonQuery()

                    End If

                End If

                vTry = vTry + 1
            Loop


        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetCantidad", ex)
        End Try

    End Sub

    Public Function GetResultados(pDes As Date, pHas As Date, Optional ByVal pIncDes As Date = Nothing, Optional ByVal pIncHas As Date = Nothing, Optional pCli As Int64 = 0, Optional pGdo As Int64 = 0, _
                              Optional pZon As Int64 = 0, Optional pRub As Int64 = 0, Optional pPla As Int64 = 0, Optional pEnc As Int64 = 0, Optional pEncItm As Int64 = 0) As DataTable

        GetResultados = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT CAST(enc.NroEncuesta AS varchar) + '.' + CAST(itm.NroItem AS varchar) AS Item, itm.Descripcion, ISNULL(COUNT(rta.ID), 0) Cantidad, ISNULL(AVG(rta.Valor), 0) Respuesta "

            SQL = SQL & "FROM EncuestasIncidentes eic "
            SQL = SQL & "INNER JOIN EncuestasIncidentesItems det ON eic.ID = det.EncuestaIncidenteId "
            SQL = SQL & "INNER JOIN EncuestasItems itm ON det.EncuestaItemId = itm.ID "
            SQL = SQL & "INNER JOIN EncuestasRespuestas rta ON det.EncuestaRespuestaId = rta.ID "
            SQL = SQL & "INNER JOIN Encuestas enc ON itm.EncuestaId = enc.ID "
            SQL = SQL & "INNER JOIN Incidentes inc ON eic.IncidenteId = inc.ID "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON inc.ID = dom.IncidenteId "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON dom.ID = vij.IncidenteDomicilioId "
            SQL = SQL & "INNER JOIN Localidades loc ON dom.LocalidadId = loc.ID "
            SQL = SQL & "INNER JOIN Clientes cli ON inc.ClienteId = cli.ID "

            SQL = SQL & "WHERE (eic.FecHorEncuesta BETWEEN '" & DateToSql(pDes, , True) & "' AND '" & DateToSql(pHas, , True, False) & "') "
            SQL = SQL & "AND (vij.ViajeId = 'IDA') "

            If pIncDes.Year <> 2999 Then SQL = SQL & "AND (inc.FecIncidente BETWEEN '" & DateToSql(pIncDes, , True) & "' AND '" & DateToSql(pIncHas, , True, False) & "') "
            If pCli > 0 Then SQL = SQL & "AND (inc.ClienteId = " & pCli & ") "
            If pGdo > 0 Then SQL = SQL & "AND (inc.GradoOperativoId = " & pGdo & ") "
            If pZon > 0 Then SQL = SQL & "AND (loc.ZonaGeograficaId = " & pZon & ") "
            If pRub > 0 Then SQL = SQL & "AND (cli.RubroClienteId = " & pRub & ") "
            If pPla > 0 Then SQL = SQL & "AND (cli.PlanId = " & pPla & ") "
            If pEnc > 0 Then SQL = SQL & "AND (itm.EncuestaId = " & pEnc & ") "
            If pEncItm > 0 Then SQL = SQL & "AND (itm.ID = " & pEncItm & ") "

            SQL = SQL & "GROUP BY CAST(enc.NroEncuesta AS varchar) + '.' + CAST(itm.NroItem AS varchar), itm.Descripcion "

            SQL = SQL & "ORDER BY 1"

            Dim cmdRdo As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdRdo.ExecuteReader)

            GetResultados = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetResultados", ex)
        End Try
    End Function


End Class

