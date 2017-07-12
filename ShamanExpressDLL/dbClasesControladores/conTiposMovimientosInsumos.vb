Imports System.Data
Imports System.Data.SqlClient
Public Class conTiposMovimientosInsumos
    Inherits typTiposMovimientosInsumos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion "
            SQL = SQL & "FROM TiposMovimientosInsumos "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdEmp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEmp.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador del tipo de movimiento"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar el nombre del tipo de movimiento"
            If vRdo <> "" Then
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            Else
                Validar = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
    Public Function GetDefault() As Int64
        GetDefault = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM TiposMovimientosInsumos WHERE flgReposicion = 1"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetDefault = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDefault", ex)
        End Try

    End Function
    Public Function SetDefault(ByVal pTdc As Int64) As Boolean
        SetDefault = False
        Try
            Dim SQL As String = "UPDATE TiposMovimientosInsumos SET flgReposicion = 0 WHERE flgReposicion = 1"
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            Dim objTiposMovimientos As New conTiposMovimientosInsumos(Me.myCnnName)
            If objTiposMovimientos.Abrir(pTdc) Then
                objTiposMovimientos.flgReposicion = 1
                SetDefault = objTiposMovimientos.Salvar(objTiposMovimientos)
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDefault", ex)
        End Try

    End Function

End Class
