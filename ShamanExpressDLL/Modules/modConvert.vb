Module modConvert
    Public Function setByteToSINO(ByVal pVal As Byte) As String
        If pVal = 1 Then
            setByteToSINO = "SI"
        Else
            setByteToSINO = "NO"
        End If
    End Function
    Public Function setBoolToInt(ByVal pVal As Boolean) As Integer
        If pVal Then
            setBoolToInt = 1
        Else
            setBoolToInt = 0
        End If
    End Function
    Public Function setIntToBool(ByVal pVal As Integer) As Boolean
        If pVal = 1 Then
            setIntToBool = True
        Else
            setIntToBool = False
        End If
    End Function
    Public Function setSINOToByte(ByVal pVal As String) As String
        If UCase(Left(pVal, 1)) = "S" Then
            setSINOToByte = 1
        Else
            setSINOToByte = 0
        End If
    End Function
    Public Function Parcer(ByVal pVal As String, Optional ByVal pDel As String = nf, Optional ByVal pPos As Integer = 0, Optional ByVal pTrim As Boolean = True) As String
        Dim vVals() As String, vStr As String
        vVals = Split(pVal, pDel)
        If pPos <= UBound(vVals) Then
            vStr = vVals(pPos)
        Else
            vStr = pVal
        End If
        If pTrim Then vStr = Trim(vStr)
        Parcer = vStr
    End Function
    Public Function toUpperLower(ByVal pVal As String) As String
        toUpperLower = ""
        If pVal <> "" Then
            toUpperLower = pVal.Substring(0, 1).ToUpper & pVal.Substring(1, pVal.Length - 1).ToLower
        End If
    End Function
End Module
