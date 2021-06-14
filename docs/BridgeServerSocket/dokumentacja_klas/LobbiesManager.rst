**************
LobbiesManager
**************

.. csharpdocsclass:: ServerSocket.Models.LobbiesManager
    :access: public
    :baseclass: System.Object
	
	Klasa do zarządzania pokojami w lokalnej przestrzeni gniazda. Domyślnie tworzone jest lobby "DEFAULT", bez hasła

Konstruktory
============

.. csharpdocsconstructor:: LobbiesManager(System.Boolean createDefaultLobby=True, System.String defaultLobbyPassword=)
    :access: public
    :param(1): 
    :param(2): 
	
	


Metody
======

.. csharpdocsmethod:: System.Void add_LobbyCreated(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public static
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_LobbyCreated(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public static
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void add_LobbyClosed(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public static
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_LobbyClosed(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public static
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void CreateLobby(System.String lobbyId, System.String lobbyPassword=)
    :access: public
    :param(1): Identyfikator Lobby
    :param(2): Hasło lobby
	
	Tworzy nowe Lobby w przestrzeni gniazda


.. csharpdocsmethod:: System.Void CloseLobby(System.String lobbyId)
    :access: public
    :param(1): Identyfikator Lobby do zamknięcia
	
	Zamyka wybrane Lobby, jeśli istnieje


.. csharpdocsmethod:: System.Void JoinLobby(EasyHosting.Models.Server.ClientConnection conn, System.String lobbyId)
    :access: public
    :param(1): Połączenie klienta
    :param(2): Identyfikator lobby
	
	Dołącza wybranego klienta do Lobby (nie jest wykonywana autoryzacja)


.. csharpdocsmethod:: System.Void JoinLobby(EasyHosting.Models.Server.ClientConnection conn, System.String lobbyId, System.String lobbyPassword)
    :access: public
    :param(1): Połączenie klienta
    :param(2): Identyfikator Lobby
    :param(3): Hasło do Lobby
	
	Dołącza wybranego klienta do Lobby jeśli hasło do Lobby jest poprawne


Własności
=========

Pola
====

.. csharpdocsproperty:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> Lobbies
    :access: public
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> LobbyCreated
    :access: private static
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> LobbyClosed
    :access: private static
	
	


Wydarzenia
==========

.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> LobbyCreated
    :access: public static event
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> LobbyClosed
    :access: public static event
	
	


