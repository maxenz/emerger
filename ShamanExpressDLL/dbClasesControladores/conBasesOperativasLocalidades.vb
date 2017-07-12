Imports System.Data
Imports System.Data.SqlClient
Public Class conBasesOperativasLocalidades
    Inherits typBasesOperativasLocalidades
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetByBaseOperativa(ByVal pBas As Int64) As DataTable

        GetByBaseOperativa = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT bas.ID, loc.ID AS LocalidadId, pai.AbreviaturaId AS Pais, prv.AbreviaturaId AS Provincia, "
            SQL = SQL & "par.Descripcion AS Partido, loc.Descripcion AS Localidad "

            SQL = SQL & "FROM BasesOperativasLocalidades bas "
            SQL = SQL & "INNER JOIN BasesOperativas cab ON bas.BaseOperativaId = cab.ID "
            SQL = SQL & "INNER JOIN Localidades loc ON bas.LocalidadId = loc.ID "
            SQL = SQL & "INNER JOIN Localidades par ON loc.PartidoId = par.ID "
            SQL = SQL & "INNER JOIN Provincias prv ON loc.ProvinciaId = prv.ID "
            SQL = SQL & "INNER JOIN Paises pai ON prv.PaisId = pai.ID "

            SQL = SQL & "WHERE (cab.ID = " & pBas & ") "

            SQL = SQL & "ORDER BY pai.AbreviaturaId, prv.AbreviaturaId, par.Descripcion, loc.Descripcion "

            Dim cmdPre As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdPre.ExecuteReader)

            GetByBaseOperativa = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByBaseOperativa", ex)
        End Try
    End Function

    Public Function GetLastLocalidad(ByVal pBas As Int64) As Int64

        GetLastLocalidad = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 loc.ID "

            SQL = SQL & "FROM BasesOperativasLocalidades bas "
            SQL = SQL & "INNER JOIN BasesOperativas cab ON bas.BaseOperativaId = cab.ID "
            SQL = SQL & "INNER JOIN Localidades loc ON bas.LocalidadId = loc.ID "

            SQL = SQL & "WHERE (cab.id = " & pBas & ") "

            SQL = SQL & "ORDER BY bas.regFechaHora DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetLastLocalidad = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetLastLocalidad", ex)
        End Try

    End Function


End Class
