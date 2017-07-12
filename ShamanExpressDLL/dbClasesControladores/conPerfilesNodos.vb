Imports System.Data
Imports System.Data.SqlClient

Public Class conPerfilesNodos
    Inherits typPerfilesNodos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pPer As Int64, ByVal pNod As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            '--------> QUERY
            SQL = "SELECT ID FROM PerfilesNodos WHERE PerfilId = " & pPer & " AND sysNodoId = " & pNod

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetDescripcionAcceso(ByVal pAcc As Integer) As String
        Select Case pAcc
            Case 1 : GetDescripcionAcceso = "Solo Lectura"
            Case 2 : GetDescripcionAcceso = "Escritura"
            Case 3 : GetDescripcionAcceso = "Administración"
            Case Else : GetDescripcionAcceso = "Sin Acceso"
        End Select
    End Function
End Class
