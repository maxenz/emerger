Imports System.Data
Imports System.Data.SqlClient
Public Class conMovilesSucesos
    Inherits typMovilesSucesos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function SetSucesoIncidente(ByVal objMovilSuceso As conMovilesSucesos, Optional pTpo As Integer = 0, Optional pRunTrigger As Boolean = True) As Boolean
        SetSucesoIncidente = False
        Try

            Dim vSav As Boolean = False

            If objMovilSuceso.Salvar(objMovilSuceso) Then

                vSav = True

                If pRunTrigger Then
                    '----> Pongo el móvil actual
                    Dim objActual As New conMovilesActuales(objMovilSuceso.myCnnName)

                    If objActual.Abrir(objActual.GetIDByIndex(objMovilSuceso.MovilId.ID)) Then
                        If objActual.FechaHoraMovimiento <= objMovilSuceso.FechaHoraSuceso Then
                            '------> Reviso si estaba en camino a algun incidente...
                            If objActual.IncidenteViajeId.ID <> objMovilSuceso.IncidenteSucesoId.IncidenteViajeId.ID Then
                                If objActual.SucesoIncidenteId.AbreviaturaId = "S" Or objActual.SucesoIncidenteId.AbreviaturaId = "B" Then
                                    Me.InterrumpirViaje(objActual.IncidenteViajeId.ID, objActual.MovilId.ID)
                                End If
                            End If
                            If pTpo = 0 Then
                                objActual.SucesoIncidenteId.SetObjectId(objMovilSuceso.IncidenteSucesoId.SucesoIncidenteId.ID)
                            Else
                                Dim objSuceso As New conSucesosIncidentes(objMovilSuceso.myCnnName)
                                objActual.SucesoIncidenteId.SetObjectId(objSuceso.GetIDByAbreviaturaId("C"))
                                objSuceso = Nothing
                            End If
                            objActual.FechaHoraMovimiento = objMovilSuceso.FechaHoraSuceso
                            objActual.IncidenteViajeId.SetObjectId(objMovilSuceso.IncidenteSucesoId.IncidenteViajeId.ID)
                            If objMovilSuceso.IncidenteSucesoId.SucesoIncidenteId.AbreviaturaId <> "S" Then
                                objActual.LocalidadId.SetObjectId(objMovilSuceso.IncidenteSucesoId.IncidenteViajeId.IncidenteDomicilioId.LocalidadId.ID)
                            End If
                            objActual.MotivoCondicionalId.SetObjectId(objMovilSuceso.MotivoCondicionalId.ID)
                            objActual.TpoCondicional = pTpo
                            vSav = objActual.Salvar(objActual)
                        End If
                    End If

                    objActual = Nothing
                End If
            Else
                vSav = False
            End If

            SetSucesoIncidente = vSav
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetSucesoIncidente", ex)
        End Try
    End Function
    Private Function InterrumpirViaje(ByVal pVij As Long, pMov As Long) As Boolean
        InterrumpirViaje = False
        Try
            Dim objViaje As New conIncidentesViajes(Me.myCnnName)
            Dim objIncidenteSuceso As New conIncidentesSucesos(Me.myCnnName)

            If pVij > 0 Then
                If objViaje.Abrir(pVij) Then
                    '----> Acomo el viaje
                    objViaje.MovilId.SetObjectId(objIncidenteSuceso.GetMovilPendiente(pVij, pMov))
                    If objViaje.MovilId.GetObjectId = 0 Then
                        objViaje.HorarioOperativo.horDespacho = NullDateMax
                        objViaje.HorarioOperativo.horSalida = NullDateMax
                    End If

                    If objViaje.Salvar(objViaje) Then
                        '----> Agrego Historial

                        Dim objSuceso As New conSucesosIncidentes(Me.myCnnName)

                        objIncidenteSuceso.CleanProperties(objIncidenteSuceso)
                        objIncidenteSuceso.IncidenteViajeId.SetObjectId(pVij)
                        objIncidenteSuceso.FechaHoraSuceso = GetCurrentTime()
                        objIncidenteSuceso.SucesoIncidenteId.SetObjectId(objSuceso.GetIDByAbreviaturaId("T"))
                        objIncidenteSuceso.MovilId.SetObjectId(pMov)

                        If objIncidenteSuceso.Salvar(objIncidenteSuceso) Then
                            InterrumpirViaje = True
                        End If

                        objSuceso = Nothing
                        objIncidenteSuceso = Nothing

                    End If
                End If
            End If

            objViaje = Nothing
        Catch ex As Exception
            HandleError(Me.GetType.Name, "InterrumpirViaje", ex)
        End Try
    End Function
    Public Function SetCondicionamiento(pMov As Int64, pMot As Int64, pTpo As Integer) As Boolean

        Dim cnnKey As String = "Condicional"
        SetCondicionamiento = False

        Try

            If shamanStartUp.AbrirConexion(cnnKey) Then

                Dim objMovilSuceso As New conMovilesSucesos(cnnKey)
                Dim vSav As Boolean = False

                cnnsTransNET.Add(cnnKey, cnnsNET(cnnKey).BeginTransaction)

                objMovilSuceso.CleanProperties(objMovilSuceso)
                objMovilSuceso.MovilId.SetObjectId(pMov)
                objMovilSuceso.MotivoCondicionalId.SetObjectId(pMot)
                objMovilSuceso.FechaHoraSuceso = GetCurrentTime()
                objMovilSuceso.FechaHoraFinal = GetCurrentTime.AddMinutes(pTpo)

                vSav = objMovilSuceso.SetSucesoIncidente(objMovilSuceso, pTpo)

                If vSav Then
                    cnnsTransNET(cnnKey).Commit()
                Else
                    cnnsTransNET(cnnKey).Rollback()
                End If
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)

                objMovilSuceso = Nothing

                SetCondicionamiento = vSav

            End If

        Catch ex As Exception

            If cnnsNET.Contains(cnnKey) Then
                cnnsTransNET(cnnKey).Rollback()
                cnnsTransNET.Remove(cnnKey)
                cnnsNET.Remove(cnnKey)
            End If

            HandleError(Me.GetType.Name, "SetCondicionamiento", ex)

        End Try
    End Function
    Public Function SetAltaOperativa(ByVal objMovilSuceso As typMovilesSucesos) As Boolean
        SetAltaOperativa = False
        Try
            If objMovilSuceso.Salvar(objMovilSuceso) Then
                '----> Pongo el móvil actual
                Dim objActual As New conMovilesActuales(objMovilSuceso.myCnnName)
                Dim objSuceso As New conSucesosIncidentes(objMovilSuceso.myCnnName)
                Dim objVehiculo As New conVehiculos(objMovilSuceso.myCnnName)

                objActual.CleanProperties(objActual)
                objActual.MovilId.SetObjectId(objMovilSuceso.MovilId.ID)
                objActual.VehiculoId.SetObjectId(objMovilSuceso.VehiculoId.ID)
                objActual.TipoMovilId.SetObjectId(objMovilSuceso.TipoMovilId.ID)
                objActual.BaseOperativaId.SetObjectId(objMovilSuceso.BaseOperativaId.ID)
                objActual.SucesoIncidenteId.SetObjectId(objSuceso.GetIDByAbreviaturaId("L"))
                objActual.FechaHoraMovimiento = objMovilSuceso.FechaHoraSuceso
                objActual.LocalidadId.SetObjectId(objMovilSuceso.BaseOperativaId.LocalidadId.ID)
                SetAltaOperativa = objActual.Salvar(objActual)

                objVehiculo.SetKilometraje(objMovilSuceso.VehiculoId.ID, objMovilSuceso.Kilometraje)

                objActual = Nothing
                objSuceso = Nothing
                objVehiculo = Nothing

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetAltaOperativa", ex)
        End Try
    End Function
    Public Function ValidarAltaOperativa(Optional ByVal pMsg As Boolean = True) As Boolean
        ValidarAltaOperativa = False
        Try
            Dim vRdo As String = ""
            ValidarAltaOperativa = True
            If Me.MovilId.GetObjectId = 0 Then vRdo = "Debe especificar el móvil a dar de alta"
            If Me.BaseOperativaId.GetObjectId = 0 Then vRdo = "Debe establecer la base operativa desde donde inicia su guardia"
            If Me.TipoMovilId.GetObjectId = 0 Then vRdo = "Debe establecer el tipo de móvil que será en la guardia"
            If vRdo = "" Then
                Dim objActuales As New conMovilesActuales
                If objActuales.Abrir(objActuales.GetIDByIndex(Me.MovilId.GetObjectId)) Then
                    vRdo = "El móvil seleccionado ya se encontraba operativo"
                End If
                objActuales = Nothing
            End If
            If vRdo <> "" Then
                ValidarAltaOperativa = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "ValidarAltaOperativa", ex)
        End Try
    End Function
    Public Function SetBajaOperativa(ByVal pMov As Int64, ByVal pKmt As Int64) As Boolean
        SetBajaOperativa = False
        Try
            Dim objMovilSuceso As New conMovilesSucesos
            Dim objMovilActual As New conMovilesActuales
            Dim vUlt As Int64 = 0

            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM MovilesSucesos WHERE (MovilId = " & pMov & ") "
            SQL = SQL & "AND ((FechaHoraFinal IS NULL) OR (FechaHoraFinal = '" & DateToSql(NullDateTime) & "')) "
            SQL = SQL & "ORDER BY FechaHoraSuceso DESC "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            If Not vOutVal Is Nothing Then

                vUlt = CType(vOutVal, Int64)

                If objMovilSuceso.Abrir(vUlt) Then

                    objMovilSuceso.FechaHoraFinal = GetCurrentTime()
                    objMovilSuceso.KilometrajeFinal = pKmt

                    If objMovilSuceso.Salvar(objMovilSuceso) Then
                        If objMovilActual.Eliminar(objMovilActual.GetIDByIndex(objMovilSuceso.MovilId.ID)) Then

                            SetBajaOperativa = True

                            Dim objVehiculo As New conVehiculos
                            objVehiculo.SetKilometraje(objMovilSuceso.VehiculoId.ID, pKmt)
                            objVehiculo = Nothing

                        End If
                    End If

                End If

            End If

            objMovilSuceso = Nothing
            objMovilActual = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetBajaOperativa", ex)
        End Try

    End Function
End Class
