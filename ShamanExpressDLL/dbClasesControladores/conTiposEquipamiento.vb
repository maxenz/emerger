Imports System.Data
Imports System.Data.SqlClient
Public Class conTiposEquipamiento
    Inherits typTiposEquipamiento
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable
        GetAll = Nothing
        Try
            Dim SQL As String

            SQL = "SELECT ID, Descripcion, CASE ClasificacionId WHEN 0 THEN 'COMUNICACIONES' ELSE 'ELECTROMEDICINA' END AS Clasificacion "
            SQL = SQL & "FROM TiposEquipamiento "
            SQL = SQL & "ORDER BY Descripcion"

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
            If Me.Descripcion = "" Then vRdo = "Debe determinar la descripción del tipo de equipamiento"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
