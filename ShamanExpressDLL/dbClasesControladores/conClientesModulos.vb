Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesModulos
    Inherits typClientesModulos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByCliente(ByVal pCli As Int64, Optional ByVal pCon As Int64 = 0) As DataTable

        GetByCliente = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT DISTINCT mdl.ID, mdl.NroModulo, mdl.Cantidad "
            SQL = SQL & "FROM ClientesModulos mdl "
            SQL = SQL & "INNER JOIN ClientesModulosConceptos con ON mdl.ID = con.ClienteModuloId "
            SQL = SQL & "WHERE (mdl.ClienteId = " & pCli & ") "
            If pCon > 0 Then SQL = SQL & "AND (con.ConceptoFacturacionId = " & pCon & ") "
            SQL = SQL & "ORDER BY mdl.NroModulo"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdBas.ExecuteReader)

            dt.Columns.Add("Conceptos", GetType(String))

            For vIdx = 0 To dt.Rows.Count - 1
                dt(vIdx)("Conceptos") = Me.getConceptosString(dt(vIdx)("ID"))
            Next vIdx

            GetByCliente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByCliente", ex)
        End Try
    End Function


    Private Function getConceptosString(ByVal pMod As Long, Optional ByVal pCon As String = "") As String

        getConceptosString = ""

        Try
            Dim SQL As String
            Dim vConStr As String = ""

            SQL = "SELECT con.AbreviaturaId FROM ClientesModulosConceptos mdl "
            SQL = SQL & "INNER JOIN ConceptosFacturacion con ON (mdl.ConceptoFacturacionId = con.ID) "
            SQL = SQL & "WHERE (mdl.ClienteModuloId = " & pMod & ") "
            If pCon <> "" Then SQL = SQL & " AND (con.AbreviaturaId LIKE '%" & pCon & "%') "

            Dim cmdCon As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vTbl As New DataTable
            Dim vIdx As Integer

            vTbl.Load(cmdCon.ExecuteReader)

            For vIdx = 0 To vTbl.Rows.Count - 1

                If vConStr = "" Then
                    vConStr = vTbl.Rows(vIdx)(0)
                Else
                    vConStr = vConStr & " - " & vTbl.Rows(vIdx)(0)
                End If

            Next vIdx

            getConceptosString = vConStr

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getConceptosString", ex)
        End Try
    End Function

    Public Function GetNextNroModulo(ByVal pCli As Int64) As Integer

        GetNextNroModulo = 0

        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(MAX(NroModulo), 0) + 1 FROM ClientesModulos WHERE ClienteId = " & pCli

            Dim cmdNxt As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdNxt.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetNextNroModulo = CInt(vOutVal)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNextNroModulo", ex)
        End Try
    End Function

    Public Function Validar(ByVal pCntCon As Integer, Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.NroModulo <= 0 Then vRdo = "El número de módulo debe ser superior a cero"
            If Me.Cantidad <= 0 And vRdo = "" Then vRdo = "La cantidad debe ser superior a cero"
            If pCntCon <= 0 And vRdo = "" Then vRdo = "Debe seleccionar al menos un concepto"

            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try

    End Function

End Class
