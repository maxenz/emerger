Imports System.Data
Imports System.Data.SqlClient
Public Class conConfiguracionesRegionalesReglas
    Inherits typConfiguracionesRegionalesReglas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pCnfId As Int64, ByVal pVal As String, Optional ByVal pTip As Integer = 0) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM ConfiguracionesRegionalesReglas WHERE ConfiguracionRegionalId = " & pCnfId
            SQL = SQL & " AND TipoConfiguracion = " & pTip & " AND Valor1 = '" & pVal & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function GetValor2(ByVal pCnf As Int64, pVal As String, Optional ByVal pTip As Integer = 0) As String
        GetValor2 = ""
        Try
            Dim SQL As String

            SQL = "SELECT Valor2 FROM ConfiguracionesRegionalesReglas "
            SQL = SQL & "WHERE (ConfiguracionRegionalId = " & pCnf & ") AND (TipoConfiguracion = " & pTip & ") "
            SQL = SQL & "AND (Valor1 = '" & pVal & "') "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetValor2 = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetValor2", ex)
        End Try
    End Function

    Public Function GetValor1(ByVal pCnf As Int64, pVal As String, Optional ByVal pTip As Integer = 0) As String
        GetValor1 = ""
        Try
            Dim SQL As String

            SQL = "SELECT Valor1 FROM ConfiguracionesRegionalesReglas "
            SQL = SQL & "WHERE (ConfiguracionRegionalId = " & pCnf & ") AND (TipoConfiguracion = " & pTip & ") "
            SQL = SQL & "AND (Valor2 = '" & pVal & "') "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetValor1 = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetValor1", ex)
        End Try
    End Function

    Public Function GetByConfiguracionRegional(ByVal pCnf As Int64) As DataTable

        GetByConfiguracionRegional = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, TipoConfiguracion, CASE TipoConfiguracion WHEN 0 THEN 'Etiquetas' ELSE 'Máscara de Ingreso' END AS Tipo, "
            SQL = SQL & "Valor1, Valor2 FROM ConfiguracionesRegionalesReglas "
            SQL = SQL & "WHERE (ConfiguracionRegionalId = " & pCnf & ") "
            SQL = SQL & " ORDER BY TipoConfiguracion, Valor1"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByConfiguracionRegional = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByConfiguracionRegional", ex)
        End Try
    End Function

    Private Function getTipoConfiguracion(pVal As Integer) As String
        getTipoConfiguracion = ""
        Try
            Select Case pVal
                Case 0 : getTipoConfiguracion = "Etiquetas"
                Case 1 : getTipoConfiguracion = "Máscara de Ingreso"
            End Select
        Catch ex As Exception
            HandleError(Me.GetType.Name, "getTipoConfiguracion", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False

        Try

            Dim vRdo As String = ""
            If Me.Valor1 = "" Then vRdo = "Debe establecer el valor 1"
            If Me.Valor2 = "" And vRdo = "" Then vRdo = "Debe establecer el valor 2"

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
