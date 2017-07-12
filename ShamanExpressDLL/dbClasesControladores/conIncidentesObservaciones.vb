Imports System.Data
Imports System.Data.SqlClient
Public Class conIncidentesObservaciones
    Inherits typIncidentesObservaciones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByIncidenteId(ByVal pInc As Int64, Optional ByVal pRcl As Boolean = False) As String
        GetByIncidenteId = ""
        Try
            Dim SQL As String, vObs As String = ""

            SQL = "SELECT Observaciones FROM IncidentesObservaciones WHERE (IncidenteId = " & pInc & ") "
            If pRcl Then SQL = SQL & "AND (flgReclamo = 1) "
            SQL = SQL & "ORDER BY regFechaHora"

            Dim cmdObs As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer = 0
            dt.Load(cmdObs.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1
                If vObs = "" Then
                    vObs = dt(vIdx).Item(0)
                Else
                    vObs = vObs & " // " & dt.Rows(vIdx).Item(0)
                End If
            Next vIdx

            GetByIncidenteId = vObs
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByIncidenteId", ex)
        End Try
    End Function
End Class
