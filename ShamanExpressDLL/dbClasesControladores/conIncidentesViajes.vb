Imports System.Data
Imports System.Data.SqlClient
Public Class conIncidentesViajes
    Inherits typIncidentesViajes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pInc As Int64, Optional ByVal pVij As String = "IDA") As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String
            SQL = "SELECT A.ID FROM IncidentesViajes A INNER JOIN IncidentesDomicilios B ON (A.IncidenteDomicilioId = B.ID) "
            SQL = SQL & "WHERE B.IncidenteId = " & pInc & " AND A.ViajeId = '" & pVij & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function GetLastSuceso(ByVal pVij As Int64) As String
        GetLastSuceso = ""
        Try
            Dim SQL As String
            SQL = "SELECT TOP 1 B.AbreviaturaId FROM IncidentesSucesos A "
            SQL = SQL & "INNER JOIN SucesosIncidentes B ON (A.SucesoIncidenteId = B.ID) "
            SQL = SQL & "WHERE (A.IncidenteViajeId = " & pVij & ") AND (Condicion <> 'APO') "
            SQL = SQL & "ORDER BY Orden DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetLastSuceso = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetLastSuceso", ex)
        End Try
    End Function
    Public Function GetNextViaje(ByVal pVij As Int64) As Int64
        GetNextViaje = 0
        Try
            Dim objDomicilio As New conIncidentesDomicilios(Me.myCnnName)
            If Me.Abrir(pVij) Then
                If Me.IncidenteDomicilioId.IncidenteId.GradoOperativoId.ClasificacionId = gdoClasificacion.gdoTraslado Then
                    '------> Atendiendo una IDA
                    If Me.IncidenteDomicilioId.TipoDomicilio = 0 Then
                        GetNextViaje = objDomicilio.GetIDByIndex(Me.IncidenteDomicilioId.IncidenteId.ID, 1)
                    ElseIf Me.IncidenteDomicilioId.TipoDomicilio = 1 Then
                        GetNextViaje = objDomicilio.GetIDByIndex(Me.IncidenteDomicilioId.IncidenteId.ID, 0)
                    End If
                End If
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNextViaje", ex)
        End Try
    End Function
    Public Function haveViajesAnulacion(ByVal pInc As Int64) As Boolean
        haveViajesAnulacion = False

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 vij.ID FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ") AND (ISNULL(vij.MotivoNoRealizacionId, 0) = 0) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmFind.ExecuteScalar > 0 Then haveViajesAnulacion = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "haveViajesAnulacion", ex)
        End Try
    End Function
    Public Function haveViajesRecuperacion(ByVal pInc As Int64) As Boolean
        haveViajesRecuperacion = False

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 vij.ID FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ") AND (ISNULL(vij.MotivoNoRealizacionId, 0) > 0) "

            Dim cmdHave As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmdHave.ExecuteScalar > 0 Then

                SQL = "SELECT ISNULL(COUNT(ID), 0) FROM IncidentesDomicilios "
                SQL = SQL & "WHERE (IncidenteId = " & pInc & ") "

                cmdHave.CommandText = SQL
                Dim vCntVij As Integer = cmdHave.ExecuteScalar

                If vCntVij = 1 Then
                    haveViajesRecuperacion = True
                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "haveViajesRecuperacion", ex)
        End Try
    End Function
    Public Function RecuperarViaje(pInc As Int64) As Boolean

        Dim cnnKey As String = "Recuperacion"

        RecuperarViaje = False

        Try

            If shamanStartUp.AbrirConexion(cnnKey) Then

                cnnsTransNET.Add(cnnKey, cnnsNET(cnnKey).BeginTransaction)

                Dim vSav As Boolean = False

                Dim objIncidente As New conIncidentes(cnnKey)
                Dim objViaje As New conIncidentesViajes(cnnKey)
                Dim objIncidentesObservaciones As New conIncidentesObservaciones(cnnKey)

                If objIncidente.Abrir(pInc) Then

                    If objViaje.Abrir(objViaje.GetIDByIndex(pInc)) Then

                        Dim vMot As String = objViaje.MotivoNoRealizacionId.Descripcion
                        Dim vMov As String = objViaje.MovilId.Movil

                        objViaje.MotivoNoRealizacionId.SetObjectId(0)
                        objViaje.MovilId.SetObjectId(0)
                        objViaje.MovilPreasignadoId.SetObjectId(0)
                        objViaje.flgStatus = 0

                        If objViaje.Salvar(objViaje) Then
                            '-----> To Log
                            Dim vObs As String = "FINALIZO MOVIL " & vMov & " POR MOTIVO " & vMot

                            objIncidentesObservaciones.CleanProperties(objIncidentesObservaciones)
                            objIncidentesObservaciones.IncidenteId.SetObjectId(pInc)
                            objIncidentesObservaciones.Observaciones = vObs

                            If objIncidentesObservaciones.Salvar(objIncidentesObservaciones) Then
                                objIncidente.Observaciones = objIncidentesObservaciones.GetByIncidenteId(pInc)
                                If objIncidente.Salvar(objIncidente) Then
                                    vSav = True
                                End If
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

                objIncidente = Nothing
                objViaje = Nothing
                objIncidentesObservaciones = Nothing

            End If

        Catch ex As Exception

            If cnnsNET.Contains(cnnKey) Then
                cnnsTransNET(cnnKey).Rollback()
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)
            End If

            HandleError(Me.GetType.Name, "RecuperarViaje", ex)

        End Try
    End Function
    Public Function CanAnular(ByVal pVij As Int64, Optional pMsg As Boolean = True) As Boolean

        CanAnular = True

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 mov.Movil, sus.Descripcion "
            SQL = SQL & "FROM MovilesActuales mvc "
            SQL = SQL & "INNER JOIN Moviles mov ON mvc.MovilId = mov.ID "
            SQL = SQL & "INNER JOIN SucesosIncidentes sus ON mvc.SucesoIncidenteId = sus.ID "
            SQL = SQL & "WHERE (mvc.IncidenteViajeId = " & pVij & ") "
            SQL = SQL & "AND ((sus.AbreviaturaId = 'A') OR (sus.AbreviaturaId = 'D') OR (sus.AbreviaturaId = 'H')) "

            Dim cmdMov As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdMov.ExecuteReader)

            If dt.Rows.Count > 0 Then

                CanAnular = False

                If pMsg Then
                    MsgBox("El móvil " & dt(0)(0) & " se encuentra en estado " & dt(0)(1) & vbCrLf & "Finalice el servicio", MsgBoxStyle.Critical, "Finalización")
                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CanAnular", ex)
        End Try
    End Function

    Public Function Anular(ByVal pVij As Int64, ByVal pMot As Decimal, ByVal pObs As String) As Boolean

        Dim cnnKey As String = "Anulacion"

        Anular = False

        Try

            If shamanStartUp.AbrirConexion(cnnKey) Then

                Dim objViaje As New conIncidentesViajes(cnnKey)
                Dim objObservacion As New conIncidentesObservaciones(cnnKey)
                Dim objIncidente As New conIncidentes(cnnKey)
                Dim objIncidenteSuceso As New conIncidentesSucesos(cnnKey)
                Dim objSucesoId As New conSucesosIncidentes(cnnKey)

                Dim vSav As Boolean = False
                Dim vFnd As Boolean = False

                Me.myCnnName = cnnKey

                cnnsTransNET.Add(cnnKey, cnnsNET(cnnKey).BeginTransaction)

                If objViaje.Abrir(pVij) Then

                    vSav = True

                    '-----> Libero los móviles involucrados
                    Dim SQL As String

                    SQL = "SELECT mov.MovilId FROM MovilesActuales mov "
                    SQL = SQL & "INNER JOIN SucesosIncidentes suc ON (mov.SucesoIncidenteId = suc.ID) "
                    SQL = SQL & "WHERE (mov.IncidenteViajeId = " & pVij & ") AND (suc.AbreviaturaId <> 'R') "

                    Dim cmdMov As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    Dim dt As New DataTable
                    Dim vIdx As Integer = 0
                    dt.Load(cmdMov.ExecuteReader)

                    Do Until vIdx = dt.Rows.Count Or Not vSav

                        vFnd = True

                        objIncidenteSuceso.CleanProperties(objIncidenteSuceso)
                        objIncidenteSuceso.IncidenteViajeId.SetObjectId(pVij)
                        objIncidenteSuceso.FechaHoraSuceso = GetCurrentTime()
                        objIncidenteSuceso.SucesoIncidenteId.SetObjectId(objSucesoId.GetIDByAbreviaturaId("R"))
                        objIncidenteSuceso.MovilId.SetObjectId(dt(vIdx).Item(0))
                        objIncidenteSuceso.Condicion = "TIT"

                        vSav = objIncidenteSuceso.addSuceso(objIncidenteSuceso, , pMot, , True)

                        vIdx = vIdx + 1

                    Loop

                    If Not vFnd Then

                        objViaje.flgStatus = 1
                        objViaje.MotivoNoRealizacionId.SetObjectId(pMot)
                        Dim vIncDom As Int64 = objIncidenteSuceso.GetDomicilioInicial(objViaje.IncidenteDomicilioId.IncidenteId.ID, objViaje.ViajeId)
                        If vIncDom > 0 Then objViaje.IncidenteDomicilioId.SetObjectId(vIncDom)

                        vSav = objViaje.Salvar(objViaje)

                    End If

                    If vSav Then

                        SQL = "SELECT vij.ID "
                        SQL = SQL & "FROM IncidentesProgramaciones prg "
                        SQL = SQL & "INNER JOIN Incidentes inc ON (prg.IncidenteId = inc.ID) "
                        SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (prg.IncidentePrgId = dom.IncidenteId) "
                        SQL = SQL & "INNER JOIN IncidentesViajes vij ON (dom.ID = vij.IncidenteDomicilioId) "
                        SQL = SQL & "WHERE (prg.IncidenteId = " & objViaje.IncidenteDomicilioId.IncidenteId.ID & ") "
                        SQL = SQL & "AND (vij.ViajeId = '" & objViaje.ViajeId & "') "
                        SQL = SQL & "AND (prg.flgSimultaneo = 1) "

                        cmdMov.CommandText = SQL
                        Dim dtSim As New DataTable
                        dtSim.Load(cmdMov.ExecuteReader)

                        For vIdx = 0 To dtSim.Rows.Count - 1

                            If objViaje.Abrir(dtSim(vIdx)(0)) Then
                                objViaje.flgStatus = 1
                                objViaje.MotivoNoRealizacionId.SetObjectId(pMot)
                                Dim vIncDom As Int64 = objIncidenteSuceso.GetDomicilioInicial(objViaje.IncidenteDomicilioId.IncidenteId.ID, objViaje.ViajeId)
                                If vIncDom > 0 Then objViaje.IncidenteDomicilioId.SetObjectId(vIncDom)
                                If Not objViaje.Salvar(objViaje) Then
                                    vSav = False
                                End If
                            End If

                        Next vIdx

                    End If

                    If vSav Then

                        If objIncidente.Abrir(objViaje.IncidenteDomicilioId.IncidenteId.ID) Then

                            If pObs <> "" Then
                                objObservacion.CleanProperties(objObservacion)
                                objObservacion.IncidenteId.SetObjectId(objIncidente.ID)
                                objObservacion.Observaciones = pObs

                                If objObservacion.Salvar(objObservacion) Then
                                    objIncidente.Observaciones = objObservacion.GetByIncidenteId(objIncidente.ID)
                                    vSav = objIncidente.Salvar(objIncidente)
                                End If
                            End If

                            If vSav Then

                                If shamanConfig.modNumeracion = 1 And objIncidente.NroIncidente = "" Then

                                    '----> Determino si hay mas viajes
                                    SQL = "SELECT TOP 1 vij.ID FROM IncidentesViajes vij "
                                    SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON vij.IncidenteDomicilioId = dom.ID "
                                    SQL = SQL & "WHERE dom.IncidenteId = " & objIncidente.ID & " AND vij.flgStatus = 2 "

                                    cmdMov.CommandText = SQL
                                    Dim dtPen As New DataTable
                                    dtPen.Load(cmdMov.ExecuteReader)

                                    If dtPen.Rows.Count = 0 Then
                                        objIncidente.NroIncidente = objIncidente.TrasladoId.ToString
                                        vSav = objIncidente.Salvar(objIncidente)
                                    End If

                                End If

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

                Anular = vSav

            End If

        Catch ex As Exception

            If cnnsNET.Contains(cnnKey) Then
                cnnsTransNET(cnnKey).Rollback()
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)
            End If

            HandleError(Me.GetType.Name, "SetIncidente", ex)

        End Try

    End Function

    Public Function GetTripulanteId(pIncVij As Int64, Optional pTrp As String = "MED") As Int64

        GetTripulanteId = 0

        Try
            If Me.Abrir(pIncVij) Then

                Dim vFhr As DateTime = Me.HorarioOperativo.horLlegada
                If vFhr = NullDateTime Then vFhr = Me.HorarioOperativo.horSalida

                '----> Obtengo Tripulante...

                If vFhr <> NullDateTime Then

                    Dim SQL As String

                    SQL = "SELECT PersonalId FROM GrillaOperativaTripulantes WHERE (MovilId = '" & Me.MovilId.ID & "') "
                    SQL = SQL & "AND (('" & DateTimeToSql(vFhr) & "' BETWEEN FicEntrada AND FicSalida) "
                    SQL = SQL & "OR ('" & DateTimeToSql(vFhr) & "' BETWEEN PacEntrada AND PacSalida)) "
                    SQL = SQL & "AND (PuestoGrilla = '" & pTrp & "') "

                    Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
                    If Not vOutVal Is Nothing Then GetTripulanteId = CType(vOutVal, Int64)

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetTripulanteId", ex)
        End Try
    End Function

    Public Function GetViajes(ByVal pInc As Int64, Optional ByVal pVista As vijCombo = vijCombo.vijTodos) As DataTable

        GetViajes = Nothing

        Try

            Dim SQL As String
            SQL = "SELECT vij.ID, vij.ViajeId FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & ") "
            Select Case pVista
                Case vijCombo.vijConDiagnostico : SQL = SQL & "AND (ISNULL(vij.MotivoNoRealizacionId, 0) = 0) "
                Case vijCombo.vijConCierre : SQL = SQL & "AND (vij.flgStatus = 1) "
            End Select

            SQL = SQL & "ORDER BY vij.ViajeId"

            Dim cmdVij As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdVij.ExecuteReader)

            GetViajes = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetViajes", ex)
        End Try

    End Function


End Class
