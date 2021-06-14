*****
Lobby
*****

.. csharpdocsclass:: ServerSocket.Models.Lobby
    :access: public
    :baseclass: System.Object
	
	

Konstruktory
============

.. csharpdocsconstructor:: Lobby(System.String lobbyId, System.String password=, ServerSocket.Models.LobbiesManager parentManager=null)
    :access: public
    :param(1): 
    :param(2): 
    :param(3): 
	
	


Metody
======

.. csharpdocsmethod:: System.Boolean get_Disposed()
    :access: public
	
	


.. csharpdocsmethod:: System.Void add_Closed(System.EventHandler value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_Closed(System.EventHandler value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void set_Id(System.String value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.String get_Id()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_ParentManager(ServerSocket.Models.LobbiesManager value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: ServerSocket.Models.LobbiesManager get_ParentManager()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_LobbyState(ServerSocket.Models.LobbyState value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: ServerSocket.Models.LobbyState get_LobbyState()
    :access: public
	
	


.. csharpdocsmethod:: System.Boolean UsernameAlreadyJoined(System.String username)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void Finalize()
    :access: protected
	
	


.. csharpdocsmethod:: System.Void SetLobbyState(ServerSocket.Models.LobbyState newState, System.Boolean sendBroadcast=True)
    :access: public
    :param(1): Nowy stan do ustawienia
    :param(2): Określa czy należy wysłać broadcast do graczy o zmianie stanu lobby (jeżeli nastąpi zmiana stanu)
	
	Zmienia stan lobby jeśli jest inny niż podany w argumencie "newState"


.. csharpdocsmethod:: System.Void Join(EasyHosting.Models.Server.ClientConnection newConnection)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void RemoveFromLobby(EasyHosting.Models.Server.ClientConnection client)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void Close()
    :access: public
	
	


.. csharpdocsmethod:: System.Void Broadcast(Newtonsoft.Json.Linq.JObject communicate)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void Dispose()
    :access: public
	
	


.. csharpdocsmethod:: System.Void OnClientConnectionDisposed(System.Object sender, System.EventArgs args)
    :access: protected
    :param(1): 
    :param(2): 
	
	


Własności
=========

.. csharpdocsproperty:: System.Boolean Disposed
    :access: public
	
	


.. csharpdocsproperty:: System.String Id
    :access: public
	
	


.. csharpdocsproperty:: ServerSocket.Models.LobbiesManager ParentManager
    :access: public
	
	


.. csharpdocsproperty:: ServerSocket.Models.LobbyState LobbyState
    :access: public
	
	


Pola
====

.. csharpdocsproperty:: System.Boolean _Disposed
    :access: private
	
	


.. csharpdocsproperty:: System.EventHandler Closed
    :access: private
	
	


.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> ConnectedClients
    :access: public
	
	


.. csharpdocsproperty:: EasyHosting.Models.Actions.ActionsManager ActionsManager
    :access: public
	
	


.. csharpdocsproperty:: GameManagerLib.Models.Match Game
    :access: public
	
	


.. csharpdocsproperty:: System.String Password
    :access: public
	
	


.. csharpdocsproperty:: EasyHosting.Models.Server.ClientConnection LobbyOwner
    :access: public
	
	


Wydarzenia
==========

.. csharpdocsproperty:: System.EventHandler Closed
    :access: public event
	
	


