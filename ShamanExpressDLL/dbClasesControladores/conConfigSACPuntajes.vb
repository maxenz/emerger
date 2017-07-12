Imports System.Data
Imports System.Data.SqlClient
Public Class conConfigSACPuntajes
    Inherits typConfigSACPuntajes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT sac.ID, sac.punDesde AS Desde, sac.punHasta AS Hasta, gdo.AbreviaturaId AS Grado, gdo.Descripcion AS Nombre "
            SQL = SQL & "FROM ConfigSACPuntajes sac "
            SQL = SQL & "LEFT JOIN GradosOperativos gdo ON (sac.GradoOperativoId = gdo.ID) "
            SQL = SQL & "ORDER BY sac.punDesde"

            Dim cmdSac As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdSac.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "RefreshGrid", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.punDesde > Me.punHasta Then vRdo = "El valor desde debe ser inferior al valor hasta"
            If Me.GradoOperativoId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe determinar el grado operativo"
            If vRdo = "" Then
                '---------------> Verifiación de Rangos de Puntajes (3 - variantes)
                Dim SQL As String
                SQL = "SELECT ID FROM ConfigSACPuntajes WHERE ID <> " & Me.ID
                SQL = SQL & " AND " & Me.punDesde & " BETWEEN punDesde AND punHasta "

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                If cmFind.ExecuteScalar Is Nothing Then

                    SQL = "SELECT ID FROM ConfigSACPuntajes WHERE ID <> " & Me.ID
                    SQL = SQL & " AND " & Me.punHasta & " BETWEEN punDesde AND punHasta "
                    cmFind.CommandText = SQL

                    If cmFind.ExecuteScalar Is Nothing Then

                        SQL = "SELECT ID FROM ConfigSACPuntajes WHERE ID <> " & Me.ID
                        SQL = SQL & " AND " & Me.punDesde & " <= punDesde "
                        SQL = SQL & " AND " & Me.punHasta & " >= punHasta "
                        cmFind.CommandText = SQL

                        If Not cmFind.ExecuteScalar Is Nothing Then
                            vRdo = "El rango de números se superpone a rangos existentes"
                        End If
                    Else
                        vRdo = "El rango de números se superpone a rangos existentes"
                    End If
                Else
                    vRdo = "El rango de números se superpone a rangos existentes"
                End If
            End If
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
    Public Function getMaxPuntaje() As Integer
        getMaxPuntaje = 0
        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(MAX(punDesde), 0) FROM ConfigSACPuntajes"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getMaxPuntaje = CType(vOutVal, Integer)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getMaxPuntaje", ex)
        End Try
    End Function
    Public Function getGradoOperativoIdFromPuntaje(ByVal pPun As Integer) As Int64
        getGradoOperativoIdFromPuntaje = 0
        Try

            Dim SQL As String

            SQL = "SELECT GradoOperativoId FROM ConfigSACPuntajes "
            SQL = SQL & "WHERE (punDesde <= " & pPun & ") AND (punHasta >= " & pPun & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getGradoOperativoIdFromPuntaje = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getGradoOperativoIdFromPuntaje", ex)
        End Try
    End Function
End Class
