****************
ClientConnection
****************

.. sphinxsharp:type:: public class ClientConnection
	
	

Konstruktory
============

.. sphinxsharp:method:: public ClientConnection(TcpClient tcpClient, ServerSocket serverSocket=null)
	:param(1): 
	:param(2): 
	
	


Metody
======

.. sphinxsharp:method:: public ServerSocket get_ServerSocket()
	
	


.. sphinxsharp:method:: private Void set_ServerSocket(ServerSocket value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Session get_Session()
	
	


.. sphinxsharp:method:: protected Void set_Session(Session value)
	:param(1): 
	
	


.. sphinxsharp:method:: protected BsonDataWriter get_BsonWriter()
	
	


.. sphinxsharp:method:: protected BsonDataReader get_BsonReader()
	
	


.. sphinxsharp:method:: public TimeSpan GetConnectionTime()
	
	


.. sphinxsharp:method:: public TcpClient get_TcpClient()
	
	


.. sphinxsharp:method:: protected Void set_TcpClient(TcpClient value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Boolean get_DataAvailable()
	
	


.. sphinxsharp:method:: public JObject GetData()
	
	


.. sphinxsharp:method:: public Void WriteData(JObject data)
	:param(1): 
	
	


.. sphinxsharp:method:: public Void AddCommunicate(JObject communicate)
	:param(1): Komunikat
	
	Dodaje nowy komunikat do kolejki. Po przetworzeniu zapytań wszystkie komunikaty z kolejki są wysyłane do użytkownika


.. sphinxsharp:method:: public Void SendCommunicates()
	
	


.. sphinxsharp:method:: public Void Flush()
	
	


