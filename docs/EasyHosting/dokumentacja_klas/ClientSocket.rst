************
ClientSocket
************

.. sphinxsharp:type:: public class ClientSocket
	
	

Konstruktory
============

.. sphinxsharp:method:: public ClientSocket(String ipAddress, Int32 port=33564, UInt32 bufferSize=16384)
	:param(1): 
	:param(2): 
	:param(3): 
	
	


Metody
======

.. sphinxsharp:method:: public Boolean get_Initialized()
	
	


.. sphinxsharp:method:: private Void set_Initialized(Boolean value)
	:param(1): 
	
	


.. sphinxsharp:method:: public TcpClient get_TcpClient()
	
	


.. sphinxsharp:method:: private Void set_TcpClient(TcpClient value)
	:param(1): 
	
	


.. sphinxsharp:method:: public UInt32 get_BufferSize()
	
	


.. sphinxsharp:method:: private Void set_BufferSize(UInt32 value)
	:param(1): 
	
	


.. sphinxsharp:method:: public String get_IpAddress()
	
	


.. sphinxsharp:method:: private Void set_IpAddress(String value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Int32 get_Port()
	
	


.. sphinxsharp:method:: private Void set_Port(Int32 value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Boolean get_Authorized()
	
	


.. sphinxsharp:method:: private Void set_Authorized(Boolean value)
	:param(1): 
	
	


.. sphinxsharp:method:: public ConnectionState get_ConnectionState()
	
	


.. sphinxsharp:method:: protected Void set_ConnectionState(ConnectionState value)
	:param(1): 
	
	


.. sphinxsharp:method:: private Void Init()
	
	


.. sphinxsharp:method:: protected BsonDataWriter get_BsonWriter()
	
	


.. sphinxsharp:method:: protected BsonDataReader get_BsonReader()
	
	


.. sphinxsharp:method:: public JObject Send(Object data)
	:param(1): 
	
	


