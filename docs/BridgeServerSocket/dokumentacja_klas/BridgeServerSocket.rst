******************
BridgeServerSocket
******************

.. csharpdocsclass:: ServerSocket.Models.BridgeServerSocket
    :access: public
    :baseclass: EasyHosting.Models.Server.ServerSocket
	
	

Konstruktory
============

.. csharpdocsconstructor:: BridgeServerSocket()
    :access: public
	
	


Metody
======

.. csharpdocsmethod:: System.Boolean AuthorizeConnection(EasyHosting.Models.Server.ClientConnection conn, Newtonsoft.Json.Linq.JObject requestData)
    :access: protected
    :param(1): Połączenie klienta
    :param(2): Dane zapytania (dane autoryzacyjne)
	
	Metoda autoryzująca połączenie klienckie. Po poprawnej autoryzacji, łączy z odpowiednim Lobby


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject HandleRequest(EasyHosting.Models.Server.ClientConnection conn, Newtonsoft.Json.Linq.JObject requestData)
    :access: protected
    :param(1): Połączenie klienta
    :param(2): Dane przychodzące od klienta
	
	Obsługa zapytań klienta


Własności
=========

.. csharpdocsproperty:: System.Boolean Initialized
    :access: public
	
	


.. csharpdocsproperty:: System.Net.Sockets.TcpListener TcpListener
    :access: public
	
	


.. csharpdocsproperty:: System.Net.IPAddress IpAddress
    :access: public
	
	


.. csharpdocsproperty:: System.Int32 Port
    :access: public
	
	


Pola
====

.. csharpdocsproperty:: ServerSocket.Models.LobbiesManager LobbiesManager
    :access: public
	
	


.. csharpdocsproperty:: System.TimeSpan TimeForAuthorization
    :access: protected
	
	


.. csharpdocsproperty:: System.TimeSpan MaxIdleTime
    :access: protected
	
	


Wydarzenia
==========

