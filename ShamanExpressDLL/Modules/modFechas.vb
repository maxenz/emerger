Imports System.Data
Imports System.Data.SqlClient

Public Module modFechas
    '----> Declares
    Public IsFormatEnglish As Boolean
    Public NullDateMax As Date
    Public NullDateMin As Date
    Public NullDateEdit As String
    Public SQLNullDate As String
    Public NullDateTime As DateTime

    Public Sub InitDateVars()
        IsFormatEnglish = getIsFormatEnglish()
        If Not IsFormatEnglish Then
            SQLNullDate = "1899-12-30"
        Else
            SQLNullDate = "1899-30-12"
        End If
        Try
            NullDateMax = CDate("31/12/2999")
            NullDateTime = SetToDateTime(CDate("30/12/1899"), "00:00", True)
        Catch ex As Exception
            NullDateMax = CDate("12/31/2999")
            NullDateTime = SetToDateTime(CDate("12/30/1899"), "00:00", True)
        End Try
        NullDateEdit = "#12:00:00 AM#"
    End Sub

    Private Function getIsFormatEnglish()
        Try
            Dim SQL As String = "SELECT ID FROM Incidentes WHERE FecIncidente = '" & Now.Year & "-31-12'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(cnnDefault))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            getIsFormatEnglish = True

        Catch ex As Exception
            getIsFormatEnglish = False
        End Try
    End Function

    Public Function valDesdeHasta(ByVal pDes As Date, ByVal pHas As Date, Optional ByVal pMsg As Boolean = True, Optional ByVal pDesMax As Boolean = False, Optional ByVal pHasMax As Boolean = False) As Boolean
        Dim vRdo As String = ""
        valDesdeHasta = True
        If (pDes = NullDateMax Or pDes = NullDateEdit) And Not pDesMax Then
            vRdo = "La fecha desde es incorrecta"
        ElseIf (pHas = NullDateMax Or pDes = NullDateEdit) And Not pHasMax Then
            vRdo = "La fecha hasta es incorrecta"
        ElseIf pDes > pHas Then
            vRdo = "La fecha desde debe ser inferior a la fecha hasta"
        End If
        If vRdo <> "" Then
            valDesdeHasta = False
            If pMsg Then MsgBox(vRdo, vbCritical, "Error en rango de fechas")
        End If
    End Function
    Public Function DateToGrid(ByVal pFec As Date) As String
        If pFec <> NullDateMax Then
            DateToGrid = pFec.ToShortDateString
        Else
            DateToGrid = ""
        End If
    End Function
    Public Function DateToSql(ByVal pFec As Date, Optional ByVal pForQry As Boolean = True, Optional ByVal pStm As Boolean = False, Optional ByVal pIni As Boolean = True) As String
        Dim vFec As String

        If Not IsFormatEnglish Or Not pForQry Then
            vFec = Format(pFec.Year, "0000") & "-" & Format(pFec.Month, "00") & "-" & Format(pFec.Day, "00")
        Else
            vFec = Format(pFec.Year, "0000") & "-" & Format(pFec.Day, "00") & "-" & Format(pFec.Month, "00")
        End If

        If Not pStm Then
            DateToSql = vFec
        Else
            If pIni Then
                DateToSql = vFec & " 00:00:00"
            Else
                DateToSql = vFec & " 23:59:59"
            End If
        End If

    End Function

    Public Function DateTimeToSql(ByVal pFhr As DateTime, Optional ByVal pForQry As Boolean = True) As String
        Dim vFec As String

        If pFhr.Year > 1000 Then
            If Not IsFormatEnglish Or Not pForQry Then
                vFec = Format(pFhr.Year, "0000") & "-" & Format(pFhr.Month, "00") & "-" & Format(pFhr.Day, "00")
            Else
                vFec = Format(pFhr.Year, "0000") & "-" & Format(pFhr.Day, "00") & "-" & Format(pFhr.Month, "00")
            End If

            DateTimeToSql = vFec & " " & Format(pFhr.Hour, "00") & ":" & Format(pFhr.Minute, "00") & ":" & Format(pFhr.Second, "00")

        Else

            DateTimeToSql = SQLNullDate

        End If

    End Function

    Public Function DateToAnsi(ByVal pFec As Date) As Int64
        DateToAnsi = Val(Format(pFec.Year, "0000") & Format(pFec.Month, "00") & Format(pFec.Day, "00"))
    End Function

    Public Function AnsiToDate(ByVal pFec As Int64) As Date
        AnsiToDate = CDate(pFec.ToString.Substring(6, 2) & "/" & pFec.ToString.Substring(4, 2) & "/" & pFec.ToString.Substring(0, 4))
    End Function

    Public Function ShowDateTime(ByVal pFhr As DateTime, Optional ByVal pShw As Byte = 0, Optional ByVal pFul As Boolean = False) As String
        ShowDateTime = ""
        If pFhr.Date <> NullDateMax Then
            Select Case pShw
                Case 0
                    ShowDateTime = pFhr.Date
                Case 1
                    ShowDateTime = Format(pFhr.Hour, "00") & ":" & Format(pFhr.Minute, "00")
                Case 2
                    ShowDateTime = DateToSql(pFhr.Date) & " " & Format(pFhr.Hour, "00") & ":" & Format(pFhr.Minute, "00") & ":" & Format(pFhr.Second, "00")
            End Select
        End If
    End Function

    Public Function SetToDateTime(ByVal pFec As Date, ByVal pHor As String, Optional ByVal pIni As Boolean = True) As DateTime
        SetToDateTime = NullDateTime
        Try
            If pFec <> NullDateMax And pFec.ToString <> "01/01/0001 12:00:00 a.m." Then
                Select Case pHor.Length
                    Case 5
                        If pIni Then
                            SetToDateTime = DateToSql(pFec, False) & " " & Format(Val(Parcer(pHor, ":")), "00") & ":" & Format(Val(Parcer(pHor, ":", 1)), "00") & ":00"
                        Else
                            SetToDateTime = DateToSql(pFec, False) & " " & Format(Val(Parcer(pHor, ":")), "00") & ":" & Format(Val(Parcer(pHor, ":", 1)), "00") & ":59"
                        End If
                    Case 8
                        SetToDateTime = DateToSql(pFec, False) & " " & pHor
                    Case Else
                        SetToDateTime = DateToSql(pFec, False) & " " & Format(Val(Parcer(pHor, ":")), "00") & ":" & Format(Val(Parcer(pHor, ":", 1)), "00") & ":" & Format(Val(Parcer(pHor, ":", 2)), "00")
                End Select
            End If
        Catch
        End Try
    End Function

    Public Function getNroDayWeek(ByVal pVal As Date) As Integer
        If pVal.DayOfWeek <> DayOfWeek.Sunday Then
            getNroDayWeek = pVal.DayOfWeek
        Else
            getNroDayWeek = 7
        End If
    End Function

    Public Function getDayWeek(ByVal pVal As Integer) As String
        Select Case pVal
            Case 0 : getDayWeek = "Lunes"
            Case 1 : getDayWeek = "Martes"
            Case 2 : getDayWeek = "Miércoles"
            Case 3 : getDayWeek = "Jueves"
            Case 4 : getDayWeek = "Viernes"
            Case 5 : getDayWeek = "Sábado"
            Case 6 : getDayWeek = "Domingo"
            Case Else : getDayWeek = "Feriados"
        End Select
    End Function

    Public Function getMesNombre(ByVal pVal As Integer) As String
        Select Case pVal
            Case 1 : getMesNombre = "Enero"
            Case 2 : getMesNombre = "Febrero"
            Case 3 : getMesNombre = "Marzo"
            Case 4 : getMesNombre = "Abril"
            Case 5 : getMesNombre = "Mayo"
            Case 6 : getMesNombre = "Junio"
            Case 7 : getMesNombre = "Julio"
            Case 8 : getMesNombre = "Agosto"
            Case 9 : getMesNombre = "Septiembre"
            Case 10 : getMesNombre = "Octubre"
            Case 11 : getMesNombre = "Noviembre"
            Case Else : getMesNombre = "Diciembre"
        End Select
    End Function

    Public Function getNroQuincena(ByVal pVal As Date) As Integer
        If pVal.Day <= 15 Then
            getNroQuincena = 1
        Else
            getNroQuincena = 2
        End If
    End Function

    Public Function valTime(ByVal pVal As String, Optional ByVal pMsg As Boolean = True, Optional ByVal pAllowNull As Boolean = True) As Boolean
        Dim vRdo As String = ""
        Dim vHor As Integer = Val(Parcer(pVal, ":"))
        Dim vMin As Integer = Val(Parcer(pVal, ":", 1))
        valTime = True
        If vHor < 0 Or vHor > 23 Then
            vRdo = "La hora establecida es inválida"
        ElseIf vMin < 0 Or vMin > 59 Then
            vRdo = "Los minutos establecidos son incorrectos"
        ElseIf Not pAllowNull And (pVal = "" Or pVal = "__:__" Or pVal = "  :") Then
            vRdo = "Los minutos establecidos son incorrectos"
        End If
        If vRdo <> "" Then
            valTime = False
            If pMsg Then MsgBox(vRdo, vbCritical, System.AppDomain.CurrentDomain.FriendlyName)
        End If
    End Function

    Public Function TimeToGrid(ByVal pVal As String) As String
        Dim vHor As Integer = Val(Parcer(pVal, ":"))
        Dim vMin As Integer = Val(Parcer(pVal, ":", 1))
        TimeToGrid = Format(vHor, "00") & ":" & Format(vMin, "00")
    End Function

    Public Function GetMinutes(ByVal pVal As String) As Integer
        Dim vHor As Integer = Val(Parcer(pVal, ":"))
        Dim vMin As Integer = Val(Parcer(pVal, ":", 1))
        GetMinutes = (vHor * 60) + vMin
    End Function

    Public Function MinutesToTime(ByVal pVal As Integer) As String
        Dim vHor As String = Format(Int(pVal / 60), "00")
        Dim vMin As String = Format(pVal - Val(vHor * 60), "00")
        MinutesToTime = vHor & ":" & vMin
    End Function

    Public Function GetPeriodo(ByVal pVal As Long) As String
        If pVal.ToString.Length = 6 Then
            GetPeriodo = pVal.ToString.Substring(5, 2) & "/" & pVal.ToString.Substring(0, 4)
        Else
            GetPeriodo = ""
        End If
    End Function

    Public Function SetPeriodo(ByVal pVal As String) As Long
        If pVal.Length = 7 And pVal.Contains("/") Then
            Dim vPer() As String = pVal.Split("/")
            SetPeriodo = Val(vPer(1) & vPer(0))
        Else
            SetPeriodo = 0
        End If
    End Function

End Module
