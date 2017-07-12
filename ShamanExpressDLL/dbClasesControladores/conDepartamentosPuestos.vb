Imports System.Data
Imports System.Data.SqlClient
Public Class conDepartamentosPuestos
    Inherits typDepartamentosPuestos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByDepartamento(ByVal pDep As Int64) As DataTable

        GetByDepartamento = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, Descripcion AS Puesto, PuestoGrilla AS Grilla FROM DepartamentosPuestos "
            SQL = SQL & "WHERE (DepartamentoId = " & pDep & ") "
            SQL = SQL & " ORDER BY Descripcion"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByDepartamento = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByDepartamento", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del puesto"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class

