Imports System.Data
Imports System.Data.SqlClient

Public Class conIncidentesSintomas
    Inherits typIncidentesSintomas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetLastSAC(pInc As Int64, pSin As Int64) As Int64
        GetLastSAC = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 cab.ID FROM IncidentesSintomas cab "
            SQL = SQL & "INNER JOIN IncidentesSintomasPregs prg ON (cab.ID = prg.IncidenteSintomaId) "
            SQL = SQL & "WHERE (cab.IncidenteId = " & pInc & ") AND (cab.SintomaId = " & pSin & ") AND (prg.flgPediatrico < 2) "
            SQL = SQL & "ORDER BY cab.regFechaHora DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetLastSAC = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetLastSAC", ex)
        End Try
    End Function
    Public Function IsPreArribo(pId As Int64) As Boolean
        IsPreArribo = False
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 cab.ID FROM IncidentesSintomas cab "
            SQL = SQL & "INNER JOIN IncidentesSintomasPregs prg ON (cab.ID = prg.IncidenteSintomaId) "
            SQL = SQL & "WHERE (cab.ID = " & pId & ") AND (prg.flgPediatrico = 2) "
            SQL = SQL & "ORDER BY cab.regFechaHora DESC"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then IsPreArribo = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "IsPreArribo", ex)
        End Try
    End Function
    Public Sub tmpDelSession(ByVal pPid As Int64)
        Try
            Dim SQL As String = "DELETE FROM tmpIncidentesSintomas WHERE PID = " & pPid
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()
        Catch ex As Exception
            HandleError(Me.GetType.Name, "ClearSesion", ex)
        End Try
    End Sub
    Public Function tmpNewSession(ByVal pPid As Int64, ByVal pSin As Int64) As Int64
        tmpNewSession = 0
        Try
            Dim objTemporal As New typtmpIncidentesSintomas
            Dim objSintomas As New typSintomas

            If objSintomas.Abrir(pSin) Then
                objTemporal.CleanProperties(objTemporal)
                objTemporal.PID = pPid
                objTemporal.SintomaId.SetObjectId(pSin)
                objTemporal.Descripcion = objSintomas.Descripcion
                If objTemporal.Salvar(objTemporal) Then
                    tmpNewSession = objTemporal.ID
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "tmpNewSession", ex)
        End Try
    End Function
    Public Function tmpSetGrado(ByVal pTmpId As Int64, ByVal pGdo As Int64) As Boolean
        tmpSetGrado = False
        Try
            Dim objTemporal As New typtmpIncidentesSintomas

            If objTemporal.Abrir(pTmpId) Then
                objTemporal.GradoOperativoId.SetObjectId(pGdo)
                If objTemporal.Salvar(objTemporal) Then
                    tmpSetGrado = True
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "tmpSetGrado", ex)
        End Try

    End Function
    Private Function tmpGetSintomaPreguntaId(ByVal pTmpId As Int64, ByVal pPre As Integer) As Int64
        tmpGetSintomaPreguntaId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM tmpIncidentesSintomasPregs WHERE tmpIncidenteSintomaId = " & pTmpId & " AND PreguntaId = " & pPre
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then tmpGetSintomaPreguntaId = CType(vOutVal, Int64)
        Catch ex As Exception
            HandleError(Me.GetType.Name, "tmpGetSintomaPreguntaId", ex)
        End Try
    End Function

End Class
