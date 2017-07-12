Imports System.Data
Imports System.Data.SqlClient
Public Class conEncuestas
    Inherits typEncuestas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, NroEncuesta, Descripcion, Activo FROM Encuestas "
            SQL = SQL & "ORDER BY Descripcion"

            Dim cmQry As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmQry.ExecuteReader)

            dt.Columns.Add("flgActivo", GetType(Boolean))

            For vIdx = 0 To dt.Rows.Count - 1
                dt(vIdx)("flgActivo") = setIntToBool(dt(vIdx)("Activo"))
            Next

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try

    End Function

    Public Function GetIDByDescripcion(ByVal pVal As String) As Int64

        GetIDByDescripcion = 0

        Try
            Dim SQL As String
            SQL = "SELECT ID FROM Encuestas WHERE Descripcion = '" & pVal & "'"
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try

    End Function
End Class
