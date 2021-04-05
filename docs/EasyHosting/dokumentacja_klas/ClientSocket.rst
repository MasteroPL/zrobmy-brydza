************
ClientSocket
************

.. csharpdocsclass:: EasyHosting.Models.Client.ClientSocket
    :access: public
    :baseclass: System.Object
	
	

Konstruktory
============

.. csharpdocsconstructor:: ClientSocket(System.String ipAddress, System.Int32 port=33564, System.UInt32 bufferSize=16384)
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
	
	


.. csharpdocsmethod:: System.Net.Sockets.TcpClient get_TcpClient()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_TcpClient(System.Net.Sockets.TcpClient value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.UInt32 get_BufferSize()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_BufferSize(System.UInt32 value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.String get_IpAddress()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_IpAddress(System.String value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.Int32 get_Port()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_Port(System.Int32 value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.Boolean get_Authorized()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_Authorized(System.Boolean value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: EasyHosting.Models.ConnectionState get_ConnectionState()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_ConnectionState(EasyHosting.Models.ConnectionState value)
    :access: protected
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void Init()
    :access: private
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Bson.BsonDataWriter get_BsonWriter()
    :access: protected
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Bson.BsonDataReader get_BsonReader()
    :access: protected
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject Send(System.Object data)
    :access: public
    :param(1): 
	
	


Własności
=========

.. csharpdocsproperty:: System.Boolean Initialized
    :access: public
	
	Określa, czy TcpClient został zainicjalizowany


.. csharpdocsproperty:: System.Net.Sockets.TcpClient TcpClient
    :access: public
	
	


.. csharpdocsproperty:: System.UInt32 BufferSize
    :access: public
	
	Zdefiniowany rozmiar bufora nadającego i odbierającego


.. csharpdocsproperty:: System.String IpAddress
    :access: public
	
	


.. csharpdocsproperty:: System.Int32 Port
    :access: public
	
	


.. csharpdocsproperty:: System.Boolean Authorized
    :access: public
	
	


.. csharpdocsproperty:: EasyHosting.Models.ConnectionState ConnectionState
    :access: public
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataWriter BsonWriter
    :access: protected
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataReader BsonReader
    :access: protected
	
	


Pola
====

.. csharpdocsproperty:: Newtonsoft.Json.JsonSerializer JsonSerializer
    :access: private
	
	


.. csharpdocsproperty:: System.Boolean _Initialized
    :access: private
	
	


.. csharpdocsproperty:: System.Net.Sockets.TcpClient _TcpClient
    :access: private
	
	


.. csharpdocsproperty:: System.UInt32 _BufferSize
    :access: private
	
	


.. csharpdocsproperty:: System.String _IpAddress
    :access: private
	
	


.. csharpdocsproperty:: System.Int32 _Port
    :access: private
	
	


.. csharpdocsproperty:: System.Boolean _Authorized
    :access: private
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataWriter _BsonWriter
    :access: private
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataReader _BsonReader
    :access: private
	
	


Wydarzenia
==========

