Imports System.Data
Imports System.Data.SqlClient
Public Class conTiposMoviles
    Inherits typTiposMoviles
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion FROM TiposMoviles "
            SQL = SQL & " ORDER BY AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = True
        Try
            Dim vRdo As String = ""
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador del tipo de móvil"
            If Me.Descripcion = "" Then vRdo = "Debe determinar la descripción del tipo de móvil"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Function GetGradosOperativosLine(ByVal pTmv As Int64) As String
        GetGradosOperativosLine = ""
        Try
            Dim SQL As String, vTxt As String = ""
            SQL = "SELECT C.AbreviaturaId FROM TiposMovilesGrados A "
            SQL = SQL & "INNER JOIN GradosOperativos B ON (A.GradoOperativoId = B.ID) "
            SQL = SQL & "INNER JOIN ConceptosFacturacion C ON (B.ConceptoFacturacion1Id = C.ID) "
            SQL = SQL & "WHERE (A.TipoMovilId = " & pTmv & ") "
            SQL = SQL & "ORDER BY B.Orden"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdGrl.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                If vTxt = "" Then
                    vTxt = dt(vIdx).Item(0)
                Else
                    vTxt = vTxt & " - " & dt(vIdx).Item(0)
                End If

            Next vIdx

            GetGradosOperativosLine = vTxt
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetGradosOperativosLine", ex)
        End Try
    End Function

End Class
