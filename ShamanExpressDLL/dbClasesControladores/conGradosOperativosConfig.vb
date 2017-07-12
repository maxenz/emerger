Imports System.Data
Imports System.Data.SqlClient
Public Class conGradosOperativosConfig
    Inherits typGradosOperativosConfig
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Sub SetDTByGradoOperativo(ByVal pGdo As Int64, ByVal pTip As String, ByRef dtRangos As DataTable)
        Try

            Dim SQL As String

            If pTip = "ALE" Then
                SQL = "SELECT CASE TipoConfiguracion WHEN 'DSP' THEN 'Despacho' WHEN 'SAL' THEN 'Salida' WHEN 'DPL' THEN 'Desplazamiento' WHEN 'ATE' THEN 'Atención' WHEN 'DRV' THEN 'Derivación' WHEN 'ITE' THEN 'Internación' END AS Tiempo, "
                SQL = SQL & "valDesde, valHasta, valRefNumeric FROM GradosOperativosConfig "
                SQL = SQL & "WHERE (GradoOperativoId = " & pGdo & ") AND (TipoConfiguracion <> 'LIS') "
                SQL = SQL & "ORDER BY Tiempo, valDesde"
            Else
                SQL = "SELECT valDesde, valHasta, valRefString, valRefNumeric FROM GradosOperativosConfig "
                SQL = SQL & "WHERE (GradoOperativoId = " & pGdo & ") AND (TipoConfiguracion = '" & pTip & "') "
                SQL = SQL & "ORDER BY valDesde"
            End If

            Dim cmBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vTbl As New DataTable
            Dim vIdx As Integer
            vTbl.Load(cmBas.ExecuteReader)

            For vIdx = 0 To vTbl.Rows.Count - 1

                Dim dtRow As DataRow = dtRangos.NewRow

                If pTip = "ALE" Then
                    dtRow("Tiempo") = vTbl(vIdx)("Tiempo")
                    dtRow("valRefNumeric") = vTbl(vIdx)("valRefNumeric")
                End If

                dtRow("valDesde") = vTbl(vIdx)("valDesde")
                dtRow("valHasta") = vTbl(vIdx)("valHasta")

                dtRangos.Rows.Add(dtRow)

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDTByGradoOperativo", ex)
        End Try
    End Sub

End Class
