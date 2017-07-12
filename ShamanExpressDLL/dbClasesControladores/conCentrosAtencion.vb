Imports System.Data
Imports System.Data.SqlClient
Public Class conCentrosAtencion
    Inherits typCentrosAtencion
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetQueryBase() As DataTable

        GetQueryBase = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT cen.ID, cen.AbreviaturaId, cen.Descripcion, cen.Domicilio, loc.AbreviaturaId AS Localidad "
            SQL = SQL & "FROM CentrosAtencion cen "
            SQL = SQL & "LEFT JOIN Localidades loc ON (cen.LocalidadId = loc.ID) "
            SQL = SQL & " ORDER BY cen.AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetQueryBase = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryBase", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador del centro de atención"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar el nombre del centro de atención"
            If Me.LocalidadId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe determinar la localidad del centro de atención"
            If Me.Domicilio.dmCalle = "" And vRdo = "" Then vRdo = "Debe determinar el domicilio del  centro de atención"
            If vRdo <> "" Then
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            Else
                Validar = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
    Public Function GetDefault() As Int64
        GetDefault = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM CentrosAtencion WHERE flgDefault = 1"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetDefault = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDefault", ex)
        End Try

    End Function
    Public Function SetDefault(ByVal pCen As Int64) As Boolean
        SetDefault = False
        Try
            Dim SQL As String = "UPDATE CentrosAtencion SET flgDefault = 0 WHERE flgDefault = 1"
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            Dim objCentrosAtencion As New conCentrosAtencion(Me.myCnnName)
            If objCentrosAtencion.Abrir(pCen) Then
                objCentrosAtencion.flgDefault = 1
                SetDefault = objCentrosAtencion.Salvar(objCentrosAtencion)
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDefault", ex)
        End Try

    End Function

End Class

