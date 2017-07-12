Imports System.Data
Imports System.Data.SqlClient
Public Class contmpLogFacturacion
    Inherits typtmpLogFacturacion
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Sub DelByPID(ByVal pPID As Int64)
        Try
            Dim SQL As String = "DELETE FROM tmpLogFacturacion WHERE PID = " & pPID
            Dim cmdDel As New SqlCommand(SQL, cnnsNET(cnnDefault), cnnsTransNET(cnnDefault))
            cmdDel.ExecuteNonQuery()
        Catch ex As Exception
            HandleError(Me.GetType.Name, "DelByPID", ex)
        End Try
    End Sub
    Public Function GetIDByVentaDocumentoId(ByVal pPID As Int64, ByVal pDoc As Int64) As Int64
        GetIDByVentaDocumentoId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM tmpLogFacturacion WHERE PID = " & pPID & " AND VentaDocumentoId = " & pDoc
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByVentaDocumentoId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByVentaDocumentoId", ex)
        End Try

    End Function


End Class
