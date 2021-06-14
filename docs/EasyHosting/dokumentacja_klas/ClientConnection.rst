****************
ClientConnection
****************

.. csharpdocsclass:: EasyHosting.Models.Server.ClientConnection
    :access: public
    :baseclass: System.Object
	
	

Konstruktory
============

.. csharpdocsconstructor:: ClientConnection(System.Net.Sockets.TcpClient tcpClient, EasyHosting.Models.Server.ServerSocket serverSocket=null)
    :access: public
    :param(1): 
    :param(2): 
	
	


Metody
======

.. csharpdocsmethod:: System.Void add_BeforeDispose(System.EventHandler value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_BeforeDispose(System.EventHandler value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Boolean get_Disposed()
    :access: public
	
	


.. csharpdocsmethod:: EasyHosting.Models.Server.ServerSocket get_ServerSocket()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_ServerSocket(EasyHosting.Models.Server.ServerSocket value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: EasyHosting.Models.Server.Session get_Session()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_Session(EasyHosting.Models.Server.Session value)
    :access: protected
    :param(1): 
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Bson.BsonDataWriter get_BsonWriter()
    :access: protected
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Bson.BsonDataReader get_BsonReader()
    :access: protected
	
	


.. csharpdocsmethod:: System.TimeSpan GetConnectionTime()
    :access: public
	
	


.. csharpdocsmethod:: System.Net.Sockets.TcpClient get_TcpClient()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_TcpClient(System.Net.Sockets.TcpClient value)
    :access: protected
    :param(1): 
	
	


.. csharpdocsmethod:: System.Boolean get_DataAvailable()
    :access: public
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject GetData()
    :access: public
	
	


.. csharpdocsmethod:: System.Void WriteData(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): Dane do wpisania
	
	Wpisuje dane do strumienia komunikacji z klientem


.. csharpdocsmethod:: System.Void AddCommunicate(Newtonsoft.Json.Linq.JObject communicate)
    :access: public
    :param(1): Komunikat
	
	Dodaje nowy komunikat do kolejki. Po przetworzeniu zapytań wszystkie komunikaty z kolejki są wysyłane do użytkownika


.. csharpdocsmethod:: System.Void SendCommunicates()
    :access: public
	
	


.. csharpdocsmethod:: System.Void Flush()
    :access: public
	
	


.. csharpdocsmethod:: System.Void Dispose()
    :access: public
	
	


Własności
=========

.. csharpdocsproperty:: System.Boolean Disposed
    :access: public
	
	


.. csharpdocsproperty:: EasyHosting.Models.Server.ServerSocket ServerSocket
    :access: public
	
	ServerSocket z którym klient jest połączony


.. csharpdocsproperty:: EasyHosting.Models.Server.Session Session
    :access: public
	
	Sesja przypisana do klienta


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataWriter BsonWriter
    :access: protected
	
	Prefedefiniowany BsonWriter do serializacji binarnej komunikacji z klientem


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataReader BsonReader
    :access: protected
	
	Predefiniowany BsonReader do deserializacji binarnej komunikacji z klientem


.. csharpdocsproperty:: System.Net.Sockets.TcpClient TcpClient
    :access: public
	
	Fizyczne połączenie klienta


.. csharpdocsproperty:: System.Boolean DataAvailable
    :access: public
	
	Określa czy klient nadał jakieś dane


Pola
====

.. csharpdocsproperty:: System.EventHandler BeforeDispose
    :access: private
	
	


.. csharpdocsproperty:: System.Boolean _Disposed
    :access: private
	
	


.. csharpdocsproperty:: System.Collections.Generic.LinkedList<Newtonsoft.Json.Linq.JObject> CommunicatesQueue
    :access: private
	
	Komunikaty typu "PUSH", czyli wysałane z serwera do użytkownika. Nie są to odpowiedzi do zapytań


.. csharpdocsproperty:: EasyHosting.Models.Server.ServerSocket _ServerSocket
    :access: private
	
	


.. csharpdocsproperty:: EasyHosting.Models.Server.Session _Session
    :access: private
	
	


.. csharpdocsproperty:: Newtonsoft.Json.JsonSerializer JsonSerializer
    :access: private
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataWriter _BsonWriter
    :access: private
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataReader _BsonReader
    :access: private
	
	


.. csharpdocsproperty:: System.DateTime ConnectedAt
    :access: private
	
	


.. csharpdocsproperty:: System.DateTime LastActivateAt
    :access: public
	
	


.. csharpdocsproperty:: System.Net.Sockets.TcpClient _TcpClient
    :access: private
	
	


Wydarzenia
==========

.. csharpdocsproperty:: System.EventHandler BeforeDispose
    :access: public event
	
	


