Imports System.Data
Imports System.Data.SqlClient
Public Class conlckJobs
    Inherits typlckJobs
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Sub JobTraslado()
        Try

            If Not lockRun("TRASLADOS") Then Exit Sub

            Dim SQL As String
            Dim objIncidente As New conIncidentes
            Dim objLckIncidente As New conlckIncidentes
            Dim objViaje As New conIncidentesViajes

            '-------> Corro el Job
            SQL = "SELECT A.ID, B.IncidenteId FROM IncidentesViajes A "
            SQL = SQL & "INNER JOIN IncidentesDomicilios B ON (A.IncidenteDomicilioId = B.ID) "
            SQL = SQL & "WHERE (A.flgStatus = 2) AND (A.reqHorLlegada <= '" & DateToSql(Now.Date) & " 23:59:59') "
            SQL = SQL & "ORDER BY A.reqHorLlegada"

            Dim cmdJob As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdJob.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1
                '----> Le pongo nro. de incidente
                If objIncidente.Abrir(dt(vIdx)(1)) Then
                    If objIncidente.NroIncidente = "" Then
                        objIncidente.FecIncidente = Now.Date
                        If shamanConfig.modNumeracion = 0 Then
                            objIncidente.NroIncidente = objLckIncidente.getNewIncidente(Now.Date)
                        Else
                            objIncidente.NroIncidente = objIncidente.TrasladoId
                        End If
                    End If
                    If objIncidente.Salvar(objIncidente) Then
                        If objViaje.Abrir(dt(vIdx).Item(0)) Then
                            objViaje.flgStatus = 0
                            objViaje.Salvar(objViaje)
                        End If
                    End If
                End If

            Next vIdx

            '-------> Job
            SQL = "SELECT inc.ID FROM IncidentesViajes vij "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON (vij.IncidenteDomicilioId = dom.ID) "
            SQL = SQL & "INNER JOIN Incidentes inc ON (dom.IncidenteId = inc.ID) "
            SQL = SQL & "WHERE (vij.flgStatus = 0) AND (inc.NroIncidente = '') "

            cmdJob.CommandText = SQL
            Dim dtNro As New DataTable
            dtNro.Load(cmdJob.ExecuteReader)

            For vIdx = 0 To dtNro.Rows.Count - 1
                '----> Le pongo nro. de incidente
                If objIncidente.Abrir(dtNro(vIdx)(0)) Then
                    If shamanConfig.modNumeracion = 0 Then
                        objIncidente.NroIncidente = objLckIncidente.getNewIncidente(objIncidente.FecIncidente)
                    Else
                        objIncidente.NroIncidente = objIncidente.TrasladoId
                    End If
                    If objIncidente.Salvar(objIncidente) Then
                    End If
                End If

            Next vIdx

            '-------> Cierro el JOB
            unlockRun("TRASLADOS")

        Catch ex As Exception
            HandleError(Me.GetType.Name, "JobTraslado", ex)
        End Try

    End Sub
    Public Sub JobMovilesRetorno()
        Try

            If Not lockRun("REGMOVIL") Then Exit Sub

            Dim SQL As String
            Dim objMovilActual As New conMovilesActuales(Me.myCnnName)
            Dim objSucesoIncidente As New conSucesosIncidentes(Me.myCnnName)

            '-------> Corro el Job
            SQL = "SELECT mov.ID, mov.FechaHoraMovimiento, vij.virTpoDesplazamiento "
            SQL = SQL & "FROM MovilesActuales mov "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON (mov.IncidenteViajeId = vij.ID) "
            SQL = SQL & "INNER JOIN SucesosIncidentes suc ON (mov.SucesoIncidenteId = suc.ID) "
            SQL = SQL & "WHERE (suc.AbreviaturaId = 'R') "

            Dim cmdJob As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdJob.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                Dim vFecReq As DateTime = DateAdd(DateInterval.Minute, dt(vIdx).Item(2) * 2, dt(vIdx).Item(1))

                If vFecReq <= GetCurrentTime() Then
                    If objMovilActual.Abrir(dt(vIdx).Item(0)) Then
                        objMovilActual.SucesoIncidenteId.SetObjectId(objSucesoIncidente.GetIDByAbreviaturaId("L"))
                        objMovilActual.FechaHoraMovimiento = GetCurrentTime()
                        objMovilActual.IncidenteViajeId.SetObjectId(0)
                        objMovilActual.LocalidadId.SetObjectId(objMovilActual.BaseOperativaId.LocalidadId.ID)
                        objMovilActual.Salvar(objMovilActual)
                    End If
                End If

            Next vIdx

            objMovilActual = Nothing
            objSucesoIncidente = Nothing
            '-------> Cierro el JOB
            unlockRun("REGMOVIL")

        Catch ex As Exception
            HandleError(Me.GetType.Name, "JobMovilesRetorno", ex)
        End Try

    End Sub
    Public Sub JobMovilesCondicionales()
        Try

            If Not lockRun("CNDMOVIL") Then Exit Sub

            Dim SQL As String
            Dim objMovilActual As New conMovilesActuales(Me.myCnnName)
            Dim objSucesoIncidente As New conSucesosIncidentes(Me.myCnnName)

            '-------> Corro el Job
            SQL = "SELECT mov.ID, mov.FechaHoraMovimiento, mov.TpoCondicional "
            SQL = SQL & "FROM MovilesActuales mov "
            SQL = SQL & "INNER JOIN SucesosIncidentes suc ON (mov.SucesoIncidenteId = suc.ID) "
            SQL = SQL & "WHERE (suc.AbreviaturaId = 'C') "

            Dim cmdJob As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdJob.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                Dim vFecReq As DateTime = DateAdd(DateInterval.Minute, dt(vIdx).Item(2), dt(vIdx).Item(1))

                If vFecReq <= GetCurrentTime() Then
                    If objMovilActual.Abrir(dt(vIdx).Item(0)) Then
                        objMovilActual.SucesoIncidenteId.SetObjectId(objSucesoIncidente.GetIDByAbreviaturaId("L"))
                        objMovilActual.FechaHoraMovimiento = GetCurrentTime()
                        objMovilActual.IncidenteViajeId.SetObjectId(0)
                        objMovilActual.LocalidadId.SetObjectId(objMovilActual.BaseOperativaId.LocalidadId.ID)
                        objMovilActual.MotivoCondicionalId.SetObjectId(0)
                        objMovilActual.TpoCondicional = 0
                        objMovilActual.Salvar(objMovilActual)
                    End If
                End If

            Next vIdx

            objMovilActual = Nothing
            objSucesoIncidente = Nothing
            '-------> Cierro el JOB
            unlockRun("CNDMOVIL")

        Catch ex As Exception
            HandleError(Me.GetType.Name, "JobMovilesCondicionales", ex)
        End Try

    End Sub
    Public Function lockRun(ByVal pJob As String, Optional ByVal pJobSub As Int64 = 0) As Boolean
        lockRun = False
        Try
            Dim objJob As New conlckJobs, vRun As Boolean = False

            If objJob.Abrir(objJob.getIDByIndex(pJob, pJobSub)) Then
                If objJob.flgStatus = 0 Then
                    objJob.flgStatus = 1
                    objJob.ultEjecucion = GetCurrentTime()
                    vRun = objJob.Salvar(objJob)
                Else
                    Dim vTpoMin As Integer
                    Select Case pJob
                        Case "GRILLA" : vTpoMin = 1
                        Case "REGMOVIL" : vTpoMin = 8
                        Case Else : vTpoMin = 3
                    End Select

                    If Math.Abs(DateDiff(DateInterval.Minute, objJob.ultEjecucion, Now)) >= vTpoMin Then
                        objJob.flgStatus = 1
                        objJob.ultEjecucion = GetCurrentTime()
                        vRun = objJob.Salvar(objJob)
                    End If

                End If
            Else
                objJob.JobName = pJob
                objJob.JobSubId = pJobSub
                objJob.ultEjecucion = GetCurrentTime()
                objJob.flgStatus = 1
                vRun = objJob.Salvar(objJob)
            End If
            lockRun = vRun
        Catch ex As Exception
            HandleError(Me.GetType.Name, "lockRun", ex)
        End Try
    End Function
    Public Function unlockRun(ByVal pJob As String, Optional ByVal pJobSub As Int64 = 0) As Boolean
        unlockRun = False
        Try
            Dim objJob As New conlckJobs, vRun As Boolean = False

            If objJob.Abrir(objJob.getIDByIndex(pJob, pJobSub)) Then
                objJob.flgStatus = 0
                vRun = objJob.Salvar(objJob)
            End If

            unlockRun = vRun
        Catch ex As Exception
            HandleError(Me.GetType.Name, "unlockRun", ex)
        End Try
    End Function
    Private Function getIDByIndex(ByVal pJob As String, Optional ByVal pJobSub As Int64 = 0) As Int64
        getIDByIndex = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM lckJobs WHERE (JobName = '" & pJob & "') AND (JobSubId = " & pJobSub & ")"
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getIDByIndex = CType(vOutVal, Int64)
        Catch ex As Exception
            HandleError(Me.GetType.Name, "getIDByIndex", ex)
        End Try
    End Function

End Class
