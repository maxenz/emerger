Imports System.Data
Imports System.Data.SqlClient
Public Class conMoviles
    Inherits typMoviles
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetQueryBase(Optional ByVal pAct As Integer = 1, Optional ByVal pTab As Integer = 0) As DataTable

        GetQueryBase = Nothing

        Try
            Dim SQL As String = ""

            Select Case pTab
                Case 0 : SQL = "SELECT mov.ID, mov.Movil, tmv.Descripcion AS TipoMovil, veh.Dominio, mar.Marca + ' - ' + mar.Modelo AS MarcaModelo "
                Case 1 : SQL = "SELECT mov.ID, mov.Movil, tmv.Descripcion AS TipoMovil, '' AS Dominio, pre.RazonSocial AS MarcaModelo "
                Case 2 : SQL = "SELECT mov.ID, mov.Movil, tmv.Descripcion AS TipoMovil, '' AS Dominio, per.Apellido + ' ' + per.Nombre AS MarcaModelo "
            End Select

            SQL = SQL & "FROM Moviles mov "
            SQL = SQL & "LEFT JOIN TiposMoviles tmv ON (mov.TipoMovilId = tmv.ID) "
            SQL = SQL & "LEFT JOIN Vehiculos veh ON (mov.VehiculoId = veh.ID) "
            SQL = SQL & "LEFT JOIN MarcasModelos mar ON (veh.MarcaModeloId = mar.ID) "
            SQL = SQL & "LEFT JOIN Prestadores pre ON (mov.PrestadorId = pre.ID) "
            SQL = SQL & "LEFT JOIN Personal per ON (mov.PersonalId = per.ID) "

            If pTab >= 0 Then SQL = sqlWhere(SQL) & "(mov.relTabla = " & pTab & ") "
            If pAct = 1 Then SQL = sqlWhere(SQL) & "(mov.Activo = 1)"
            If pAct = 2 Then SQL = sqlWhere(SQL) & "(mov.Activo = 0)"

            SQL = SQL & " ORDER BY mov.Movil"

            Dim cmdBus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBus.ExecuteReader)

            GetQueryBase = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryBase", ex)
        End Try
    End Function

    Public Function GetQuerySearch(Optional ByVal pTab As Integer = 0, Optional ByVal pAct As Integer = 1) As DataTable

        GetQuerySearch = Nothing

        Try
            Dim SQL As String = ""

            Select Case pTab
                Case 0
                    SQL = "SELECT mov.ID, mov.Movil, tmv.Descripcion AS Descripcion FROM Moviles mov "
                    SQL = SQL & "INNER JOIN TiposMoviles tmv ON (mov.TipoMovilId = tmv.ID) "
                    If pTab >= 0 Then SQL = sqlWhere(SQL) & "(mov.relTabla = " & pTab & ") "
                Case 1
                    SQL = "SELECT mov.ID, mov.Movil, pre.RazonSocial AS Descripcion FROM Moviles mov "
                    SQL = SQL & "INNER JOIN Prestadores pre ON (mov.PrestadorId = pre.ID) "
                    If pTab >= 0 Then SQL = sqlWhere(SQL) & "(mov.relTabla = " & pTab & ") "
                Case 2
                    SQL = "SELECT mov.ID, mov.Movil, per.Apellido + ' - ' + per.Nombre AS Descripcion FROM Moviles mov "
                    SQL = SQL & "INNER JOIN Personal per ON (mov.PersonalId = per.ID) "
                    If pTab >= 0 Then SQL = sqlWhere(SQL) & "(mov.relTabla = " & pTab & ") "
            End Select

            If pAct = 1 Then SQL = sqlWhere(SQL) & "(mov.Activo = 1)"
            If pAct = 2 Then SQL = sqlWhere(SQL) & "(mov.Activo = 0)"
            SQL = SQL & " ORDER BY mov.Movil"

            Dim cmdBus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBus.ExecuteReader)

            GetQuerySearch = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQuerySearch", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.Movil = "" Then vRdo = "El número de móvil es incorrecto"
            If Me.TipoMovilId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe determinar el tipo de móvil"
            If Me.BaseOperativaId.GetObjectId = 0 And Me.relTabla = 0 And vRdo = "" Then vRdo = "Debe determinar la base operativa a la que pertenece"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Function GetIDByMovil(ByVal pVal As String, Optional ByVal pAct As Boolean = True) As Int64
        GetIDByMovil = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM Moviles WHERE Movil = '" & UCase(pVal) & "'"
            If pAct Then SQL = SQL & " AND Activo = 1"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByMovil = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByMovil", ex)
        End Try
    End Function

    Public Function GetLocalidadesByMovil(ByVal pMov As Long) As DataTable

        GetLocalidadesByMovil = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT A.LocalidadId, B.Descripcion, B.AbreviaturaId FROM MovilesLocalidades A "
            SQL = SQL & "INNER JOIN Localidades B ON (A.LocalidadId = B.ID) "
            SQL = SQL & "WHERE (A.MovilId = " & pMov & ") "
            SQL = SQL & "ORDER BY B.Descripcion"

            Dim cmdLoc As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdLoc.ExecuteReader)

            GetLocalidadesByMovil = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetLocalidadesByMovil", ex)
        End Try
    End Function

End Class
