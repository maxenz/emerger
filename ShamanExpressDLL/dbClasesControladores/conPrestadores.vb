Imports System.Data
Imports System.Data.SqlClient
Public Class conPrestadores
    Inherits typPrestadores
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByAbreviaturaId(ByVal pAbr As String) As Int64
        GetIDByAbreviaturaId = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Prestadores WHERE (AbreviaturaId = '" & pAbr & "') AND (TipoPrestador = 0) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAbreviaturaId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbreviaturaId", ex)
        End Try
    End Function

    Public Function GetEmpresas(Optional ByVal pAct As Integer = 1) As DataTable

        GetEmpresas = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT pre.ID, pre.AbreviaturaId, pre.RazonSocial AS Prestador, loc.Descripcion AS Localidad, tmv.Descripcion AS Alcance "
            SQL = SQL & "FROM Prestadores pre "
            SQL = SQL & "LEFT JOIN Moviles mov ON (pre.ID = mov.PrestadorId) "
            SQL = SQL & "LEFT JOIN TiposMoviles tmv ON (mov.TipoMovilId = tmv.ID) "
            SQL = SQL & "LEFT JOIN Localidades loc ON (pre.LocalidadId = loc.ID) "
            SQL = SQL & "WHERE (pre.TipoPrestador = 0) "
            If pAct = 1 Then SQL = SQL & "AND (pre.Activo = 1)"
            If pAct = 2 Then SQL = SQL & "AND (pre.Activo = 0)"
            SQL = SQL & "ORDER BY pre.AbreviaturaId"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetEmpresas = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetEmpresas", ex)
        End Try
    End Function
    Public Function GetPrestadoresMoviles(Optional ByVal pAct As Integer = 1) As DataTable

        GetPrestadoresMoviles = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT pre.ID, pre.RazonSocial AS Prestador "
            SQL = SQL & "FROM Prestadores pre WHERE (pre.TipoPrestador = 1) "
            If pAct = 1 Then SQL = SQL & "AND (pre.Activo = 1)"
            If pAct = 2 Then SQL = SQL & "AND (pre.Activo = 0)"
            SQL = SQL & " ORDER BY pre.RazonSocial"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdEqp.ExecuteReader)

            dt.Columns.Add("Telefono", GetType(String))
            dt.Columns.Add("Moviles", GetType(String))

            For vIdx = 0 To dt.Rows.Count - 1
                dt(vIdx)("Telefono") = Me.getTelefonosLine(dt(vIdx).Item(0))
                dt(vIdx)("Moviles") = Me.getMovilesLine(dt(vIdx).Item(0))
            Next

            GetPrestadoresMoviles = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetPrestadoresMoviles", ex)
        End Try
    End Function
    Private Function getTelefonosLine(ByVal pPre As Int64) As String
        getTelefonosLine = ""
        Try
            Dim SQL As String, vTel As String = ""
            SQL = "SELECT Telefono FROM PrestadoresContactos WHERE PrestadorId = " & pPre

            Dim cmdPre As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdPre.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1
                If vTel = "" Then
                    vTel = dt(vIdx).Item(0)
                Else
                    vTel = vTel & " // " & dt(vIdx).Item(0)
                End If
            Next vIdx

            getTelefonosLine = vTel

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getTelefonosLine", ex)
        End Try
    End Function
    Private Function getMovilesLine(ByVal pPre As Int64) As String
        getMovilesLine = ""
        Try
            Dim SQL As String, vMov As String = ""
            SQL = "SELECT A.Movil FROM Moviles A INNER JOIN Vehiculos B ON (A.VehiculoId = B.ID) "
            SQL = SQL & "WHERE (B.PrestadorId = " & pPre & ")"

            Dim cmdPre As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdPre.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1
                If vMov = "" Then
                    vMov = dt(vIdx).Item(0)
                Else
                    vMov = vMov & " // " & dt(vIdx).Item(0)
                End If
            Next vIdx

            getMovilesLine = vMov

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getMovilesLine", ex)
        End Try
    End Function
    Public Function GetMovilesByPrestador(ByVal pPre As Int64) As DataTable

        GetMovilesByPrestador = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT A.ID, A.Dominio, B.Marca + ' - ' + B.Modelo AS MarcaModelo, C.Movil, D.Descripcion AS TipoMovil "
            SQL = SQL & "FROM Vehiculos A "
            SQL = SQL & "INNER JOIN MarcasModelos B ON (A.MarcaModeloId = B.ID) "
            SQL = SQL & "LEFT JOIN Moviles C ON (A.ID = C.VehiculoId) "
            SQL = SQL & "LEFT JOIN TiposMoviles D ON (C.TipoMovilId = D.ID) "
            SQL = SQL & "WHERE (A.PrestadorId = " & pPre & ") ORDER BY C.Movil, A.Dominio"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetMovilesByPrestador = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMovilesByPrestador", ex)
        End Try
    End Function
    Public Function VincularVehiculo(ByVal pPre As Int64, ByVal pVeh As Int64) As Boolean
        VincularVehiculo = False
        Try
            Dim objVehiculo As New conVehiculos(Me.myCnnName)
            If objVehiculo.Abrir(pVeh) Then
                objVehiculo.flgPropio = 0
                objVehiculo.PrestadorId.SetObjectId(pPre)
                If objVehiculo.Salvar(objVehiculo) Then
                    VincularVehiculo = True
                End If
            End If
            objVehiculo = Nothing
        Catch ex As Exception
            HandleError(Me.GetType.Name, "VincularVehiculo", ex)
        End Try
    End Function
    Public Function DesvincularVehiculo(ByVal pVeh As Int64, Optional ByVal pMsg As Boolean = True) As Boolean
        DesvincularVehiculo = False
        Try
            Dim objVehiculo As New conVehiculos(Me.myCnnName)

            If objVehiculo.Abrir(pVeh) Then
                objVehiculo.flgPropio = 1
                objVehiculo.PrestadorId.SetObjectId(0)
                If objVehiculo.Salvar(objVehiculo) Then
                    If pMsg Then
                        MsgBox("Se estableció la unidad  " & objVehiculo.Dominio & " como propia", MsgBoxStyle.Information, "Prestadores")
                    End If
                    DesvincularVehiculo = True
                End If
            End If
            objVehiculo = Nothing
        Catch ex As Exception
            HandleError(Me.GetType.Name, "DesvincularVehiculo", ex)
        End Try
    End Function
    Public Function GetMovilId(ByVal pPre As Int64) As Int64
        GetMovilId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM Moviles WHERE PrestadorId = " & pPre

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetMovilId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMovilId", ex)
        End Try
    End Function
    Public Function SaveAsMovil(ByVal pPre As Int64, ByVal pTmv As Int64) As Boolean
        SaveAsMovil = False
        Try
            Dim objMovil As New conMoviles(Me.myCnnName)
            '-----> Abro o Limpio para nuevo
            objMovil.Abrir(Me.GetMovilId(pPre))
            '-----> Propiedades
            objMovil.Movil = Me.AbreviaturaId
            objMovil.relTabla = 1
            objMovil.TipoMovilId.SetObjectId(pTmv)
            objMovil.PrestadorId.SetObjectId(pPre)
            objMovil.Activo = Me.Activo
            SaveAsMovil = objMovil.Salvar(objMovil)
            objMovil = Nothing
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SaveAsMovil", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" And Me.TipoPrestador = 0 Then vRdo = "Debe determinar el código identificador del prestador"
            If Me.RazonSocial = "" Then vRdo = "Debe determinar la razón social del prestador"
            If Me.LocalidadId.GetObjectId = 0 Then vRdo = "Debe determinar la localidad del prestador"
            If Me.Domicilio.dmCalle = "" Then vRdo = "Debe determinar el domicilio del prestador"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class

