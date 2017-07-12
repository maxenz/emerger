Imports System.Data
Imports System.Data.SqlClient
Public Class conPrestadoresContactos
    Inherits typPrestadoresContactos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByPrestador(ByVal pPre As Int64) As DataTable

        GetByPrestador = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, Nombre, Email, Telefono FROM PrestadoresContactos "
            SQL = SQL & "WHERE (PrestadorId = " & pPre & ") ORDER BY Nombre"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetByPrestador = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByPrestador", ex)
        End Try
    End Function
End Class
