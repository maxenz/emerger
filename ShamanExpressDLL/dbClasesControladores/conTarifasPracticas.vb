Imports System.Data
Imports System.Data.SqlClient
Public Class conTarifasPracticas
    Inherits typTarifasPracticas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll(Optional ByVal pLiq As Boolean = False, Optional ByVal pAct As Integer = 1) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion FROM TarifasPracticas "
            SQL = SQL & "WHERE (flgLiquidacion = " & setBoolToInt(pLiq) & ") "
            If pAct = 1 Then SQL = SQL & "AND (Activo = 1) "
            If pAct = 2 Then SQL = SQL & "AND (Activo = 0) "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

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
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código de la tarifa"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción de la tarifa"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Function Valorizar(ByVal pTar As Long, ByVal pPra As Long, pFec As Date) As Double
        Valorizar = 0
        Try
            '-----> Obtengo el valor corriente
            Dim objPracticas As New conPracticas

            Dim vValPra As Decimal = objPracticas.GetValorVenta(pPra, pFec)

            Dim SQL As String

            SQL = "SELECT ID FROM TarifasPracticasDetalle WHERE (TarifaPracticaId = " & pTar & ") AND (PracticaId = " & pPra & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            If Not vOutVal Is Nothing Then

                Dim objValor As New conTarifasPracticasDetalle(Me.myCnnName)

                If objValor.Abrir(vOutVal) Then

                    Select Case objValor.TipoAplicacion
                        Case 0 : vValPra = 0
                        Case 1 : vValPra = vValPra + Math.Round(((vValPra * objValor.Valor) / 100), 2)
                        Case 2 : vValPra = vValPra - Math.Round(((vValPra * objValor.Valor) / 100), 2)
                        Case 3 : vValPra = objValor.Valor
                    End Select

                End If

                objValor = Nothing

            End If

            Valorizar = vValPra

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Valorizar", ex)
        End Try
    End Function

End Class
