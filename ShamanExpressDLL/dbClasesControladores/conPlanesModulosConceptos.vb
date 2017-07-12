Imports System.Data
Imports System.Data.SqlClient
Public Class conPlanesModulosConceptos
    Inherits typPlanesModulosConceptos
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
End Class
