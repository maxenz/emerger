Imports System.Data.SqlClient
Imports System.IO

Public Module modErrors

    Public Sub HandleError(ByVal pClass As String, ByVal pSub As String, ByVal ex As Exception, Optional pMsg As Boolean = True, Optional pTabSql As String = "", Optional pStep As String = "", Optional pLastExec As LastExec = Nothing)

        Try

            Dim sqlErr As SqlException
            sqlErr = TryCast(ex, SqlException)

            '-----> Errores de SQL
            If Not sqlErr Is Nothing Then
                If pTabSql = "" Then pTabSql = pClass
                Select Case sqlErr.Number
                    Case -1
                        TraceError(sqlErr.Message, pSub, pMsg, pLastExec)
                    Case 547
                        Dim vFKVal() As String = sqlErr.Message.Split(Chr(34))
                        Dim vShwDft As Boolean = True
                        If vFKVal.Length >= 1 Then
                            If vFKVal(1).Substring(0, 2) = "FK" Then
                                Dim vFKPar() As String = vFKVal(1).Split("_")
                                If vFKVal(0).ToUpper.Contains("INSERT") Then
                                    TraceError("Debe completar la relación con la tabla " & vFKPar(2), pTabSql, pMsg, pLastExec)
                                ElseIf vFKVal(0).ToUpper.Contains("DELETE") Then
                                    TraceError("Existe al menos un registro en " & vFKPar(1) & " para el valor que trata de eliminar", pTabSql, pMsg, pLastExec)
                                End If
                                vShwDft = False
                            End If
                        End If
                        If vShwDft Then
                            TraceError("Eliminar ese registro causaría problemas de integridad referencial", pTabSql, pMsg, pLastExec)
                        End If
                    Case 2601
                        TraceError("Su incorporación causaría valores claves duplicados", pTabSql, pMsg, pLastExec)
                    Case Else
                        TraceError(sqlErr.Message, pTabSql, pMsg, pLastExec)
                End Select

            Else

                If pStep = "" Then
                    TraceError(pSub & ") " & ex.GetType.FullName & ": " & ex.Message, pClass, pMsg, pLastExec)
                Else
                    TraceError(pSub & ") " & ex.GetType.FullName & ": " & ex.Message & vbCrLf & pStep, pClass, pMsg, pLastExec)
                End If

            End If

            VerifyConnections()

        Catch exHan As Exception
            If Not dllMode Then
                MsgBox("HandleError: " & exHan.Message, MsgBoxStyle.Critical, "modErrors")
            End If
        End Try

    End Sub

    Private Sub VerifyConnections()

        Try
            If cnnsNET.Count > 0 Then
                Dim cnnCheck As SqlConnection
                For Each cnnCheck In cnnsNET.Values
                    If cnnCheck.State = ConnectionState.Broken Or cnnCheck.State = ConnectionState.Closed Then
                        cnnCheck.Open()
                    End If
                Next
            End If
        Catch ex As Exception
            If Not dllMode Then
                MsgBox("VerifyConnections: " & ex.Message, MsgBoxStyle.Critical, "modErrors")
            End If
        End Try

    End Sub

    Private Sub TraceError(pErrMsg As String, pErrTit As String, Optional pMsg As Boolean = True, Optional pLastExec As LastExec = Nothing)

        If pMsg And Not dllMode Then
            MsgBox(pErrMsg, MsgBoxStyle.Critical, pErrTit)
        End If

        If Not pLastExec Is Nothing Then

            pLastExec.SetValues(pErrTit, pErrMsg)

        End If


    End Sub

    Public Sub TraceLogInit(pLogMsg As String, Optional pDel As Boolean = False)

        If sysLogInit Then

            'Dim logFile As String = Application.StartupPath & "\sysLogInit.log"
            Dim logFile As String = System.AppDomain.CurrentDomain.BaseDirectory & "sysLogInit.log"

            Dim sw As StreamWriter

            If pDel Then
                If File.Exists(logFile) Then
                    File.Delete(logFile)
                End If
                sw = File.CreateText(logFile)
            Else
                sw = File.AppendText(logFile)
            End If

            sw.WriteLine(Now.ToString & ": " & pLogMsg)

            sw.Flush()
            sw.Close()

        End If

    End Sub

    Public Sub TraceLogHKey(pLogMsg As String, Optional pDel As Boolean = False)

        If sysLogHKey Then

            'Dim logFile As String = Application.StartupPath & "\sysLogHKey.log"
            Dim logFile As String = System.AppDomain.CurrentDomain.BaseDirectory & "sysLogHKey.log"
            Dim sw As StreamWriter

            If pDel Then
                If File.Exists(logFile) Then
                    File.Delete(logFile)
                End If
                sw = File.CreateText(logFile)
            Else
                sw = File.AppendText(logFile)
            End If

            sw.WriteLine(Now.ToString & ": " & pLogMsg)

            sw.Flush()
            sw.Close()

        End If

    End Sub

End Module
