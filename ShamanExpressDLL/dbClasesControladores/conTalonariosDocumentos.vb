Imports System.Data
Imports System.Data.SqlClient
Public Class conTalonariosDocumentos
    Inherits typTalonariosDocumentos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIDByIndex(ByVal pTal As Int64, pTdc As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM TalonariosDocumentos WHERE (TalonarioId = " & pTal & ") AND (TipoComprobanteId = " & pTdc & ")"

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbreviaturaId", ex)
        End Try

    End Function

End Class
