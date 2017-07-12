Imports System.Data
Imports System.Data.SqlClient
Public Class conPoligonos
    Inherits typPoligonos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll(Optional ByVal pAct As Integer = 1, Optional ByVal pClf As Integer = -1) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion, "
            SQL = SQL & "CASE ClasificacionPoligono WHEN 0 THEN 'Operativo' ELSE 'Cobranzas' END AS Clasificacion "
            SQL = SQL & "FROM Poligonos "
            If pAct = 1 Then SQL = sqlWhere(SQL) & "(Activo = 1)"
            If pAct = 2 Then SQL = sqlWhere(SQL) & "(Activo = 0)"
            If pClf >= 0 Then SQL = sqlWhere(SQL) & "(ClasificacionPoligono = " & pClf & ")"
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código del poligono"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del poligono"
            If Me.ClasificacionPoligono = -1 And vRdo = "" Then vRdo = "Debe determinar la clasificación del poligono"

            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
