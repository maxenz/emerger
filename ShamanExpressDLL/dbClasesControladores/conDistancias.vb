Imports System.Data
Imports System.Data.SqlClient
Public Class conDistancias
    Inherits typDistancias
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByLocalidad(ByVal pLoc As Int64) As DataTable

        GetByLocalidad = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT dis.ID, dis.LocalidadDestinoId, loc.AbreviaturaId, loc.Descripcion, dis.Distancia "
            SQL = SQL & "FROM Distancias dis INNER JOIN Localidades loc ON (dis.LocalidadDestinoId = loc.ID) "
            SQL = SQL & "WHERE (dis.LocalidadOrigenId = " & pLoc & ") "
            SQL = SQL & "ORDER BY loc.AbreviaturaId, loc.Descripcion"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByLocalidad = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByLocalidad", ex)
        End Try
    End Function
    Public Function GetIDByIndex(ByVal pOri As Int64, ByVal pDst As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Distancias WHERE (LocalidadOrigenId = " & pOri & ") AND (LocalidadDestinoId = " & pDst & ")"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function GetDistancia(ByVal pOri As Int64, ByVal pDst As Int64, Optional ByVal pUseDft As Boolean = True, Optional ByVal pLocDft As Long = 0) As Decimal
        GetDistancia = 0
        Try
            If Me.Abrir(Me.GetIDByIndex(pOri, pDst)) Then
                GetDistancia = Me.Distancia
            ElseIf Me.Abrir(Me.GetIDByIndex(pDst, pOri)) Then
                GetDistancia = Me.Distancia
            Else
                If pOri <> pDst And pUseDft Then
                    Dim vSysDft As Boolean = True
                    If pLocDft > 0 Then
                        If Me.Abrir(Me.GetIDByIndex(pOri, pLocDft)) Then
                            GetDistancia = Me.Distancia
                            vSysDft = False
                        End If
                    End If
                    If vSysDft Then
                        If Me.Abrir(Me.GetIDByIndex(pOri, New conLocalidades(Me.myCnnName).GetDefault)) Then
                            GetDistancia = Me.Distancia
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetDistancia", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.LocalidadOrigenId.GetObjectId = 0 Then vRdo = "Debe determinar la localidad de origen"
            If Me.LocalidadDestinoId.GetObjectId = 0 Then vRdo = "Debe determinar la localidad de destino"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
