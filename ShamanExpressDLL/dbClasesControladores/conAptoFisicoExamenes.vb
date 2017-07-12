Imports System.Data
Imports System.Data.SqlClient
Public Class conAptoFisicoExamenes
    Inherits typAptoFisicoExamenes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pCli As Int64, ByVal pIte As Int64, pFec As Date) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM AptoFisicoExamenes WHERE ClienteId = " & pCli & " AND ClienteIntegranteId = " & pIte & " AND FecExamen = '" & DateTimeToSql(pFec) & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetExamenesFechas(ByVal pCli As Int64, ByVal pIte As Int64) As DataTable

        GetExamenesFechas = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, FecExamen, CONVERT(VARCHAR, FecExamen, 108) AS Hora "
            SQL = SQL & "FROM AptoFisicoExamenes WHERE ClienteId = " & pCli & " AND ClienteIntegranteId = " & pIte
            SQL = SQL & " ORDER BY FecExamen"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetExamenesFechas = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetExamenesFechas", ex)
        End Try
    End Function

    Public Function Validar(Optional pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            If Me.NroDocumento = 0 Then vRdo = "Debe establecer el número de documento"
            If Me.Apellido = "" And vRdo = "" Then vRdo = "Debe establecer el apellido del paciente"
            If Me.Nombre = "" And vRdo = "" Then vRdo = "Debe establecer el nombre del paciente"

            If vRdo = "" Then
                Validar = True
            Else
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, "Exámenes")
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
