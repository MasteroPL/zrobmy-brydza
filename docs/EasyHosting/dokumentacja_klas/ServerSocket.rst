************
ServerSocket
************

.. sphinxsharp:type:: public class ServerSocket
	
	

Konstruktory
============

.. sphinxsharp:method:: public ServerSocket(IPAddress ipAddress=null, Int32 port=33564, Int32 secondsForAuthorization=10)
	:param(1): 
	:param(2): 
	:param(3): 
	
	


Metody
======

.. sphinxsharp:method:: public Boolean get_Initialized()
	
	


.. sphinxsharp:method:: private Void set_Initialized(Boolean value)
	:param(1): 
	
	


.. sphinxsharp:method:: public TcpListener get_TcpListener()
	
	


.. sphinxsharp:method:: private Void set_TcpListener(TcpListener value)
	:param(1): 
	
	


.. sphinxsharp:method:: public IPAddress get_IpAddress()
	
	


.. sphinxsharp:method:: private Void set_IpAddress(IPAddress value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Int32 get_Port()
	
	


.. sphinxsharp:method:: private Void set_Port(Int32 value)
	:param(1): 
	
	


.. sphinxsharp:method:: private Void HandleIncommingConnections()
	
	


.. sphinxsharp:method:: private Void Listen()
	
	


.. sphinxsharp:method:: public Void Start()
	
	


.. sphinxsharp:method:: public Void StartInThread()
	
	


.. sphinxsharp:method:: protected Boolean AuthorizeConnection(ClientConnection conn, JObject requestData)
	:param(1): Połączenie z którego przyszły dane autoryzacyjne
	:param(2): 
	
	Metoda wywoływana po uzyskaniu pierwszego strumienia danych z niezautoryzowanego połączenia. Powinna zwalidować poprawność danych autoryzacyjnych w przychodzącym strumieniu danych i zwrócić "true" jeśli autoryzacja przebiegła pomyslnie lub "false" w przeciwnym przypadku


.. sphinxsharp:method:: protected JObject HandleRequest(ClientConnection conn, JObject requestData)
	:param(1): Połączenie klienta
	:param(2): Dane przychodzące od klienta
	
	Metoda wywoływana po uzyskaniu strumienia danych ze zautoryzowanego połączenia. Strumień danych jest konwertowany do obiektu JObject i przekazywany wraz z połączeniem.


