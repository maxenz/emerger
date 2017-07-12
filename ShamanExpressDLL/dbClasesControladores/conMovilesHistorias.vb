Imports System.Data
Imports System.Data.SqlClient
Public Class conMovilesHistorias
    Inherits typMovilesHistorias
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByMovil(ByVal pMov As Int64) As DataTable

        GetByMovil = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT his.ID, his.VigenciaDesde, his.VigenciaHasta, his.Movil, tmv.Descripcion AS TipoMovil, veh.Dominio FROM MovilesHistorias his "
            SQL = SQL & "INNER JOIN TiposMoviles tmv ON (his.TipoMovilId = tmv.ID) "
            SQL = SQL & "LEFT JOIN Vehiculos veh ON (his.VehiculoId = veh.ID) "
            SQL = SQL & "WHERE (his.MovilId = " & pMov & ") "
            SQL = SQL & " ORDER BY his.VigenciaDesde DESC"

            Dim cmdHis As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdHis.ExecuteReader)

            GetByMovil = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByMovil", ex)
        End Try

    End Function
    Public Sub SetHistorial(ByVal pMov As Int64)
        Try
            Dim SQL As String, vHis As Int64 = 0, objMovil As New conMoviles(Me.myCnnName), objHis As New conMovilesHistorias(Me.myCnnName), objVehiculo As New conVehiculos(Me.myCnnName)
            Dim vAdd As Boolean = False, vUpd As Boolean = True
            '----> Query Last
            SQL = "SELECT TOP 1 ID FROM MovilesHistorias WHERE MovilId = " & pMov & " ORDER BY VigenciaDesde DESC"
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then vHis = CType(vOutVal, Int64)
            If vHis > 0 Then
                '-----> Comparo ambos objetos
                If objMovil.Abrir(pMov) Then
                    If objHis.Abrir(vHis) Then
                        If objMovil.relTabla <> objHis.relTabla Then
                            vAdd = True
                        ElseIf objMovil.Movil <> objHis.Movil Then
                            vAdd = True
                        ElseIf objMovil.TipoMovilId.ID <> objHis.TipoMovilId.ID Then
                            vAdd = True
                        ElseIf objMovil.BaseOperativaId.ID <> objHis.BaseOperativaId.ID Then
                            vAdd = True
                        ElseIf objMovil.VehiculoId.ID <> objHis.VehiculoId.ID Then
                            vAdd = True
                        ElseIf objMovil.PrestadorId.ID <> objHis.PrestadorId.ID Then
                            vAdd = True
                        ElseIf objMovil.Activo <> objHis.Activo Then
                            vAdd = True
                        End If
                        If vAdd Then
                            If objHis.VigenciaDesde < Now.Date Then
                                objHis.VigenciaHasta = Now.AddDays(-1).Date
                                If objHis.Salvar(objHis) Then
                                    vUpd = AddHistorial(pMov)
                                End If
                            Else
                                vUpd = AddHistorial(pMov, objHis.ID)
                            End If
                        End If
                    End If
                End If
            Else
                vUpd = AddHistorial(pMov)
            End If
            '---> Cierro Objetos
            objHis = Nothing
            '-----> Comparo ambos objetos
            If vUpd Then
                If objMovil.Abrir(pMov) Then
                    If objVehiculo.Abrir(objMovil.VehiculoId.ID) Then
                        objVehiculo.Situacion = objMovil.Activo
                        objVehiculo.Salvar(objVehiculo)
                    End If
                End If
                '-----> Arreglo Estado
                SQL = "UPDATE Vehiculos SET Situacion = 2 WHERE Situacion = 1 AND ID NOT IN(SELECT VehiculoId FROM Moviles) "
                Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                cmDel.ExecuteNonQuery()
            End If
            objMovil = Nothing
            objVehiculo = Nothing
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetHistorial", ex)
        End Try
    End Sub
    Private Function AddHistorial(ByVal pMov As Long, Optional ByVal pIdMod As Long = 0) As Boolean
        Dim objMovil As New conMoviles(Me.myCnnName), objHis As New conMovilesHistorias(Me.myCnnName), vPro As Boolean = True
        AddHistorial = False
        '----> Almacen
        If objMovil.Abrir(pMov) Then
            If pIdMod = 0 Then
                objHis.CleanProperties(objHis)
            Else
                vPro = objHis.Abrir(pIdMod)
            End If
            If vPro Then
                '-----> Salvo
                objHis.MovilId.SetObjectId(objMovil.ID)
                objHis.VigenciaDesde = Now.Date
                objHis.VigenciaHasta = NullDateMax
                objHis.relTabla = objMovil.relTabla
                objHis.Movil = objMovil.Movil
                objHis.TipoMovilId.SetObjectId(objMovil.TipoMovilId.ID)
                objHis.BaseOperativaId.SetObjectId(objMovil.BaseOperativaId.ID)
                objHis.VehiculoId.SetObjectId(objMovil.VehiculoId.ID)
                objHis.PrestadorId.SetObjectId(objMovil.PrestadorId.ID)
                objHis.Activo = objMovil.Activo
                AddHistorial = objHis.Salvar(objHis)
            End If
        End If
        objMovil = Nothing
        objHis = Nothing
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Dim vRdo As String = ""
        Validar = True
        If Me.Movil = "" Then vRdo = "El número de móvil es incorrecto"
        If Me.TipoMovilId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe determinar el tipo de móvil"
        If Me.VigenciaDesde = NullDateMax And vRdo = "" Then vRdo = "La fecha desde es incorrecta"
        If Me.VigenciaHasta = NullDateMax And vRdo = "" Then vRdo = "La fecha hasta es incorrecta"
        If Me.VigenciaDesde = Me.VigenciaHasta And vRdo = "" Then vRdo = "La fecha desde debe ser inferior a la fecha hasta"
        If vRdo = "" Then
            '---------------> Verifiación de Rangos de Fechas (3 - variantes)
            Dim SQL As String
            SQL = "SELECT ID FROM MovilesHistorias WHERE MovilId = " & Me.MovilId.ID & " AND ID <> " & Me.ID
            SQL = SQL & " AND '" & DateToSql(Me.VigenciaDesde) & "' BETWEEN VigenciaDesde AND VigenciaHasta "
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))

            If cmFind.ExecuteScalar Is Nothing Then
                SQL = "SELECT ID FROM MovilesHistorias WHERE MovilId = " & Me.MovilId.ID & " AND ID <> " & Me.ID
                SQL = SQL & " AND '" & DateToSql(Me.VigenciaHasta) & "' BETWEEN VigenciaDesde AND VigenciaHasta "
                cmFind.CommandText = SQL
                If cmFind.ExecuteScalar Is Nothing Then
                    SQL = "SELECT ID FROM MovilesHistorias WHERE MovilId = " & Me.MovilId.ID & " AND ID <> " & Me.ID
                    SQL = SQL & " AND '" & DateToSql(Me.VigenciaDesde) & "' <= VigenciaDesde "
                    SQL = SQL & " AND '" & DateToSql(Me.VigenciaHasta) & "' >= VigenciaHasta "
                    cmFind.CommandText = SQL
                    If Not cmFind.ExecuteScalar Is Nothing Then
                        vRdo = "El rango de fechas se superpone a rangos existentes"
                    End If
                Else
                    vRdo = "El rango de fechas se superpone a rangos existentes"
                End If
            Else
                vRdo = "El rango de fechas se superpone a rangos existentes"
            End If
        End If
        If vRdo <> "" Then
            Validar = False
            If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
        End If
    End Function


End Class
