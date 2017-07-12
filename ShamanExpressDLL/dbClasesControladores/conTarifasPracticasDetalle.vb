Imports System.Data
Imports System.Data.SqlClient
Public Class conTarifasPracticasDetalle
    Inherits typTarifasPracticasDetalle
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByTarifa(ByVal pTar As Int64) As DataTable

        GetByTarifa = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT tar.ID, pra.Descripcion, "
            SQL = SQL & "CASE tar.TipoAplicacion WHEN 0 THEN 'Exento' WHEN 1 THEN 'Recargo' WHEN 2 THEN 'Descuento' ELSE 'Importe Fijo' END AS Aplicacion, "
            SQL = SQL & "tar.Valor "
            SQL = SQL & "FROM TarifasPracticasDetalle tar "
            SQL = SQL & "INNER JOIN Practicas pra ON (tar.PracticaId = pra.ID) "
            SQL = SQL & "WHERE (tar.TarifaPracticaId = " & pTar & ") "
            SQL = SQL & "ORDER BY pra.Descripcion"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByTarifa = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByTarifa", ex)
        End Try
    End Function
    Private Function getTipoAplicacion(ByVal pVal As Byte) As String
        getTipoAplicacion = ""
        Try
            Select Case pVal
                Case 0 : getTipoAplicacion = "Exento"
                Case 1 : getTipoAplicacion = "Recargo"
                Case 2 : getTipoAplicacion = "Descuento"
                Case 3 : getTipoAplicacion = "Importe Fijo"
            End Select
        Catch ex As Exception
            HandleError(Me.GetType.Name, "getTipoAplicacion", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.PracticaId.GetObjectId = 0 Then vRdo = "Debe determinar la práctica de la tarifa"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
