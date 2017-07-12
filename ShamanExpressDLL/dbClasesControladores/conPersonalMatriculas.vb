Imports System.Data
Imports System.Data.SqlClient
Public Class conPersonalMatriculas
    Inherits typPersonalMatriculas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pPer As Int64, ByVal pTipMat As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM PersonalMatriculas WHERE PersonalId = " & pPer & " AND TipoMatriculaId = " & pTipMat

            Dim cmdFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmdFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetAll(ByVal pPer As Int64, Optional pApl As Integer = 0) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT per.ID, CASE AplicacionId WHEN 0 THEN 'Todos' WHEN 1 THEN 'Médicos' WHEN 2 THEN 'Choferes' WHEN 3 THEN 'Enfermeros' END AS AplicacionId, "
            SQL = SQL & "mat.Descripcion AS TipoMatriculaId, per.FecVencimiento, per.Referencia "
            SQL = SQL & "FROM PersonalMatriculas per "
            SQL = SQL & "INNER JOIN TiposMatriculas mat ON (per.TipoMatriculaId = mat.ID) "
            SQL = SQL & "WHERE (per.PersonalId = " & pPer & ") "
            If pApl > 0 Then SQL = SQL & "AND (mat.AplicacionId = " & pApl & ") "
            SQL = SQL & "ORDER BY mat.AplicacionId"

            Dim cmdMat As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdMat.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Sub SetPersonal(ByVal pPer As Int64, ByVal dtMatriculas As DataTable)

        Try

            Dim SQL As String = "DELETE FROM PersonalMatriculas WHERE PersonalId = " & pPer
            Dim cmdDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdDel.ExecuteNonQuery()

            Dim vIdx As Integer

            For vIdx = 0 To dtMatriculas.Rows.Count - 1

                Dim objPersonalMatricula As New conPersonalMatriculas(Me.myCnnName)

                objPersonalMatricula.CleanProperties(objPersonalMatricula)
                objPersonalMatricula.PersonalId.SetObjectId(pPer)
                objPersonalMatricula.TipoMatriculaId.SetObjectId(dtMatriculas.Rows(vIdx)("TipoMatriculaId"))
                objPersonalMatricula.FecVencimiento = dtMatriculas.Rows(vIdx)("FecVencimiento")
                If Not IsDBNull(dtMatriculas.Rows(vIdx)("Referencia")) Then objPersonalMatricula.Referencia = dtMatriculas.Rows(vIdx)("Referencia")

                objPersonalMatricula.Salvar(objPersonalMatricula)

            Next

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPersonal", ex)
        End Try
    End Sub

    Public Function Validar(Optional pMsg As Boolean = True) As Boolean
        Validar = True
        Try
            Dim vRdo As String = ""
            If Me.TipoMatriculaId.GetObjectId = 0 Then vRdo = "Debe determinar el tipo de matrícula"
            If Me.FecVencimiento.Year = 299 Then vRdo = "Debe determinar la fecha de vencimiento"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
