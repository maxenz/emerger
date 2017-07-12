Imports System.Data
Imports System.Data.SqlClient
Public Class conTarifasPrestaciones
    Inherits typTarifasPrestaciones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByTarifaVigencia(ByVal pTarVigId As Int64) As DataTable

        GetByTarifaVigencia = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT tar.ID, con.AbreviaturaId AS Concepto, tar.KmDesde, tar.KmHasta, tar.Importe, tar.RecNocturno, tar.RecPediatrico, tar.RecDerivacion, "
            SQL = SQL & "tar.ImpDemora, tar.ImpKmExcedente, tar.ImpDtoRetorno, Alias1, Alias2 "
            SQL = SQL & "FROM TarifasPrestaciones tar "
            SQL = SQL & "INNER JOIN ConceptosFacturacion con ON (tar.ConceptoFacturacionId = con.ID) "
            SQL = SQL & "WHERE (tar.TarifaVigenciaId = " & pTarVigId & ") "
            SQL = SQL & "ORDER BY con.AbreviaturaId, tar.KmDesde"
            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetByTarifaVigencia = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByTarifa", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.ConceptoFacturacionId.GetObjectId = 0 Then vRdo = "Debe determinar el concepto de facturación"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
