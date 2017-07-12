Imports System.Data
Imports System.Data.SqlClient
Public Class conProvincias
    Inherits typProvincias
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByPais(ByVal pPai As Long) As DataTable

        GetByPais = Nothing

        Try

            Dim SQL As String
            SQL = "SELECT ID, AbreviaturaId, Descripcion FROM Provincias "
            SQL = SQL & "WHERE (PaisId = " & pPai & ") "
            SQL = SQL & " ORDER BY AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByPais = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByPais", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador de la provincia"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción de la provincia"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class
