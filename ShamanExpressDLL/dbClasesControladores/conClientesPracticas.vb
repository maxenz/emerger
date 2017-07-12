Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesPracticas
    Inherits typClientesPracticas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pCli As Int64, ByVal pPra As Int64, Optional ByVal pPla As Int64 = 0) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM ClientesPracticas "
            SQL = SQL & "WHERE (ClienteId = " & pCli & ") AND (PracticaId = " & pPra & ") "
            SQL = SQL & "AND (ClientePlanInternoId = " & pPla & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetByCliente(ByVal pCli As Long, Optional ByVal pPla As Int64 = 0) As DataTable

        GetByCliente = Nothing

        Try

            Dim dtBind As New DataTable

            dtBind.Columns.Add("ID", GetType(Int64))
            dtBind.Columns.Add("Descripcion", GetType(String))
            dtBind.Columns.Add("Cubierto", GetType(Boolean))
            dtBind.Columns.Add("CoPago", GetType(Double))

            Dim SQL As String

            SQL = "SELECT ID, Descripcion FROM Practicas ORDER BY Descripcion"

            Dim cmBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            Dim objRelacion As New conClientesPracticas(Me.myCnnName)

            dt.Load(cmBas.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                Dim dtRow As DataRow = dtBind.NewRow

                dtRow("ID") = dt.Rows(vIdx)("ID")
                dtRow("Descripcion") = dt.Rows(vIdx)("Descripcion")
                dtRow("Cubierto") = True
                dtRow("CoPago") = 0

                If objRelacion.Abrir(objRelacion.GetIDByIndex(pCli, dt.Rows(vIdx).Item(0), pPla)) Then
                    dtRow("Cubierto") = setIntToBool(objRelacion.flgCubierto)
                    dtRow("CoPago") = objRelacion.CoPago
                End If

                dtBind.Rows.Add(dtRow)

            Next vIdx

            objRelacion = Nothing

            GetByCliente = dtBind

        Catch ex As Exception
            HandleError(Me.GetType.Name, "RefreshGrid", ex)
        End Try
    End Function

End Class
