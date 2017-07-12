Imports System.Data
Imports System.Data.SqlClient
Public Class conIncidentesAdjuntos
    Inherits typIncidentesAdjuntos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByIncidente(ByVal pInc As Int64) As DataTable

        GetByIncidente = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AdjuntoClasificacionId, Observaciones, Archivo "
            SQL = SQL & "FROM IncidentesAdjuntos "
            SQL = SQL & "WHERE (IncidenteId = " & pInc & ") "
            SQL = SQL & "ORDER BY regFechaHora"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdGrl.ExecuteReader)

            GetByIncidente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByIncidente", ex)
        End Try
    End Function

End Class
