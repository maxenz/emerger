Imports System.Data
Imports System.Data.SqlClient
Public Class conFormasPago
    Inherits typFormasPago
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion FROM FormasPago "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function
    Public Function GetDefaultCobranza() As Int64
        GetDefaultCobranza = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM FormasPago WHERE flgDefaultCobranza = 1"
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetDefaultCobranza = CType(vOutVal, Int64)
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDefaultCobranza", ex)
        End Try

    End Function
    Public Function SetDefaultCobranza(ByVal pFpg As Int64) As Boolean
        SetDefaultCobranza = False
        Try
            Dim SQL As String = "UPDATE FormasPago SET flgDefaultCobranza = 0 WHERE flgDefaultCobranza = 1"
            Dim cmUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmUpd.ExecuteNonQuery()

            Dim objFormasPago As New conFormasPago(Me.myCnnName)
            If objFormasPago.Abrir(pFpg) Then
                objFormasPago.flgDefaultCobranza = 1
                SetDefaultCobranza = objFormasPago.Salvar(objFormasPago)
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDefaultCobranza", ex)
        End Try

    End Function

End Class
