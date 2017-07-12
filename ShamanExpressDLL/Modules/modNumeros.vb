Public Module modNumeros
    Public wSepDecimal As String

    Public Sub InitSepDecimal()
        If CDec("2.5") > 3 Then
            wSepDecimal = ","
        Else
            wSepDecimal = "."
        End If
    End Sub

    Public Function prnDbl(ByVal pVal As String, Optional ByVal pDig As Integer = 2, Optional ByVal pNul As Boolean = False) As String
        Dim vIdx As Integer, vFor As String
        vFor = ""
        For vIdx = 1 To pDig
            vFor = vFor & "0"
        Next vIdx
        If Not pNul Then
            prnDbl = Format(GetDouble(pVal), "##,##0." & Format(0, vFor))
        Else
            If pVal = 0 Or pVal = -1 Then
                prnDbl = ""
            Else
                prnDbl = Format(GetDouble(pVal), "##,##0." & Format(0, vFor))
            End If
        End If
    End Function
    Public Function shwDbl(ByVal pVal As Double, Optional ByVal pDig As Integer = 2, Optional ByVal pNul As Boolean = False) As String
        Dim vIdx As Integer, vFor As String
        vFor = ""
        For vIdx = 1 To pDig
            vFor = vFor & "0"
        Next vIdx
        If Not pNul Then
            shwDbl = Format(pVal, "##,##0." & Format(0, vFor))
        Else
            If pVal = 0 Or pVal = -1 Then
                shwDbl = ""
            Else
                shwDbl = Format(pVal, "##,##0." & Format(0, vFor))
            End If
        End If
    End Function
    Public Function GetDouble(ByVal pVal As String, Optional ByVal pFcv As Boolean = False) As Double
        GetDouble = 0
        Try
            If Not pVal Is Nothing Then
                pVal = pVal.Replace("%", "")
                pVal = pVal.Trim
                If (Left(pVal, 1) = ".") Or (Left(pVal, 1) = ",") Then
                    pVal = "0" & wSepDecimal & Mid(pVal, 2, Len(pVal))
                End If
                If pFcv Then
                    If InStr(1, pVal, ".") > 0 And wSepDecimal = "," Then
                        pVal = Parcer(pVal, ".") & wSepDecimal & Parcer(pVal, ".", 1)
                    End If
                End If
                GetDouble = CDbl(pVal)
            End If
        Catch ex As Exception
            GetDouble = 0
        End Try
    End Function
    Public Function GetNumericValue(ByVal pVal As String) As Decimal
        GetNumericValue = 0

        Try
            Dim vIdx As Integer
            Dim vValNew As String = ""

            For vIdx = 0 To pVal.Length - 1
                If Asc(pVal.Substring(vIdx, 1)) >= 48 And Asc(pVal.Substring(vIdx, 1)) <= 57 Then
                    If vValNew = "" Then
                        vValNew = pVal.Substring(vIdx, 1)
                    Else
                        vValNew = vValNew & pVal.Substring(vIdx, 1)
                    End If
                End If
            Next

            GetNumericValue = CDec(vValNew)

        Catch ex As Exception

        End Try

    End Function

    Public Function GetCoordenada(pVal As String) As Double
        GetCoordenada = 0
        Try

            Dim vCoo As Double = 0
            Dim vStrCoo() As String = pVal.Split(" ")

            vCoo = Val(vStrCoo(0)) + (Val(vStrCoo(2)) / 60) + (GetDouble(vStrCoo(4).Replace(".", wSepDecimal)) / 3600)
            If vStrCoo(6) = "S" Or vStrCoo(6) = "O" Then vCoo = vCoo * -1
            GetCoordenada = vCoo

        Catch ex As Exception

        End Try
    End Function

    Public Function ShowCoordenada(ByVal pVal As Double, Optional pLatMod As Boolean = True) As String

        ShowCoordenada = ""

        Try

            Dim vDec As Double
            Dim vGdo As String
            Dim vMin As String
            Dim vSeg As String
            Dim vPoi As String

            If Left(pVal, 1) = "-" Then
                If pLatMod Then
                    vPoi = "S"
                Else
                    vPoi = "O"
                End If
            Else
                If pLatMod Then
                    vPoi = "N"
                Else
                    vPoi = "E"
                End If
            End If

            pVal = Math.Abs(pVal)

            vGdo = Format(Int(pVal), "00")
            vDec = (pVal - Int(pVal)) * 60
            vMin = Format(Int(vDec), "00")
            vDec = (vDec - Int(vDec)) * 60
            vSeg = shwDbl(vDec, 1).Replace(wSepDecimal, ".")

            If InStr(vSeg, ".") = 2 Then
                vSeg = "0" & vSeg
            End If

            ShowCoordenada = vGdo & " ° " & vMin & " " & Chr(39) & " " & vSeg & " " & Chr(39) & Chr(39) & " " & vPoi

        Catch ex As Exception

        End Try

    End Function

End Module
