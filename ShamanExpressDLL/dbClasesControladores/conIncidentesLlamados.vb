Imports System.Data
Imports System.Data.SqlClient
Public Class conIncidentesLlamados
    Inherits typIncidentesLlamados
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pGrb As String, ByVal pAge As String, ByVal pNro As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM IncidentesLlamados WHERE GrabacionId = '" & pGrb & "' AND AgenteId = '" & pAge & "' AND NroInterno = " & pNro

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
End Class
