************
ServerSocket
************

.. csharpdocsclass:: EasyHosting.Models.Server.ServerSocket
    :access: public
    :baseclass: System.Object
	
	

Konstruktory
============

.. csharpdocsconstructor:: ServerSocket(System.Net.IPAddress ipAddress=null, System.Int32 port=33564, System.Int32 secondsForAuthorization=10)
    :access: public
    :param(1): 
    :param(2): 
    :param(3): 
	
	


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
	
	


.. csharpdocsmethod:: System.Void Start()
    :access: public
	
	


.. csharpdocsmethod:: System.Void StartInThread()
    :access: public
	
	


.. csharpdocsmethod:: System.Boolean AuthorizeConnection(EasyHosting.Models.Server.ClientConnection conn, Newtonsoft.Json.Linq.JObject requestData)
    :access: protected
    :param(1): Połączenie z którego przyszły dane autoryzacyjne
    :param(2): 
	
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
	
	


.. csharpdocsproperty:: System.Int32 Port
    :access: public
	
	


Pola
====

.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> UnauthorizedConnections
    :access: private
	
	


.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> AuthorizedConnections
    :access: private
	
	


.. csharpdocsproperty:: System.TimeSpan TimeForAuthorization
    :access: protected
	
	


.. csharpdocsproperty:: System.Boolean _Initialized
    :access: private
	
	


.. csharpdocsproperty:: System.Net.Sockets.TcpListener _TcpListener
    :access: private
	
	


.. csharpdocsproperty:: System.Net.IPAddress _IpAddress
    :access: private
	
	


.. csharpdocsproperty:: System.Int32 _Port
    :access: private
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Linq.JObject AuthorizationSuccessfulResponse
    :access: protected
	
	


Wydarzenia
==========

