Imports System.Data
Imports System.Data.SqlClient
Public Class conInfoPacienteItems
    Inherits typInfoPacienteItems
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT itm.ID, grp.Descripcion AS InfoPacienteGrupo, itm.Descripcion "
            SQL = SQL & "FROM InfoPacienteItems itm "
            SQL = SQL & "INNER JOIN InfoPacienteGrupos grp ON (itm.InfoPacienteGrupoId = grp.ID) "
            SQL = SQL & "ORDER BY grp.Descripcion, itm.Descripcion"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

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
            If Me.InfoPacienteGrupoId.GetObjectId = 0 Then vRdo = "Debe determinar el grupo al que pertenece"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del ítem"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
