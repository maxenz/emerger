Imports System.Data.SqlClient

Public Class NETTrans
    Inherits System.Collections.DictionaryBase

    Public Sub Add(ByVal pKey As String, ByVal pCnn As SqlTransaction)
        If Not Dictionary.Contains(pKey) Then
            Dictionary.Add(pKey, pCnn)
        End If
    End Sub

    Default Public Property Item(ByVal pKey As String) As SqlTransaction
        Get
            Return DirectCast(Dictionary.Item(pKey), SqlTransaction)
        End Get
        Set(ByVal Value As SqlTransaction)
            Dictionary.Item(pKey) = Value
        End Set
    End Property

    Public ReadOnly Property Keys() As ICollection
        Get
            Return Dictionary.Keys
        End Get
    End Property

    Public ReadOnly Property Values() As ICollection
        Get
            Return Dictionary.Values
        End Get
    End Property

    Public Function Contains(ByVal pKey As String) As Boolean
        Return Dictionary.Contains(pKey)
    End Function

    Public Sub Remove(ByVal pKey As String)
        Dictionary.Remove(pKey)
    End Sub

End Class
