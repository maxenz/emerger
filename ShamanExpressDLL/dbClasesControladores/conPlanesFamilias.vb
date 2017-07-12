Imports System.Data
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Public Class conPlanesFamilias
    Inherits typPlanesFamilias
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIDByDescripcion(ByVal pDes As String) As Int64
        GetIDByDescripcion = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM PlanesFamilias WHERE Descripcion = '" & pDes & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try
    End Function

    Public Function GetAll(Optional ByVal pAct As Integer = 1) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT fam.ID, fam.Descripcion, ali.Porcentaje, fam.NroImpresion FROM PlanesFamilias fam "
            SQL = SQL & "LEFT JOIN AlicuotasIva ali ON (fam.AlicuotaIvaId = ali.ID) "
            SQL = SQL & "ORDER BY fam.Descripcion"

            Dim cmdBus As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBus.ExecuteReader)

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
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción de la familias de planes"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
