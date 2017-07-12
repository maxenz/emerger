Imports System.Data
Imports System.Data.SqlClient
Public Class conPoligonosCordenadas
    Inherits typPoligonosCoordenadas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByPoligono(ByVal pPoligono As Int64) As DataTable

        GetByPoligono = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, PoligonoId, Latitud, Longitud"
            SQL = SQL & " FROM PoligonosCoordenadas "
            SQL = SQL & "WHERE (PoligonoId = " & pPoligono & ") "

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByPoligono = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByPoligono", ex)
        End Try
    End Function

    Public Function DeleteByPoligono(ByVal pPoligono As Int64) As Integer

        DeleteByPoligono = -1

        Try

            Dim SQL As String

            SQL = "DELETE "
            SQL = SQL & "FROM PoligonosCoordenadas "
            SQL = SQL & "WHERE (PoligonoId = " & pPoligono & ") "

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            DeleteByPoligono = cmdBas.ExecuteNonQuery

        Catch ex As Exception
            HandleError(Me.GetType.Name, "DeleteByPoligono", ex)
        End Try
    End Function

    Public Function ValidarLatLong(Optional ByVal pMsg As Boolean = True) As Boolean
        ValidarLatLong = True
        Try
            Dim vRdo As String = ""
            If Me.Longitud = 0 Then vRdo = "Debe determinar la longitud de la coordenada"
            If Me.Latitud = 0 Then vRdo = "Debe determinar la latitud de la coordenada"
            If vRdo <> "" Then
                ValidarLatLong = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.PoligonoId.GetObjectId = 0 Then vRdo = "Debe determinar el poligono de la coordenada"
            If Me.Longitud = 0 Then vRdo = "Debe determinar la longitud de la coordenada"
            If Me.Latitud = 0 Then vRdo = "Debe determinar la latitud de la coordenada"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
