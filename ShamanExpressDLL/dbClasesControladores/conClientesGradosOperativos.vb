Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesGradosOperativos
    Inherits typClientesGradosOperativos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pCli As Long, ByVal pGdo As Long) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM ClientesGradosOperativos WHERE (ClienteId = " & pCli & ") AND (GradoOperativoId = " & pGdo & ")"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function HaveCoberturaPropia(ByVal pCli As Long) As Boolean
        HaveCoberturaPropia = False
        Try

            Dim SQL As String
            SQL = "SELECT TOP 1 ID FROM ClientesGradosOperativos WHERE (ClienteId = " & pCli & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then HaveCoberturaPropia = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "HaveCoberturaPropia", ex)
        End Try
    End Function

    Public Function GetByCliente(ByVal pCli As Long) As DataTable

        GetByCliente = Nothing

        Try

            Dim dtBind As New DataTable

            dtBind.Columns.Add("ID", GetType(Int64))
            dtBind.Columns.Add("GradoOperativoId", GetType(Int64))
            dtBind.Columns.Add("VisualColor", GetType(Double))
            dtBind.Columns.Add("AbreviaturaId", GetType(String))
            dtBind.Columns.Add("Cubierto", GetType(Boolean))
            dtBind.Columns.Add("CoPago", GetType(Double))

            Dim SQL As String

            SQL = "SELECT DISTINCT agp.GradoAgrupacionId, agp.AbreviaturaId, agp.Orden, agp.VisualColor FROM GradosOperativos gdo "
            SQL = SQL & "INNER JOIN GradosOperativos agp ON (gdo.GradoAgrupacionId = agp.ID) "
            SQL = SQL & "ORDER BY agp.Orden"

            Dim cmBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            Dim objRelacion As New conClientesGradosOperativos(Me.myCnnName)

            dt.Load(cmBas.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                Dim dtRow As DataRow = dtBind.NewRow

                dtRow("ID") = 0
                dtRow("GradoOperativoId") = dt.Rows(vIdx)("GradoAgrupacionId")
                dtRow("VisualColor") = dt.Rows(vIdx)("VisualColor")
                dtRow("AbreviaturaId") = dt.Rows(vIdx)("AbreviaturaId")
                dtRow("Cubierto") = False
                dtRow("CoPago") = 0

                If objRelacion.Abrir(objRelacion.GetIDByIndex(pCli, dt.Rows(vIdx).Item(0))) Then
                    dtRow("ID") = objRelacion.ID
                    dtRow("Cubierto") = setIntToBool(objRelacion.flgCubierto)
                    dtRow("CoPago") = objRelacion.CoPago
                End If

                dtBind.Rows.Add(dtRow)

            Next vIdx

            objRelacion = Nothing

            GetByCliente = dtBind

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByCliente", ex)
        End Try
    End Function

End Class
