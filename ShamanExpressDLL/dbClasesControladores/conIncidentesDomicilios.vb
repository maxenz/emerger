Imports System.Data
Imports System.Data.SqlClient
Public Class conIncidentesDomicilios
    Inherits typIncidentesDomicilios
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pInc As Long, Optional ByVal pTdm As Integer = 0, Optional ByVal pAnx As Integer = 0) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM IncidentesDomicilios WHERE (IncidenteId = " & pInc & ") AND (TipoDomicilio = " & pTdm & ") AND (NroAnexo = " & pAnx & ") "
            SQL = SQL & "ORDER BY ID DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
End Class
