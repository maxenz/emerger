Imports System.Data
Imports System.Data.SqlClient
Public Class conSintomasPreguntas
    Inherits typSintomasPreguntas
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetBySintoma(ByVal pSin As Int64, ByVal pPed As Integer) As DataTable

        GetBySintoma = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, TipoFrase, Frase, Respuesta1, Respuesta2, PreguntaId FROM SintomasPreguntas "
            SQL = SQL & "WHERE (SintomaId = " & pSin & ") AND (flgPediatrico = " & pPed & ") "
            SQL = SQL & "ORDER BY PreguntaId"

            Dim cmdGrl As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdGrl.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                If dt(vIdx).Item(1) = "C" Then
                    dt(vIdx).Item(2) = Space(5) & dt(vIdx).Item(2)
                End If

            Next vIdx

            GetBySintoma = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetBySintoma", ex)
        End Try

    End Function

    Public Function getNextPreguntaId(ByVal pSin As Int64, ByVal pPed As Integer) As Integer
        getNextPreguntaId = 1
        Try
            Dim SQL As String

            SQL = "SELECT ISNULL(MAX(PreguntaId), 0) + 1 FROM SintomasPreguntas WHERE (SintomaId = " & pSin & ") "
            SQL = SQL & "AND (flgPediatrico = " & pPed & ")"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getNextPreguntaId = CType(vOutVal, Integer)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getNextPreguntaId", ex)
        End Try

    End Function
    Public Function Swap(ByVal pOri As Long, ByVal pDst As Long) As Boolean
        Swap = False
        Try
            Dim objOri As New conSintomasPreguntas(Me.myCnnName)
            Dim objDst As New conSintomasPreguntas(Me.myCnnName)
            Dim vVal As Integer

            If objOri.Abrir(pOri) Then
                If objDst.Abrir(pDst) Then
                    vVal = objOri.PreguntaId
                    objOri.PreguntaId = objDst.PreguntaId
                    objDst.PreguntaId = 9999
                    If objDst.Salvar(objDst) Then
                        If objOri.Salvar(objOri) Then
                            objDst.PreguntaId = vVal
                            If objDst.Salvar(objDst) Then
                                Swap = True
                            End If
                        End If
                    End If
                End If
            End If
            objOri = Nothing
            objDst = Nothing
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Swap", ex)
        End Try
    End Function
    Private Function shwPuntaje(ByVal pVal As Integer) As String
        If pVal > 0 Then
            shwPuntaje = pVal
        Else
            shwPuntaje = ""
        End If
    End Function
    Public Function Validar() As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.TipoFrase = "" And Me.flgPediatrico < 2 Then vRdo = "Debe especificar el tipo de frase"
            If Me.Frase = "" And vRdo = "" Then vRdo = "Debe establecer el valor de la frase"
            If vRdo <> "" Then
                Validar = False
                MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

End Class
