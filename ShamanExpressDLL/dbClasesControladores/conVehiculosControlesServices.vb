Imports System.Data
Imports System.Data.SqlClient

Public Class conVehiculosControlesServices
    Inherits typVehiculosControlesServices
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByIndex(ByVal pCon As Int64, ByVal pSrv As Int64, Optional pMan As Boolean = False) As Long
        GetIDByIndex = 0
        Try
            Dim SQL As String

            If Not pMan Then
                SQL = "SELECT ID FROM VehiculosControlesServices WHERE VehiculoControlId = " & pCon & " AND ServiceVehiculoId = " & pSrv
            Else
                SQL = "SELECT ID FROM VehiculosControlesServices WHERE VehiculoControlId = " & pCon & " AND SectorArregloVehiculoId = " & pSrv
            End If

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIndex = CType(vOutVal, Long)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIndex", ex)
        End Try
    End Function

    Public Function GetLastProximo(ByVal pVeh As Int64, ByVal pSrv As Int64) As Long
        GetLastProximo = 0
        Try
            Dim SQL As String

            SQL = "SELECT TOP 1 srv.KmProximoControl - ctr.Kilometraje "
            SQL = SQL & "FROM VehiculosControlesServices srv "
            SQL = SQL & "INNER JOIN VehiculosControles ctr ON (srv.VehiculoControlId = ctr.ID) "
            SQL = SQL & "WHERE (ctr.VehiculoId = " & pVeh & ") AND (ServiceVehiculoId = " & pSrv & ") "
            SQL = SQL & "AND (srv.KmProximoControl > ctr.Kilometraje) "
            SQL = SQL & "ORDER BY ctr.FecHorControl"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetLastProximo = CType(vOutVal, Long)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetLastProximo", ex)
        End Try
    End Function

    Public Sub SetDTServicesByControl(pId As Int64, ByVal pDt As DataTable)

        Try

            Dim SQL As String = "DELETE FROM VehiculosControlesServices WHERE VehiculoControlId = " & pId & " AND ServiceVehiculoId > 0 "
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            Dim vIdx As Integer

            For vIdx = 0 To pDt.Rows.Count - 1

                If pDt.Rows(vIdx)(5) Then

                    Dim objService As New typVehiculosControlesServices(Me.myCnnName)

                    objService.CleanProperties(objService)
                    objService.VehiculoControlId.SetObjectId(pId)
                    objService.ServiceVehiculoId.SetObjectId(pDt.Rows(vIdx)(0))
                    If Not IsDBNull(pDt.Rows(vIdx)(3)) Then
                        objService.KmProximoControl = Val(pDt.Rows(vIdx)(3))
                    End If
                    If Not IsDBNull(pDt.Rows(vIdx)(4)) Then
                        objService.Observaciones = pDt.Rows(vIdx)(4)
                    End If
                    objService.Salvar(objService)

                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDTServicesByControl", ex)
        End Try

    End Sub

    Public Sub SetDTMantenimientoByControl(pId As Int64, ByVal pDt As DataTable)

        Try

            Dim SQL As String = "DELETE FROM VehiculosControlesServices WHERE VehiculoControlId = " & pId & " AND SectorArregloVehiculoId > 0 "
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            Dim vIdx As Integer

            For vIdx = 0 To pDt.Rows.Count - 1

                If pDt.Rows(vIdx)(3) Then

                    Dim objService As New typVehiculosControlesServices(Me.myCnnName)

                    objService.CleanProperties(objService)
                    objService.VehiculoControlId.SetObjectId(pId)
                    objService.SectorArregloVehiculoId.SetObjectId(pDt.Rows(vIdx)(0))
                    If Not IsDBNull(pDt.Rows(vIdx)(2)) Then
                        objService.Observaciones = pDt.Rows(vIdx)(2)
                    End If
                    objService.Salvar(objService)

                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetDTMantenimientoByControl", ex)
        End Try

    End Sub

End Class
