Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesPlanes
    Inherits typClientesPlanes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pCli As Int64, ByVal pPla As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM ClientesPlanes WHERE ClienteId = " & pCli & " AND PlanId = " & pPla

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function EliminarByCliente(pId As Int64) As Boolean
        Dim SQL As String

        EliminarByCliente = False
        Try
            Dim cmOpe As New SqlCommand

            SQL = "DELETE FROM ClientesPlanes WHERE ClienteId = " & pId
            cmOpe.Connection = cnnsNET(Me.myCnnName)
            cmOpe.Transaction = cnnsTransNET(Me.myCnnName)
            cmOpe.CommandText = SQL
            cmOpe.ExecuteNonQuery()

            EliminarByCliente = True
        Catch ex As Exception
            HandleError(Me.GetType.Name, "EliminarByCliente", ex)
        End Try
    End Function

    Public Function EliminarByClientePlan(pCli As Int64, pPla As Int64) As Boolean
        Dim SQL As String

        EliminarByClientePlan = False
        Try
            Dim cmOpe As New SqlCommand

            SQL = "DELETE FROM ClientesPlanes WHERE ClienteId = " & pCli & " AND PlanId = " & pPla
            cmOpe.Connection = cnnsNET(Me.myCnnName)
            cmOpe.Transaction = cnnsTransNET(Me.myCnnName)
            cmOpe.CommandText = SQL
            cmOpe.ExecuteNonQuery()

            EliminarByClientePlan = True
        Catch ex As Exception
            HandleError(Me.GetType.Name, "EliminarByCliente", ex)
        End Try
    End Function


    Public Function GetPlanPrincipal(ByVal pCli As Int64) As Int64
        GetPlanPrincipal = 0
        Try
            If pCli > 0 Then
                Dim SQL As String
                SQL = "SELECT PlanId FROM ClientesPlanes WHERE (ClienteId = " & pCli & ") AND (flgPrincipal = 1)"

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then GetPlanPrincipal = CType(vOutVal, Int64)

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetPlanPrincipal", ex)
        End Try
    End Function

    Public Function SetPlanPrincipal(ByVal pCli As Int64, ByVal pPla As Int64) As Boolean
        SetPlanPrincipal = False
        Try
            Dim objPlanPrincipal As New conClientesPlanes(Me.myCnnName)
            If pPla > 0 Then
                If Not objPlanPrincipal.Abrir(objPlanPrincipal.GetIDByIndex(pCli, objPlanPrincipal.GetPlanPrincipal(pCli))) Then
                    objPlanPrincipal.ClienteId.SetObjectId(pCli)
                End If
                objPlanPrincipal.PlanId.SetObjectId(pPla)
                objPlanPrincipal.flgPrincipal = 1
                SetPlanPrincipal = objPlanPrincipal.Salvar(objPlanPrincipal)
            Else
                Dim SQL As String
                SQL = "DELETE FROM ClientesPlanes WHERE ClienteId = " & pCli & " AND flgPrincipal = 1"
                Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                cmdUpd.ExecuteNonQuery()
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPlanPrincipal", ex)
        End Try
    End Function

    Public Sub GetCobertura(ByVal pCli As Int64, ByVal pGdo As Int64, ByRef pCob As Boolean, ByRef pCos As Double, ByRef pExcApl As Integer, ByRef pTar As Int64)
        pCob = False
        pCos = 0
        pExcApl = 0
        pTar = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 cli.ID FROM ClientesPlanes cli "
            SQL = SQL & "INNER JOIN PlanesGradosOperativos gdo ON (cli.PlanId = gdo.PlanId) "
            SQL = SQL & "WHERE (cli.ClienteId = " & pCli & ") AND (gdo.GradoOperativoId = " & pGdo & ") "
            SQL = SQL & "AND (gdo.flgCubierto = 1) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmFind.ExecuteScalar > 0 Then pCob = True

            SQL = "SELECT TOP 1 gdo.CoPago FROM ClientesPlanes cli "
            SQL = SQL & "INNER JOIN PlanesGradosOperativos gdo ON (cli.PlanId = gdo.PlanId) "
            SQL = SQL & "WHERE (cli.ClienteId = " & pCli & ") AND (gdo.GradoOperativoId = " & pGdo & ") "
            SQL = SQL & "AND (gdo.flgCubierto = 1) ORDER BY gdo.CoPago DESC"

            cmFind.CommandText = SQL
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then pCos = CType(vOutVal, Double)

            If pCob Then

                SQL = "SELECT pla.aplExcedente, pla.TarifaId FROM ClientesPlanes cli "
                SQL = SQL & "INNER JOIN Planes pla ON (cli.PlanId = pla.ID) "
                SQL = SQL & "WHERE (cli.ClienteId = " & pCli & ") AND (cli.flgPrincipal = 1) "

                cmFind.CommandText = SQL
                Dim dtVals As New DataTable
                dtVals.Load(cmFind.ExecuteReader)

                If dtVals.Rows.Count > 0 Then

                    pExcApl = dtVals(0)(0)
                    pTar = dtVals(0)(1)

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetCobertura", ex)
        End Try
    End Sub


End Class
