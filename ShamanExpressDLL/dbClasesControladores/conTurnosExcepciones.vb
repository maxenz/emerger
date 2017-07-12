Imports System.Data
Imports System.Data.SqlClient
Public Class conTurnosExcepciones
    Inherits typTurnosExcepciones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            If Not valDesdeHasta(Me.FechaHoraDesde, Me.FechaHoraHasta, pMsg) Then
                Exit Function
            End If
            If Me.PersonalId.GetObjectId = 0 Then vRdo = "Debe establecer el profesional en cuestión"
            If Me.PracticaId.GetObjectId = 0 Then vRdo = "Debe establecer la práctica en cuestión"
            If Me.CentroAtencionSalaId.GetObjectId = 0 Then vRdo = "Debe establecer la sala en cuestión"
            If vRdo = "" Then
                If Me.TipoExcepcion = 0 Then
                Else
                    '------> Valido Sala
                    Dim objSala As New conCentrosAtencionSalas(Me.myCnnName)
                    vRdo = objSala.CheckHorario(Me.CentroAtencionSalaId.GetObjectId, Me.FechaHoraDesde, Me.FechaHoraHasta, Me.PersonalId.GetObjectId)
                End If
            End If
            If vRdo <> "" Then
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            Else
                Validar = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "RefreshGrid", ex)
        End Try
    End Function


End Class
