Imports System.Data
Imports System.Data.SqlClient
Public Class conVehiculos
    Inherits typVehiculos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Overrides Function CanDelete(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean
        CanDelete = False

        Try
            Dim SQL As String = "SELECT TOP 1 Movil FROM Moviles WHERE VehiculoId = " & pId
            Dim cmdVeh As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vVal As String = CType(cmdVeh.ExecuteScalar, String)

            If Not vVal Is Nothing Then
                If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "El vehículo está vinculado al móvil " & vVal, MsgBoxStyle.Critical, "Shaman")
            Else
                SQL = "SELECT TOP 1 ID FROM MovilesHistorias WHERE VehiculoId = " & pId
                cmdVeh.CommandText = SQL
                If Not cmdVeh.ExecuteScalar Is Nothing Then
                    If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "Existen servicios anteriores realizados por dicho vehículo", MsgBoxStyle.Critical, "Shaman")
                Else
                    CanDelete = True
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CanDelete", ex)
        End Try
    End Function
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT veh.ID, veh.Dominio, mar.Marca + ' - ' + mar.Modelo AS MarcaModelo, "
            SQL = SQL & "CASE veh.Situacion WHEN 0 THEN 'BAJA' WHEN 1 THEN 'ASIGNADO' WHEN 2 THEN 'SIN ASIGNAR' WHEN 3 THEN 'MULETO' END AS Situacion "
            SQL = SQL & "FROM Vehiculos veh "
            SQL = SQL & "LEFT JOIN MarcasModelos mar ON (veh.MarcaModeloId = mar.ID) "
            SQL = SQL & "ORDER BY veh.Dominio"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.Dominio = "" Then vRdo = "Debe determinar el número de dominio de la unidad"
            If Me.MarcaModeloId.GetObjectId = 0 Then vRdo = "Debe determinar la marca y el modelo de la unidad"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
    Private Function shrSituacion(ByVal pVal As String) As String
        shrSituacion = ""
        Try
            If UCase(pVal) Like "B" Then
                shrSituacion = "0"
            ElseIf UCase(pVal) Like "A" Then
                shrSituacion = "1"
            ElseIf UCase(pVal) Like "S" Then
                shrSituacion = "2"
            ElseIf UCase(pVal) Like "M" Then
                shrSituacion = "3"
            ElseIf pVal <> "" Then
                shrSituacion = pVal
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "shrSituacion", ex)
        End Try
    End Function
    Public Function getSituacion(ByVal pVal As Byte) As String
        getSituacion = ""
        Try
            Select Case pVal
                Case 0 : getSituacion = "BAJA"
                Case 1 : getSituacion = "ASIGNADO"
                Case 2 : getSituacion = "SIN ASIGNAR"
                Case 3 : getSituacion = "MULETO"
            End Select
        Catch ex As Exception
            HandleError(Me.GetType.Name, "getSituacion", ex)
        End Try
    End Function
    Public Function GetIDByDominio(ByVal pVal As String) As Int64
        GetIDByDominio = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM Vehiculos WHERE Dominio = '" & UCase(pVal) & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDominio = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDominio", ex)
        End Try
    End Function
    Public Function getMovil() As String
        getMovil = ""
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT Movil FROM Moviles WHERE VehiculoId = " & Me.ID

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getMovil = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getMovil", ex)
        End Try
    End Function
    Public Sub SetKilometraje(pVeh As Int64, pKmt As Int64)
        Try
            If Me.Abrir(pVeh) Then
                If Me.Kilometraje < pKmt Then
                    Me.Kilometraje = pKmt
                    Me.regKilometraje = GetCurrentTime()
                    Me.Salvar(Me)
                End If
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetKilometraje", ex)
        End Try
    End Sub

End Class
