Imports System.Data
Imports System.Data.SqlClient
Public Class conAuditIncidentes
    Inherits typAuditIncidentes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByIncidenteId(pInc As Int64) As DataTable

        GetByIncidenteId = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT aud.ID, 0 AS TablaId, fld.Descripcion AS Campo, aud.ValorAnterior, aud.regAntFechaHora, uat.Identificacion AS UsuarioAnterior, "
            SQL = SQL & "aud.ValorNuevo, aud.regFechaHora, usr.Identificacion AS UsuarioNuevo "
            SQL = SQL & "FROM AuditIncidentes aud "
            SQL = SQL & "INNER JOIN ReporteadoresFields fld ON aud.ReporteadorFieldId = fld.ID "
            SQL = SQL & "INNER JOIN Usuarios uat ON aud.regAntUsuarioId = uat.ID "
            SQL = SQL & "INNER JOIN Usuarios usr ON aud.regUsuarioId = usr.ID "

            SQL = SQL & "WHERE (aud.IncidenteId = " & pInc & " ) "

            SQL = SQL & "UNION "

            SQL = SQL & "SELECT aud.ID, 0 AS TablaId, fld.Descripcion AS Campo, aud.ValorAnterior, aud.regAntFechaHora, uat.Identificacion AS UsuarioAnterior, "
            SQL = SQL & "aud.ValorNuevo, aud.regFechaHora, usr.Identificacion AS UsuarioNuevo "
            SQL = SQL & "FROM AuditIncidentesDomicilios aud "
            SQL = SQL & "INNER JOIN IncidentesDomicilios dom ON aud.IncidenteDomicilioId = dom.ID "
            SQL = SQL & "INNER JOIN ReporteadoresFields fld ON aud.ReporteadorFieldId = fld.ID "
            SQL = SQL & "INNER JOIN Usuarios uat ON aud.regAntUsuarioId = uat.ID "
            SQL = SQL & "INNER JOIN Usuarios usr ON aud.regUsuarioId = usr.ID "

            SQL = SQL & "WHERE (dom.IncidenteId = " & pInc & " ) "

            SQL = SQL & "ORDER BY 8 DESC"

            Dim cmdAud As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdAud.ExecuteReader)

            GetByIncidenteId = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByIncidenteId", ex)
        End Try
    End Function

End Class
