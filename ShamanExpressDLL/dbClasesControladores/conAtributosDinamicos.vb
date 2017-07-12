Imports System.Data
Imports System.Data.SqlClient
Public Class conAtributosDinamicos
    Inherits typAtributosDinamicos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pTbl As Integer, pAbr As String) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM AtributosDinamicos WHERE TablaDestinoId = " & pTbl & " AND AbreviaturaId = '" & pAbr & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetByTabla(pTbl As dynAtributos) As DataTable

        GetByTabla = Nothing

        Try
            Dim SQL As String
            SQL = "SELECT ID, AbreviaturaId, Descripcion, NroOrden FROM AtributosDinamicos "
            SQL = SQL & "WHERE (TablaDestinoId = " & pTbl & ") "
            SQL = SQL & " ORDER BY AbreviaturaId"

            Dim cmdBus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBus.ExecuteReader)

            GetByTabla = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByTabla", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.TablaDestinoId < 0 Then vRdo = "Debe especificar donde se aplicará el atributo"
            If Me.AbreviaturaId = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del atributo"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del atributo"
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

End Class
