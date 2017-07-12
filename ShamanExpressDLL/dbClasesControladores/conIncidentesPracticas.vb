Imports System.Data
Imports System.Data.SqlClient
Public Class conIncidentesPracticas
    Inherits typIncidentesPracticas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pInc As Int64, Optional ByVal pCla As Integer = 0) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM IncidentesPracticas WHERE IncidenteId = " & pInc & " AND Clasificacion = " & pCla

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function GetPracticasByIncidentePractica(ByVal pIncPra As Int64) As DataTable

        GetPracticasByIncidentePractica = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT inc.ID, inc.SanatorioId, san.Descripcion AS Sanatorio, inc.PracticaId, pra.Descripcion AS Practica, inc.Cantidad "
            SQL = SQL & "FROM IncidentesPracticasDetalle inc "
            SQL = SQL & "INNER JOIN IncidentesPracticas cab ON (inc.IncidentePracticaId = cab.ID) "
            SQL = SQL & "LEFT JOIN Sanatorios san ON (inc.SanatorioId = san.ID) "
            SQL = SQL & "LEFT JOIN Practicas pra ON (inc.PracticaId = pra.ID) "
            SQL = SQL & "WHERE (inc.IncidentePracticaId = " & pIncPra & ") "
            SQL = SQL & "AND (ISNULL(cab.Clasificacion, 0) = 0) "
            SQL = SQL & "ORDER BY san.Descripcion, pra.Descripcion"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdGrl.ExecuteReader)

            GetPracticasByIncidentePractica = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetPracticasByIncidentePractica", ex)
        End Try
    End Function
    Public Function GetPrestacionesByIncidentePractica(ByVal pIncPra As Int64) As DataTable

        GetPrestacionesByIncidentePractica = Nothing

        Try
            Dim SQL As String
            Dim dt As New DataTable

            dt.Columns.Add("ID", GetType(Int64))
            dt.Columns.Add("PracticaId", GetType(Int64))
            dt.Columns.Add("Seleccion", GetType(Boolean))
            dt.Columns.Add("Prestacion", GetType(String))
            dt.Columns.Add("Cantidad", GetType(Integer))

            SQL = "SELECT ID, Descripcion FROM Practicas ORDER BY Descripcion"

            Dim cmdPra As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dtPra As New DataTable
            Dim vIdx As Integer
            dtPra.Load(cmdPra.ExecuteReader)

            For vIdx = 0 To dtPra.Rows.Count - 1

                Dim objDetalle As New conIncidentesPracticasDetalle(Me.myCnnName)

                Dim dtRow As DataRow = dt.NewRow

                If objDetalle.Abrir(objDetalle.GetIDByIndex(pIncPra, 0, dtPra(vIdx)("ID"))) Then
                    dtRow("ID") = objDetalle.ID
                    dtRow("Seleccion") = True
                    dtRow("Cantidad") = objDetalle.Cantidad
                Else
                    dtRow("ID") = 0
                    dtRow("Seleccion") = False
                    dtRow("Cantidad") = 0
                End If

                dtRow("PracticaId") = dtPra(vIdx)("ID")
                dtRow("Prestacion") = dtPra(vIdx)("Descripcion")

                dt.Rows.Add(dtRow)

            Next vIdx

            GetPrestacionesByIncidentePractica = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetPrestacionesByIncidentePractica", ex)
        End Try
    End Function

    Public Sub SetPrestaciones(ByVal pId As Int64, ByVal pTblView As DataView)
        Try

            Dim vIdx As Integer, objDetalle As conIncidentesPracticasDetalle

            Dim SQL As String = "UPDATE IncidentesPracticasDetalle SET flgPurge = 1 WHERE IncidentePracticaId = " & pId
            Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdUpd.ExecuteNonQuery()

            Dim vTblPre As DataTable = pTblView.Table

            For vIdx = 0 To vTblPre.Rows.Count - 1

                Dim dtRow As DataRow = vTblPre.Rows(vIdx)

                If dtRow("Seleccion") Then

                    objDetalle = New conIncidentesPracticasDetalle(Me.myCnnName)

                    If Not objDetalle.Abrir(dtRow("ID")) Then
                        objDetalle.IncidentePracticaId.SetObjectId(pId)
                        objDetalle.PracticaId.SetObjectId(dtRow("PracticaId"))
                    End If

                    objDetalle.Cantidad = dtRow("Cantidad")
                    objDetalle.flgPurge = 0
                    objDetalle.Salvar(objDetalle)
                    objDetalle = Nothing

                End If

            Next

            SQL = "DELETE FROM IncidentesPracticasDetalle WHERE IncidentePracticaId = " & pId & " AND flgPurge = 1"
            cmdUpd.CommandText = SQL
            cmdUpd.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPrestaciones", ex)
        End Try
    End Sub

    Public Sub SetPrestacionesLaboratorio(ByVal pInc As Int64, ByVal pLabTst As Integer, ByVal pLabCnt As Integer)
        Try
            If pLabTst > 0 Then

                Dim objIncidentePracticas As New conIncidentesPracticas

                If Not objIncidentePracticas.Abrir(objIncidentePracticas.GetIDByIndex(pInc, 1)) Then
                    objIncidentePracticas.IncidenteId.SetObjectId(pInc)
                    objIncidentePracticas.Clasificacion = 1
                    If Not objIncidentePracticas.Salvar(objIncidentePracticas) Then
                        Exit Sub
                    End If
                End If

                Dim SQL As String

                SQL = "SELECT ID FROM Practicas WHERE ISNULL(flgLaboratorioTest, 0) = 1"

                Dim cmdPra As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dtPra As New DataTable
                Dim vIdx As Integer
                dtPra.Load(cmdPra.ExecuteReader)

                For vIdx = 0 To dtPra.Rows.Count - 1
                    Dim objDetalle As New conIncidentesPracticasDetalle
                    If Not objDetalle.Abrir(objDetalle.GetIDByIndex(objIncidentePracticas.ID, 0, dtPra(vIdx)("ID"))) Then
                        objDetalle.IncidentePracticaId.SetObjectId(objIncidentePracticas.ID)
                        objDetalle.PracticaId.SetObjectId(dtPra(vIdx)("ID"))
                        objDetalle.Cantidad = pLabCnt
                        objDetalle.Salvar(objDetalle)
                    Else
                        If objDetalle.Cantidad < pLabCnt Then objDetalle.Cantidad = pLabCnt
                        objDetalle.Salvar(objDetalle)
                    End If
                    objDetalle = Nothing
                Next vIdx

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPrestacionesLaboratorio", ex)
        End Try
    End Sub

    Public Function GetQueryBase(ByVal pDes As Date, ByVal pHas As Date, Optional ByVal pSan As Int64 = 0, Optional ByVal pPra As Int64 = 0, Optional ByVal pMed As Int64 = 0) As DataTable

        GetQueryBase = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT pra.ID, LEFT(CONVERT(VARCHAR, inc.FecIncidente, 3), 10) + ' - ' + inc.NroIncidente AS Incidente, cli.AbreviaturaId AS Cliente, "
            SQL = SQL & "cli.RazonSocial, inc.Paciente, mov.Movil, per.Apellido + ', ' + per.Nombre AS Medico, san.Descripcion AS Sanatorio, "
            SQL = SQL & "pde.Descripcion AS Practica, pra.Cantidad, CASE pra.flgConsumida WHEN 0 THEN 'No' ELSE 'Si' END AS Realizada, pra.NroComprobante AS Comprobante, "
            SQL = SQL & "pra.Importe, pra.MedicoSanatorio AS Recibe, pra.Observaciones "
            SQL = SQL & "FROM IncidentesPracticasDetalle pra "
            SQL = SQL & "INNER JOIN IncidentesPracticas cab ON (pra.IncidentePracticaId = cab.ID) "
            SQL = SQL & "INNER JOIN Incidentes inc ON (cab.IncidenteId = inc.ID) "
            SQL = SQL & "INNER JOIN Clientes cli ON (inc.ClienteId = cli.ID) "
            SQL = SQL & "LEFT JOIN Moviles mov ON (cab.MovilId = mov.ID) "
            SQL = SQL & "LEFT JOIN Personal per ON (cab.MedicoId = per.ID) "
            SQL = SQL & "LEFT JOIN Sanatorios san ON (pra.SanatorioId = san.ID) "
            SQL = SQL & "INNER JOIN Practicas pde ON (pra.PracticaId = pde.ID) "
            SQL = SQL & "WHERE (inc.FecIncidente BETWEEN '" & DateToSql(pDes) & "' AND '" & DateToSql(pHas) & "') "
            If pSan > 0 Then SQL = SQL & "AND (pra.SanatorioId = " & pSan & ") "
            If pPra > 0 Then SQL = SQL & "AND (pra.PracticaId = " & pPra & ") "
            If pMed > 0 Then SQL = SQL & "AND (cab.MedicoId = " & pMed & ") "
            SQL = SQL & "ORDER BY inc.FecIncidente, inc.NroIncidente"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdGrl.ExecuteReader)

            GetQueryBase = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryBase", ex)
        End Try
    End Function

    Public Sub SetPracticas(ByVal pId As Int64, ByVal dt As DataTable)
        Try

            Dim vIdx As Integer, objDetalle As conIncidentesPracticasDetalle

            Dim SQL As String = "UPDATE IncidentesPracticasDetalle SET flgPurge = 1 WHERE IncidentePracticaId = " & pId
            Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdUpd.ExecuteNonQuery()

            For vIdx = 0 To dt.Rows.Count - 1

                objDetalle = New conIncidentesPracticasDetalle(Me.myCnnName)

                If Not objDetalle.Abrir(dt(vIdx)("ID")) Then
                    objDetalle.IncidentePracticaId.SetObjectId(pId)
                End If

                objDetalle.SanatorioId.SetObjectId(dt(vIdx)("SanatorioId"))
                objDetalle.PracticaId.SetObjectId(dt(vIdx)("PracticaId"))
                objDetalle.Cantidad = dt(vIdx)("Cantidad")
                objDetalle.flgPurge = 0
                objDetalle.Salvar(objDetalle)
                objDetalle = Nothing

            Next

            SQL = "DELETE FROM IncidentesPracticasDetalle WHERE IncidentePracticaId = " & pId & " AND flgPurge = 1"
            cmdUpd.CommandText = SQL
            cmdUpd.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPracticas", ex)
        End Try
    End Sub

End Class
