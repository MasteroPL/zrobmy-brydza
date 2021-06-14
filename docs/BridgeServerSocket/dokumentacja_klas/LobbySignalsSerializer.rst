**********************
LobbySignalsSerializer
**********************

.. csharpdocsclass:: ServerSocket.Serializers.LobbySignalsSerializer
    :access: public
    :baseclass: EasyHosting.Models.Serialization.BaseSerializer
	
	Serializator odpowiadający za komunikaty dotyczące stanu Lobby, np. zamknięcie Lobby, pauza, odpauzowanie Lobby

Konstruktory
============

.. csharpdocsconstructor:: LobbySignalsSerializer()
    :access: public
	
	


.. csharpdocsconstructor:: LobbySignalsSerializer(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): 
	
	


Metody
======

Własności
=========

.. csharpdocsproperty:: Newtonsoft.Json.Linq.JObject DataOrigin
    :access: public
	
	


.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> GlobalErrors
    :access: public
	
	


.. csharpdocsproperty:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> Errors
    :access: public
	
	


Pola
====

.. csharpdocsproperty:: System.String Signal
    :access: public
	
	


.. csharpdocsproperty:: System.String Message
    :access: public
	
	


.. csharpdocsproperty:: System.String SIGNAL_REMOVED_FROM_LOBBY
    :access: public static
	
	


.. csharpdocsproperty:: System.String SIGNAL_PAUSED
    :access: public static
	
	


.. csharpdocsproperty:: System.String SIGNAL_UNPAUSED
    :access: public static
	
	


.. csharpdocsproperty:: System.String SIGNAL_CLOSED
    :access: public static
	
	


.. csharpdocsproperty:: System.String SIGNAL_STATE_BIDDING
    :access: public static
	
	Sygnał oznaczający przełączenie lobby w stan "bidding"


.. csharpdocsproperty:: System.String SIGNAL_STATE_PLAYING
    :access: public static
	
	Przełączenie lobby w stan rozegrania


.. csharpdocsproperty:: System.String SIGNAL_ROUND_FINISHED
    :access: public static
	
	


.. csharpdocsproperty:: System.String SIGNAL_GAME_FINISHED
    :access: public static
	
	


Wydarzenia
==========

