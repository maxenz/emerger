Imports System.Data
Imports System.Data.SqlClient
Public Class conMarcasModelos
    Inherits typMarcasModelos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, Marca, Modelo FROM MarcasModelos "
            SQL = SQL & " ORDER BY Marca, Modelo"

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
            If Me.Marca = "" Then vRdo = "Debe determinar la descripción de la marca"
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
