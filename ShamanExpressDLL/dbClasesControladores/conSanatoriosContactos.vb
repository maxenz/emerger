Imports System.Data
Imports System.Data.SqlClient
Public Class conSanatoriosContactos
    Inherits typSanatoriosContactos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetBySanatorio(ByVal pSan As Int64) As DataTable

        GetBySanatorio = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, Nombre, Email, Telefono FROM SanatoriosContactos "
            SQL = SQL & "WHERE (SanatorioId = " & pSan & ") ORDER BY Nombre"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetBySanatorio = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetBySanatorio", ex)
        End Try
    End Function
End Class
