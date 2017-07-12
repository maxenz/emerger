Imports System.Data
Imports System.Data.SqlClient
Public Class conNomenclador
    Inherits typNomenclador
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion "
            SQL = SQL & "FROM Nomenclador "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Sub SetUnidadesDT(ByVal pNom As Decimal, ByRef dtUnidades As DataTable)
        Try
            Dim SQL As String
            SQL = "SELECT com.UnidadArancelariaId, uni.AbreviaturaId, uni.Descripcion, com.Cantidad "
            SQL = SQL & "FROM NomencladorUnidades com "
            SQL = SQL & "INNER JOIN UnidadesArancelarias uni ON (com.UnidadArancelariaId = uni.ID) "
            SQL = SQL & "WHERE (NomencladorId = " & pNom & ") "
            SQL = SQL & "ORDER BY uni.AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdBas.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                Dim dtRow As DataRow = dtUnidades.NewRow
                Dim vCol As Integer

                For vCol = 0 To dtUnidades.Columns.Count - 1
                    dtRow(vCol) = dt(vIdx)(vCol)
                Next vCol

                dtUnidades.Rows.Add(dtRow)

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetUnidadesDT", ex)
        End Try
    End Sub

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código del nomenclador"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar el nombre del nomenclador"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function


End Class
