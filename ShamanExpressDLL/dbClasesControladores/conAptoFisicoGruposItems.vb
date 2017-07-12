Imports System.Data
Imports System.Data.SqlClient
Public Class conAptoFisicoGruposItems
    Inherits typAptoFisicoGruposItems
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByGrupo(ByVal pGrp As Int64) As DataTable

        GetByGrupo = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, NroItemGrupo, SubGrupo, Descripcion, "
            SQL = SQL & "CASE TipoValor WHEN 1 THEN 'Tilde' WHEN 2 THEN 'Choice SubGrupo' WHEN 3 THEN 'Texto' WHEN 4 THEN 'Patologia' WHEN 5 THEN 'Antecedente' ELSE '' END AS TipoValor "
            SQL = SQL & "FROM AptoFisicoGruposItems "
            SQL = SQL & "WHERE (AptoFisicoGrupoId = " & pGrp & ") "
            SQL = SQL & " ORDER BY NroItemGrupo"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByGrupo = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByGrupo", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del ítem"
            If Me.SubGrupo = "" And Me.TipoValor = 2 And vRdo = "" Then vRdo = "Debe determinar la descripción del ítem"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
End Class

