Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesMailing
    Inherits typClientesMailing
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pCon As Int64, ByVal pAcc As accMailingClientes) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM ClientesMailing WHERE ClienteContactoId = " & pCon & " AND MailingAccionId = " & pAcc

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function GetClienteIdByMail(ByVal pMail As String) As Int64
        GetClienteIdByMail = 0
        Try
            Dim SQL As String

            SQL = "SELECT con.ClienteId FROM ClientesMailing mai "
            SQL = SQL & "INNER JOIN ClientesContactos con ON (mai.ClienteContactoId = con.ID) "
            SQL = SQL & "WHERE (con.Email = '" & pMail & "') AND (mai.MailingAccionId = 1) "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetClienteIdByMail = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetClienteIdByMail", ex)
        End Try
    End Function

    Public Function GetMailingString(ByVal pCli As Int64, ByVal pAcc As accMailingClientes) As String

        GetMailingString = ""

        Try

            Dim SQL As String

            SQL = "SELECT con.Email FROM ClientesContactos con "
            SQL = SQL & "INNER JOIN ClientesMailing mai ON (con.ID = mai.ClienteContactoId) "
            SQL = SQL & "WHERE (con.ClienteId = '" & pCli & "') AND (mai.MailingAccionId = " & pAcc & ") "

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            Dim vArrStr As String = ""

            dt.Load(cmdGrl.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                If vArrStr = "" Then
                    vArrStr = dt(vIdx)(0)
                Else
                    vArrStr = vArrStr & ";" & dt(vIdx)(0)
                End If

            Next

            GetMailingString = vArrStr


        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMailingString", ex)
        End Try
    End Function

End Class
