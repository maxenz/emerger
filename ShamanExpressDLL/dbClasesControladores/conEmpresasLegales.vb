Imports System.Data
Imports System.Data.SqlClient
Public Class conEmpresasLegales
    Inherits typEmpresasLegales
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion, CUIT "
            SQL = SQL & "FROM EmpresasLegales "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdEmp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEmp.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador de la empresa legal"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar el nombre de la empresa legal"
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
            SQL = "SELECT ID FROM EmpresasLegales WHERE flgDefault = 1"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetDefault = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDefault", ex)
        End Try

    End Function
    Public Function SetDefault(ByVal pEmp As Int64) As Boolean
        SetDefault = False
        Try
            Dim SQL As String = "UPDATE EmpresasLegales SET flgDefault = 0 WHERE flgDefault = 1"
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            Dim objEmpresasLegales As New conEmpresasLegales(Me.myCnnName)
            If objEmpresasLegales.Abrir(pEmp) Then
                objEmpresasLegales.flgDefault = 1
                SetDefault = objEmpresasLegales.Salvar(objEmpresasLegales)
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDefault", ex)
        End Try

    End Function

End Class

