Imports System.Data
Imports System.Data.SqlClient
Public Class conClientesTarifasPracticas
    Inherits typClientesTarifasPracticas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pCli As Int64, ByVal pTar As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM ClientesTarifasPracticas WHERE ClienteId = " & pCli & " AND TarifaPracticaId = " & pTar

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function GetTarifaPrincipal(ByVal pCli As Int64) As Int64
        GetTarifaPrincipal = 0
        Try
            If pCli > 0 Then
                Dim SQL As String
                SQL = "SELECT TarifaPracticaId FROM ClientesTarifasPracticas WHERE (ClienteId = " & pCli & ") "

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then GetTarifaPrincipal = CType(vOutVal, Int64)

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetTarifaPrincipal", ex)
        End Try
    End Function
    Public Function SetTarifaPrincipal(ByVal pCli As Int64, ByVal pTar As Int64) As Boolean
        SetTarifaPrincipal = False
        Try
            Dim objTarifaPrincipal As New conClientesTarifasPracticas(Me.myCnnName)
            If pTar > 0 Then
                If Not objTarifaPrincipal.Abrir(objTarifaPrincipal.GetIDByIndex(pCli, objTarifaPrincipal.GetTarifaPrincipal(pCli))) Then
                    objTarifaPrincipal.ClienteId.SetObjectId(pCli)
                End If
                objTarifaPrincipal.TarifaPracticaId.SetObjectId(pTar)
                SetTarifaPrincipal = objTarifaPrincipal.Salvar(objTarifaPrincipal)
            Else
                If objTarifaPrincipal.GetIDByIndex(pCli, objTarifaPrincipal.GetTarifaPrincipal(pCli)) > 0 Then
                    If objTarifaPrincipal.Eliminar(objTarifaPrincipal.GetIDByIndex(pCli, objTarifaPrincipal.GetTarifaPrincipal(pCli))) Then
                        SetTarifaPrincipal = True
                    End If
                Else
                    SetTarifaPrincipal = True
                End If
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetTarifaPrincipal", ex)
        End Try
    End Function

End Class
