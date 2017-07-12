Imports System.Data
Imports System.Data.SqlClient
Public Class conIncidentesOtrosConceptos
    Inherits typIncidentesOtrosConceptos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pInc As Int64, ByVal pCon As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM IncidentesOtrosConceptos WHERE IncidenteId = " & pInc & " AND ConceptoFacturacionId = " & pCon

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function GetOtrosConceptosByIncidente(ByVal pInc As Int64) As DataTable

        GetOtrosConceptosByIncidente = Nothing

        Try
            Dim SQL As String
            Dim dt As New DataTable

            dt.Columns.Add("ID", GetType(Int64))
            dt.Columns.Add("ConceptoFacturacionId", GetType(Int64))
            dt.Columns.Add("Seleccion", GetType(Boolean))
            dt.Columns.Add("Prestacion", GetType(String))
            dt.Columns.Add("Cantidad", GetType(Integer))

            SQL = "SELECT ID, Descripcion FROM ConceptosFacturacion WHERE Clasificacion = 1 ORDER BY Descripcion"

            Dim cmdPra As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dtPra As New DataTable
            Dim vIdx As Integer
            dtPra.Load(cmdPra.ExecuteReader)

            For vIdx = 0 To dtPra.Rows.Count - 1

                Dim dtRow As DataRow = dt.NewRow

                If Me.Abrir(Me.GetIDByIndex(pInc, dtPra(vIdx)("ID"))) Then
                    dtRow("ID") = Me.ID
                    dtRow("Seleccion") = True
                    dtRow("Cantidad") = Me.Cantidad
                Else
                    dtRow("ID") = 0
                    dtRow("Seleccion") = False
                    dtRow("Cantidad") = 0
                End If

                dtRow("ConceptoFacturacionId") = dtPra(vIdx)("ID")
                dtRow("Prestacion") = dtPra(vIdx)("Descripcion")

                dt.Rows.Add(dtRow)

            Next vIdx

            GetOtrosConceptosByIncidente = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "RefreshGrid", ex)
        End Try
    End Function

    Public Sub SetOtrosConceptos(ByVal pInc As Int64, ByVal pTblView As DataView)
        Try

            Dim vIdx As Integer, objIncConcepto As conIncidentesOtrosConceptos

            Dim SQL As String = "UPDATE IncidentesOtrosConceptos SET flgPurge = 1 WHERE IncidenteId = " & pInc
            Dim cmdUpd As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdUpd.ExecuteNonQuery()

            Dim vTblPre As DataTable = pTblView.Table

            For vIdx = 0 To vTblPre.Rows.Count - 1

                Dim dtRow As DataRow = vTblPre.Rows(vIdx)

                If dtRow("Seleccion") Then

                    objIncConcepto = New conIncidentesOtrosConceptos(Me.myCnnName)

                    If Not objIncConcepto.Abrir(dtRow("ID")) Then
                        objIncConcepto.IncidenteId.SetObjectId(pInc)
                    End If

                    objIncConcepto.ConceptoFacturacionId.SetObjectId(dtRow("ConceptoFacturacionId"))

                    objIncConcepto.Cantidad = dtRow("Cantidad")
                    objIncConcepto.flgPurge = 0
                    objIncConcepto.Salvar(objIncConcepto)
                    objIncConcepto = Nothing

                End If

            Next

            SQL = "DELETE FROM IncidentesOtrosConceptos WHERE IncidenteId = " & pInc & " AND flgPurge = 1"
            cmdUpd.CommandText = SQL
            cmdUpd.ExecuteNonQuery()

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPrestaciones", ex)
        End Try
    End Sub

    Public Sub SetOtrosConceptosLaboratorio(ByVal pInc As Int64, ByVal pLabTst As Integer, ByVal pLabCnt As Integer)
        Try
            If pLabTst > 0 Then

                Dim SQL As String

                SQL = "SELECT ID FROM ConceptosFacturacion WHERE ISNULL(flgLaboratorioTest, 0) = 1"

                Dim cmdPra As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                Dim dtPra As New DataTable
                Dim vIdx As Integer
                dtPra.Load(cmdPra.ExecuteReader)

                For vIdx = 0 To dtPra.Rows.Count - 1

                    Dim objIncConcepto As New conIncidentesOtrosConceptos

                    If Not objIncConcepto.Abrir(objIncConcepto.GetIDByIndex(pInc, dtPra(vIdx)("ID"))) Then
                        objIncConcepto.IncidenteId.SetObjectId(pInc)
                        objIncConcepto.ConceptoFacturacionId.SetObjectId(dtPra(vIdx)("ID"))
                        objIncConcepto.Cantidad = pLabCnt
                        objIncConcepto.Salvar(objIncConcepto)
                    Else
                        If objIncConcepto.Cantidad < pLabCnt Then objIncConcepto.Cantidad = pLabCnt
                        objIncConcepto.Salvar(objIncConcepto)
                    End If
                    objIncConcepto = Nothing
                Next vIdx

            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetOtrosConceptosLaboratorio", ex)
        End Try
    End Sub

End Class
