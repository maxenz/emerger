Imports System.Data.SqlClient

Public Class NETCnns
    Inherits System.Collections.DictionaryBase

    Public Sub Add(ByVal pKey As String, ByVal pCnn As SqlConnection)
        Dictionary.Add(pKey, pCnn)
    End Sub

    Default Public Property Item(ByVal pKey As String) As SqlConnection
        Get
            Return DirectCast(Dictionary.Item(pKey), SqlConnection)
        End Get
        Set(ByVal Value As SqlConnection)
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
        Dictionary.Item(pKey).Close()
        Dictionary.Remove(pKey)
    End Sub

End Class
