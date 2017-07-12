Imports System.Data
Imports System.Data.SqlClient
Public Class conTarifasVigencias
    Inherits typTarifasVigencias
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll(Optional pLiq As Boolean = False, Optional ByVal pAct As Integer = 1) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT vig.ID, tar.AbreviaturaId, tar.Descripcion, vig.VigenciaDesde, vig.VigenciaHasta "
            SQL = SQL & "FROM TarifasVigencias vig "
            SQL = SQL & "INNER JOIN Tarifas tar ON vig.TarifaId = tar.ID "
            SQL = SQL & "WHERE (tar.flgLiquidacion = " & setBoolToInt(pLiq) & ") "
            If pAct = 1 Then SQL = SQL & "AND ('" & DateToSql(Now.Date) & "' BETWEEN vig.VigenciaDesde AND vig.VigenciaHasta) "
            If pAct = 2 Then SQL = SQL & "AND ('" & DateToSql(Now.Date) & "' NOT BETWEEN vig.VigenciaDesde AND vig.VigenciaHasta) "
            SQL = SQL & "ORDER BY tar.AbreviaturaId"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function GetVigenciasCount(ByVal pTarId As Int64) As Int64
        GetVigenciasCount = 0
        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(COUNT(vig.ID), 0) FROM TarifasVigencias vig "
            SQL = SQL & "INNER JOIN Tarifas pad ON vig.TarifaId = pad.ID "
            SQL = SQL & "INNER JOIN TarifasVigencias hij ON pad.ID = hij.TarifaId "
            SQL = SQL & "WHERE (vig.ID = " & pTarId & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetVigenciasCount = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetVigenciasCount", ex)
        End Try

    End Function

    Public Function GetLastVigenciaId(ByVal pTarId As Int64) As Int64
        GetLastVigenciaId = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM TarifasVigencias WHERE TarifaId = " & pTarId
            SQL = SQL & " ORDER BY VigenciaDesde DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetLastVigenciaId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetLastVigenciaId", ex)
        End Try

    End Function

    Public Function GetVigenciaIdByTarifaFecha(ByVal pTarId As Int64, ByVal pFec As Date) As Int64

        GetVigenciaIdByTarifaFecha = 0

        Try
            Dim SQL As String

            SQL = "SELECT ID FROM TarifasVigencias WHERE TarifaId = " & pTarId
            SQL = SQL & "AND '" & DateToSql(pFec) & "' BETWEEN VigenciaDesde AND VigenciaHasta"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetVigenciaIdByTarifaFecha = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetVigenciaIdByTarifaFecha", ex)
        End Try

    End Function


    Public Sub SetPrestacionesCopyPaste(pTarCpyId As Int64, pTarVigId As Int64)
        Try
            Dim objPrestaciones As New conTarifasPrestaciones(Me.myCnnName)
            Dim dt As DataTable = objPrestaciones.GetByTarifaVigencia(pTarCpyId)
            Dim vIdx As Integer

            For vIdx = 0 To dt.Rows.Count - 1

                If objPrestaciones.Abrir(dt(vIdx)("ID")) Then

                    Dim objAddPrestaciones As New typTarifasPrestaciones(Me.myCnnName)

                    objAddPrestaciones.CleanProperties(objAddPrestaciones)
                    objAddPrestaciones.TarifaVigenciaId.SetObjectId(pTarVigId)

                    objAddPrestaciones.TarifaId.SetObjectId(objPrestaciones.TarifaId.ID)
                    objAddPrestaciones.ConceptoFacturacionId.SetObjectId(objPrestaciones.ConceptoFacturacionId.ID)
                    objAddPrestaciones.KmDesde = objPrestaciones.KmDesde
                    objAddPrestaciones.KmHasta = objPrestaciones.KmHasta
                    objAddPrestaciones.Importe = objPrestaciones.Importe
                    objAddPrestaciones.RecNocturno = objPrestaciones.RecNocturno
                    objAddPrestaciones.RecPediatrico = objPrestaciones.RecPediatrico
                    objAddPrestaciones.RecDerivacion = objPrestaciones.RecDerivacion
                    objAddPrestaciones.ImpDemora = objPrestaciones.ImpDemora
                    objAddPrestaciones.ImpKmExcedente = objPrestaciones.ImpKmExcedente
                    objAddPrestaciones.ImpDtoRetorno = objPrestaciones.ImpDtoRetorno
                    objAddPrestaciones.Alias1 = objPrestaciones.Alias1
                    objAddPrestaciones.Alias2 = objPrestaciones.Alias2

                    If objAddPrestaciones.Salvar(objAddPrestaciones) Then

                    End If

                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPrestacionesFromLastVigencia", ex)
        End Try
    End Sub

    Public Function Validar(Optional pValNew As Boolean = True, Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Dim objVigencia As New conTarifasVigencias

            If pValNew Then
                If objVigencia.Abrir(objVigencia.GetLastVigenciaId(Me.TarifaId.GetObjectId)) Then
                    If objVigencia.VigenciaDesde >= Me.VigenciaDesde Then vRdo = "La fecha desde debe ser superior a " & objVigencia.VigenciaDesde
                End If
            Else

                Dim SQL As String

                SQL = "SELECT TOP 1 ID FROM TarifasVigencias WHERE (TarifaId = " & Me.TarifaId.GetObjectId() & ") "
                SQL = SQL & "AND ('" & DateToSql(Me.VigenciaDesde) & "' BETWEEN VigenciaDesde AND VigenciaHasta) "
                SQL = SQL & "AND (ID <> " & Me.ID & ") "

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then vRdo = "La vigencia 'Desde' se superpone con otro rango"

                If vRdo = "" Then

                    SQL = "SELECT TOP 1 ID FROM TarifasVigencias WHERE (TarifaId = " & Me.TarifaId.GetObjectId() & ") "
                    SQL = SQL & "AND ('" & DateToSql(Me.VigenciaHasta) & "' BETWEEN VigenciaDesde AND VigenciaHasta) "
                    SQL = SQL & "AND (ID <> " & Me.ID & ") "

                    cmFind.CommandText = SQL
                    vOutVal = CType(cmFind.ExecuteScalar, String)
                    If Not vOutVal Is Nothing Then vRdo = "La vigencia 'Hasta' se superpone con otro rango"

                    If vRdo = "" Then

                        SQL = "SELECT TOP 1 ID FROM TarifasVigencias WHERE (TarifaId = " & Me.TarifaId.GetObjectId() & ") "
                        SQL = SQL & "AND ('" & DateToSql(Me.VigenciaDesde) & "' <= VigenciaDesde) "
                        SQL = SQL & "AND ('" & DateToSql(Me.VigenciaHasta) & "' >= VigenciaHasta) "
                        SQL = SQL & "AND (ID <> " & Me.ID & ") "

                        cmFind.CommandText = SQL
                        vOutVal = CType(cmFind.ExecuteScalar, String)
                        If Not vOutVal Is Nothing Then vRdo = "Las vigencias desde/hasta se superponen con un rango existente"

                    End If

                End If

            End If

            objVigencia = Nothing

            If vRdo <> "" Then
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            Else
                Validar = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
