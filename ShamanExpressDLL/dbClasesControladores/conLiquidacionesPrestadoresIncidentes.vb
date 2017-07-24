Imports System.Data
Imports System.Data.SqlClient
Public Class conLiquidacionesPrestadoresIncidentes
    Inherits typLiquidacionesPrestadoresIncidentes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByLiquidacionId(pLiqId As Int64) As DataTable

        GetByLiquidacionId = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT det.ID, inc.ID AS IncidenteId, inc.FecIncidente, inc.NroIncidente, ISNULL(con.AbreviaturaId, '') AS Concepto, cli.AbreviaturaId AS Cliente, "
            SQL = SQL & "inc.Paciente, ori.Descripcion AS LocOrigen, dst.Descripcion AS LocDestino, det.Kilometraje, det.Importe "

            SQL = SQL & "FROM LiquidacionesPrestadoresIncidentes det "
            SQL = SQL & "INNER JOIN IncidentesViajes vij ON det.IncidenteViajeId = vij.ID "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON vij.IncidenteDomicilioId = dom.ID "
            SQL = SQL & "INNER JOIN Incidentes inc ON dom.IncidenteId = inc.ID "
            SQL = SQL & "LEFT JOIN ConceptosFacturacion con ON det.ConceptoFacturacionId = con.ID "
            SQL = SQL & "LEFT JOIN Clientes cli ON inc.ClienteId = cli.ID "
            SQL = SQL & "LEFT JOIN Localidades ori ON det.LocalidadOrigenId = ori.ID "
            SQL = SQL & "LEFT JOIN Localidades dst ON det.LocalidadDestinoId = dst.ID "

            SQL = SQL & "WHERE (det.LiquidacionPrestadorId = " & pLiqId & ") "

            SQL = SQL & "ORDER BY inc.FecIncidente, inc.NroIncidente"

            Dim cmdPer As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdPer.ExecuteReader)

            GetByLiquidacionId = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByLiquidacionId", ex)
        End Try
    End Function

End Class
