Imports System.Data
Imports System.Data.SqlClient
Public Class conRubrosClientes
    Inherits conAllGenerico01
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Overrides Function CanDelete(ByVal pId As String, Optional ByVal pMsg As Boolean = True) As Boolean
        CanDelete = False

        Try

            Dim SQL As String = "SELECT TOP 1 AbreviaturaId FROM Clientes WHERE RubroClienteId = " & pId
            Dim cmAli As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vVal As String = cmAli.ExecuteScalar

            If Not vVal Is Nothing Then
                If pMsg Then MsgBox("Imposible Eliminar" & vbCrLf & "El rubro en cuestión está vinculado al cliente " & vVal, MsgBoxStyle.Critical, "Shaman")
            Else
                CanDelete = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "CanDelete", ex)
        End Try
    End Function

End Class
