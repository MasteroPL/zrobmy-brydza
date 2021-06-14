************
ServerSocket
************

.. csharpdocsclass:: EasyHosting.Models.Server.ServerSocket
    :access: public
    :baseclass: System.Object
	
	Klasa definiująca podstawowe funkcjonalności gniazda serwera, tj. przechwytywanie połączeń, rozpoznawanie połączeń zautoryzowanych, wymaganie autoryzacji. Wymaga nadpisania AuthorizeConnection oraz HandleRequest

Konstruktory
============

.. csharpdocsconstructor:: ServerSocket(System.Net.IPAddress ipAddress=null, System.Int32 port=33564, System.Int32 secondsForAuthorization=10, System.Int32 secondsAllowedIdle=10)
    :access: public
    :param(1): 
    :param(2): 
    :param(3): 
    :param(4): 
	
	


Metody
======

.. csharpdocsmethod:: System.Boolean get_Initialized()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_Initialized(System.Boolean value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.Net.Sockets.TcpListener get_TcpListener()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_TcpListener(System.Net.Sockets.TcpListener value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.Net.IPAddress get_IpAddress()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_IpAddress(System.Net.IPAddress value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.Int32 get_Port()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_Port(System.Int32 value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void HandleIncommingConnections()
    :access: private
	
	


.. csharpdocsmethod:: System.Void Listen()
    :access: private
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject GetAuthorizationResponseSuccessful()
    :access: protected
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject GetAuthorizationResponseFailed()
    :access: protected
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject GetDisconnectedSignal()
    :access: protected
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject GetAuthorizationTimeoutSignal()
    :access: protected
	
	


.. csharpdocsmethod:: System.Void Start()
    :access: public
	
	


.. csharpdocsmethod:: System.Void StartInThread()
    :access: public
	
	


.. csharpdocsmethod:: System.Boolean DisconnectClient(EasyHosting.Models.Server.ClientConnection clientToDisconnect)
    :access: public
    :param(1): Klient do odłączenia
	
	Odłącza klienta od serwera


.. csharpdocsmethod:: System.Boolean ClientConnected(EasyHosting.Models.Server.ClientConnection client, System.Boolean searchDependingOnStatus=False, EasyHosting.Models.Server.ConnectionStatus connectionStatus=ANY)
    :access: public
    :param(1): Klient do sprawdzenia
    :param(2): Jeśli true, użyty zostanie dodatkowy filtr, sprwadzający tylko klientów zautoryzowanych lub tylko niezautoryzacowanych
    :param(3): Jeśli searchDependingOnStatus = true, po jakim statusie powinniśmy wyszukiwać połączenia
	
	Sprawdza, czy klient jest połączony z serwerem


.. csharpdocsmethod:: System.Boolean AuthorizeConnection(EasyHosting.Models.Server.ClientConnection conn, Newtonsoft.Json.Linq.JObject requestData)
    :access: protected
    :param(1): Połączenie z którego przyszły dane autoryzacyjne
    :param(2): Dane przychodzące od klienta
	
	Metoda wywoływana po uzyskaniu pierwszego strumienia danych z 
	niezautoryzowanego połączenia. Powinna zwalidować poprawność 
	danych autoryzacyjnych w przychodzącym strumieniu danych
	i zwrócić "true" jeśli autoryzacja przebiegła pomyslnie lub
	"false" w przeciwnym przypadku


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject HandleRequest(EasyHosting.Models.Server.ClientConnection conn, Newtonsoft.Json.Linq.JObject requestData)
    :access: protected
    :param(1): Połączenie klienta
    :param(2): Dane przychodzące od klienta
	
	Metoda wywoływana po uzyskaniu strumienia danych ze 
	zautoryzowanego połączenia. Strumień danych jest konwertowany
	do obiektu JObject i przekazywany wraz z połączeniem.


Własności
=========

.. csharpdocsproperty:: System.Boolean Initialized
    :access: public
	
	Określa, czy TcpListener został zainicjalizowany i nasłuchuje połączeń


.. csharpdocsproperty:: System.Net.Sockets.TcpListener TcpListener
    :access: public
	
	Zainicjalizowany TcpListener


.. csharpdocsproperty:: System.Net.IPAddress IpAddress
    :access: public
	
	Określa na jakim adresie IP nasłuchuje gniazdo


.. csharpdocsproperty:: System.Int32 Port
    :access: public
	
	Określa port na którym nasłuchuje gniazdo


Pola
====

.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> UnauthorizedConnections
    :access: private
	
	


.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> AuthorizedConnections
    :access: private
	
	


.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> ClientsToDisconnect
    :access: private
	
	


.. csharpdocsproperty:: System.TimeSpan TimeForAuthorization
    :access: protected
	
	Określa po jakim czasie bez poprawnej autoryzacji połączenie z klientem zostanie zamknięte przez gniazdo


.. csharpdocsproperty:: System.TimeSpan MaxIdleTime
    :access: protected
	
	


.. csharpdocsproperty:: System.Boolean _Initialized
    :access: private
	
	


.. csharpdocsproperty:: System.Net.Sockets.TcpListener _TcpListener
    :access: private
	
	


.. csharpdocsproperty:: System.Net.IPAddress _IpAddress
    :access: private
	
	


.. csharpdocsproperty:: System.Int32 _Port
    :access: private
	
	


Wydarzenia
==========

