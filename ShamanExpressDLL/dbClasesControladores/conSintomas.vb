Public Class conSintomas
    Inherits conAllGenerico00
    Public Sub New(Optional ByVal pCnnName As String = "")
        MyBase.New(pCnnName)
    End Sub
    Public Sub Importar()
        Dim fileName As New System.IO.StreamReader("D:\Sintomas.txt")
        Dim vSin() As String
        Dim objSintomaAdd As New conSintomas

        Do Until fileName.EndOfStream
            vSin = fileName.ReadLine.Split("^")
            If vSin(2) = "A" Then
                objSintomaAdd.CleanProperties(objSintomaAdd)
                objSintomaAdd.Descripcion = vSin(1)
                objSintomaAdd.Salvar(objSintomaAdd)
            End If
        Loop
    End Sub

    Public Sub ImportarPreguntas()
        Dim fileName As New System.IO.StreamReader("D:\SintomasPreguntas.txt")
        Dim vSin() As String, vSinId As Decimal
        Dim objSintomasPreguntas As New conSintomasPreguntas

        Do Until fileName.EndOfStream

            vSin = fileName.ReadLine.Split("^")
            vSinId = GetIDByDescripcion(vSin(0))

            If vSinId > 0 Then

                objSintomasPreguntas.CleanProperties(objSintomasPreguntas)
                objSintomasPreguntas.SintomaId.SetObjectId(vSinId)
                If vSin(1) = "A" Then
                    objSintomasPreguntas.flgPediatrico = 0
                Else
                    objSintomasPreguntas.flgPediatrico = 1
                End If
                objSintomasPreguntas.PreguntaId = objSintomasPreguntas.getNextPreguntaId(vSinId, 0)
                Select Case vSin(2)
                    Case "R" : objSintomasPreguntas.TipoFrase = "P"
                    Case "P" : objSintomasPreguntas.TipoFrase = "E"
                    Case "M" : objSintomasPreguntas.TipoFrase = "C"
                End Select
                objSintomasPreguntas.Frase = vSin(3)
                objSintomasPreguntas.Respuesta1 = Val(vSin(4))
                objSintomasPreguntas.Respuesta2 = Val(vSin(5))
                objSintomasPreguntas.Salvar(objSintomasPreguntas)

            End If

        Loop

    End Sub
End Class
