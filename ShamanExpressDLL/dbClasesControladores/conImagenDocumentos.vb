Imports System.Data
Imports System.Data.SqlClient
Imports System.IO

Public Class conImagenDocumentos
    Inherits typImagenDocumentos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pAbr As String) As Int64
        GetIDByIndex = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM ImagenDocumentos WHERE AbreviaturaId = '" & pAbr & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetAll(Optional ByVal pClf As imgDiseño = imgDiseño.Documentos) As DataTable

        GetAll = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion, CantidadCopias, CantidadRenglones FROM ImagenDocumentos "
            SQL = SQL & "WHERE (ISNULL(ClasificacionId, 1) = " & pClf & ") "
            SQL = SQL & " ORDER BY AbreviaturaId"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function Exportar(pImg As Int64) As DataSet
        Exportar = Nothing
        Try

            '------------> Armo cabecera

            Dim SQL As String = "SELECT * FROM ImagenDocumentos WHERE ID = " & pImg

            Dim cmdRpt As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdRpt.ExecuteReader)

            dt.TableName = "Cabecera"

            '------------> Armo detalle

            SQL = "SELECT * FROM ImagenDocRenglones WHERE ImagenDocumentoId = " & pImg

            cmdRpt.CommandText = SQL
            Dim dtDet As New DataTable
            dtDet.Load(cmdRpt.ExecuteReader)

            dtDet.TableName = "Detalle"

            '------------> Guardo
            Dim dtFinal As New DataSet

            dtFinal.DataSetName = "Imagen_Comprobante"
            dtFinal.Tables.Add(dt)
            dtFinal.Tables.Add(dtDet)

            '------------> Guardo
            Exportar = dtFinal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Exportar", ex)
        End Try
    End Function

    Public Function Importar(pFil As String) As Boolean
        Importar = False
        Try

            Dim dtFinal As New DataSet

            dtFinal.ReadXml(pFil)

            Dim dt As DataTable = dtFinal.Tables(0)

            If dt.Rows.Count > 0 Then

                Dim vImgId As Int64 = Me.GetIDByIndex(dt(0)("AbreviaturaId"))

                If vImgId > 0 Then
                    If MsgBox("La imagen " & dt(0)("AbreviaturaId") & " ya existe en el sistema" & vbCrLf & "¿ Desea actualizar su definición ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton2) = MsgBoxResult.No Then
                        Exit Function
                    End If
                End If

                Dim objImagen As New conImagenDocumentos(Me.myCnnName)

                If Not objImagen.Abrir(vImgId) Then

                    objImagen.ClasificacionId = dt(0)("ClasificacionId")
                    objImagen.AbreviaturaId = dt(0)("AbreviaturaId")
                    objImagen.Descripcion = dt(0)("Descripcion")
                    objImagen.CantidadDocumentos = dt(0)("CantidadDocumentos")
                    objImagen.CantidadCopias = dt(0)("CantidadCopias")
                    objImagen.CantidadRenglones = dt(0)("CantidadRenglones")
                    objImagen.flgImporteLetras = dt(0)("flgImporteLetras")

                    If Not objImagen.Salvar(objImagen) Then
                        Exit Function
                    End If

                End If

                Dim SQL As String = "DELETE FROM ImagenDocRenglones WHERE ImagenDocumentoId = " & objImagen.ID
                Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
                cmDel.ExecuteNonQuery()

                Dim dtDet As DataTable = dtFinal.Tables(1)
                Dim vIdx As Integer

                For vIdx = 0 To dtDet.Rows.Count - 1

                    Dim objRenglon = New typImagenDocRenglones(Me.myCnnName)

                    objRenglon.CleanProperties(objRenglon)
                    objRenglon.ImagenDocumentoId.SetObjectId(objImagen.ID)
                    objRenglon.Descripcion = dtDet(vIdx)("Descripcion")
                    objRenglon.Salvar(objRenglon)

                    objRenglon = Nothing

                Next vIdx

                If vImgId > 0 Then
                    MsgBox("Se actualizó correctamente la imagen " & dt(0)("Descripcion"), MsgBoxStyle.Information, "Imágenes")
                Else
                    MsgBox("Se creó correctamente la imagen " & dt(0)("Descripcion"), MsgBoxStyle.Information, "Imágenes")
                End If

                Importar = True

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "Importar", ex)
        End Try
    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador de la imagen"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del imagen"
            If Me.CantidadDocumentos <= 0 And Me.ClasificacionId = imgDiseño.Documentos And vRdo = "" Then vRdo = "La cantidad de documentos debe ser superior a 0"
            If Me.CantidadCopias <= 0 And Me.ClasificacionId = imgDiseño.Documentos And vRdo = "" Then vRdo = "La cantidad de copias debe ser superior a 0"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Sub PasteDibujo(ByVal pRpt As Int64, ByVal pPst As Int64)
        Try

            Dim objRenglon As typImagenDocRenglones

            Dim SQL As String

            SQL = "SELECT Descripcion FROM ImagenDocRenglones WHERE ImagenDocumentoId = " & pPst

            Dim cmdRpt As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdRpt.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                objRenglon = New typImagenDocRenglones(Me.myCnnName)
                objRenglon.CleanProperties(objRenglon)
                objRenglon.ImagenDocumentoId.SetObjectId(pRpt)
                objRenglon.Descripcion = dt(vIdx)(0)
                objRenglon.Salvar(objRenglon)

                objRenglon = Nothing

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "PasteDibujo", ex)
        End Try
    End Sub

End Class
