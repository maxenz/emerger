Imports System.Data
Imports System.Data.SqlClient

Public Class conUsuariosPreferencias
    Inherits typUsuariosPreferencias
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAllPreferencias(ByVal pIdUser As Int64, ByVal pNombreForm As String) As DataTable

        Try

            Dim SQL As String

            SQL = "SELECT FC.NombreControl, FC.Valor, FC.XML "
            SQL = SQL & " FROM UsuariosPreferencias UP "
            SQL = SQL & " INNER JOIN FormulariosControles FC ON UP.ID = FC.UsuariosPreferenciasID "
            SQL = SQL & " WHERE UP.UsuarioId = " + pIdUser.ToString
            SQL = SQL & " AND FC.NombreFormulario = " + pNombreForm

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            Return dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
            Return Nothing
        End Try
    End Function

    Public Function GetHojaEstilo(ByVal pPID As Int64) As String
        Try
            Dim SQL As String
            SQL = "SELECT HE.Nombre FROM  = "
            SQL = SQL & " UsuariosPreferencias UP "
            SQL = SQL & " INNER JOIN HojaEstilo HE ON UP.HojaEstiloId = HE.ID" & pPID


            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            Return vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
            Return String.Empty
        End Try

    End Function

End Class
