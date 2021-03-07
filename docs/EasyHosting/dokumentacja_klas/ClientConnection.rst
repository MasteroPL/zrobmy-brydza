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
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void AddCommunicate(Newtonsoft.Json.Linq.JObject communicate)
    :access: public
    :param(1): Komunikat
	
	Dodaje nowy komunikat do kolejki. Po przetworzeniu zapytań wszystkie komunikaty z kolejki są wysyłane do użytkownika


.. csharpdocsmethod:: System.Void SendCommunicates()
    :access: public
	
	


.. csharpdocsmethod:: System.Void Flush()
    :access: public
	
	


Własności
=========

.. csharpdocsproperty:: EasyHosting.Models.Server.ServerSocket ServerSocket
    :access: public
	
	


.. csharpdocsproperty:: EasyHosting.Models.Server.Session Session
    :access: public
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataWriter BsonWriter
    :access: protected
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Bson.BsonDataReader BsonReader
    :access: protected
	
	


.. csharpdocsproperty:: System.Net.Sockets.TcpClient TcpClient
    :access: public
	
	


.. csharpdocsproperty:: System.Boolean DataAvailable
    :access: public
	
	


Pola
====

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
	
	


.. csharpdocsproperty:: System.Net.Sockets.TcpClient _TcpClient
    :access: private
	
	


Wydarzenia
==========

