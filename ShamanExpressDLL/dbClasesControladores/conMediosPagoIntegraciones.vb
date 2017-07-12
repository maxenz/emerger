Imports System.Data
Imports System.Data.SqlClient
Public Class conMediosPagoIntegraciones
    Inherits typMediosPagoIntegraciones
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub

    Public Function GetIDByIntegracionId(ByVal pIte As iteMediosPago) As Int64
        GetIDByIntegracionId = 0
        Try
            Dim SQL As String
            SQL = "SELECT ID FROM MediosPagoIntegraciones WHERE IntegracionId = " & pIte
            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetIDByIntegracionId = CType(vOutVal, Int64)

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetIDByIntegracionId", ex)
        End Try

    End Function

    Public Function GetAll() As DataTable
        GetAll = Nothing
        Try
            Dim SQL As String

            SQL = "SELECT med.ID, med.Descripcion, fpg.AbreviaturaId AS FormaPago, tar.Descripcion AS Tarjeta, bco.Descripcion AS Banco "
            SQL = SQL & "FROM MediosPagoIntegraciones med "
            SQL = SQL & "LEFT JOIN FormasPago fpg ON med.FormaPagoId = fpg.ID "
            SQL = SQL & "LEFT JOIN TarjetasCredito tar ON med.TarjetaCreditoId = tar.ID "
            SQL = SQL & "LEFT JOIN Bancos bco ON med.BancoId = bco.ID "
            SQL = SQL & "ORDER BY med.Descripcion"

            Dim cmdMed As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdMed.ExecuteReader)

            GetAll = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAll", ex)
        End Try
    End Function

    Public Function GetAtributosDT(pMpg As Int64) As DataTable

        GetAtributosDT = Nothing

        Try

            Dim SQL As String

            SQL = "SELECT Atributo, Valor FROM MediosPagoIntegracionesAtr WHERE MedioPagoIntegracionId = " & pMpg

            Dim cmdMed As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim dt As New DataTable
            dt.Load(cmdMed.ExecuteReader)

            GetAtributosDT = dt

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAtributosDT", ex)
        End Try

    End Function

    Public Function Validar(Optional ByVal pMsg As Boolean = True) As Boolean
        Validar = False
        Try
            Dim vRdo As String = ""
            If Me.Descripcion = "" Then vRdo = "Debe determinar la descripción de la integración"
            If vRdo <> "" Then
                If pMsg Then MsgBox(vRdo, MsgBoxStyle.Critical, Me.Tabla)
            Else
                Validar = True
            End If
        Catch ex As Exception
            HandleError(Me.GetType.Name, "Validar", ex)
        End Try
    End Function

    Public Sub SetAtributos(pMpg As Int64, dt As DataTable)

        Try

            Dim SQL As String

            SQL = "DELETE FROM MediosPagoIntegracionesAtr WHERE MedioPagoIntegracionId = " & pMpg
            Dim cmdDel As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            cmdDel.ExecuteNonQuery()

            Dim vIdx As Integer

            For vIdx = 0 To dt.Rows.Count - 1

                Dim objAtributo As New typMediosPagoIntegracionesAtr(Me.myCnnName)

                objAtributo.CleanProperties(objAtributo)

                objAtributo.MedioPagoIntegracionId.SetObjectId(pMpg)
                objAtributo.Atributo = dt(vIdx)("Atributo")
                objAtributo.Valor = dt(vIdx)("Valor")

                objAtributo.Salvar(objAtributo)

            Next vIdx

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAtributosDT", ex)
        End Try


    End Sub

    Public Function GetAtributo(pIte As Int64, pAtr As String, pDft As String) As String

        GetAtributo = pDft

        Try

            Dim SQL As String

            SQL = "SELECT Valor FROM MediosPagoIntegracionesAtr WHERE MedioPagoIntegracionId = " & pIte & " AND Atributo = '" & pAtr & "'"

            Dim cmFind As New SqlCommand(SQL, cnnsNET(Me.myCnnName), cnnsTransNET(Me.myCnnName))
            Dim vOutVal As String = CType(cmFind.ExecuteScalar, String)
            If Not vOutVal Is Nothing Then GetAtributo = vOutVal

        Catch ex As Exception
            HandleError(Me.GetType.Name, "GetAtributo", ex)
        End Try

    End Function

    Public Function prnDouble(ByVal pVal As Double, Optional pInt As Integer = 12, Optional pDec As Integer = 2) As String

        prnDouble = ""

        Try
            Dim vNum() As String = pVal.ToString.Split(wSepDecimal)

            Dim vZerInt As String = ""
            Dim vZerDec As String = ""
            Dim vIdx As Integer

            For vIdx = 1 To pInt
                vZerInt = vZerInt & "0"
            Next
            For vIdx = 1 To pDec
                vZerDec = vZerDec & "0"
            Next

            If UBound(vNum) = 1 Then

                If vNum(1).Length < pDec Then
                    Dim vDecPar As String = Val(vNum(1)).ToString

                    If vDecPar.Length < pDec Then
                        For vIdx = vDecPar.Length + 1 To pDec
                            vDecPar = vDecPar & "0"
                        Next vIdx
                    End If

                    prnDouble = Format(Val(vNum(0)), vZerInt) & vDecPar

                Else

                    prnDouble = Format(Val(vNum(0)), vZerInt) & vNum(1).Substring(0, pDec)

                End If

            Else

                prnDouble = Format(Val(vNum(0)), vZerInt) & vZerDec

            End If

        Catch ex As Exception
            HandleError(Me.GetType.Name, "prnDouble", ex)
        End Try

    End Function



End Class
