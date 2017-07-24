Imports System.Data
Imports System.Data.SqlClient
Public Class conConceptosFacturacion
    Inherits typConceptosFacturacion
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function IsTraslado(ByVal pId As Int64) As Boolean
        IsTraslado = False
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM GradosOperativos WHERE ((ConceptoFacturacion1Id = " & pId & ") OR (ConceptoFacturacion2Id = " & pId & ")) AND (ClasificacionId = 1) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmFind.ExecuteScalar > 0 Then IsTraslado = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "IsTraslado", ex)
        End Try
    End Function

    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion, "
            SQL = SQL & "CASE ISNULL(Clasificacion, 0) "
            SQL = SQL & "WHEN 0 THEN 'SERVICIOS' "
            SQL = SQL & "WHEN 1 THEN 'PRESTACIONES ADICIONALES' "
            SQL = SQL & "WHEN 2 THEN 'PLANES Y ABONOS' "
            SQL = SQL & "WHEN 3 THEN 'AJUSTES' ELSE 'OTROS' END AS Clasificacion  "
            SQL = SQL & "FROM ConceptosFacturacion "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdCon As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdCon.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "RefreshGrid", ex)
        End Try

    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código del concepto de facturación"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del concepto de facturación"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try

    End Function

    Public Function GetSubConceptoId(ByVal pSub As String) As Int64
        GetSubConceptoId = 0

        Try
            Dim SQL As String
            SQL = "SELECT ID FROM ConceptosFacturacion WHERE (Clasificacion = 5) AND (AbreviaturaId = '" & pSub & "') "
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then
                GetSubConceptoId = CType(vOutVal, Int64)
            Else
                Dim objConcepto As New typConceptosFacturacion
                objConcepto.CleanProperties(objConcepto)

                objConcepto.AbreviaturaId = pSub
                Select Case pSub
                    Case "BAS" : objConcepto.Descripcion = "VALOR BASE"
                    Case "KEX" : objConcepto.Descripcion = "KILOMETRO EXCEDENTE"
                    Case "ESH" : objConcepto.Descripcion = "HORA DE ESPERA"
                    Case "ESM" : objConcepto.Descripcion = "MEDIA HORA DE ESPERA"
                    Case "NOC" : objConcepto.Descripcion = "RECARGO NOCTURNO"
                    Case "PED" : objConcepto.Descripcion = "RECARGO PEDIATRICO"
                    Case "DER" : objConcepto.Descripcion = "RECARGO DERIVACION"
                    Case "DRT" : objConcepto.Descripcion = "DESCUENTO RETORNO"
                End Select
                objConcepto.Clasificacion = conClasificaciones.SubConceptos

                If objConcepto.Salvar(objConcepto) Then
                    GetSubConceptoId = objConcepto.ID
                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetSubConceptoId", ex)
        End Try
    End Function

End Class
