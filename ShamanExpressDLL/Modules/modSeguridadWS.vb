Imports Microsoft.Win32

Public Module modSeguridadWS

    Public Function HWS_InicializoConexion(Optional pMsg As Boolean = True) As Boolean

        HWS_InicializoConexion = False

        Try

            WSInit(pMsg)
            HWS_InicializoConexion = True

        Catch ex As Exception

            HandleError("modSeguridadWS", "HWS_InicializoConexion", ex)

        End Try

    End Function

    Private Sub WSInit(Optional pMsg As Boolean = True)

        Try

            Dim wsIni As New GPShamanWS.ServiceSoapClient

            wsIni.Open()
            wsIni.Close()
            wsIni = Nothing

            TraceLogInit("Ejecutando WSInit con sysUsingWS = " & CInt(sysUsingWS))

        Catch ex As Exception

            HandleError("modSeguridadWS", "HWS_InicializoConexion", ex)

        End Try

    End Sub

    Public Function HWS_SeteoCadenaConexion(ByRef pCnn As String, ByRef pCat As String, ByRef pUsr As String, ByRef pPsw As String) As Boolean

        HWS_SeteoCadenaConexion = False

        Try

            TraceLogInit("Ejecutando WSSetConnectionString con sysUsingWS = " & CInt(sysUsingWS))

            If WSSetConnectionString(pCnn, pCat, pUsr, pPsw) Then
                HWS_SeteoCadenaConexion = True
            End If

        Catch ex As Exception

            HandleError("modSeguridadWS", "HWS_SeteoCadenaConexion", ex)

        End Try

    End Function

    Private Function WSSetConnectionString(ByRef pCnn As String, ByRef pCat As String, ByRef pUsr As String, ByRef pPsw As String, Optional pMsg As Boolean = True) As Boolean

        WSSetConnectionString = False

        Try

            Dim wsIni As New GPShamanWS.ServiceSoapClient

            Dim vStrCnn As String = wsIni.getSerialSetLogLast(sysHardKey.Replace(" ", "/"), setBoolToInt(sysRemoto))

            If vStrCnn <> "0" Then

                Dim vStrVal() As String = vStrCnn.Split("^")

                If UBound(vStrVal) >= 4 Then

                    pCnn = vStrVal(0)
                    pCat = vStrVal(1)
                    pUsr = vStrVal(2)
                    pPsw = vStrVal(3)
                    shamanStartUp.SetSysProductos(vStrVal(4))

                    If UBound(vStrVal) >= 5 Then
                        Dim vVto As Date = CDate(vStrVal(5))
                        If DateDiff(DateInterval.Day, Now, vVto) + 1 <= 20 Then
                            If pMsg Then
                                Dim vDias As Integer = DateDiff(DateInterval.Day, Now, vVto) + 1
                                If vDias > 1 Then
                                    MsgBox("Faltan " & vDias & " días para el vencimiento de su licencia", MsgBoxStyle.Exclamation, "Shaman")
                                Else
                                    MsgBox("Falta " & vDias & " día para el vencimiento de su licencia", MsgBoxStyle.Exclamation, "Shaman")
                                End If
                            End If
                        End If
                    End If

                    WSSetConnectionString = True

                Else

                    If pMsg Then MsgBox("Error en la verificación de licencia", MsgBoxStyle.Critical, "Shaman")
                    wsIni.Close()
                    wsIni = Nothing

                End If

            End If

            wsIni.Close()
            wsIni = Nothing

        Catch ex As Exception

            HandleError("modSeguridadWS", "WSSetConnectionString", ex)

        End Try

    End Function

    Private Sub HWS_VerificoConexion()

        Try

            TraceLogInit("Ejecutando HWS_VerificoConexion con sysUsingWS = " & CInt(sysUsingWS))

            WSVerifyLicense()

        Catch ex As Exception

            HandleError("modSeguridadWS", "HWS_VerificoConexion", ex)

        End Try

    End Sub

    Private Function WSVerifyLicense(Optional pMsg As Boolean = True) As Boolean

        WSVerifyLicense = False

        Try

            TraceLogHKey("Inicio: WSVerifyLicense")

            Dim wsIni As New GPShamanWS.ServiceSoapClient

            Dim vStrCnn As String = wsIni.getSerialSetLog(sysHardKey.Replace(" ", "/"))

            If vStrCnn = "0" Then

                If pMsg Then MsgBox("Error en la verificación de licencia", MsgBoxStyle.Critical, "Shaman")

            End If

            wsIni.Close()
            wsIni = Nothing

            WSVerifyLicense = True

            TraceLogHKey("Fin: WSVerifyLicense")

        Catch ex As Exception

            HandleError("modSeguridadWS", "WSVerifyLicense", ex)

        End Try

    End Function

    Public Function GetByRegistry(MyLastExec As LastExec) As Boolean

        GetByRegistry = False

        Try

            Dim aplKey As RegistryKey

            aplKey = Registry.LocalMachine.OpenSubKey("Software\Shaman\Express")

            If Not aplKey Is Nothing Then

                cnnDataSource = aplKey.GetValue("cnnDataSource")
                cnnCatalog = aplKey.GetValue("cnnCatalog")
                cnnUser = aplKey.GetValue("cnnUser")
                cnnPassword = aplKey.GetValue("cnnPassword")
                shamanStartUp.SetSysProductos(aplKey.GetValue("sysProductos").ToString)

                GetByRegistry = True

            End If

        Catch ex As Exception

            HandleError("modSeguridadWS", "GetByRegistry", ex, , , , MyLastExec)

        End Try

    End Function


    Private Sub HWS_FinalizoConexion()
        Try

        Catch ex As Exception
            HandleError("modSeguridadWS", "HWS_FinalizoConexion", ex)
        End Try
    End Sub


End Module
