************
ClientSocket
************

.. csharpdocsclass:: EasyHosting.Models.Client.ClientSocket
    :access: public
    :baseclass: System.Object
	
	

Konstruktory
============

.. csharpdocsconstructor:: ClientSocket(System.String ipAddress, System.Int32 port=33564)
    :access: public
    :param(1): 
    :param(2): 
	
	


Metody
======

.. csharpdocsmethod:: System.Void add_RequestSent(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_RequestSent(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void add_RequestResponseReceived(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_RequestResponseReceived(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void add_SignalReceived(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_SignalReceived(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public
    :param(1): 
	
	


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
	
	


.. csharpdocsmethod:: System.Int64 get_CurrentRequestId()
    :access: public
	
	


.. csharpdocsmethod:: System.Void Init()
    :access: private
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Bson.BsonDataWriter get_BsonWriter()
    :access: protected
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Bson.BsonDataReader get_BsonReader()
    :access: protected
	
	


.. csharpdocsmethod:: EasyHosting.Models.Client.Request SendRequest(Newtonsoft.Json.Linq.JObject requestData)
    :access: public
    :param(1): Dane zapytania do wysłania, metoda obudowuje je dodatkowo odpowiednimi meta-danymi zapytania
	
	Tworzy i wysyła zapytanie do serwera. Na odpowiedź na zapytanie należy nasłuchiwać na event'cie RequestResponseReceived


.. csharpdocsmethod:: System.Void WriteData(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): 
	
	Wypisuje dane na strumień wyjściowy do serwera


.. csharpdocsmethod:: System.Void ProcessReceivedData(Newtonsoft.Json.Linq.JObject data)
    :access: protected
    :param(1): Dane do przetworzenia
	
	Przetwarza dane otrzymane od serwera, określa czy są odpowiedzią na zapytanie, czy sygnałem od serwera


.. csharpdocsmethod:: System.Void UpdateCommunication()
    :access: public
	
	


Własności
=========

.. csharpdocsproperty:: System.Boolean Initialized
    :access: public
	
	Określa, czy TcpClient został zainicjalizowany


.. csharpdocsproperty:: System.Net.Sockets.TcpClient TcpClient
    :access: public
	
	


.. csharpdocsproperty:: System.String IpAddress
    :access: public
	
	


.. csharpdocsproperty:: System.Int32 Port
    :access: public
	
	


.. csharpdocsproperty:: System.Boolean Authorized
    :access: public
	
	


.. csharpdocsproperty:: System.Int64 CurrentRequestId
    :access: public
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataWriter BsonWriter
    :access: protected
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataReader BsonReader
    :access: protected
	
	


Pola
====

.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> RequestSent
    :access: private
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> RequestResponseReceived
    :access: private
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> SignalReceived
    :access: private
	
	


.. csharpdocsproperty:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> OnGoingRequests
    :access: protected
	
	


.. csharpdocsproperty:: Newtonsoft.Json.JsonSerializer JsonSerializer
    :access: private
	
	


.. csharpdocsproperty:: System.Boolean _Initialized
    :access: private
	
	


.. csharpdocsproperty:: System.Net.Sockets.TcpClient _TcpClient
    :access: private
	
	


.. csharpdocsproperty:: System.String _IpAddress
    :access: private
	
	


.. csharpdocsproperty:: System.Int32 _Port
    :access: private
	
	


.. csharpdocsproperty:: System.Boolean _Authorized
    :access: private
	
	


.. csharpdocsproperty:: System.Int64 _CurrentRequestId
    :access: protected
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataWriter _BsonWriter
    :access: private
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataReader _BsonReader
    :access: private
	
	


Wydarzenia
==========

.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> RequestSent
    :access: public event
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> RequestResponseReceived
    :access: public event
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> SignalReceived
    :access: public event
	
	


