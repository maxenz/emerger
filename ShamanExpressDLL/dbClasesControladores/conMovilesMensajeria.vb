Imports System.Data
Imports System.Data.SqlClient
Public Class conMovilesMensajeria
    Inherits typMovilesMensajeria
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByMovil(ByVal pMovId As Int64) As DataTable

        GetByMovil = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT 0 AS ID, ID AS MetodoMensajeriaId, MetodoId, Descripcion, SPACE(20) AS rabbitAlias, SPACE(150) AS Email "
            SQL = SQL & "FROM MetodosMensajeria ORDER BY Descripcion"

            Dim cmdMet As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdMet.ExecuteReader)

            Dim colSel As DataColumn = dt.Columns.Add("flgSeleccion", GetType(Boolean))
            colSel.DefaultValue = False
            colSel.ReadOnly = False

            dt.Columns("ID").ReadOnly = False
            dt.Columns("rabbitAlias").ReadOnly = False
            dt.Columns("Email").ReadOnly = False

            For vIdx = 0 To dt.Rows.Count - 1

                If Me.Abrir(Me.GetIDByIndex(pMovId, dt(vIdx)("MetodoMensajeriaId"))) Then

                    dt(vIdx)("ID") = Me.ID
                    dt(vIdx)("flgSeleccion") = True
                    dt(vIdx)("rabbitAlias") = Me.rabbitAlias
                    dt(vIdx)("Email") = Me.Email

                Else

                    dt(vIdx)("ID") = 0
                    dt(vIdx)("flgSeleccion") = False
                    dt(vIdx)("rabbitAlias") = ""
                    dt(vIdx)("Email") = ""

                End If

            Next

            GetByMovil = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByMovil", ex)
        End Try
    End Function

    Public Function GetIDByIndex(ByVal pMovId As Int64, ByVal pMsgId As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM MovilesMensajeria WHERE MovilId = " & pMovId & " AND MetodoMensajeriaId = " & pMsgId

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetIDByAliasRabbit(ByVal pRabbitAlias As String) As Int64
        GetIDByAliasRabbit = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM MovilesMensajeria WHERE rabbitAlias = '" & pRabbitAlias & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAliasRabbit = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAliasRabbit", ex)
        End Try
    End Function

    Public Function GetRabbitTags() As DataTable

        GetRabbitTags = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT mov.rabbitAlias FROM MovilesMensajeria mov "
            SQL = SQL & "INNER JOIN MetodosMensajeria msg ON mov.MetodoMensajeriaId = msg.ID "
            SQL = SQL & "WHERE msg.MetodoId = " & msgMetodos.RabbitMQ

            Dim cmdMet As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdMet.ExecuteReader)

            GetRabbitTags = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAllRabbitMQ", ex)
        End Try
    End Function

End Class
