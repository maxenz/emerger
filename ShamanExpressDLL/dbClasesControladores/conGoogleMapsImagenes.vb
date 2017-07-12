Imports System.Data
Imports System.Data.SqlClient
Public Class conGoogleMapsImagenes
    Inherits typGoogleMapsImagenes
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetByGradoOperativoId(ByVal pGdo As Int64) As String
        GetByGradoOperativoId = ""
        Try
            Dim SQL As String

            SQL = "SELECT Imagen FROM GoogleMapsImagenes WHERE GradoOperativoId = " & pGdo

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetByGradoOperativoId = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByGradoOperativoId", ex)
        End Try
    End Function

    Public Function SetByGradoOperativo(ByVal pGdo As Int64, ByVal pImg As String) As Boolean

        SetByGradoOperativo = False

        Try
            Dim SQL As String

            SQL = "DELETE FROM GoogleMapsImagenes WHERE GradoOperativoId = " & pGdo

            Dim cmdDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdDel.ExecuteNonQuery()

            Dim objImagen As New conGoogleMapsImagenes(Me.myCnnName)

            objImagen.CleanProperties(objImagen)
            objImagen.GradoOperativoId.SetObjectId(pGdo)
            objImagen.Imagen = pImg

            If objImagen.Salvar(objImagen) Then
                SetByGradoOperativo = True
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetByGradoOperativo", ex)
        End Try
    End Function

    Public Function GetByIncidenteId(ByVal pInc As Int64) As String
        GetByIncidenteId = ""
        Try
            Dim SQL As String

            SQL = "SELECT goo.Imagen FROM Incidentes inc "
            SQL = SQL & "INNER JOIN GoogleMapsImagenes goo ON (inc.GradoOperativoId = goo.GradoOperativoId) "
            SQL = SQL & "WHERE (inc.ID = " & pInc & ") "

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetByIncidenteId = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetByIncidenteId", ex)
        End Try
    End Function

End Class
