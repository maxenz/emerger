Imports System.Data
Imports System.Data.SqlClient
Public Class conLiquidacionesPrestadores
    Inherits typLiquidacionesPrestadores
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIdByIndex(ByVal pPer As Int64, pPre As Int64) As Int64
        GetIdByIndex = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM LiquidacionesPrestadores WHERE PeriodoLiquidacionId = " & pPer & " AND PrestadorId = " & pPre
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIdByIndex = CType(vOutVal, Int64)
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIdByIndex", ex)
        End Try
    End Function

    Public Function GetByPeriodoLiquidacion(pPerId As Int64) As DataTable

        GetByPeriodoLiquidacion = Nothing

        Try
            Dim SQL As String
            Dim objPeriodo As New conPeriodosLiquidaciones

            If objPeriodo.Abrir(pPerId) Then

                SQL = "SELECT liq.ID, liq.PrestadorId, mov.ID AS MovilId, pre.AbreviaturaId, pre.RazonSocial, liq.Importe "
                SQL = SQL & "FROM LiquidacionesPrestadores liq "
                SQL = SQL & "INNER JOIN Prestadores pre ON liq.PeriodoLiquidacionId = pre.ID "
                SQL = SQL & "INNER JOIN Moviles mov ON pre.ID = mov.PrestadorId "
                SQL = SQL & "WHERE PeriodoLiquidacionId = " & pPerId

                If objPeriodo.flgCerrado = 0 Then

                    SQL = SQL & " UNION "

                    SQL = SQL & "SELECT 0 AS ID, pre.ID AS PrestadorId, mov.ID AS MovilId, pre.AbreviaturaId, pre.RazonSocial, 0.00 AS Importe "
                    SQL = SQL & "FROM Prestadores pre "
                    SQL = SQL & "INNER JOIN Moviles mov ON pre.ID = mov.PrestadorId "
                    SQL = SQL & "WHERE pre.Activo = 1 "
                    SQL = SQL & "AND pre.ID NOT IN(SELECT PrestadorId "
                    SQL = SQL & "FROM LiquidacionesPrestadores WHERE PeriodoLiquidacionId = " & pPerId & ")"

                End If

                SQL = SQL & " ORDER BY 2"

                Dim cmdPre As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dt As New DataTable
                Dim vIdx As Integer
                dt.Load(cmdPre.ExecuteReader)

                dt.Columns.Add("flgSeleccion", GetType(Boolean))
                dt.Columns("flgSeleccion").DefaultValue = False

                For vIdx = 0 To dt.Rows.Count - 1
                    dt(vIdx)("flgSeleccion") = False
                Next

                GetByPeriodoLiquidacion = dt

            End If

            objPeriodo = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByPeriodoLiquidacion", ex)
        End Try
    End Function
End Class
