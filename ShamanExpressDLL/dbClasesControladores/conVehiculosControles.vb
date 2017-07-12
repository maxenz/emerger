Imports System.Data
Imports System.Data.SqlClient

Public Class conVehiculosControles
    Inherits typVehiculosControles
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetHistorialByVehiculo(pVeh As Int64) As DataTable

        GetHistorialByVehiculo = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, FecHorControl, SPACE(100) AS Realizacion, Kilometraje, Observaciones "
            SQL = SQL & "FROM VehiculosControles "
            SQL = SQL & "WHERE (VehiculoId = " & pVeh & ") "
            SQL = SQL & " ORDER BY FecHorControl"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdGrl.ExecuteReader)

            dt.Columns("Realizacion").ReadOnly = False

            For vIdx = 0 To dt.Rows.Count - 1

                dt(vIdx)("Realizacion") = Me.getControlDetalle(dt(vIdx)(0))

            Next vIdx

            GetHistorialByVehiculo = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetHistorialByVehiculo", ex)
        End Try
    End Function

    Private Function getControlDetalle(pId As Int64) As String

        getControlDetalle = ""

        Try
            Dim SQL As String

            SQL = "SELECT SUM(CASE ISNULL(ServiceVehiculoId, 0) WHEN 0 THEN 0 ELSE 1 END) AS cntService, "
            SQL = SQL & "SUM(CASE ISNULL(SectorArregloVehiculoId, 0) WHEN 0 THEN 0 ELSE 1 END) AS cntArreglo "
            SQL = SQL & "FROM VehiculosControlesServices "
            SQL = SQL & "WHERE (VehiculoControlId = " & pId & ") "

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdGrl.ExecuteReader)

            If dt.Rows.Count > 0 Then

                If dt.Rows(0)(0) > 0 And dt.Rows(0)(1) > 0 Then
                    getControlDetalle = "SERVICE Y MANTENIMIENTO"
                ElseIf dt.Rows(0)(0) > 0 Then
                    getControlDetalle = "SERVICE"
                ElseIf dt.Rows(0)(1) > 0 Then
                    getControlDetalle = "MANTENIMIENTO"
                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getControlDetalle", ex)
        End Try

    End Function

    Public Function Validar(Optional pMsg As Boolean = True) As Boolean
        Validar = False

        Try

            Dim vRdo As String = ""
            If Me.VehiculoId.GetObjectId = 0 Then vRdo = "El vehículo seleccionado es incorrecto"
            If Me.Kilometraje <= 0 And vRdo = "" Then vRdo = "El kilometraje debe ser superior a 0"
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
