Imports System.Data
Imports System.Data.SqlClient

Public Class conEncuestasIncidentesItems
    Inherits typEncuestasIncidentesItems
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIdByIndex(ByVal pEncIncId As Int64, pItm As Decimal) As Int64
        GetIdByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM EncuestasIncidentesItems WHERE (EncuestaIncidenteId = " & pEncIncId & ") AND (EncuestaItemId = " & pItm & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIdByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIdByIndex", ex)
        End Try

    End Function

End Class

