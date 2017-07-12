Imports System.Data
Imports System.Data.SqlClient
Public Class conPlanesTarifasIntegrantesRangos
    Inherits typPlanesTarifasIntegrantesRangos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByPlanTarifaIntegrante(ByVal pPlaItg As Int64) As DataTable

        GetByPlanTarifaIntegrante = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT Desde, Hasta, Importe FROM PlanesTarifasIntegrantesRangos "
            SQL = SQL & "WHERE (PlanTarifaIntegranteId = " & pPlaItg & ") "
            SQL = SQL & "ORDER BY Desde"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByPlanTarifaIntegrante = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "RefreshGrid", ex)
        End Try
    End Function

End Class
