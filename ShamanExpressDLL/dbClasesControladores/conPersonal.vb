Imports System.Data
Imports System.Data.SqlClient
Public Class conPersonal
    Inherits typPersonal
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetQueryBase(Optional ByVal pAct As Integer = 1, Optional ByVal pPre As Integer = 0) As DataTable

        GetQueryBase = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT per.ID, per.Legajo, per.Apellido, per.Nombre, dep.Descripcion AS Departamento, pue.Descripcion AS Puesto "
            SQL = SQL & "FROM Personal per "
            SQL = SQL & "LEFT JOIN DepartamentosPuestos pue ON (per.DepartamentoPuestoId = pue.ID) "
            SQL = SQL & "LEFT JOIN Departamentos dep ON (pue.DepartamentoId = dep.ID) "
            SQL = sqlWhere(SQL) & "(per.flgPrestador = " & pPre & ") "
            If pAct = 1 Then SQL = sqlWhere(SQL) & "(per.virActivo = 1)"
            If pAct = 2 Then SQL = sqlWhere(SQL) & "(per.virActivo = 0)"
            SQL = SQL & " ORDER BY per.Legajo"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetQueryBase = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "RefreshGrid", ex)
        End Try
    End Function
    Public Function GetQueryBaseTelefonos(Optional ByVal pAct As Integer = 1, Optional ByVal pPre As Integer = 0) As DataTable

        GetQueryBaseTelefonos = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT per.ID, per.Legajo, per.Apellido, per.Nombre, per.Telefono01 AS Telefono "
            SQL = SQL & "FROM Personal per "
            SQL = sqlWhere(SQL) & "(per.flgPrestador = " & pPre & ") "
            If pAct = 1 Then SQL = sqlWhere(SQL) & "(per.virActivo = 1)"
            If pAct = 2 Then SQL = sqlWhere(SQL) & "(per.virActivo = 0)"
            SQL = SQL & " ORDER BY per.Legajo"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetQueryBaseTelefonos = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQueryBaseTelefonos", ex)
        End Try
    End Function
    Public Function GetQuerySearch(Optional ByVal pAct As Integer = 1, Optional ByVal pPre As Integer = 0) As DataTable

        GetQuerySearch = Nothing

        Try
            Dim SQL As String

            SQL = "SELECT per.ID, per.Legajo, per.Apellido, per.Nombre, pue.PuestoGrilla AS Puesto "
            SQL = SQL & "FROM Personal per "
            SQL = SQL & "LEFT JOIN DepartamentosPuestos pue ON (per.DepartamentoPuestoId = pue.ID) "
            If pPre <= 1 Then SQL = sqlWhere(SQL) & "(per.flgPrestador = " & pPre & ") "
            If pAct = 1 Then SQL = sqlWhere(SQL) & "(per.virActivo = 1)"
            SQL = SQL & " ORDER BY per.Legajo"

            Dim cmdEqp As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdEqp.ExecuteReader)

            GetQuerySearch = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetQuerySearch", ex)
        End Try
    End Function

    Public Sub SetPactados(ByVal pPer As Int64, ByVal dtHorarios As DataTable)
        Try
            Dim vIdx As Integer, objPactado As typPersonalPactados, objMovil As New conMoviles(Me.myCnnName)

            Dim SQL As String = "DELETE FROM PersonalPactados WHERE PersonalId = " & pPer
            Dim cmDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmDel.ExecuteNonQuery()

            For vIdx = 0 To dtHorarios.Rows.Count - 1

                If dtHorarios(vIdx)("HorEntrada1") <> "" Then
                    objPactado = New typPersonalPactados(Me.myCnnName)
                    objPactado.CleanProperties(objPactado)
                    objPactado.PersonalId.SetObjectId(pPer)
                    objPactado.DiaSemana = vIdx + 1
                    objPactado.IngresoId = getNextIngresoId(pPer, vIdx + 1, dtHorarios(vIdx)("MovilId1"))
                    objPactado.HorEntrada = setTime(dtHorarios(vIdx)("HorEntrada1"))
                    objPactado.HorSalida = setTime(dtHorarios(vIdx)("HorSalida1"))
                    objPactado.MovilId.SetObjectId(dtHorarios(vIdx)("MovilId1"))
                    objPactado.Salvar(objPactado)
                    objPactado = Nothing
                End If
                If dtHorarios(vIdx)("HorEntrada2") <> "" Then
                    objPactado = New typPersonalPactados(Me.myCnnName)
                    objPactado.CleanProperties(objPactado)
                    objPactado.PersonalId.SetObjectId(pPer)
                    objPactado.DiaSemana = vIdx + 1
                    objPactado.IngresoId = getNextIngresoId(pPer, vIdx + 1, dtHorarios(vIdx)("MovilId2"))
                    objPactado.HorEntrada = setTime(dtHorarios(vIdx)("HorEntrada2"))
                    objPactado.HorSalida = setTime(dtHorarios(vIdx)("HorSalida2"))
                    objPactado.MovilId.SetObjectId(dtHorarios(vIdx)("MovilId2"))
                    objPactado.Salvar(objPactado)
                    objPactado = Nothing
                End If
                If dtHorarios(vIdx)("HorEntrada2") <> "" Then
                    objPactado = New typPersonalPactados(Me.myCnnName)
                    objPactado.CleanProperties(objPactado)
                    objPactado.PersonalId.SetObjectId(pPer)
                    objPactado.DiaSemana = vIdx + 1
                    objPactado.IngresoId = getNextIngresoId(pPer, vIdx + 1, dtHorarios(vIdx)("MovilId2"))
                    objPactado.HorEntrada = setTime(dtHorarios(vIdx)("HorEntrada2"))
                    objPactado.HorSalida = setTime(dtHorarios(vIdx)("HorSalida2"))
                    objPactado.MovilId.SetObjectId(dtHorarios(vIdx)("MovilId2"))
                    objPactado.Salvar(objPactado)
                    objPactado = Nothing
                End If
            Next
        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetPactados", ex)
        End Try
    End Sub

    Private Function getNextIngresoId(ByVal pPer As Int64, ByVal pDia As Integer, ByVal pMov As Int64) As Integer
        getNextIngresoId = 1
        Try
            Dim SQL As String
            SQL = "SELECT ISNULL(MAX(IngresoId), 0) + 1 FROM PersonalPactados "
            SQL = SQL & "WHERE (PersonalId = " & pPer & ") AND (DiaSemana = " & pDia & ") AND (MovilId = " & pMov & ")"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getNextIngresoId = CType(vOutVal, Integer)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getNextIngresoId", ex)
        End Try
    End Function

    Public Sub GetPactados(ByVal pPer As Int64, ByRef dtHorarios As DataTable)
        Try
            Dim SQL As String

            SQL = "SELECT pac.DiaSemana, pac.HorEntrada, pac.HorSalida, pac.MovilId, ISNULL(mov.Movil, '') AS Movil "
            SQL = SQL & "FROM PersonalPactados pac "
            SQL = SQL & "LEFT JOIN Moviles mov ON (pac.MovilId = mov.ID) "
            SQL = SQL & "WHERE pac.PersonalId = " & pPer
            SQL = SQL & " ORDER BY pac.DiaSemana, pac.HorEntrada, pac.HorSalida, mov.Movil, pac.IngresoId"

            Dim cmdPac As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            Dim vIdx As Integer
            dt.Load(cmdPac.ExecuteReader)

            For vIdx = 0 To dt.Rows.Count - 1

                Dim dtMatch() As DataRow = dtHorarios.Select("DiaSemana='" & getDayWeek(dt(vIdx)(0) - 1) & "'")

                If dtMatch.Length > 0 Then

                    Dim dtRow As DataRow = dtMatch(0)

                    If dtRow("HorEntrada1") = "" Then
                        dtRow("HorEntrada1") = dt(vIdx)("HorEntrada").ToString.Substring(0, 5)
                        dtRow("HorSalida1") = dt(vIdx)("HorSalida").ToString.Substring(0, 5)
                        dtRow("MovilId1") = dt(vIdx)("MovilId")
                        dtRow("Movil1") = dt(vIdx)("Movil")
                    ElseIf dtRow("HorEntrada2") = "" Then
                        dtRow("HorEntrada2") = dt(vIdx)("HorEntrada").ToString.Substring(0, 5)
                        dtRow("HorSalida2") = dt(vIdx)("HorSalida").ToString.Substring(0, 5)
                        dtRow("MovilId2") = dt(vIdx)("MovilId")
                        dtRow("Movil2") = dt(vIdx)("Movil")
                    Else
                        dtRow("HorEntrada3") = dt(vIdx)("HorEntrada").ToString.Substring(0, 5)
                        dtRow("HorSalida3") = dt(vIdx)("HorSalida").ToString.Substring(0, 5)
                        dtRow("MovilId3") = dt(vIdx)("MovilId")
                        dtRow("Movil3") = dt(vIdx)("Movil")
                    End If

                End If

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetPactados", ex)
        End Try
    End Sub
    Private Function getNextIngresoAtencionId(ByVal pPer As Int64, ByVal pDia As Integer, ByVal pSal As Int64) As Integer
        getNextIngresoAtencionId = 1
        Try
            Dim SQL As String
            SQL = "SELECT ISNULL(MAX(IngresoId), 0) + 1 FROM PersonalHorariosAtencion "
            SQL = SQL & "WHERE (PersonalId = " & pPer & ") AND (DiaSemana = " & pDia & ") AND (CentroAtencionSalaId = " & pSal & ")"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getNextIngresoAtencionId = CType(vOutVal, Integer)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getNextIngresoAtencionId", ex)
        End Try
    End Function
    Private Function setTime(ByVal pVal As String, Optional ByVal pMax As Boolean = False) As String
        If pVal <> "" Then
            If Not pMax Then
                setTime = pVal & ":00"
            Else
                setTime = pVal & ":59"
            End If
        Else
            setTime = ""
        End If
    End Function
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.Legajo = "" Then vRdo = "Debe determinar el número de legajo"
            If Me.Apellido = "" And vRdo = "" Then vRdo = "Debe determinar el apellido"
            If Me.Nombre = "" And vRdo = "" Then vRdo = "Debe determinar el nombre"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function
    Public Function GetIDByLegajo(ByVal pVal As String, Optional ByVal pValAct As Boolean = True) As Int64
        GetIDByLegajo = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT ID FROM Personal WHERE (Legajo = '" & UCase(pVal) & "') "
            If pValAct Then SQL = SQL & " AND (virActivo = 1)"
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByLegajo = CType(vOutVal, Int64)
        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByPersonal", ex)
        End Try
    End Function
    Public Function SetMovil(ByVal pPer As Int64, ByVal pChk As Boolean, ByVal pAbr As String, ByVal pTmv As Int64) As Boolean
        SetMovil = False
        Try
            Dim objMovil As New conMoviles(Me.myCnnName)
            Dim objPersonal As New conPersonal(Me.myCnnName)

            If objPersonal.Abrir(pPer) Then
                If pChk Then
                    If Not objMovil.Abrir(Me.GetMovilId(pPer)) Then
                        objMovil.CleanProperties(objMovil)
                    End If
                    objMovil.Movil = pAbr
                    objMovil.relTabla = 2
                    objMovil.TipoMovilId.SetObjectId(pTmv)
                    objMovil.PersonalId.SetObjectId(pPer)
                    objMovil.Activo = objPersonal.Activo
                    SetMovil = objMovil.Salvar(objMovil)
                Else
                    If objMovil.Abrir(Me.GetMovilId(pPer)) Then
                        objMovil.Activo = 0
                        SetMovil = objMovil.Salvar(objMovil)
                    Else
                        SetMovil = True
                    End If
                End If
            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "SetMovil", ex)
        End Try
    End Function

    Public Function GetMovilId(ByVal pPer As Int64) As Int64
        GetMovilId = 0
        Try
            Dim SQL As String

            SQL = "SELECT ID FROM Moviles WHERE PersonalId = " & pPer
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetMovilId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetMovilId", ex)
        End Try

    End Function

    Public Function getPracticaPrincipal(ByVal pPer As Int64) As Int64
        getPracticaPrincipal = 0
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT TOP 1 PracticaId FROM PersonalHorariosAtencion WHERE PersonalId = " & pPer

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then getPracticaPrincipal = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "getPracticaPrincipal", ex)
        End Try
    End Function

    Public Function havePractica(ByVal pPer As Int64, ByVal pPra As Int64) As Boolean
        havePractica = False
        Try
            Dim SQL As String
            '--------> QUERY
            SQL = "SELECT TOP 1 ID FROM PersonalHorariosAtencion WHERE (PersonalId = " & pPer & ") AND (PracticaId = " & pPra & ")"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            If cmFind.ExecuteScalar > 0 Then havePractica = True

        Catch ex As Exception
            HandleError(Me.GetType.Name, "havePractica", ex)
        End Try
    End Function

End Class
