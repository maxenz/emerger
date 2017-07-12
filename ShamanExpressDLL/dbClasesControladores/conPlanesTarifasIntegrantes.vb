Imports System.Data
Imports System.Data.SqlClient
Public Class conPlanesTarifasIntegrantes
    Inherits typPlanesTarifasIntegrantes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByPlan(ByVal pPla As Long) As DataTable

        GetByPlan = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT rel.ID, rel.IntegranteClasificacionId, clf.Descripcion, rel.EdadDesde, rel.EdadHasta, rel.Importe "
            SQL = SQL & "FROM PlanesTarifasIntegrantes rel "
            SQL = SQL & "INNER JOIN IntegrantesClasificaciones clf ON (rel.IntegranteClasificacionId = clf.ID) "
            SQL = SQL & "WHERE (rel.PlanId = " & pPla & ") "
            SQL = SQL & "ORDER BY rel.flgRequerido DESC, clf.flgIngresoMultiple, clf.Descripcion, rel.EdadDesde"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetByPlan = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByPlan", ex)
        End Try
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.IntegranteClasificacionId.GetObjectId = 0 Then vRdo = "Debe establecer el tipo de integrante"
            If Me.EdadDesde > Me.EdadHasta And vRdo = "" Then vRdo = "El valor de la edad desde deber ser inferior al valor de la edad hasta"
            If vRdo = "" Then
                Dim SQL As String

                SQL = "SELECT ID FROM PlanesTarifasIntegrantes "
                SQL = SQL & "WHERE (PlanId = " & Me.PlanId.GetObjectId & ") AND (IntegranteClasificacionId = " & Me.IntegranteClasificacionId.GetObjectId & ") "
                SQL = SQL & "AND (ID <> " & Me.ID & ") "
                SQL = SQL & "AND ((" & Me.EdadDesde & " BETWEEN EdadDesde AND EdadHasta) "
                SQL = SQL & "OR (" & Me.EdadHasta & " BETWEEN EdadDesde AND EdadHasta) "
                SQL = SQL & "OR ((" & Me.EdadDesde & " < EdadDesde) AND (" & Me.EdadHasta & " > EdadHasta))) "

                Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                If cmFind.ExecuteScalar > 0 Then
                    vRdo = "El rango de edades causaría superposiciones"
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

End Class
