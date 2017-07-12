Imports System.Data
Imports System.Data.SqlClient
Public Class conCentrosAtencionContactos
    Inherits typCentrosAtencionContactos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByCentro(ByVal pCen As Int64) As DataTable

        GetByCentro = Nothing
        Try
            Dim SQL As String

            SQL = "SELECT ID, Nombre, Email, Telefono FROM CentrosAtencionContactos "
            SQL = SQL & "WHERE (CentroAtencionId = " & pCen & ") ORDER BY Nombre"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetByCentro = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByCentro", ex)
        End Try
    End Function
End Class
