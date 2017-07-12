Imports System.Data
Imports System.Data.SqlClient
Public Class conDiagnosticos
    Inherits typDiagnosticos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Function GetIDByAbreviaturaId(ByVal pVal As String) As Int64

        GetIDByAbreviaturaId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM Diagnosticos WHERE (AbreviaturaId = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByAbreviaturaId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByAbreviaturaId", ex)
        End Try

    End Function

    Public Function GetIDByDescripcion(ByVal pVal As String) As Int64

        GetIDByDescripcion = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM Diagnosticos WHERE (Descripcion = '" & pVal & "')"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByDescripcion = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByDescripcion", ex)
        End Try

    End Function

    Public Function GetAll() As DataTable

        GetAll = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT ID, AbreviaturaId, Descripcion "
            SQL = SQL & "FROM Diagnosticos "
            SQL = SQL & "ORDER BY AbreviaturaId"

            Dim cmdBas As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdBas.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Sub Importar()
        Dim fileName As New System.IO.StreamReader("D:\diags.txt")
        Dim vDig() As String
        Dim objDiagnosticoAdd As New typDiagnosticos

        Do Until fileName.EndOfStream

            vDig = fileName.ReadLine.Split("^")

            objDiagnosticoAdd.CleanProperties(objDiagnosticoAdd)
            objDiagnosticoAdd.AbreviaturaId = vDig(0)
            objDiagnosticoAdd.Descripcion = vDig(1)

            Select Case vDig(2)
                Case 1 : objDiagnosticoAdd.GradoOperativoId.SetObjectId(1)
                Case 2 : objDiagnosticoAdd.GradoOperativoId.SetObjectId(6)
                Case 3 : objDiagnosticoAdd.GradoOperativoId.SetObjectId(9)
                Case 4 : objDiagnosticoAdd.GradoOperativoId.SetObjectId(7)
            End Select

            objDiagnosticoAdd.Salvar(objDiagnosticoAdd)

        Loop
    End Sub
    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            Validar = True
            If Me.AbreviaturaId = "" Then vRdo = "Debe determinar el código identificador del diagnóstico"
            If Me.Descripcion = "" And vRdo = "" Then vRdo = "Debe determinar la descripción del diagnóstico"
            If Me.GradoOperativoId.GetObjectId = 0 And vRdo = "" Then vRdo = "Debe determinar el grado operativo del diagnóstico"
            If vRdo <> "" Then
                Validar = False
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try

    End Function
End Class
