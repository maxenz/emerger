Public Module modGPShaman

    Public Sub SetLatLong(ByRef pDom As usrDomicilio, ByVal pLoc As Decimal, Optional pCnnName As String = cnnDefault)
        Try

            If shamanConfig.flgServicioMapas = 1 Then

                Dim objLocalidad As New conLocalidades(pCnnName)
                Dim vDom As String = pDom.Domicilio

                If objLocalidad.Abrir(pLoc) Then
                    If pDom.dmAltura = 0 Then
                        If pDom.dmEntreCalle1 <> "" Then vDom = vDom & ", entre " & pDom.dmEntreCalle1
                        If pDom.dmEntreCalle2 <> "" Then vDom = vDom & ", y " & pDom.dmEntreCalle2
                    End If
                    If objLocalidad.Descripcion <> objLocalidad.PartidoId.Descripcion And objLocalidad.PartidoId.ID = 0 Then
                        vDom = vDom & ", " & objLocalidad.Descripcion
                    End If

                    vDom = vDom & ", " & objLocalidad.PartidoId.Descripcion
                    vDom = vDom & ", " & objLocalidad.ProvinciaId.Descripcion
                    vDom = vDom & ", " & objLocalidad.ProvinciaId.PaisId.Descripcion

                    objLocalidad = Nothing

                    Dim objSoap As New GPShamanWS.ServiceSoapClient("ServiceSoap")
                    Dim vLatLon As String = objSoap.GetLatLong(vDom)

                    If vLatLon <> "" Then
                        Dim vDev() As String
                        vDev = vLatLon.Split("/")
                        pDom.dmLatitud = GetDouble(vDev(0).Replace(".", wSepDecimal))
                        pDom.dmLongitud = GetDouble(vDev(1).Replace(".", wSepDecimal))
                    End If

                    objSoap = Nothing

                End If
            End If

        Catch ex As Exception
            HandleError("modGPShaman", "SetLatLong", ex)
        End Try
    End Sub


End Module
