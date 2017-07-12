Imports System.Data
Imports System.Data.SqlClient
Public Class conInsumosMovimientos
    Inherits typInsumosMovimientos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Overrides Function Eliminar(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean

        Eliminar = False

        Try

            MyLastExec = New LastExec

            If Me.CanDelete(pId, pMsg) Then

                Dim SQL As String
                Dim objInsumoStock As New conInsumosStock(Me.myCnnName)

                SQL = "SELECT cab.CentroAtencionId, det.InsumoId, tmv.flgIncrementa, det.Cantidad "
                SQL = SQL & "FROM InsumosMovimientosDetalle det "
                SQL = SQL & "INNER JOIN InsumosMovimientos cab ON (det.InsumoMovimientoId = cab.ID) "
                SQL = SQL & "INNER JOIN TiposMovimientosInsumos tmv ON (cab.TipoMovimientoInsumoId = tmv.ID) "
                SQL = SQL & "WHERE (det.InsumoMovimientoId = " & pId & ") "

                Dim cmdDet As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dt As New DataTable
                Dim vIdx As Integer
                dt.Load(cmdDet.ExecuteReader)

                For vIdx = 0 To dt.Rows.Count - 1

                    Dim vInc As Integer = 1
                    If dt(vIdx)("flgIncrementa") = 1 Then vInc = 0

                    SetStock(dt(vIdx)("InsumoId"), dt(vIdx)("CentroAtencionId"), dt(vIdx)("Cantidad"), vInc)

                Next vIdx


                Dim cmOpe As New SqlCommand
                '--------> QUERY el RecordSet
                SQL = "DELETE FROM " & Me.Tabla & " WHERE ID = " & pId
                cmOpe.Connection = cnnsNET(Me.myCnnName)
                cmOpe.Transaction = cnnsTransNET(Me.myCnnName)
                cmOpe.CommandText = SQL
                cmOpe.ExecuteNonQuery()
                Eliminar = True

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Eliminar", ex, pMsg, Me.Tabla, , MyLastExec)
        End Try
    End Function


    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            If Me.FecMovimiento = NullDateMax Then vRdo = "La fecha establecida es incorrecta"
            If Me.TipoMovimientoInsumoId.GetObjectId = 0 Then vRdo = "Debe establecer el tipo de movimiento"
            If Me.CentroAtencionId.GetObjectId = 0 Then vRdo = "Debe establecer el centro de atención"
            If vRdo <> "" Then
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            Else
                Validar = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "RefreshGrid", ex)
        End Try
    End Function

    Public Sub GetInsumos(ByVal pMov As Int64, ByRef dtInsumos As DataTable)
        Try

            Dim SQL As String

            SQL = "SELECT det.ID, det.InsumoId, ISNULL(det.IncidenteId, 0) AS IncidenteId, ISNULL(det.TurnoId, 0) AS TurnoId, "
            SQL = SQL & "inc.FecIncidente, inc.NroIncidente, ISNULL(tur.Paciente, '') AS Paciente, ISNULL(cli.AbreviaturaId, '') AS Cliente, "
            SQL = SQL & "ins.AbreviaturaId, ins.Descripcion, det.Cantidad, det.Importe "
            SQL = SQL & "FROM InsumosMovimientosDetalle det "
            SQL = SQL & "INNER JOIN Insumos ins ON (det.InsumoId = ins.ID) "
            SQL = SQL & "LEFT JOIN Incidentes inc ON (det.IncidenteId = inc.ID) "
            SQL = SQL & "LEFT JOIN Turnos tur ON (det.TurnoId = tur.ID) "
            SQL = SQL & "LEFT JOIN Clientes cli ON (tur.ClienteId = cli.ID) "
            SQL = SQL & "WHERE (det.InsumoMovimientoId = " & pMov & ") "
            SQL = SQL & "ORDER BY ISNULL(det.IncidenteId, 0), ins.AbreviaturaId"

            Dim cmdDet As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            Dim dtColumn As DataColumn
            dt.Load(cmdDet.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                Dim dtRow As DataRow = dtInsumos.NewRow

                For Each dtColumn In dtInsumos.Columns
                    If dtColumn.ColumnName = "Servicio" Then
                        If dt.Rows(vIdx)("IncidenteId") > 0 Then
                            dtRow(dtColumn.ColumnName) = dt.Rows(vIdx)("FecIncidente") & " - " & dt.Rows(vIdx)("NroIncidente")
                        ElseIf dt.Rows(vIdx)("TurnoId") > 0 Then
                            dtRow(dtColumn.ColumnName) = dt.Rows(vIdx)("Cliente") & " - " & dt.Rows(vIdx)("Paciente")
                        Else
                            dtRow(dtColumn.ColumnName) = ""
                        End If
                    Else
                        dtRow(dtColumn.ColumnName) = dt(vIdx)(dtColumn.ColumnName)
                    End If
                Next

                dtInsumos.Rows.Add(dtRow)

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetInsumos", ex)
        End Try
    End Sub

    Public Function GetMovimientosByIncidente(ByVal pInc As Int64) As DataTable

        GetMovimientosByIncidente = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT DISTINCT cab.ID, cab.FecMovimiento, tmv.Descripcion AS TipoMovimiento, mov.Movil, per.Apellido + ' ' + per.Nombre AS Personal "
            SQL = SQL & "FROM InsumosMovimientosDetalle det "
            SQL = SQL & "INNER JOIN InsumosMovimientos cab ON det.InsumoMovimientoId = cab.ID "
            SQL = SQL & "INNER JOIN TiposMovimientosInsumos tmv ON cab.TipoMovimientoInsumoId = tmv.ID "
            SQL = SQL & "LEFT JOIN Moviles mov ON cab.MovilId = mov.ID "
            SQL = SQL & "LEFT JOIN Personal per ON cab.PersonalId = per.ID "
            SQL = SQL & "WHERE (det.IncidenteId = " & pInc  & ") "

            Dim cmdDet As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdDet.ExecuteReader)

            GetMovimientosByIncidente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMovimientosByIncidente", ex)
        End Try
    End Function

    Public Function SetStock(ByVal pIns As Int64, ByVal pCen As Int64, ByVal pCnt As Double, ByVal pInc As Integer) As Boolean
        SetStock = True
        Try
            Dim objStock As New conInsumosStock(Me.myCnnName)
            If Not objStock.Abrir(objStock.GetIDByIndex(pIns, pCen)) Then
                objStock.InsumoId.SetObjectId(pIns)
                objStock.CentroAtencionId.SetObjectId(pCen)
            End If
            If pInc = 1 Then
                objStock.StockActual = objStock.StockActual + pCnt
            Else
                objStock.StockActual = objStock.StockActual - pCnt
            End If
            If objStock.Salvar(objStock) Then
                SetStock = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetStock", ex)
        End Try
    End Function
    Public Function ValidarIncidenteMovil(ByVal pInc As Int64, ByVal pMov As Int64) As Boolean
        ValidarIncidenteMovil = True
        Try
            If pInc > 0 And pMov > 0 Then

                Dim SQL As String

                SQL = "SELECT mvs.ID FROM IncidentesSucesos mvs "
                SQL = SQL & "INNER JOIN IncidentesViajes vij ON (mvs.IncidenteViajeId = vij.ID) "
                SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
                SQL = SQL & "INNER JOIN SucesosIncidentes suc ON (mvs.SucesoIncidenteId = suc.ID) "
                SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ") AND (mvs.MovilId = " & pMov & ") "
                SQL = SQL & "AND (suc.AbreviaturaId = 'A') "

                Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                If cmdFind.ExecuteNonQuery = 0 Then ValidarIncidenteMovil = False

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "ValidarIncidenteMovil", ex)
        End Try
    End Function

End Class
