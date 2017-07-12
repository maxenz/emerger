Imports System.Data
Imports System.Data.SqlClient
Public Class conTurnosNomenclador
    Inherits typTurnosNomenclador
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIDByIndex(ByVal pTur As Int64, pNml As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM TurnosNomenclador WHERE TurnoId = " & pTur & " AND NomencladorId = " & pNml

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Sub GetNomenclador(ByVal pTur As Int64, ByVal pPra As Int64, ByRef dt As DataTable)
        Try
            Dim SQL As String

            SQL = "SELECT pra.NomencladorId, nom.AbreviaturaId, nom.Descripcion "
            SQL = SQL & "FROM PracticasNomenclador pra "
            SQL = SQL & "INNER JOIN Nomenclador nom ON (pra.NomencladorId = nom.ID) "
            SQL = SQL & "WHERE (pra.PracticaId = " & pPra & ") "
            SQL = SQL & "ORDER BY nom.AbreviaturaId"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dtPracticas As New DataTable
            Dim vIdx As Integer
            dtPracticas.Load(cmdGrl.ExecuteReader)

            dt.Rows.Clear()

            For vIdx = 0 To dtPracticas.Rows.Count - 1

                Dim objTurnoNomenclador As New conTurnosNomenclador

                Dim dtRow As DataRow = dt.NewRow

                If objTurnoNomenclador.Abrir(objTurnoNomenclador.GetIDByIndex(pTur, dtPracticas(vIdx)("NomencladorId"))) Then

                    dtRow("ID") = objTurnoNomenclador.ID
                    dtRow("flgSeleccion") = True
                    dtRow("Cantidad") = objTurnoNomenclador.Cantidad

                Else

                    dtRow("ID") = 0
                    dtRow("flgSeleccion") = False
                    dtRow("Cantidad") = 0

                End If

                dtRow("NomencladorId") = dtPracticas(vIdx)("NomencladorId")
                dtRow("AbreviaturaId") = dtPracticas(vIdx)("AbreviaturaId")
                dtRow("Descripcion") = dtPracticas(vIdx)("Descripcion")

                dt.Rows.Add(dtRow)

            Next

            '-----> Verifico agregados especiales

            SQL = "SELECT tur.ID, tur.NomencladorId, nom.AbreviaturaId, nom.Descripcion, tur.Cantidad "
            SQL = SQL & "FROM TurnosNomenclador tur "
            SQL = SQL & "INNER JOIN Nomenclador nom ON (tur.NomencladorId = nom.ID) "
            SQL = SQL & "WHERE (tur.TurnoId = " & pTur & ") "
            SQL = SQL & "AND (tur.NomencladorId NOT IN("
            SQL = SQL & "SELECT NomencladorId FROM PracticasNomenclador "
            SQL = SQL & "WHERE (PracticaId = " & pPra & "))) "

            cmdGrl.CommandText = SQL
            Dim dtPendientes As New DataTable
            dtPendientes.Load(cmdGrl.ExecuteReader)

            For vIdx = 0 To dtPendientes.Rows.Count - 1

                Dim dtRow As DataRow = dt.NewRow

                dtRow("ID") = dtPendientes(vIdx)("ID")
                dtRow("NomencladorId") = dtPendientes(vIdx)("NomencladorId")
                dtRow("flgSeleccion") = True
                dtRow("AbreviaturaId") = dtPendientes(vIdx)("AbreviaturaId")
                dtRow("Descripcion") = dtPendientes(vIdx)("Descripcion")
                dtRow("Cantidad") = dtPendientes(vIdx)("Cantidad")

                dt.Rows.Add(dtRow)

            Next

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetNomenclador", ex)
        End Try

    End Sub

    Public Sub SetNomenclador(ByVal pTur As Int64, ByVal dt As DataTable)
        Try
            Dim SQL As String
            Dim vIdx As Integer

            SQL = "UPDATE TurnosNomenclador SET flgPurge = 1 WHERE TurnoId = " & pTur
            Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdUpd.ExecuteNonQuery()

            For vIdx = 0 To dt.Rows.Count - 1

                If dt(vIdx)("flgSeleccion") Then

                    Dim objTurnoNomenclador As New conTurnosNomenclador

                    If Not objTurnoNomenclador.Abrir(dt(vIdx)("ID")) Then
                        objTurnoNomenclador.TurnoId.SetObjectId(pTur)
                        objTurnoNomenclador.NomencladorId.SetObjectId(dt(vIdx)("NomencladorId"))
                    End If

                    objTurnoNomenclador.flgPurge = 0
                    objTurnoNomenclador.Cantidad = dt(vIdx)("Cantidad")

                    If objTurnoNomenclador.Salvar(objTurnoNomenclador) Then

                    End If

                    objTurnoNomenclador = Nothing

                End If

            Next

            SQL = "DELETE FROM TurnosNomenclador WHERE TurnoId = " & pTur & " AND flgPurge = 1"
            cmdUpd.CommandText = SQL
            cmdUpd.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetNomenclador", ex)
        End Try

    End Sub

End Class
