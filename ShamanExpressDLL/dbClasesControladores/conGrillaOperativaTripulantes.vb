Imports System.Data
Imports System.Data.SqlClient
Public Class conGrillaOperativaTripulantes
    Inherits typGrillaOperativaTripulantes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pGrl As Int64, ByVal pMov As Int64, ByVal pPer As Int64, Optional ByVal pIng As Integer = 1) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM GrillaOperativaTripulantes WHERE GrillaOperativaId = " & pGrl & " AND MovilId = " & pMov & " AND PersonalId = " & pPer
            SQL = SQL & " AND IngresoId = " & pIng
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function ficMovimiento(ByVal pPer As Long, ByVal pFec As Date) As Integer
        ficMovimiento = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM GrillaOperativaTripulantes "
            SQL = SQL & "WHERE (PersonalId = " & pPer & ") "
            SQL = SQL & "AND (PacEntrada BETWEEN '" & DateToSql(pFec, , True) & "' AND '" & DateToSql(pFec, , True, False) & "') "
            SQL = SQL & "AND (FicEntrada <> '" & SQLNullDate & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then ficMovimiento = 1

        Catch ex As Exception
            HandleError(Me.GetType.Name, "ficMovimiento", ex)
        End Try
    End Function
    Public Function ficMovil(ByVal pPer As Int64, ByVal pFec As Date) As String
        ficMovil = ""
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT mov.Movil FROM GrillaOperativaTripulantes trp INNER JOIN Moviles mov ON (trp.MovilId = mov.ID) "
            SQL = SQL & "WHERE (trp.PersonalId = " & pPer & ") "
            SQL = SQL & "AND (trp.PacEntrada BETWEEN '" & DateToSql(pFec, , True) & "' AND '" & DateToSql(pFec, , True, False) & "') "
            'AND (CONVERT(char(10), trp.PacEntrada, 121) = '" & DateToSql(pFec) & "') "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then ficMovil = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "ficMovil", ex)
        End Try
    End Function
    Public Function getIngresoId(ByVal pGrl As Int64, ByVal pMov As Int64, ByVal pPer As Int64) As Integer
        getIngresoId = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT TOP 1 IngresoId FROM GrillaOperativaTripulantes WHERE (GrillaOperativaId = " & pGrl & ") "
            SQL = SQL & " AND (MovilId = " & pMov & ") AND (PersonalId = " & pPer & ") "
            SQL = SQL & "ORDER BY IngresoId DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getIngresoId = CType(vOutVal, Integer)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getIngresoId", ex)
        End Try
    End Function
    Public Function SetFichada(ByVal pTmv As Integer) As Boolean
        SetFichada = False
        Try
            Dim SQL As String, vRid As Int64 = 0, objFichada As New conGrillaOperativaTripulantes(Me.myCnnName)
            SetFichada = True
            SQL = "SELECT TOP 1 ID FROM GrillaOperativaTripulantes WHERE (PersonalId = " & Me.PersonalId.GetObjectId & ") "
            SQL = SQL & "AND (MovilId = " & Me.MovilId.GetObjectId & ") "
            If pTmv = 0 Then
                SQL = SQL & "AND (ABS(DATEDIFF(hour, PacEntrada, '" & DateTimeToSql(Now, False) & "')) <= 8) "
                SQL = SQL & "AND (FicEntrada = '" & DateTimeToSql(NullDateMin) & "') "
            Else
                SQL = SQL & "AND (ABS(DATEDIFF(hour, PacSalida, '" & DateTimeToSql(Now, False) & "')) <= 8) "
                SQL = SQL & "AND (FicSalida = '" & DateTimeToSql(NullDateMin) & "') "
            End If
            SQL = SQL & "ORDER BY PacEntrada DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then vRid = CType(vOutVal, Int64)

            '-----> Actualizo
            If Not objFichada.Abrir(vRid) Then
                Dim objGrilla As New conGrillaOperativa
                Dim objMovil As New conMoviles

                objFichada.GrillaOperativaId.SetObjectId(objGrilla.GetIDByIndex(Me.FicEntrada.Date, True))
                objFichada.PersonalId.SetObjectId(Me.PersonalId.GetObjectId)
                objFichada.PuestoGrilla = Me.PuestoGrilla
                If objMovil.Abrir(Me.MovilId.GetObjectId) Then
                    objFichada.MovilId.SetObjectId(objMovil.ID)
                    objFichada.BaseOperativaId.SetObjectId(objMovil.BaseOperativaId.ID)
                    objFichada.VehiculoId.SetObjectId(objMovil.VehiculoId.ID)
                End If
                objFichada.IngresoId = Me.getIngresoId(objFichada.GrillaOperativaId.GetObjectId, objFichada.MovilId.GetObjectId, objFichada.PersonalId.GetObjectId) + 1
                If Me.FicEntrada <> NullDateTime Then
                    objFichada.PacEntrada = Me.FicEntrada
                Else
                    objFichada.PacEntrada = objFichada.FicSalida.AddHours(-8)
                End If
                If Me.FicSalida <> NullDateTime Then
                    objFichada.PacSalida = Me.FicSalida
                Else
                    objFichada.PacSalida = objFichada.PacEntrada.AddHours(8)
                End If
                objFichada.FicEntrada = Me.FicEntrada
                objFichada.FicSalida = Me.FicSalida

                objGrilla = Nothing

            Else
                If pTmv = 0 Then objFichada.FicEntrada = Me.FicEntrada
                If pTmv = 1 Then objFichada.FicSalida = Me.FicSalida

            End If

            objFichada.Observaciones = Me.Observaciones
            SetFichada = objFichada.Salvar(objFichada)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetFichada", ex)
        End Try
    End Function
    Public Function ValidateSplash(ByVal pTmv As Integer, Optional ByVal pMsg As Boolean = True) As Boolean
        ValidateSplash = False

        Try
            Dim objGrilla As New conGrillaOperativa

            Dim vRdo As String = ""
            ValidateSplash = True

            If Me.PersonalId.GetObjectId = 0 Then vRdo = "Debe especificar el personal que ficha"
            If Me.MovilId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe especificar el móvil donde ficha"

            If vRdo = "" Then
                If pTmv = 0 Then
                    If objGrilla.IsGrillaClose(Me.FicEntrada) Then
                        vRdo = "La grilla de la fecha " & Me.FicEntrada.Date & " ya se encuentra cerrada"
                    ElseIf Me.FicEntrada = NullDateTime And vRdo = "" Then
                        vRdo = "El horario de la fichada es incorrecto"
                    End If
                Else
                    If objGrilla.IsGrillaClose(Me.FicSalida) Then
                        vRdo = "La grilla de la fecha " & Me.FicSalida.Date & " ya se encuentra cerrada"
                    ElseIf Me.FicSalida = NullDateTime Then
                        vRdo = "El horario de la fichada es incorrecto"
                    ElseIf Me.ficMovimiento(Me.PersonalId.GetObjectId, Me.FicSalida) = 0 Then
                        vRdo = "No ha dado el ingreso correspondiente a la salida"
                    End If
                End If
            End If

            If vRdo <> "" Then
                ValidateSplash = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, "Ingresos de Personal")
            End If

            objGrilla = Nothing

        Catch ex As Exception

            HandleError(Me.GetType.Name, "ValidateSplash", ex)

        End Try

    End Function
    Public Function Validate(Optional ByVal pTmv As Integer = 0, Optional ByVal pMsg As Boolean = True) As Boolean
        Validate = False
        Try
            Dim objGrilla As New conGrillaOperativa

            Dim vRdo As String = ""
            Validate = True

            If pTmv = 0 Then
                If Me.PacEntrada > Me.PacSalida And pTmv = 0 And vRdo = "" Then
                    vRdo = "El horario de entrada debe ser inferior al horario de salida"
                ElseIf objGrilla.IsGrillaClose(Me.PacEntrada) Then
                    vRdo = "La grilla de la fecha " & Me.PacEntrada.Date & " ya se encuentra cerrada"
                ElseIf DateDiff(DateInterval.Hour, Me.PacEntrada, Me.PacSalida) > 24 Then
                    vRdo = "Las guardias no pueden ser pactadas de mas de 24 horas"
                End If
            Else
                If Me.FicEntrada > Me.FicSalida And Me.FicSalida.Year <> 1899 And pTmv = 1 And vRdo = "" Then
                    vRdo = "El horario de entrada debe ser inferior al horario de salida"
                ElseIf objGrilla.IsGrillaClose(Me.FicEntrada) Then
                    vRdo = "La grilla de la fecha " & Me.FicEntrada.Date & " ya se encuentra cerrada"
                ElseIf DateDiff(DateInterval.Hour, Me.FicEntrada, Me.FicEntrada) > 24 Then
                    vRdo = "Las fichadas no pueden superar las 24 horas"
                End If
            End If

            If Me.PersonalId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe especificar el personal que ficha"
            If Me.MovilId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe especificar el móvil donde ficha"
            If Me.PuestoGrilla = "" And vRdo = "" Then vRdo = "Debe establecer el puesto que ocupa en la grilla"

            If vRdo <> "" Then
                Validate = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If

            objGrilla = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validate", ex)
        End Try
    End Function

End Class
