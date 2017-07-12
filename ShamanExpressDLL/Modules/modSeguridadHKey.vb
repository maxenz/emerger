Imports Microsoft.Win32

Public Module modSeguridadHKey
    Public buffer As String
    Public l As Long
    Public relleno As String
    Public random As String
    Public hkestado As String
    Public conexion As String
    Public idConexion As String

    Public Function GetVariablesConexionHKey(Optional ByVal pMsg As Boolean = True) As Boolean

        GetVariablesConexionHKey = False

        Try

            Dim vIniHKey As Boolean

            vIniHKey = HKEY_InicializoConexion(pMsg)

            '----> Recuperar valores de conexión
            If vIniHKey Then

                '---------> Recupero valores de conexión

                TraceLogInit("Obteniendo DataSource por HKey")
                cnnDataSource = getString(HKEY_LeoCadenasLlave(1), False)

                TraceLogInit("Obteniendo Catalogo por HKey")
                cnnCatalog = getString(HKEY_LeoCadenasLlave(2))

                TraceLogInit("Obteniendo Usuario por HKey")
                cnnUser = getString(HKEY_LeoCadenasLlave(3))

                TraceLogInit("Obteniendo Password por HKey")
                cnnPassword = getString(HKEY_LeoCadenasLlave(4))

                TraceLogInit("Obteniendo Productos por HKey")
                shamanStartUp.SetSysProductos(getString(HKEY_LeoCadenasLlave(5)))

                TraceLogInit("Obteniendo Vencimientos por HKey")
                sysVencimiento = CType(getString(HKEY_LeoCadenasLlave(6)), Date)

                If sysVencimiento < Now.Date Then

                    TraceLogInit("HardKey Vencida (" & sysVencimiento & ")")

                ElseIf Not (cnnDataSource = "" Or cnnDataSource Is Nothing Or sysProductos Is Nothing) Then

                    GetVariablesConexionHKey = True

                End If

            End If

        Catch ex As Exception

            HandleError("modSeguridadHKey", "GetValoresHardkey", ex, pMsg)

        End Try

    End Function

    Private Function getString(ByVal pVal As String, Optional ByVal pFul As Boolean = True) As String

        getString = ""

        Try

            Dim vStrVal As String = ""
            Dim vIdx As Integer

            pVal = pVal.Trim
            pVal = pVal.Replace(Chr(0), "")

            If pFul Then
                For vIdx = 0 To pVal.Length - 1
                    If (Asc(pVal.Substring(vIdx, 1)) >= 46 And Asc(pVal.Substring(vIdx, 1)) <= 122) Or pVal.Substring(vIdx, 1) = "#" Or pVal.Substring(vIdx, 1) = "!" Then
                        vStrVal = vStrVal & pVal.Substring(vIdx, 1)
                    End If
                Next
                getString = vStrVal
            Else
                getString = pVal
            End If

        Catch ex As Exception
            HandleError("modStartUp", "getString", ex)
        End Try

    End Function


    Private Sub InitBuffer()

        Dim l As Integer

        'Inicializa con 10 bytes al azar la string usada para
        'pasar los parámetros a la función HARDkey()

        Randomize()
        relleno = Space(200)
        buffer = ""
        For l = 1 To 10
            buffer = buffer + Chr(1) 'Chr(Int((Rnd * 255) + 1))
        Next l

    End Sub

    Sub EncriptaString(ByRef buffer As String, ByVal password As String)
        'Esta rutina encripta la cadena que se pasa como parámetro
        'a la función HARDkey().

        Dim i As Integer
        Dim ctemp As Integer
        Dim cAnterior As Integer
        Dim k As Integer
        Dim pw As Integer
        Dim bufEnc As String

        cAnterior = 0
        bufEnc = ""
        For i = 0 To 199
            ctemp = Asc(Mid(buffer, i + 1, 1))
            If (ctemp < 0) Then
                ctemp = ctemp + 256
            End If
            ctemp = ctemp Xor sBox1(cAnterior)
            For k = 0 To 15
                pw = Asc(Mid(password, k + 1, 1))
                If ((k Mod 2) = 1) Then
                    ctemp = ctemp Xor sBox1(sBox2(pw))
                    ctemp = sBox2(ctemp)
                Else
                    ctemp = ctemp Xor sBox2(sBox1(pw))
                    ctemp = sBox1(ctemp)
                End If
            Next k
            ctemp = ctemp Xor sBox1(i)
            cAnterior = ctemp
            bufEnc = bufEnc + Chr(ctemp)
        Next i
        buffer = bufEnc
    End Sub

    Sub DesencriptaString(ByRef buffer As String, ByVal password As String)
        'Esta rutina desencripta la cadena que devuelve la
        'función HARDkey().

        Dim i As Integer
        Dim ctemp As Integer
        Dim cAnterior As Integer
        Dim k As Integer
        Dim pw As Integer
        Dim bufEnc As String

        cAnterior = 0
        bufEnc = ""
        For i = 0 To 199
            ctemp = Asc(Mid(buffer, i + 1, 1))
            If (ctemp < 0) Then
                ctemp = ctemp + 256
            End If
            ctemp = ctemp Xor sBox1(cAnterior)
            For k = 0 To 15
                pw = Asc(Mid(password, k + 1, 1))
                If ((k Mod 2) = 1) Then
                    ctemp = ctemp Xor sBox1(sBox2(pw))
                    ctemp = sBox2(ctemp)
                Else
                    ctemp = ctemp Xor sBox2(sBox1(pw))
                    ctemp = sBox1(ctemp)
                End If
            Next k
            ctemp = ctemp Xor sBox1(i)
            cAnterior = Asc(Mid(buffer, i + 1, 1))
            bufEnc = bufEnc + Chr(ctemp)
        Next i
        buffer = bufEnc
    End Sub

    Function ValidaString(ByVal buffer As String, ByVal random As String) As Boolean
        'Esta rutina analiza que la string devuelta por la
        'función HARDkey() sea consistente.

        Dim i As Integer
        Dim result As Boolean
        Dim st As String

        For i = 1 To 10
            Mid(buffer, i, 1) = Chr(sBox2(Asc(Mid(buffer, i, 1))))
        Next i
        result = True
        For i = 1 To 10
            If (Mid(buffer, i, 1) <> Mid(random, i, 1)) Then
                result = False
            End If
        Next i
        If (Mid(buffer, 11, 1) <> " ") Then
            result = False
        End If
        If (Mid(buffer, 20, 1) <> " ") Then
            result = False
        End If
        If (Mid(buffer, 26, 1) <> " ") Then
            result = False
        End If
        If (Mid(buffer, 31, 1) <> "-") Then
            result = False
        End If
        If (Val(Mid(buffer, 21, 5)) <> 0) Then
            result = False
        End If
        st = Mid(buffer, 21, 5)
        If (st = "00000") Then hkestado = "El comando se completó con exito"
        If (st = "00002") Then hkestado = "No se encontró protector"
        If (st = "00004") Then hkestado = "Formato de cadena o parámetro incorrecto"
        If (st = "00010") Then hkestado = "Número de conexión no válida"
        If (st = "00011") Then hkestado = "Se superó límite de usuarios permitidos"
        If (st = "00012") Then hkestado = "Módulo yá en uso por la aplicación"
        If (st = "00013") Then hkestado = "Módulo no levantado por la aplicación"
        If (st = "00020") Then hkestado = "No hay drivers HARDkey instalados"
        If (st = "00021") Then hkestado = "Versión de drivers obsoleta"
        If (st = "00022") Then hkestado = "No hay drivers SuperPro instalados"
        ValidaString = result
    End Function

    Public Function HKEY_InicializoConexion(Optional pMsg As Boolean = True) As Boolean

        HKEY_InicializoConexion = False

        Try

            TraceLogHKey("HKEY_InicializoConexion", True)

            '------> Inicio 1
            InitBuffer()
            random = buffer
            'buffer = buffer + " 00000000 00000 00000 0009 00101 00000 00000 00000 00000 00000 00000 00000"
            buffer = buffer + " 00000000 00000 00000 0009 00101 00001 00000 00000 00000 00000 00000 00000"
            buffer = buffer + relleno

            TraceLogInit("HKEY_InicializoConexion: Preparando 0009 00101 00001")

            Call EncriptaString(buffer, password)
            l = HARDkey(buffer)
            Call DesencriptaString(buffer, password)

            TraceLogInit("HKEY_InicializoConexion: Enviando 0009 00101 00001")

            '------> Inicio 2

            If (ValidaString(buffer, random) = True) Then

                InitBuffer()
                random = buffer
                buffer = buffer + " 00000000 " + sysHardKey + " 0000 01 3"
                buffer = buffer + relleno

                TraceLogInit("HKEY_InicializoConexion: Preparando 01 3")

                Call EncriptaString(buffer, password)
                l = HARDkey(buffer)
                Call DesencriptaString(buffer, password)

                TraceLogInit("HKEY_InicializoConexion: Enviando 01 3")

                If (ValidaString(buffer, random) = True) Then

                    Dim cadenaConexion As String = Mid(buffer, 12)
                    Dim idConexionObtenida As String = cadenaConexion.Substring(0, 8)
                    idConexion = idConexionObtenida
                    logHKeyPID = CType(idConexion, Long)
                    HKEY_InicializoConexion = True

                    TraceLogInit("HKEY_InicializoConexion: Se abrio conexion: " & idConexion)

                Else

                    Dim errorConexion As String = Mid(buffer, 12).Substring(9, 5)
                    Dim intErrorConexion As Integer = CInt(errorConexion)

                    If hkestado = "" Then

                        If pMsg Then
                            Select Case intErrorConexion
                                Case 2 : MsgBox("La llave no se encuentra conectada al servidor", MsgBoxStyle.Critical, "Shaman")
                                Case 11 : MsgBox("Se ha superado el número de licencias permitidas", MsgBoxStyle.Critical, "Shaman")
                            End Select
                        End If

                        TraceLogInit("HKEY_InicializoConexion: " & intErrorConexion)

                    Else

                        TraceLogInit("HKEY_InicializoConexion: " & hkestado)

                    End If

                End If

            Else

                TraceLogInit("HKEY_InicializoConexion: 0009 00101 00001")

            End If

        Catch ex As Exception

            HandleError("modFuncionesSeguridad", "HKEY_InicializoConexion", ex, pMsg)

        End Try

    End Function

    Public Sub HKEY_FinalizoConexion(Optional ByVal pIdCnn As Decimal = 0, Optional ByVal pDbg As Boolean = False)

        If sysUsingWS Then
            'HWS_FinalizoConexion()
            Exit Sub
        End If

        Dim idCierre As String
        If pIdCnn = 0 Then
            idCierre = idConexion
        Else
            idCierre = Format(pIdCnn, "00000000")
        End If

        InitBuffer()
        random = buffer
        buffer = buffer + " " + idCierre + " " + sysHardKey + " 0002 01"
        buffer = buffer + relleno
        Call EncriptaString(buffer, password)
        l = HARDkey(buffer)
        Call DesencriptaString(buffer, password)

        If (ValidaString(buffer, random) = True) Then
            If pDbg Then MsgBox("Se cerro conexion: " & idConexion)
        End If

    End Sub


    Public Sub HKEY_VerificoConexion()

        TraceLogHKey("Inicio: HKEY_VerificoConexion")

        If sysUsingWS Then
            'HWS_VerificoConexion()
            Exit Sub
        End If

        InitBuffer()
        random = buffer
        buffer = buffer + " " + idConexion + " " + sysHardKey + " 0001"
        buffer = buffer + relleno
        Call EncriptaString(buffer, password)
        l = HARDkey(buffer)
        Call DesencriptaString(buffer, password)

        If (ValidaString(buffer, random) = True) Then
            Dim errorConexion As String = Mid(buffer, 12).Substring(9, 5)

        Else

            'HWS_VerificoConexion()
            sysUsingWS = True

        End If

        TraceLogHKey("Fin: HKEY_VerificoConexion")

    End Sub

    Public Function HKEY_LeoCadenasLlave(ByVal id As Integer) As String

        HKEY_LeoCadenasLlave = ""

        InitBuffer()
        random = buffer

        Select Case id
            Case 1
                'Obtengo ip o nombre del servidor y la instancia de SQL SERVER
                buffer = buffer + " " + idConexion + " " + sysHardKey + " 0012 00000 00038"
            Case 2
                'Obtengo nombre de la base de datos
                buffer = buffer + " " + idConexion + " " + sysHardKey + " 0012 00041 00038"
            Case 3
                'Obtengo usuario SQL SERVER
                buffer = buffer + " " + idConexion + " " + sysHardKey + " 0012 00081 00038"
            Case 4
                'Obtengo password SQL SERVER
                buffer = buffer + " " + idConexion + " " + sysHardKey + " 0012 00121 00038"
            Case 5
                'Obtengo productos
                buffer = buffer + " " + idConexion + " " + sysHardKey + " 0012 00161 00038"
            Case 6
                'Obtengo vencimiento
                buffer = buffer + " " + idConexion + " " + sysHardKey + " 0012 00201 00038"
        End Select

        buffer = buffer + relleno
        Call EncriptaString(buffer, password)
        l = HARDkey(buffer)
        Call DesencriptaString(buffer, password)

        If (ValidaString(buffer, random) = True) Then

            Dim cadena As String = Mid(buffer, 12).Substring(25, 38)

            HKEY_LeoCadenasLlave = cadena

        End If

    End Function

End Module



