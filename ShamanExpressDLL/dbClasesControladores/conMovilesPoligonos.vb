Imports System.Data
Imports System.Data.SqlClient
Public Class conMovilesPoligonos
    Inherits typMovilesPoligonos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByMovil(ByVal pMovId As Int64) As DataTable

        GetByMovil = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT 0 AS ID, ID AS PoligonoId, AbreviaturaId, Descripcion, '0' AS flgEventual "
            SQL = SQL & "FROM Poligonos WHERE ClasificacionPoligono = 0 ORDER BY AbreviaturaId"

            Dim cmdMet As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdMet.ExecuteReader)

            Dim colSel As DataColumn = dt.Columns.Add("flgSeleccion", GetType(Boolean))
            colSel.DefaultValue = False
            colSel.ReadOnly = False
            dt.Columns("flgEventual").ReadOnly = False

            dt.Columns("ID").ReadOnly = False

            For vIdx = 0 To dt.Rows.Count - 1

                If Me.Abrir(Me.GetIDByIndex(pMovId, dt(vIdx)("PoligonoId"))) Then

                    dt(vIdx)("ID") = Me.ID
                    dt(vIdx)("flgSeleccion") = True
                    dt(vIdx)("flgEventual") = Me.flgEventual

                Else

                    dt(vIdx)("flgSeleccion") = False

                End If

            Next

            GetByMovil = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByMovil", ex)
        End Try
    End Function

    Public Function GetIDByIndex(ByVal pMovId As Int64, ByVal pPol As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM MovilesPoligonos WHERE MovilId = " & pMovId & " AND PoligonoId = " & pPol

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Sub SetPoligonos(pMovId As Int64, dt As DataTable)
        Try
            Dim vIdx As Integer

            For vIdx = 0 To dt.Rows.Count - 1

                Dim objPoligono As New conMovilesPoligonos(Me.myCnnName)

                If dt(vIdx)("flgSeleccion") Then

                    If Not objPoligono.Abrir(dt(vIdx)("ID")) Then

                        objPoligono.MovilId.SetObjectId(pMovId)
                        objPoligono.PoligonoId.SetObjectId(dt(vIdx)("PoligonoId"))

                    End If

                    objPoligono.flgEventual = dt(vIdx)("flgEventual")

                    objPoligono.Salvar(objPoligono)

                Else

                    If Not objPoligono.Eliminar(dt(vIdx)("ID")) Then

                    End If

                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPoligonos", ex)
        End Try
    End Sub


End Class
