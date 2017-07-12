Imports System.Data
Imports System.Data.SqlClient
Public Class conTemplates
    Inherits typTemplates

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIDByClienteId(ByVal pCli As Int64) As Int64
        GetIDByClienteId = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Templates WHERE ClienteId = " & pCli

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByClienteId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByClienteId", ex)
        End Try
    End Function

    Public Function GetAll(ByVal pRepId As rptReporteadores, Optional flgImportacion As Integer = 0) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, Titulo, flgPublico, PropietarioId FROM Templates "
            SQL = SQL & "WHERE (ReporteadorId = " & pRepId & ") "
            SQL = SQL & "AND (ISNULL(flgImportacion, 0) = " & flgImportacion & ") "
            SQL = SQL & "ORDER BY Titulo"

            Dim cmdTem As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdTem.ExecuteReader)

            Dim dtBind As DataTable = dt.Clone
            dtBind.Columns.Remove("flgPublico")
            dtBind.Columns.Add("Alcance", GetType(String))

            For vIdx = 0 To dt.Rows.Count - 1
                Dim dtRow As DataRow = dtBind.NewRow

                dtRow("ID") = dt.Rows(vIdx)("ID")
                dtRow("Titulo") = dt.Rows(vIdx)("Titulo")
                If dt(vIdx).Item(2) = 1 Then
                    dtRow("Alcance") = "Público"
                Else
                    dtRow("Alcance") = "Privado"
                End If
                dtRow("PropietarioId") = dt.Rows(vIdx)("PropietarioId")

                If dt(vIdx).Item(2) = 1 Or (dt(vIdx)(3) = logUsuario) Then
                    dtBind.Rows.Add(dtRow)
                End If

            Next vIdx

            GetAll = dtBind

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function



End Class
