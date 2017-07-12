Imports System.Data
Imports System.Data.SqlClient

Public Class conEncuestasItems
    Inherits typEncuestasItems
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIdByIndex(ByVal pEnc As Int64, pNro As Integer) As Int64
        GetIdByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM EncuestasItems WHERE (EncuestaId = " & pEnc & ") AND (NroItem = " & pNro & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIdByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIdByIndex", ex)
        End Try

    End Function

    Public Function GetByEncuesta(ByVal pEnc As Int64) As DataTable

        GetByEncuesta = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, NroItem, Descripcion "
            SQL = SQL & "FROM EncuestasItems "
            SQL = SQL & "WHERE (EncuestaId = " & pEnc & ") "
            SQL = SQL & "ORDER BY NroItem"

            Dim cmdIto As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdIto.ExecuteReader)

            GetByEncuesta = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByEncuesta", ex)
        End Try
    End Function

    Public Sub SetDTByEncuesta(ByVal pEnc As Int64, ByRef dtItm As DataTable, Optional pSavOrd As Boolean = False)

        Try

            Dim dt As DataTable = Me.GetByEncuesta(pEnc)

            dtItm.Rows.Clear()

            For vIdx = 0 To dt.Rows.Count - 1

                If pSavOrd Then

                    Dim objItem As New conEncuestasItems(Me.myCnnName)

                    If objItem.Abrir(dt(vIdx)(0)) Then

                        objItem.NroItem = vIdx + 1

                        objItem.Salvar(objItem, , False)

                    End If

                End If

                Dim dtRow As DataRow = dtItm.NewRow

                dtRow(0) = dt(vIdx)(0)
                dtRow(1) = vIdx + 1
                dtRow(2) = dt(vIdx)(2)

                dtItm.Rows.Add(dtRow)

            Next

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDTByEncuesta", ex)
        End Try
    End Sub

End Class

