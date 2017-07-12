Imports System.Data
Imports System.Data.SqlClient

Public Class conReporteadoresFields

    Inherits typReporteadoresFields

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIDByIndex(ByVal pRepId As rptReporteadores, ByVal pDes As String) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM ReporteadoresFields WHERE ReporteadorId = " & pRepId & " AND Descripcion = '" & pDes & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function


End Class
