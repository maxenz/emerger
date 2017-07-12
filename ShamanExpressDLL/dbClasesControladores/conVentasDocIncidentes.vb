Imports System.Data
Imports System.Data.SqlClient
Public Class conVentasDocIncidentes
    Inherits typVentasDocIncidentes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pDoc As Int64, ByVal pInc As Int64, ByVal pCon As Int64) As Long
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM VentasDocIncidentes WHERE VentaDocumentoId = " & pDoc & " AND IncidenteId = " & pInc & " AND ConceptoFacturacionId = " & pCon

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Long)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
End Class
