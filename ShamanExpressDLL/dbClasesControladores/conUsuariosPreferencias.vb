Imports System.Data
Imports System.Data.SqlClient

Public Class conUsuariosPreferencias
    Inherits typUsuariosPreferencias
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function DeleteAllPreferencias(ByVal pIdUserPref As Int64, ByVal pNombreForm As String) As Int16

        Try

            Dim SQL As String

            SQL = "DELETE "
            SQL = SQL & " FormulariosControles "
            SQL = SQL & " WHERE UsuariosPreferenciasID = " + pIdUserPref.ToString
            SQL = SQL & " AND NombreFormulario = '" + pNombreForm + "'"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim rowsAffected = cmdBas.ExecuteNonQuery

            Return rowsAffected

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
            Return Nothing
        End Try
    End Function

    Public Function GetAllPreferencias(ByVal pIdUser As Int64, ByVal pNombreForm As String) As DataTable

        Try

            Dim SQL As String

            SQL = "SELECT FC.NombreControl, FC.Valor, FC.XML "
            SQL = SQL & " FROM UsuariosPreferencias UP "
            SQL = SQL & " INNER JOIN FormulariosControles FC ON UP.ID = FC.UsuariosPreferenciasID "
            SQL = SQL & " WHERE FC.UsuariosPreferenciasID = " + pIdUser.ToString
            SQL = SQL & " AND FC.NombreFormulario = '" + pNombreForm + "'"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            Return dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
            Return Nothing
        End Try
    End Function

    Public Function GetUsuarioPreferencia(ByVal pIdUser As Int64) As DataTable
        Try
            Dim SQL As String

            SQL = "SELECT Id, UsuarioId, HojaEstilo FROM  "
            SQL = SQL & " UsuariosPreferencias UP "
            SQL = SQL & " WHERE UP.ID = " & pIdUser


            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            Return dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
            Return Nothing
        End Try

    End Function

End Class
