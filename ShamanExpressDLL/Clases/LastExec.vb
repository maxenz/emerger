Public Class LastExec

    Private clProcedure As String
    Private clErrorNumber As Decimal
    Private clErrorDescription As String
    Private clErrorVarsInfo As String

    Public ReadOnly Property Procedure() As String
        Get
            Return clProcedure
        End Get
    End Property

    Public ReadOnly Property ErrorNumber() As String
        Get
            Return clErrorNumber
        End Get
    End Property

    Public ReadOnly Property ErrorDescription() As String
        Get
            Return clErrorDescription
        End Get
    End Property

    Public ReadOnly Property ErrorVarsInfo() As String
        Get
            Return clErrorVarsInfo
        End Get
    End Property

    Public Sub SetValues(pProcedure As String, Optional pErrDes As String = "", Optional pVarInf As String = "", Optional pErrNro As Decimal = 0)
        clProcedure = pProcedure
        clErrorNumber = pErrNro
        clErrorDescription = pErrDes
        clErrorVarsInfo = pVarInf
    End Sub

    Public Sub New()
        clProcedure = ""
        clErrorNumber = 0
        clErrorDescription = ""
        clErrorVarsInfo = ""
    End Sub

End Class
