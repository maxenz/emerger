Imports System.Data
Imports System.Data.SqlClient
Public Class conGradosOperativos
    Inherits typGradosOperativos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIDByAbreviaturaId(ByVal pVal As String) As Int64

        GetIDByAbreviaturaId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM GradosOperativos WHERE (AbreviaturaId = '" & pVal & "')"

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

            SQL = "SELECT ID FROM GradosOperativos WHERE (Descripcion = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try

    End Function

    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT gdo.ID, gdo.AbreviaturaId, gdo.Descripcion, CASE gdo.flgUrgencia WHEN 0 THEN 'NO' ELSE 'SI' END AS flgUrgencia, "
            SQL = SQL & "agp.AbreviaturaId AS Grupo, gdo.Orden, gdo.VisualColor, gdo.ClasificacionId "
            SQL = SQL & "FROM GradosOperativos gdo "
            SQL = SQL & "LEFT JOIN GradosOperativos agp ON (gdo.GradoAgrupacionId = agp.ID) "
            SQL = SQL & "ORDER BY gdo.Orden"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try

    End Function

    Public Function GetList(Optional pSml As Boolean = False, Optional pOrdPri As Boolean = False) As DataTable

        GetList = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, "
            If pSml Then
                SQL = SQL & " AbreviaturaId AS Descripcion FROM GradosOperativos "
            Else
                SQL = SQL & " Descripcion FROM GradosOperativos "
            End If
            If Not pOrdPri Then
                SQL = SQL & " ORDER BY Descripcion"
            Else
                SQL = SQL & " ORDER BY Orden"
            End If

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetList = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetList", ex)
        End Try

    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador del grado operativo"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del grado operativo"
            If Me.GradoAgrupacionId.GetObjectId = 0 And Me.ID > 0 And vRdo = "" Then vRdo = "Debe determinar el grado con que se agrupa"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Function GetDefault() As Int64
        GetDefault = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM GradosOperativos WHERE flgDefault = 1"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetDefault = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDefault", ex)
        End Try

    End Function
    Public Function SetDefault(ByVal pCen As Int64) As Boolean
        SetDefault = False
        Try
            Dim SQL As String = "UPDATE GradosOperativos SET flgDefault = 0 WHERE flgDefault = 1"
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            Dim objGradosOperativos As New conGradosOperativos(Me.myCnnName)
            If objGradosOperativos.Abrir(pCen) Then
                objGradosOperativos.flgDefault = 1
                SetDefault = objGradosOperativos.Salvar(objGradosOperativos)
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDefault", ex)
        End Try

    End Function


End Class
