Imports System.Data
Imports System.Data.SqlClient
Public Class conLiqPrestadoresIncidentesConceptos
    Inherits typLiqPrestadoresIncidentesConceptos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByLiquidacionIncidenteId(pLiqIncId As Int64) As DataTable

        GetByLiquidacionIncidenteId = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT det.ID, con.Descripcion AS Concepto, det.Cantidad, det.Importe "

            SQL = SQL & "FROM LiqPrestadoresIncidentesConceptos det "
            SQL = SQL & "INNER JOIN ConceptosFacturacion con ON det.ConceptoFacturacionId = con.ID "

            SQL = SQL & "WHERE (det.LiqPrestadorIncidenteId = " & pLiqIncId & ") "

            Dim cmdPer As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdPer.ExecuteReader)

            GetByLiquidacionIncidenteId = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByLiquidacionIncidenteId", ex)
        End Try
    End Function

End Class
