Imports System.Data
Imports System.Data.SqlClient
Public Class conPersonalHorariosAtencion
    Inherits typPersonalHorariosAtencion
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll(ByVal pPer As Int64) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT hor.ID, hor.DiaSemana AS NroDiaSemana, CASE hor.DiaSemana WHEN 1 THEN 'Lunes' WHEN 2 THEN 'Martes' WHEN 3 THEN 'Miércoles' "
            SQL = SQL & "WHEN 4 THEN 'Jueves' WHEN 5 THEN 'Viernes' WHEN 6 THEN 'Sábado' ELSE 'Domingo' END AS DiaSemana, "
            SQL = SQL & "hor.HorEntrada, hor.HorSalida, cen.AbreviaturaId AS Centro, sal.AbreviaturaId AS Sala, pra.Descripcion AS Practica, "
            SQL = SQL & "CASE hor.modDisponibilidad WHEN 0 THEN 'Mensual' WHEN 1 THEN '1ra Quincencia' WHEN 2 THEN '2da Quincena' ELSE 'Días' END AS Disponibilidad, "
            SQL = SQL & "hor.modDisponibilidadStr, hor.FecVigenciaDesde, hor.FecVigenciaHasta "

            SQL = SQL & "FROM PersonalHorariosAtencion hor "
            SQL = SQL & "LEFT JOIN CentrosAtencionSalas sal ON (hor.CentroAtencionSalaId = sal.ID) "
            SQL = SQL & "LEFT JOIN CentrosAtencion cen ON (sal.CentroAtencionId = cen.ID) "
            SQL = SQL & "LEFT JOIN Practicas pra ON (hor.PracticaId = pra.ID) "

            SQL = SQL & "WHERE (hor.PersonalId = " & pPer & ") "

            SQL = SQL & "ORDER BY hor.DiaSemana, hor.HorEntrada"

            Dim cmdHor As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdHor.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

End Class
