Imports System.Data
Imports System.Data.SqlClient
Public Class conCobradores
    Inherits typCobradores
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Overrides Function CanDelete(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean
        CanDelete = False

        Try
            Dim SQL As String = "SELECT TOP 1 AbreviaturaId FROM Clientes WHERE CobradorId = " & pId
            Dim cmdCob As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vVal As String = cmdCob.ExecuteScalar

            If Not vVal Is Nothing Then
                If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "El cobrador en cuestión está vinculado al cliente " & vVal, MsgBoxStyle.Critical, "Shaman")
            Else
                SQL = "SELECT TOP 1 NroComprobante FROM VentasDocumentos WHERE CobradorId = " & pId
                cmdCob.CommandText = SQL
                vVal = cmdCob.ExecuteScalar
                If Not vVal Is Nothing Then
                    If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "El cobrador en cuestión está vinculado al comprobante " & vVal, MsgBoxStyle.Critical, "Shaman")
                Else
                    CanDelete = True
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CanDelete", ex)
        End Try
    End Function

    Public Function GetIDByAbreviaturaId(ByVal pVal As String) As Int64

        GetIDByAbreviaturaId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM Cobradores WHERE (AbreviaturaId = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAbreviaturaId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbreviaturaId", ex)
        End Try

    End Function

    Public Function GetIDByDescripcion(ByVal pVal As String) As Int64

        GetIDByDescripcion = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM Cobradores WHERE (Descripcion = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try

    End Function

    Public Function GetAll(Optional pOrdAbr As Boolean = True) As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion, flgCobranzas, flgVentas "
            SQL = SQL & "FROM Cobradores "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdBas.ExecuteReader)

            dt.Columns.Add("Cobra", GetType(Boolean))
            dt.Columns.Add("Vende", GetType(Boolean))

            For vIdx = 0 To dt.Rows.Count - 1
                dt(vIdx)("Cobra") = setIntToBool(dt(vIdx)("flgCobranzas"))
                dt(vIdx)("Vende") = setIntToBool(dt(vIdx)("flgVentas"))
            Next vIdx

            dt.Columns.Remove("flgCobranzas")
            dt.Columns.Remove("flgVentas")

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código del cobrador/vendedor"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar el nombre del cobrador/vendedor"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function


End Class
