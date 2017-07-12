Imports System.Data
Imports System.Data.SqlClient
Public Class conMarcasEquipamiento
    Inherits typMarcasEquipamiento
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT mar.ID, tip.Descripcion AS Tipo, mar.Marca, mar.Modelo FROM MarcasEquipamiento mar "
            SQL = SQL & "INNER JOIN TiposEquipamiento tip ON (mar.TipoEquipamientoId = tip.ID) "
            SQL = SQL & " ORDER BY tip.Descripcion, mar.Marca, mar.Modelo"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = True
        Try
            Dim vRdo As String = ""
            If Me.TipoEquipamientoId.GetObjectId = 0 Then vRdo = "Debe determinar el tipo de equipamiento"
            If Me.Marca = "" And vRdo = "" Then vRdo = "Debe determinar la descripción de la marca"
            If Me.Modelo = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del modelo"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
