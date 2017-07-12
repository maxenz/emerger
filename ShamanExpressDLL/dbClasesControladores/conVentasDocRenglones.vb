Imports System.Data
Imports System.Data.SqlClient
Public Class conVentasDocRenglones
    Inherits typVentasDocRenglones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetNextRenglon(ByVal pDoc As Decimal) As Long
        GetNextRenglon = 1
        Try
            Dim SQL As String
            SQL = "SELECT ISNULL(MAX(RenglonId), 0) + 1 FROM VentasDocRenglones WHERE VentaDocumentoId = " & pDoc

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetNextRenglon = CType(vOutVal, Long)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNextRenglon", ex)
        End Try
    End Function
End Class