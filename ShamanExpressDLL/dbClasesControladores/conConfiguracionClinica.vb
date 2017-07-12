Imports System.Data
Imports System.Data.SqlClient
Public Class conConfiguracionClinica
    Inherits typConfiguracionClinica
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function UpConfig() As Boolean
        UpConfig = False
        Try

            Dim SQL As String
            SQL = "SELECT ID FROM ConfiguracionClinica"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then
                UpConfig = Me.Abrir(CType(vOutVal, Int64))
            Else
                Me.CleanProperties(Me)
                Me.TpoTurno = 15
                If Me.Salvar(Me) Then
                    UpConfig = Me.Abrir(Me.ID)
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "UpConfig", ex)
        End Try
    End Function

End Class
