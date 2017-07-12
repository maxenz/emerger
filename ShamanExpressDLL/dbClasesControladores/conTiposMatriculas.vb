Imports System.Data
Imports System.Data.SqlClient
Public Class conTiposMatriculas
    Inherits typTiposMatriculas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable
        GetAll = Nothing
        Try
            Dim SQL As String

            SQL = "SELECT ID, CASE AplicacionId WHEN 0 THEN 'Todos' WHEN 1 THEN 'Médicos' WHEN 2 THEN 'Choferes' WHEN 3 THEN 'Enfermeros' END AS AplicacionId, Descripcion "
            SQL = SQL & "FROM TiposMatriculas "
            SQL = SQL & "ORDER BY 1, 2"

            Dim cmdMat As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdMat.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function GetByAplicable(Optional pAplicacionId As Integer = 0) As DataTable
        GetByAplicable = Nothing
        Try
            Dim SQL As String

            SQL = "SELECT ID, Descripcion FROM TiposMatriculas "
            If pAplicacionId > 0 Then SQL = SQL & "WHERE ((AplicacionId = " & pAplicacionId & ") OR (AplicacionId = 0)) "
            SQL = SQL & "ORDER BY Descripcion"

            Dim cmdMat As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdMat.ExecuteReader)

            GetByAplicable = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByAplicable", ex)
        End Try
    End Function

    Public Function GetAplicables() As DataTable
        GetAplicables = Nothing
        Try

            Dim dt As New DataTable

            dt.Columns.Add("ID", GetType(String))
            dt.Columns.Add("Descripcion", GetType(String))

            Dim dtRow As DataRow = dt.NewRow
            dtRow(0) = 0
            dtRow(1) = ""
            dt.Rows.Add(dtRow)

            dtRow = dt.NewRow
            dtRow(0) = 1
            dtRow(1) = "Médicos"
            dt.Rows.Add(dtRow)

            dtRow = dt.NewRow
            dtRow(0) = 2
            dtRow(1) = "Choferes"
            dt.Rows.Add(dtRow)

            dtRow = dt.NewRow
            dtRow(0) = 3
            dtRow(1) = "Enfermeros"
            dt.Rows.Add(dtRow)

            GetAplicables = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAplicables", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.Descripcion = "" Then vRdo = "Debe determinar la descripción del tipo de matrícula"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class
