﻿Imports System.Data
Imports System.Data.SqlClient
Public Class conBancos
    Inherits conAllGenerico01
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
End Class
