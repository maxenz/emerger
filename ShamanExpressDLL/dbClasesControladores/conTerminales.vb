Imports System.Data
Imports System.Data.SqlClient
Public Class conTerminales
    Inherits typTerminales
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetMyTerminalId() As Int64

        GetMyTerminalId = 0

        Try
            Dim SQL As String
            Dim objTerminal As New conTerminales(Me.myCnnName)

            SQL = "SELECT ID FROM Terminales WHERE NombrePC = '" & My.Computer.Name & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)

            If Not vOutVal Is Nothing Then

                GetMyTerminalId = CType(vOutVal, Int64)

            Else

                objTerminal.CleanProperties(objTerminal)

                objTerminal.NombrePC = My.Computer.Name

                If objTerminal.Salvar(objTerminal) Then
                    GetMyTerminalId = objTerminal.ID
                End If

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMyTerminalId", ex)
        End Try
    End Function
End Class
