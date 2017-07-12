Imports System.Data
Imports System.Data.SqlClient

Public Class conVehiculosDocumentacion
    Inherits typVehiculosDocumentacion
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pVeh As Int64, ByVal pDoc As Int64) As Long
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM VehiculosDocumentacion WHERE VehiculoId = " & pVeh & " AND DocumentacionVehiculoId = " & pDoc

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Long)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Sub SetDocumentacion(pVeh As Int64, ByVal pDt As DataTable)

        Try

            Dim vIdx As Integer

            For vIdx = 0 To pDt.Rows.Count - 1

                If pDt.Rows(vIdx)(3) Then

                    Dim objDocumentacion As New typVehiculosDocumentacion(Me.myCnnName)

                    If Not objDocumentacion.Abrir(Me.GetIDByIndex(pVeh, pDt.Rows(vIdx)(0))) Then
                        objDocumentacion.VehiculoId.SetObjectId(pVeh)
                        objDocumentacion.DocumentacionVehiculoId.SetObjectId(pDt.Rows(vIdx)(0))
                    End If
                    If Not IsDBNull(pDt.Rows(vIdx)(2)) Then
                        objDocumentacion.FecVencimiento = pDt.Rows(vIdx)(2)
                    End If

                    objDocumentacion.Salvar(objDocumentacion)

                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDocumentacion", ex)
        End Try

    End Sub

End Class

