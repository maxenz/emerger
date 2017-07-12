Imports Microsoft.Win32
Imports System.IO
Imports System
Imports System.Data
Imports System.Data.SqlClient

Public Class StartUp

    Public MyLastExec As LastExec

    Public Function GetValoresHardkey(Optional ByVal pMsg As Boolean = True) As Boolean

        GetValoresHardkey = False

        Try

            MyLastExec = New LastExec

            Dim aplKey As RegistryKey

            aplKey = Registry.LocalMachine.OpenSubKey("Software\Shaman\Express")

            If Not aplKey Is Nothing Then

                '------> Valores de HardKey
                sysHardKey = aplKey.GetValue("HardKey")

                '------> Sistema Remoto
                If Not aplKey.GetValue("sysRemoto") Is Nothing Then
                    sysRemoto = CBool(aplKey.GetValue("sysRemoto"))
                End If

                '------> Logs...
                If Not aplKey.GetValue("sysLogInit") Is Nothing Then
                    sysLogInit = CBool(aplKey.GetValue("sysLogInit"))
                End If
                If Not aplKey.GetValue("sysLogHKey") Is Nothing Then
                    sysLogHKey = CBool(aplKey.GetValue("sysLogHKey"))
                End If

                '-------> HKeys de prueba
                'sysHardKey = "18749/40159"  'AMEE
                'sysHardKey = "10147/41472"  'Medicall
                'sysHardKey = "15558/29351"  'Corpus
                'sysHardKey = "10347/42472"
                'sysHardKey = "20142/41272"
                'sysHardKey = "14139/42282"
                'sysHardKey = "20142/41272"  'Rest911
                'sysHardKey = "19381/24689"
                'sysHardKey = "15359/20497"  'AsesorLab
                'sysHardKey = "14547/19442"  'Sur Salud
                'sysHardKey = "56889/23116"  'Donado
                'sysHardKey = "11872/14731"  'U24
                'sysHardKey = "21532/08645" 'MCO
                'sysHardKey = "26191/34865" 'EMEC
                'sysHardKey = "29769/19282" 'Amparar
                'sysHardKey = "21332/47642" 'CGL
                'sysHardKey = "26143/18901" 'Emecor
                'sysHardKey = "24978/13499" 'Clinicus
                'sysHardKey = "11872/14732" 'Clinicus

                'sysRemoto = True

                TraceLogInit("Se recupero el valor HardKey = " & sysHardKey & " del registro de Windows", True)

                If Not sysHardKey Is Nothing Then
                    sysHardKey = sysHardKey.Replace("/", " ")
                    GetValoresHardkey = True
                Else
                    MyLastExec.SetValues(Me.GetType.Name, "La llave de instalación no se encuentra configurada")
                    If pMsg Then MsgBox("La llave de instalación no se encuentra configurada", MsgBoxStyle.Critical, "Shaman")
                End If

            Else

                MyLastExec.SetValues(Me.GetType.Name, "No se puedo obtener acceso al registro")
                If pMsg Then MsgBox("No se puedo obtener acceso al registro", MsgBoxStyle.Critical, "Shaman")

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetValoresHardkey", ex, pMsg, , , MyLastExec)
        End Try

    End Function

    Public Function GetVariablesConexion(Optional ByVal pMsg As Boolean = True, Optional pKeyMode As keyMode = keyMode.keyAll) As Boolean

        GetVariablesConexion = False

        Try

            MyLastExec = New LastExec

            If pKeyMode = keyMode.keyAll Or pKeyMode = keyMode.keyWS Then
                sysUsingWS = HWS_InicializoConexion(pMsg)
            End If

            '----> Recuperar valores de conexión
            If sysUsingWS Then

                If HWS_SeteoCadenaConexion(cnnDataSource, cnnCatalog, cnnUser, cnnPassword) Then

                    If cnnDataSource = "" Or cnnDataSource Is Nothing Or sysProductos Is Nothing Then
                        MyLastExec.SetValues(Me.GetType.Name, "Su licencia se encuentra vencida")
                        If pMsg Then MsgBox("Su licencia se encuentra vencida", MsgBoxStyle.Critical, "Shaman")
                    Else

                        If Not (cnnDataSource = "" Or cnnDataSource Is Nothing Or sysProductos Is Nothing) Then
                            GetVariablesConexion = True
                        Else
                            sysUsingWS = False
                        End If

                    End If

                Else

                    sysUsingWS = False

                End If

            End If

            If Not sysUsingWS Then

                If GetByRegistry(MyLastExec) Then
                    GetVariablesConexion = True
                Else
                    MyLastExec.SetValues(Me.GetType.Name, "El registro no se encuentra correctamente configurado")
                    If pMsg Then MsgBox("El registro no se encuentra correctamente configurado", MsgBoxStyle.Critical, "Shaman")
                End If

            End If

        Catch ex As Exception

            HandleError(Me.GetType.Name, "GetVariablesConexion", ex, pMsg, , , MyLastExec)

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

    Private Sub VerifyRegistry()

        Try

            Dim aplKey As RegistryKey

            '----------> Recupero y ajusto la registry
            aplKey = Registry.LocalMachine.OpenSubKey("Software\Shaman\Express")
            If aplKey Is Nothing Then

                aplKey = Registry.LocalMachine.CreateSubKey("Software\Shaman\Express")
                aplKey.SetValue("HardKey", "56889/23116")

            End If

            aplKey.Close()

        Catch ex As Exception
            HandleError("modStartUp", "VerifyRegistry", ex)
        End Try

    End Sub

    Public Function AbrirConexion(ByVal pName As String, Optional pMsg As Boolean = True) As Boolean

        AbrirConexion = False

        Try

            MyLastExec = New LastExec

            Dim myCnnNET As New SqlConnection

            If NETConnect(myCnnNET, pMsg) Then

                If Not cnnsNET.Contains(pName) Then
                    cnnsNET.Add(pName, myCnnNET)
                End If

                AbrirConexion = True

            End If

        Catch ex As Exception

            HandleError("AbrirConexion", GetConnectionString(), ex, , , , MyLastExec)

        End Try
    End Function

    Private Function NETConnect(ByRef pCnn As SqlConnection, Optional pMsg As Boolean = True) As Boolean
        NETConnect = False
        Try

            If Not pCnn Is Nothing Then
                If pCnn.State = 1 Then pCnn.Close()
                pCnn = Nothing
            End If

            pCnn = New SqlConnection
            pCnn.ConnectionString = GetConnectionString()

            TraceLogInit("Conectando a: " & pCnn.ConnectionString)
            pCnn.Open()

            If pCnn.State = 1 Then
                NETConnect = True
            Else
                MyLastExec.SetValues(Me.GetType.Name, "La base de datos no se encuentra disponible")
                If pMsg Then MsgBox("La base de datos no se encuentra disponible", MsgBoxStyle.Critical, "Shaman")
            End If

        Catch ex As Exception
            MyLastExec.SetValues(Me.GetType.Name, ex.Message, GetConnectionString())
            HandleError("NETConnect", GetConnectionString(), ex)
        End Try
    End Function

    Public Sub SetSysProductos(pSysCad As String)

        'Ejemplo
        '1/100#00102/00108/10002
        'pSysCad = 1

        If Not pSysCad.Contains("#") Then
            sysProductos = pSysCad.Split("/")
            'ReDim sysSubExclude(0)
            'sysSubExclude(0) = "00108"
        Else
            Dim vSysCad() As String = pSysCad.Split("#")
            sysProductos = vSysCad(0).Split("/")
            sysSubExclude = vSysCad(1).Split("/")
        End If

    End Sub

    Public Function getPID() As Int64

        getPID = 0

        Try

            Dim SQL As String = "SELECT @@SPID"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(cnnDefault))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            If Not vOutVal Is Nothing Then
                getPID = CType(vOutVal, Int64)
            Else
                getPID = 0
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try

    End Function


End Class
