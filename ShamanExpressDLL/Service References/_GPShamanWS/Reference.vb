﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.18063
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace GPShamanWS
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="GPShamanWS.ServiceSoap")>  _
    Public Interface ServiceSoap
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/GetDistanciaTiempo", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function GetDistanciaTiempo(ByVal latMov As String, ByVal lngMov As String, ByVal latDst As String, ByVal lngDst As String) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/GetDistanciaTiempo", ReplyAction:="*")>  _
        Function GetDistanciaTiempoAsync(ByVal latMov As String, ByVal lngMov As String, ByVal latDst As String, ByVal lngDst As String) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/GetDireccion", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function GetDireccion(ByVal lat As String, ByVal lng As String) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/GetDireccion", ReplyAction:="*")>  _
        Function GetDireccionAsync(ByVal lat As String, ByVal lng As String) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/GetLatLong", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function GetLatLong(ByVal direccion As String) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/GetLatLong", ReplyAction:="*")>  _
        Function GetLatLongAsync(ByVal direccion As String) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/GetPuntosEnPoligono", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function GetPuntosEnPoligono(ByVal pLat As Single, ByVal pLon As Single, ByVal pTip As String) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/GetPuntosEnPoligono", ReplyAction:="*")>  _
        Function GetPuntosEnPoligonoAsync(ByVal pLat As Single, ByVal pLon As Single, ByVal pTip As String) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/getIncidente", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function getIncidente(ByVal cod As String, ByVal fec As String) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/getIncidente", ReplyAction:="*")>  _
        Function getIncidenteAsync(ByVal cod As String, ByVal fec As String) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/getSerialSetLog", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function getSerialSetLog(ByVal serialNumber As String) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/getSerialSetLog", ReplyAction:="*")>  _
        Function getSerialSetLogAsync(ByVal serialNumber As String) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/getSerialSetLogLast", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function getSerialSetLogLast(ByVal serialNumber As String, ByVal pRemote As Integer) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/getSerialSetLogLast", ReplyAction:="*")>  _
        Function getSerialSetLogLastAsync(ByVal serialNumber As String, ByVal pRemote As Integer) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/isInGestion", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function isInGestion(ByVal user As String, ByVal pass As String, ByVal llave As String) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/isInGestion", ReplyAction:="*")>  _
        Function isInGestionAsync(ByVal user As String, ByVal pass As String, ByVal llave As String) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/setPushNotification", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function setPushNotification(ByVal nroMovil As String, ByVal mensaje As String, ByVal appID As String, ByVal restApiKey As String) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/setPushNotification", ReplyAction:="*")>  _
        Function setPushNotificationAsync(ByVal nroMovil As String, ByVal mensaje As String, ByVal appID As String, ByVal restApiKey As String) As System.Threading.Tasks.Task(Of Boolean)
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface ServiceSoapChannel
        Inherits GPShamanWS.ServiceSoap, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class ServiceSoapClient
        Inherits System.ServiceModel.ClientBase(Of GPShamanWS.ServiceSoap)
        Implements GPShamanWS.ServiceSoap
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Function GetDistanciaTiempo(ByVal latMov As String, ByVal lngMov As String, ByVal latDst As String, ByVal lngDst As String) As String Implements GPShamanWS.ServiceSoap.GetDistanciaTiempo
            Return MyBase.Channel.GetDistanciaTiempo(latMov, lngMov, latDst, lngDst)
        End Function
        
        Public Function GetDistanciaTiempoAsync(ByVal latMov As String, ByVal lngMov As String, ByVal latDst As String, ByVal lngDst As String) As System.Threading.Tasks.Task(Of String) Implements GPShamanWS.ServiceSoap.GetDistanciaTiempoAsync
            Return MyBase.Channel.GetDistanciaTiempoAsync(latMov, lngMov, latDst, lngDst)
        End Function
        
        Public Function GetDireccion(ByVal lat As String, ByVal lng As String) As String Implements GPShamanWS.ServiceSoap.GetDireccion
            Return MyBase.Channel.GetDireccion(lat, lng)
        End Function
        
        Public Function GetDireccionAsync(ByVal lat As String, ByVal lng As String) As System.Threading.Tasks.Task(Of String) Implements GPShamanWS.ServiceSoap.GetDireccionAsync
            Return MyBase.Channel.GetDireccionAsync(lat, lng)
        End Function
        
        Public Function GetLatLong(ByVal direccion As String) As String Implements GPShamanWS.ServiceSoap.GetLatLong
            Return MyBase.Channel.GetLatLong(direccion)
        End Function
        
        Public Function GetLatLongAsync(ByVal direccion As String) As System.Threading.Tasks.Task(Of String) Implements GPShamanWS.ServiceSoap.GetLatLongAsync
            Return MyBase.Channel.GetLatLongAsync(direccion)
        End Function
        
        Public Function GetPuntosEnPoligono(ByVal pLat As Single, ByVal pLon As Single, ByVal pTip As String) As String Implements GPShamanWS.ServiceSoap.GetPuntosEnPoligono
            Return MyBase.Channel.GetPuntosEnPoligono(pLat, pLon, pTip)
        End Function
        
        Public Function GetPuntosEnPoligonoAsync(ByVal pLat As Single, ByVal pLon As Single, ByVal pTip As String) As System.Threading.Tasks.Task(Of String) Implements GPShamanWS.ServiceSoap.GetPuntosEnPoligonoAsync
            Return MyBase.Channel.GetPuntosEnPoligonoAsync(pLat, pLon, pTip)
        End Function
        
        Public Function getIncidente(ByVal cod As String, ByVal fec As String) As String Implements GPShamanWS.ServiceSoap.getIncidente
            Return MyBase.Channel.getIncidente(cod, fec)
        End Function
        
        Public Function getIncidenteAsync(ByVal cod As String, ByVal fec As String) As System.Threading.Tasks.Task(Of String) Implements GPShamanWS.ServiceSoap.getIncidenteAsync
            Return MyBase.Channel.getIncidenteAsync(cod, fec)
        End Function
        
        Public Function getSerialSetLog(ByVal serialNumber As String) As String Implements GPShamanWS.ServiceSoap.getSerialSetLog
            Return MyBase.Channel.getSerialSetLog(serialNumber)
        End Function
        
        Public Function getSerialSetLogAsync(ByVal serialNumber As String) As System.Threading.Tasks.Task(Of String) Implements GPShamanWS.ServiceSoap.getSerialSetLogAsync
            Return MyBase.Channel.getSerialSetLogAsync(serialNumber)
        End Function
        
        Public Function getSerialSetLogLast(ByVal serialNumber As String, ByVal pRemote As Integer) As String Implements GPShamanWS.ServiceSoap.getSerialSetLogLast
            Return MyBase.Channel.getSerialSetLogLast(serialNumber, pRemote)
        End Function
        
        Public Function getSerialSetLogLastAsync(ByVal serialNumber As String, ByVal pRemote As Integer) As System.Threading.Tasks.Task(Of String) Implements GPShamanWS.ServiceSoap.getSerialSetLogLastAsync
            Return MyBase.Channel.getSerialSetLogLastAsync(serialNumber, pRemote)
        End Function
        
        Public Function isInGestion(ByVal user As String, ByVal pass As String, ByVal llave As String) As String Implements GPShamanWS.ServiceSoap.isInGestion
            Return MyBase.Channel.isInGestion(user, pass, llave)
        End Function
        
        Public Function isInGestionAsync(ByVal user As String, ByVal pass As String, ByVal llave As String) As System.Threading.Tasks.Task(Of String) Implements GPShamanWS.ServiceSoap.isInGestionAsync
            Return MyBase.Channel.isInGestionAsync(user, pass, llave)
        End Function
        
        Public Function setPushNotification(ByVal nroMovil As String, ByVal mensaje As String, ByVal appID As String, ByVal restApiKey As String) As Boolean Implements GPShamanWS.ServiceSoap.setPushNotification
            Return MyBase.Channel.setPushNotification(nroMovil, mensaje, appID, restApiKey)
        End Function
        
        Public Function setPushNotificationAsync(ByVal nroMovil As String, ByVal mensaje As String, ByVal appID As String, ByVal restApiKey As String) As System.Threading.Tasks.Task(Of Boolean) Implements GPShamanWS.ServiceSoap.setPushNotificationAsync
            Return MyBase.Channel.setPushNotificationAsync(nroMovil, mensaje, appID, restApiKey)
        End Function
    End Class
End Namespace
