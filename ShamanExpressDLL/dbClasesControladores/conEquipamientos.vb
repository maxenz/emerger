Imports System.Data
Imports System.Data.SqlClient
Public Class conEquipamientos
    Inherits typEquipamientos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetQueryBase() As DataTable

        GetQueryBase = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT eqp.ID, tip.Descripcion AS TipoEquipamiento, mar.Marca, mar.Modelo, eqp.NroSerie, mov.Movil, "
            SQL = SQL & "CASE ISNULL(eqp.DireccionEmail, 'X') WHEN 'X' THEN eqp.Observaciones ELSE eqp.DireccionEmail END AS Referencia "
            SQL = SQL & "FROM Equipamientos eqp "
            SQL = SQL & "INNER JOIN TiposEquipamiento tip ON (eqp.TipoEquipamientoId = tip.ID) "
            SQL = SQL & "LEFT JOIN MarcasEquipamiento mar ON (eqp.MarcaEquipamientoId = mar.ID) "
            SQL = SQL & "LEFT JOIN Moviles mov ON (eqp.MovilId = mov.ID) "
            SQL = SQL & " ORDER BY tip.Descripcion, mar.Marca, mar.Modelo, eqp.NroSerie"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetQueryBase = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryBase", ex)
        End Try
    End Function

    Public Function GetQueryContactos() As DataTable

        GetQueryContactos = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT eqp.ID, mov.Movil, tip.Descripcion AS TipoEquipamiento, mar.Marca, eqp.NroRadio, eqp.NroTelefono, eqp.DireccionEmail "
            SQL = SQL & "FROM Equipamientos eqp "
            SQL = SQL & "INNER JOIN TiposEquipamiento tip ON (eqp.TipoEquipamientoId = tip.ID) "
            SQL = SQL & "LEFT JOIN MarcasEquipamiento mar ON (eqp.MarcaEquipamientoId = mar.ID) "
            SQL = SQL & "LEFT JOIN Moviles mov ON (eqp.MovilId = mov.ID) "
            SQL = SQL & "WHERE (tip.ClasificacionId = 0) "
            SQL = SQL & "ORDER BY mov.Movil"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetQueryContactos = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryContactos", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.TipoEquipamientoId.GetObjectId = 0 Then vRdo = "Debe determinar el tipo de equipamiento"
            If Me.MarcaEquipamientoId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe determinar la marca y modelo del equipamiento"
            If Me.NroSerie = "" And vRdo = "" Then vRdo = "Debe establecer el nro. de serie"
            If Me.Estado = 1 And Me.MovilId.GetObjectId = 0 And Me.PersonalId.GetObjectId = 0 Then vRdo = "Debe establecer el móvil y/o personal"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
