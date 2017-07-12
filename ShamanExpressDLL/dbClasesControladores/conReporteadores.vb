Imports System.Data
Imports System.Data.SqlClient

Public Class conReporteadores

#Region "General"
    Inherits typReporteadores

    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetForImportacion(ByVal pRepId As rptReporteadores, Optional pTempId As Int64 = 0, Optional ByVal pExcelMode As Boolean = True, Optional pCliOwn As Int64 = 0) As DataTable

        GetForImportacion = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, OrdenImportacion, Descripcion, AliasTabla, Campo, flgReqImportacion, flgSoloImportacion "
            SQL = SQL & "FROM ReporteadoresFields WHERE (ReporteadorId = " & pRepId & ") "
            SQL = SQL & "AND OrdenImportacion > 0 ORDER BY OrdenImportacion, ID"

            Dim cmdGrid As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdGrid.ExecuteReader)


            Dim dtBind As DataTable = dt.Clone
            dtBind.Columns.Remove("ID")
            dtBind.Columns.Remove("OrdenImportacion")
            dtBind.Columns.Remove("flgReqImportacion")
            dtBind.Columns.Remove("flgSoloImportacion")

            dtBind.Columns.Add("ID", GetType(String))
            dtBind.Columns.Add("OrdenImportacion", GetType(String))
            dtBind.Columns.Add("Preliminar", GetType(String))
            dtBind.Columns.Add("flgReqImportacion", GetType(String))
            dtBind.Columns.Add("flgSoloImportacion", GetType(String))

            For vIdx As Integer = 0 To dt.Rows.Count - 1

                Dim dtRow As DataRow = dtBind.NewRow

                If pTempId = 0 Then

                    dtRow("ID") = dt.Rows(vIdx)("ID").ToString
                    If pExcelMode Then
                        dtRow("OrdenImportacion") = Me.getExcelColumn(dt.Rows(vIdx)("OrdenImportacion"))
                    Else
                        dtRow("OrdenImportacion") = dt.Rows(vIdx)("OrdenImportacion")
                    End If
                    dtRow("Descripcion") = dt.Rows(vIdx)("Descripcion")
                    dtRow("AliasTabla") = dt.Rows(vIdx)("AliasTabla")
                    dtRow("Campo") = dt.Rows(vIdx)("Campo")
                    dtRow("Preliminar") = ""
                    dtRow("flgReqImportacion") = dt.Rows(vIdx)("flgReqImportacion").ToString
                    dtRow("flgSoloImportacion") = dt.Rows(vIdx)("flgSoloImportacion").ToString

                Else

                    SQL = "SELECT fld.ID, tmp.NroAgrupacion AS OrdenImportacion, fld.Descripcion, fld.AliasTabla, fld.Campo, fld.flgReqImportacion, fld.flgSoloImportacion "
                    SQL = SQL & "FROM TemplatesFields tmp "
                    SQL = SQL & "INNER JOIN ReporteadoresFields fld ON (tmp.ReporteadorFieldId = fld.ID) "
                    SQL = SQL & "WHERE (tmp.TemplateId = " & pTempId & ") AND (tmp.NroAgrupacion = " & dt.Rows(vIdx)("OrdenImportacion") & ") "

                    Dim cmdTemp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                    Dim dtTemp As New DataTable
                    dtTemp.Load(cmdTemp.ExecuteReader)

                    If dtTemp.Rows.Count > 0 Then

                        dtRow("ID") = dtTemp.Rows(0)("ID").ToString
                        If pExcelMode Then
                            dtRow("OrdenImportacion") = Me.getExcelColumn(dtTemp.Rows(0)("OrdenImportacion"))
                        Else
                            dtRow("OrdenImportacion") = dtTemp.Rows(0)("OrdenImportacion")
                        End If
                        dtRow("Descripcion") = dtTemp.Rows(0)("Descripcion")
                        dtRow("AliasTabla") = dtTemp.Rows(0)("AliasTabla")
                        dtRow("Campo") = dtTemp.Rows(0)("Campo")
                        dtRow("Preliminar") = ""
                        dtRow("flgReqImportacion") = dtTemp.Rows(0)("flgReqImportacion").ToString
                        dtRow("flgSoloImportacion") = dtTemp.Rows(0)("flgSoloImportacion").ToString

                    Else

                        dtRow("ID") = 0
                        If pExcelMode Then
                            dtRow("OrdenImportacion") = Me.getExcelColumn(dt.Rows(vIdx)("OrdenImportacion"))
                        Else
                            dtRow("OrdenImportacion") = dt.Rows(vIdx)("OrdenImportacion")
                        End If
                        dtRow("Descripcion") = ""
                        dtRow("AliasTabla") = ""
                        dtRow("Campo") = ""
                        dtRow("Preliminar") = ""
                        dtRow("flgReqImportacion") = 0
                        dtRow("flgSoloImportacion") = 0

                    End If

                End If

                dtBind.Rows.Add(dtRow)

            Next vIdx

            GetForImportacion = dtBind

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetForImportacion", ex)
        End Try
    End Function

    Public Function getExcelColumn(ByVal pVal As Integer) As String
        getExcelColumn = ""
        Try

            pVal = pVal + 64

            Select Case pVal
                Case 65 To 90
                    getExcelColumn = Chr(pVal)
                Case 91 To 115
                    pVal = pVal - 26
                    getExcelColumn = "A" & Chr(pVal)
                Case 116 To 140
                    pVal = pVal - 51
                    getExcelColumn = "B" & Chr(pVal)
                Case 141 To 164
                    pVal = pVal - 76
                    getExcelColumn = "C" & Chr(pVal)
                Case Else
                    getExcelColumn = ""
            End Select

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getExcelColumn", ex)
        End Try
    End Function

#End Region

End Class