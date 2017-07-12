Imports System.Data
Imports System.Data.SqlClient
Public Class conPrestadoresTarifas
    Inherits typPrestadoresTarifas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pPre As Int64, ByVal pTar As Int64) As Long
        GetIDByIndex = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM PrestadoresTarifas WHERE PrestadorId = " & pPre & " AND TarifaId = " & pTar

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function GetTarifaPrincipal(ByVal pPre As Int64) As Int64
        GetTarifaPrincipal = 0
        Try
            If pPre > 0 Then
                Dim SQL As String
                SQL = "SELECT TarifaId FROM PrestadoresTarifas WHERE (PrestadorId = " & pPre & ") "

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
                If Not vOutVal Is Nothing Then GetTarifaPrincipal = CType(vOutVal, Int64)

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetTarifaPrincipal", ex)
        End Try
    End Function
    Public Function SetTarifaPrincipal(ByVal pPre As Int64, ByVal pTar As Int64) As Boolean
        SetTarifaPrincipal = False
        Try
            Dim objTarifaPrincipal As New conPrestadoresTarifas(Me.myCnnName)
            If pTar > 0 Then
                If Not objTarifaPrincipal.Abrir(objTarifaPrincipal.GetIDByIndex(pPre, objTarifaPrincipal.GetTarifaPrincipal(pPre))) Then
                    objTarifaPrincipal.PrestadorId.SetObjectId(pPre)
                End If
                objTarifaPrincipal.TarifaId.SetObjectId(pTar)
                SetTarifaPrincipal = objTarifaPrincipal.Salvar(objTarifaPrincipal)
            Else
                If objTarifaPrincipal.GetIDByIndex(pPre, objTarifaPrincipal.GetTarifaPrincipal(pPre)) > 0 Then
                    If objTarifaPrincipal.Eliminar(objTarifaPrincipal.GetIDByIndex(pPre, objTarifaPrincipal.GetTarifaPrincipal(pPre))) Then
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
