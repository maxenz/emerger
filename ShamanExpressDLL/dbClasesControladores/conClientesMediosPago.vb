Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesMediosPago
    Inherits typClientesMediosPago
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByClienteAndClasificacion(ByVal pCli As Long, ByVal pCla As Integer) As DataTable

        GetByClienteAndClasificacion = Nothing

        Try

            Dim SQL As String

            If pCla = 1 Then
                SQL = "SELECT med.ID, tar.Descripcion AS Proveedor, bco.Descripcion AS Banco, med.NroCuenta, med.Vencimiento "
                SQL = SQL & "FROM ClientesMediosPago med "
                SQL = SQL & "INNER JOIN TarjetasCredito tar ON (med.TarjetaCreditoId = tar.ID) "
                SQL = SQL & "LEFT JOIN Bancos bco ON (med.BancoId = bco.ID) "
            Else
                SQL = "SELECT med.ID, tip.Descripcion AS Proveedor, bco.Descripcion AS Banco, med.NroCuenta, med.CUIT AS Vencimiento "
                SQL = SQL & "FROM ClientesMediosPago med "
                SQL = SQL & "INNER JOIN TiposCuentas tip ON (med.TipoCuentaId = tip.ID) "
                SQL = SQL & "LEFT JOIN Bancos bco ON (med.BancoId = bco.ID) "
            End If
            SQL = SQL & "WHERE (ClienteId = " & pCli & ") ORDER BY med.flgPredeterminada DESC"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByClienteAndClasificacion = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByClienteAndClasificacion", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.ClasificacionId = 0 Then
                vRdo = "Debe establecer la clasificación de la cuenta o tarjeta"
            ElseIf Me.ClasificacionId = 1 Then
                If Me.TarjetaCreditoId.GetObjectId = 0 Then vRdo = "Debe establecer la tarjeta de crédito"
                If Me.NroCuenta = "" And vRdo = "" Then vRdo = "Debe especificar el número de la tarjeta"
                If Me.NroSeguridad = "" And vRdo = "" Then vRdo = "Debe establecer el código de seguridad"
            Else
                If Me.TipoCuentaId.GetObjectId = 0 Then vRdo = "Debe establecer el tipo de cuenta"
                If Me.NroCuenta = "" And vRdo = "" Then vRdo = "Debe especificar el número de cuenta"
                If Me.flgPropia = 0 And vRdo = "" Then
                    If Me.Titular = "" Then vRdo = "Debe establecer el titular de la cuenta"
                    If Me.CUIT = "" And vRdo = "" Then vRdo = "Debe establecer el CUIT del titular de la cuenta"
                End If
            End If

            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try

    End Function

    Public Sub SetPredeterminada(ByVal pCli As Decimal, ByVal pCla As Integer, ByVal pPrd As Integer, ByVal pRid As Decimal)

        Try

            Dim SQL As String

            If pPrd = 1 Then

                SQL = "UPDATE ClientesMediosPago SET flgPredeterminada = 0 WHERE ClienteId = " & pCli & " AND ClasificacionId = " & pCla & " AND ID <> " & pRid
                Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                cmdUpd.ExecuteNonQuery()

                SQL = "UPDATE ClientesMediosPago SET flgPredeterminada = 1 WHERE ID = " & pRid
                cmdUpd.CommandText = SQL
                cmdUpd.ExecuteNonQuery()

            Else

                SQL = "SELECT TOP 1 ID FROM ClientesMediosPago WHERE ClienteId = " & pCli & " AND ClasificacionId = " & pCla
                SQL = SQL & " ORDER BY flgPredeterminada DESC"

                Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vMod As String = CType(cmdUpd.ExecuteScalar, String)

                If Not vMod Is Nothing Then

                    SQL = "UPDATE ClientesMediosPago SET flgPredeterminada = 1 WHERE ID = " & vMod
                    cmdUpd.CommandText = SQL
                    cmdUpd.ExecuteNonQuery()

                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPredeterminada", ex)
        End Try
    End Sub

End Class
