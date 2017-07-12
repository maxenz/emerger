Imports System.Data
Imports System.Data.SqlClient
Public Class conTiposComprobantes
    Inherits typTiposComprobantes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByAbreviaturaId(ByVal pVal As String) As Int64

        GetIDByAbreviaturaId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM TiposComprobantes WHERE (AbreviaturaId = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAbreviaturaId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbreviaturaId", ex)
        End Try

    End Function
    Public Overrides Function CanDelete(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean
        CanDelete = False

        Try
            Dim objTipoComprobante As New typTiposComprobantes

            If objTipoComprobante.Abrir(pId) Then
                If objTipoComprobante.AbreviaturaId = "FAC" Or objTipoComprobante.AbreviaturaId = "REC" Or objTipoComprobante.AbreviaturaId = "DEB" Or objTipoComprobante.AbreviaturaId = "CRE" Then
                    If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "Es un registro reservado del sistema", MsgBoxStyle.Critical, "Shaman")
                Else
                    CanDelete = True
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CanDelete", ex)
        End Try
    End Function
    Public Function GetAll(Optional ByVal pClf As cmpClasificacion = cmpClasificacion.cmpFacturacion) As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion "
            SQL = SQL & "FROM TiposComprobantes "
            SQL = SQL & "WHERE (ClasificacionId = " & pClf & ") "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdTip As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdTip.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Overridable Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código del tipo de comprobante"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del tipo de comprobante"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function


End Class
