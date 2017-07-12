Imports System.Data
Imports System.Data.SqlClient
Public Class conPlanes
    Inherits typPlanes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByAbreviaturaId(ByVal pVal As String, Optional ByVal pCli As Int64 = 0) As Int64

        GetIDByAbreviaturaId = 0

        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Planes WHERE AbreviaturaId = '" & pVal & "' AND ClienteId = " & pCli
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAbreviaturaId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbreviaturaId", ex)
        End Try

    End Function
    Public Function GetIDByDescripcion(ByVal pVal As String, Optional ByVal pCli As Int64 = 0) As Int64

        GetIDByDescripcion = 0

        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Planes WHERE Descripcion = '" & pVal & "' AND ClienteId = " & pCli
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try

    End Function
    Public Function GetAll(Optional ByVal pCli As Int64 = 0) As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT pla.ID, pla.AbreviaturaId, pla.Descripcion, fam.Descripcion AS FamiliaPlan "
            SQL = SQL & "FROM Planes pla "
            SQL = SQL & "LEFT JOIN PlanesFamilias fam ON (pla.PlanFamiliaId = fam.ID) "
            SQL = SQL & "WHERE (pla.ClienteId = " & pCli & ") "
            SQL = SQL & "ORDER BY pla.AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

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
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador del plan"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar el nombre del plan"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class
