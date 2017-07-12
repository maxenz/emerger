Public Class usrHorarioOperativo
    Private clhorLlamada As DateTime
    Private clhorInicial As DateTime
    Private clhorDespacho As DateTime
    Private clhorSalida As DateTime
    Private clhorLlegada As DateTime
    Private clhorSolDerivacion As DateTime
    Private clhorDerivacion As DateTime
    Private clhorInternacion As DateTime
    Private clhorFinalizacion As DateTime
    Public Property horLlamada() As DateTime
        Get
            Return clhorLlamada
        End Get
        Set(ByVal value As DateTime)
            clhorLlamada = value
        End Set
    End Property
    Public Property horInicial() As DateTime
        Get
            Return clhorInicial
        End Get
        Set(ByVal value As DateTime)
            clhorInicial = value
        End Set
    End Property
    Public Property horDespacho() As DateTime
        Get
            Return clhorDespacho
        End Get
        Set(ByVal value As DateTime)
            clhorDespacho = value
        End Set
    End Property
    Public Property horSalida() As DateTime
        Get
            Return clhorSalida
        End Get
        Set(ByVal value As DateTime)
            clhorSalida = value
        End Set
    End Property
    Public Property horLlegada() As DateTime
        Get
            Return clhorLlegada
        End Get
        Set(ByVal value As DateTime)
            clhorLlegada = value
        End Set
    End Property
    Public Property horSolDerivacion() As DateTime
        Get
            Return clhorSolDerivacion
        End Get
        Set(ByVal value As DateTime)
            clhorSolDerivacion = value
        End Set
    End Property
    Public Property horDerivacion() As DateTime
        Get
            Return clhorDerivacion
        End Get
        Set(ByVal value As DateTime)
            clhorDerivacion = value
        End Set
    End Property
    Public Property horInternacion() As DateTime
        Get
            Return clhorInternacion
        End Get
        Set(ByVal value As DateTime)
            clhorInternacion = value
        End Set
    End Property
    Public Property horFinalizacion() As DateTime
        Get
            Return clhorFinalizacion
        End Get
        Set(ByVal value As DateTime)
            clhorFinalizacion = value
        End Set
    End Property
    Public Sub CleanProperties(ByVal pObj As usrHorarioOperativo)
        pObj.horLlamada = NullDateMax
        pObj.horInicial = NullDateMax
        pObj.horDespacho = NullDateMax
        pObj.horSalida = NullDateMax
        pObj.horLlegada = NullDateMax
        pObj.horSolDerivacion = NullDateMax
        pObj.horDerivacion = NullDateMax
        pObj.horInternacion = NullDateMax
        pObj.horFinalizacion = NullDateMax
    End Sub

End Class
