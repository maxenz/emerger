Imports System.Data
Imports System.Data.SqlClient
Public Class conAptoFisicoInfo
    Inherits typAptoFisicoInfo
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll(pClf As infAptoFisicoItem) As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, Descripcion "
            SQL = SQL & "FROM AptoFisicoInfo "
            SQL = SQL & "WHERE ClasificacionId = " & pClf
            SQL = SQL & " ORDER BY Descripcion"

            Dim cmdCon As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdCon.ExecuteReader)

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
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del registro"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try

    End Function


End Class
