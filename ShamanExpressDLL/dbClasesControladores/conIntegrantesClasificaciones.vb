Imports System.Data
Imports System.Data.SqlClient
Public Class conIntegrantesClasificaciones
    Inherits typIntegrantesClasificaciones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion "
            SQL = SQL & "FROM IntegrantesClasificaciones "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function GetIDByAbreviaturaId(ByVal pVal As String) As Int64

        GetIDByAbreviaturaId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM IntegrantesClasificaciones WHERE (AbreviaturaId = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAbreviaturaId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbreviaturaId", ex)
        End Try

    End Function

    Public Function GetIDByDescripcion(ByVal pVal As String) As Int64

        GetIDByDescripcion = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM IntegrantesClasificaciones WHERE (Descripcion = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try

    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código de la clasificación"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar el nombre de la clasificación"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Function GetForImportacion() As Int64
        GetForImportacion = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM IntegrantesClasificaciones ORDER BY flgIngresoMultiple DESC, Descripcion DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetForImportacion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDefault", ex)
        End Try
    End Function
    Public Function GetTitularSubGrupo() As Int64
        GetTitularSubGrupo = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM IntegrantesClasificaciones WHERE flgTitularSubGrupo = 1"
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetTitularSubGrupo = CType(vOutVal, Int64)
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetTitularSubGrupo", ex)
        End Try

    End Function
    Public Function SetTitularSubGrupo(ByVal pClf As Int64) As Boolean
        SetTitularSubGrupo = False
        Try
            Dim SQL As String = "UPDATE IntegrantesClasificaciones SET flgTitularSubGrupo = 0 WHERE flgTitularSubGrupo = 1"
            Dim cmUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmUpd.ExecuteNonQuery()

            Dim objIntegrantesClasificaciones As New conIntegrantesClasificaciones(Me.myCnnName)
            If objIntegrantesClasificaciones.Abrir(pClf) Then
                objIntegrantesClasificaciones.flgTitularSubGrupo = 1
                SetTitularSubGrupo = objIntegrantesClasificaciones.Salvar(objIntegrantesClasificaciones)
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetTitularSubGrupo", ex)
        End Try

    End Function

End Class
