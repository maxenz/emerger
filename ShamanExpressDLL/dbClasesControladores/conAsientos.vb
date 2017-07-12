Imports System.Data
Imports System.Data.SqlClient

Public Class conAsientos
    Inherits typAsientos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetQueryBase(ByVal pDes As Date, ByVal pHas As Date, Optional ByVal pAtrId As Int64 = 0, Optional ByVal pTipAtr As Integer = 0) As DataTable
        GetQueryBase = Nothing
        Try

            Dim SQL As String

            SQL = "SELECT asi.ID, tdc.AbreviaturaId AS Tipo, asi.NroComprobante, asi.FecComprobante, "
            SQL = SQL & "CASE ProveedorId WHEN 0 THEN CASE ClienteId WHEN 0 THEN per.Apellido + ' ' + per.Nombre ELSE cli.RazonSocial END ELSE prv.Descripcion END AS Interactor, "
            SQL = SQL & "asi.Concepto "
            SQL = SQL & "FROM Asientos asi "
            SQL = SQL & "INNER JOIN TiposComprobantes tdc ON (asi.TipoComprobanteId = tdc.ID) "
            SQL = SQL & "LEFT JOIN Proveedores prv ON (asi.ProveedorId = prv.ID) "
            SQL = SQL & "LEFT JOIN Clientes cli ON (asi.ClienteId = cli.ID) "
            SQL = SQL & "LEFT JOIN Personal per ON (asi.PersonalId = per.ID) "
            SQL = SQL & "WHERE (asi.FecComprobante BETWEEN '" & DateToSql(pDes) & "' AND '" & DateToSql(pHas) & "') "
            SQL = SQL & "ORDER BY tdc.AbreviaturaId, asi.NroComprobante"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetQueryBase = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryBase", ex)
        End Try
    End Function

    Public Function Validar(pDeb As Decimal, pHab As Decimal, Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = True
        Try
            Dim vRdo As String = ""
            If Me.TipoComprobanteId.GetObjectId = 0 Then vRdo = "Debe determinar el tipo de comprobante"
            If Me.NroComprobante = "" And vRdo = "" Then vRdo = "Debe determinar el número de comprobante"
            If vRdo = "" Then
                Dim objTiposComprobantes As New conTiposComprobantes
                If objTiposComprobantes.Abrir(Me.TipoComprobanteId.GetObjectId) Then
                    Select Case objTiposComprobantes.Aplicable
                        Case 1
                            If Me.ProveedorId.GetObjectId = 0 Then vRdo = "Debe determinar el proveedor del comprobante"
                            If Me.TipoGastoId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe clasificar el gasto"
                        Case 2
                            If Me.PersonalId.GetObjectId = 0 Then vRdo = "Debe determinar el empleado del comprobante"
                        Case 3
                            If Me.ClienteId.GetObjectId = 0 Then vRdo = "Debe determinar el cliente del comprobante"
                            If Me.TipoOtroIngresoId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe clasificar el ingreso"
                    End Select
                End If
            End If
            If pDeb <> pHab And vRdo = "" Then vRdo = "El debe y el haber no coinciden"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Function GetProximoNroOrden(ByVal pTbl As Integer) As Integer
        GetProximoNroOrden = 0
        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(MAX(NroOrden), 0) + 1 FROM AtributosDinamicos WHERE TablaDestinoId = " & pTbl

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            GetProximoNroOrden = cmdFind.ExecuteScalar

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetProximoNroOrden", ex)
        End Try
    End Function

    Public Function MoveFirst(ByVal pFec As Date) As Int64
        MoveFirst = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM Asientos "
            SQL = SQL & "WHERE (FecComprobante = '" & DateToSql(pFec) & "') "
            SQL = SQL & "ORDER BY ID"

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then
                MoveFirst = CType(vOutVal, Int64)
            Else
                SQL = "SELECT TOP 1 ID FROM Asientos "
                SQL = SQL & "ORDER BY ID"
                cmdFind.CommandText = SQL
                vOutVal = cmdFind.ExecuteScalar
                If Not vOutVal Is Nothing Then
                    MoveFirst = CType(vOutVal, Int64)
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "MoveLast", ex)
        End Try

    End Function
    Public Function MovePrevious(ByVal pFec As Date, ByVal pAsiRef As Long) As Int64
        MovePrevious = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM Asientos "
            SQL = SQL & "WHERE (FecComprobante = '" & DateToSql(pFec) & "') "
            SQL = SQL & "AND ID < " & pAsiRef
            SQL = SQL & "ORDER BY ID DESC"

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then
                MovePrevious = CType(vOutVal, Int64)
            Else
                SQL = "SELECT TOP 1 ID FROM Asientos "
                SQL = SQL & "WHERE ID < " & pAsiRef
                SQL = SQL & " ORDER BY ID DESC"
                cmdFind.CommandText = SQL
                vOutVal = cmdFind.ExecuteScalar
                If Not vOutVal Is Nothing Then
                    MovePrevious = CType(vOutVal, Int64)
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "MovePrevious", ex)
        End Try

    End Function
    Public Function MoveNext(ByVal pFec As Date, ByVal pAsiRef As Long) As Int64
        MoveNext = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM Asientos "
            SQL = SQL & "WHERE (FecComprobante = '" & DateToSql(pFec) & "') "
            SQL = SQL & "AND ID > " & pAsiRef
            SQL = SQL & "ORDER BY ID"

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then
                MoveNext = CType(vOutVal, Int64)
            Else
                SQL = "SELECT TOP 1 ID FROM Asientos "
                SQL = SQL & "WHERE ID > " & pAsiRef
                SQL = SQL & "ORDER BY ID"
                cmdFind.CommandText = SQL
                vOutVal = cmdFind.ExecuteScalar
                If Not vOutVal Is Nothing Then
                    MoveNext = CType(vOutVal, Int64)
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "MoveNext", ex)
        End Try

    End Function
    Public Function MoveLast(ByVal pFec As Date) As Int64
        MoveLast = 0

        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 ID FROM Asientos "
            SQL = SQL & "WHERE (FecComprobante = '" & DateToSql(pFec) & "') "
            SQL = SQL & "ORDER BY ID DESC"

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then
                MoveLast = CType(vOutVal, Int64)
            Else
                SQL = "SELECT TOP 1 ID FROM Asientos "
                SQL = SQL & "ORDER BY ID DESC"
                cmdFind.CommandText = SQL
                vOutVal = cmdFind.ExecuteScalar
                If Not vOutVal Is Nothing Then
                    MoveLast = CType(vOutVal, Int64)
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "MoveLast", ex)
        End Try

    End Function

    Public Function GetNextNroComprobante(ByVal pTdc As Int64) As String
        GetNextNroComprobante = " 000000000001"
        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(MAX(NroComprobante), ' 000000000000') FROM Asientos WHERE TipoComprobanteId = " & pTdc

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vNroCmp As String = CType(cmdFind.ExecuteScalar, String)
            If Not vNroCmp Is Nothing Then
                GetNextNroComprobante = " 0000" & Format(Val(vNroCmp.Substring(5, 8)) + 1, "00000000")
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNextNroComprobante", ex)
        End Try
    End Function

    Public Sub SetCuentas(pId As Int64, dt As DataTable)
        Try
            Dim vIdx As Integer
            Dim objCuentas As New typAsientosCuentas

            Dim SQL As String = "UPDATE AsientosCuentas SET flgPurge = 1 WHERE AsientoId = " & pId
            Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdUpd.ExecuteNonQuery()

            For vIdx = 0 To dt.Rows.Count - 1

                objCuentas = New typAsientosCuentas(Me.myCnnName)

                If Not objCuentas.Abrir(dt(vIdx)("ID")) Then
                    objCuentas.AsientoId.SetObjectId(pId)
                End If

                objCuentas.NroRenglon = dt(vIdx)("NroRenglon")
                objCuentas.CuentaAsientoId.SetObjectId(dt(vIdx)("CuentaAsientoId"))

                If dt(vIdx)("Debe") > 0 Then
                    objCuentas.DebeHaber = "D"
                    objCuentas.Importe = dt(vIdx)("Debe")
                Else
                    objCuentas.DebeHaber = "H"
                    objCuentas.Importe = dt(vIdx)("Haber")
                End If

                objCuentas.flgPurge = 0
                objCuentas.Salvar(objCuentas)
                objCuentas = Nothing

            Next vIdx

            SQL = "DELETE FROM AsientosCuentas WHERE AsientoId = " & pId & " AND flgPurge = 1"
            cmdUpd.CommandText = SQL
            cmdUpd.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetCuentas", ex)
        End Try
    End Sub

    Public Sub GetCuentas(ByVal pId As Int64, ByRef dt As DataTable)
        Try

            dt.Rows.Clear()

            Dim SQL As String

            SQL = "SELECT ren.ID, ren.NroRenglon, ren.CuentaAsientoId, cta.AbreviaturaId, cta.Descripcion, "
            SQL = SQL & "CASE ren.DebeHaber WHEN 'D' THEN ren.Importe ELSE 0 END AS Debe, "
            SQL = SQL & "CASE ren.DebeHaber WHEN 'H' THEN ren.Importe ELSE 0 END AS Haber "
            SQL = SQL & "FROM AsientosCuentas ren "
            SQL = SQL & "INNER JOIN CuentasAsientos cta ON (ren.CuentaAsientoId = cta.ID) "
            SQL = SQL & "WHERE (ren.AsientoId = " & pId & ") "
            SQL = SQL & "ORDER BY ren.NroRenglon"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            dt.Load(cmdBas.ExecuteReader)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetCuentas", ex)
        End Try
    End Sub

    Public Function GetIdByVentaDocumentoId(ByVal pDocId As Int64) As Int64
        GetIdByVentaDocumentoId = 0
        Try

            Dim SQL As String

            SQL = "SELECT ID FROM Asientos WHERE VentaDocumentoId = " & pDocId

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIdByVentaDocumentoId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIdByVentaDocumentoId", ex)
        End Try
    End Function

End Class
