Imports System.Data
Imports System.Data.SqlClient

Public Class conEncuestasRespuestas
    Inherits typEncuestasRespuestas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIdByIndex(ByVal pEnc As Int64, pNro As Integer) As Int64
        GetIdByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM EncuestasRespuestas WHERE (EncuestaId = " & pEnc & ") AND (NroRespuesta = " & pNro & ") "

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

            SQL = "SELECT ID, NroRespuesta, Descripcion, Valor "
            SQL = SQL & "FROM EncuestasRespuestas "
            SQL = SQL & "WHERE (EncuestaId = " & pEnc & ") "
            SQL = SQL & "ORDER BY NroRespuesta"

            Dim cmdIto As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdIto.ExecuteReader)

            GetByEncuesta = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByEncuesta", ex)
        End Try
    End Function

    Public Sub SetDTByEncuesta(ByVal pEnc As Int64, ByRef dtRta As DataTable, Optional pSavOrd As Boolean = False)

        Try

            Dim dt As DataTable = Me.GetByEncuesta(pEnc)

            dtRta.Rows.Clear()

            For vIdx = 0 To dt.Rows.Count - 1

                If pSavOrd Then

                    Dim objRespuesta As New conEncuestasRespuestas(Me.myCnnName)

                    If objRespuesta.Abrir(dt(vIdx)(0)) Then

                        objRespuesta.NroRespuesta = vIdx + 1

                        objRespuesta.Salvar(objRespuesta, , False)

                    End If

                End If

                Dim dtRow As DataRow = dtRta.NewRow

                dtRow(0) = dt(vIdx)(0)
                dtRow(1) = vIdx + 1
                dtRow(2) = dt(vIdx)(2)
                dtRow(3) = dt(vIdx)(3)

                dtRta.Rows.Add(dtRow)

            Next

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDTByEncuesta", ex)
        End Try
    End Sub

End Class

