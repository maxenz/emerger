Imports System.Data
Imports System.Data.SqlClient
Public Class conTalonarios
    Inherits typTalonarios
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll(ByVal pFac As Boolean) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT DISTINCT tal.ID, emp.AbreviaturaId AS Empresa, tal.Letra, tal.NroSucursal, tal.NroSiguiente, img.Descripcion AS Imagen "
            SQL = SQL & "FROM Talonarios tal "
            SQL = SQL & "INNER JOIN TalonariosDocumentos doc ON (tal.ID = doc.TalonarioId) "
            SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (doc.TipoComprobanteId = tdc.ID) "
            SQL = SQL & "LEFT JOIN EmpresasLegales emp ON (tal.EmpresaLegalId = emp.ID) "
            SQL = SQL & "LEFT JOIN ImagenDocumentos img ON (tal.ImagenDocumentoId = img.ID) "

            If pFac Then
                SQL = SQL & "WHERE (tdc.ClasificacionId = " & cmpClasificacion.cmpFacturacion & ") "
            Else
                SQL = SQL & "WHERE (tdc.ClasificacionId = " & cmpClasificacion.cmpCobranzaFacturacion & ") "
            End If

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdEqp.ExecuteReader)

            dt.Columns.Add("Documentos", GetType(String))

            For vIdx = 0 To dt.Rows.Count - 1
                dt(vIdx)("Documentos") = Me.getTiposDocumentosStr(dt(vIdx)("ID"))
            Next vIdx

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function
    Private Function getTiposDocumentosStr(ByVal pTal As Int64, Optional ByVal pTdc As String = "") As String
        getTiposDocumentosStr = ""
        Try
            Dim SQL As String, vStr As String = ""

            SQL = "SELECT tdc.AbreviaturaId FROM TalonariosDocumentos tal "
            SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (tal.TipoComprobanteId = tdc.ID) "
            SQL = SQL & "WHERE (tal.TalonarioId = " & pTal & ") "

            If pTdc.Length > 0 Then SQL = SQL & "AND (tdc.AbreviaturaId = '" & pTdc & "') "
            SQL = SQL & "ORDER BY tdc.AbreviaturaId"

            Dim cmdTal As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdTal.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                If vStr = "" Then
                    vStr = dt(vIdx).Item(0)
                Else
                    vStr = vStr & " - " & dt(vIdx).Item(0)
                End If

            Next vIdx

            getTiposDocumentosStr = vStr

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getTiposDocumentosStr", ex)
        End Try

    End Function
    Public Function GetNextNroTalonario() As Int64
        GetNextNroTalonario = 0
        Try
            Dim SQL As String
            SQL = "SELECT ISNULL(MAX(NroTalonario), 0) + 1 FROM Talonarios"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then
                GetNextNroTalonario = CType(vOutVal, Int64)
            Else
                GetNextNroTalonario = 1
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNextNroTalonario", ex)
        End Try
    End Function

    Public Function haveFacturacion(pTal As Int64) As Boolean
        haveFacturacion = False
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM VentasDocumentos WHERE TalonarioId = " & pTal

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then
                haveFacturacion = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "haveFacturacion", ex)
        End Try
    End Function

    Public Function Validar(ByVal pCntTdc As Integer, Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.NroTalonario <= 0 Then vRdo = "El número establecido de talonario es incorrecto"
            If Me.NroInicial <= 0 And vRdo = "" Then vRdo = "El número inicial debe ser superior a 0"
            If Me.NroFinal <= 0 And vRdo = "" Then vRdo = "El número final debe ser superior a 0"
            If Me.NroSiguiente <= 0 And vRdo = "" Then vRdo = "El número siguiente debe ser superior a 0"
            If Me.NroInicial > Me.NroFinal And vRdo = "" Then vRdo = "El número inicial debe ser inferior al número final"
            If Me.NroInicial > Me.NroSiguiente And vRdo = "" Then vRdo = "El número inicial debe ser inferior o igual al número siguiente"
            If Me.NroSiguiente > Me.NroFinal And vRdo = "" Then vRdo = "El número final debe ser superior o igual al número siguiente"
            If pCntTdc = 0 And vRdo = "" Then vRdo = "Debe seleccionar al menos un tipo de comprobante"
            If pCntTdc > 1 And Me.TipoControl = ctrTalonarios.CAE And vRdo = "" Then vRdo = "Para emisión por CAE solo se permite un tipo de comprobante"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNextNroTalonario", ex)
        End Try
    End Function
End Class
