Imports System.Data
Imports System.Data.SqlClient
Public Class conAgentesRing
    Inherits typAgentesRing
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetMyCall(Optional ByVal pIsRinging As Boolean = True) As Boolean
        GetMyCall = False
        Try
            If shamanConfig.IntegracionCallCenter <> callIntegraciones.NoIntegrado Then
                Dim SQL As String

                SQL = "SELECT ID FROM AgentesRing WHERE AgenteId = '" & logAgenteId & "' "
                If pIsRinging Then SQL = SQL & "AND (flgAtendido = 0)"

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then
                    Dim vRid As Int64 = CType(vOutVal, Int64)
                    If Me.Abrir(vRid) Then
                        GetMyCall = True
                    End If
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMyCall", ex)
        End Try
    End Function

    Public Function GetIDByAgenteId(ByVal pAge As String) As Int64
        GetIDByAgenteId = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM AgentesRing WHERE (AgenteId = '" & pAge & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAgenteId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAgenteId", ex)
        End Try
    End Function


End Class
