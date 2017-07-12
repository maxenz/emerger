Imports System.Data
Imports System.Data.SqlClient
Public Class conUsuariosLogueados
    Inherits typUsuariosLogueados
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String
            Dim vTpo As String

            SQL = "SELECT log.ID, usr.Identificacion, usr.Nombre, log.PID, log.HKeyPId, ter.NombrePC, log.FechaHoraInicio "
            SQL = SQL & "FROM UsuariosLogueados log "
            SQL = SQL & "INNER JOIN Usuarios usr ON (log.UsuarioId = usr.ID) "
            SQL = SQL & "LEFT JOIN Terminales ter ON (log.TerminalId = ter.ID) "
            SQL = SQL & " ORDER BY usr.Identificacion"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdBas.ExecuteReader)

            dt.Columns.Add("Tiempo", GetType(String))

            For vIdx = 0 To dt.Rows.Count - 1

                If DateDiff(DateInterval.Hour, Now, dt.Rows(vIdx)("FechaHoraInicio")) > 0 Then
                    vTpo = DateDiff(DateInterval.Hour, dt.Rows(vIdx)("FechaHoraInicio"), Now) & " hs."
                Else
                    vTpo = DateDiff(DateInterval.Minute, dt.Rows(vIdx)("FechaHoraInicio"), Now) & " min."
                End If

                dt.Rows(vIdx)("Tiempo") = vTpo

            Next vIdx

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function
    Public Function GetIDByIndex(ByVal pPID As Int64) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM UsuariosLogueados WHERE PID = " & pPID

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function
    Public Function Loguear() As Int64

        Loguear = 0

        Try
            Dim objSession As New conUsuariosLogueados

            If Not objSession.Abrir(objSession.GetIDByIndex(logPID)) Then
                objSession.CleanProperties(objSession)
                objSession.PID = logPID
            End If

            objSession.UsuarioId.SetObjectId(logUsuario)
            objSession.TerminalId.SetObjectId(logTerminal)
            objSession.HKeyPID = logHKeyPID
            objSession.FechaHoraInicio = GetCurrentTime()

            objSession.PerfilId.SetObjectId(logPerfilId)
            objSession.AgenteId = logAgenteId

            If objSession.Salvar(objSession) Then
                Loguear = objSession.ID
            End If

            objSession = Nothing

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Loguear", ex)
        End Try

    End Function
End Class
