**************
ActionsManager
**************

.. csharpdocsclass:: EasyHosting.Models.Actions.ActionsManager
    :access: public
    :baseclass: System.Object
	
	Klasa do zarządzania dostępnymi akcjami

Konstruktory
============

.. csharpdocsconstructor:: ActionsManager(System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> actionsDictionary)
    :access: public
    :param(1): 
	
	


.. csharpdocsconstructor:: ActionsManager()
    :access: public
	
	


Metody
======

.. csharpdocsmethod:: System.Void AddAction(System.String actionName, EasyHosting.Models.Actions.BaseAction action)
    :access: public
    :param(1): Nazwa (identyfikator) akcji
    :param(2): Obiekt definiujący akcję
	
	Dodaje akcję do listy dostępnych akcji w tym managerze


.. csharpdocsmethod:: System.Void AddActions(System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> actions)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject PerformActions(EasyHosting.Models.Server.ClientConnection conn, Newtonsoft.Json.Linq.JObject actionsData)
    :access: public
    :param(1): 
    :param(2): 
            Definicja akcji. Struktura:
            {
            	"actions": [
            		{
            			"name": "action-name-1",
            			"data": { ... }
            		},
            		{
            			"name": "action-name-2",
            			"data": { ... }
            		},
            		...
            	]
            }
            
	
	Wykonuje akcje zdefiniowane w źródłowym JObject.


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject PerformActions(EasyHosting.Models.Server.ClientConnection conn, System.Collections.Generic.IEnumerable<Newtonsoft.Json.Linq.JObject> actions)
    :access: public
    :param(1): 
    :param(2): 
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject PerformAction(EasyHosting.Models.Server.ClientConnection conn, System.String actionName, Newtonsoft.Json.Linq.JObject actionData)
    :access: public
    :param(1): 
    :param(2): Nazwa akcji
    :param(3): Dane akcji
	
	Wykonuje pojedynczą akcję


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject PerformAction(EasyHosting.Models.Server.ClientConnection conn, Newtonsoft.Json.Linq.JObject actionData)
    :access: public
    :param(1): 
    :param(2): 
            Dane pojedycznej akcji
            Struktura:
            {
            	"name": "action-name-1",
            	"data": { ... }
            }
            
	
	Wykonuje pojedynczą akcję


Własności
=========

Pola
====

.. csharpdocsproperty:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> ActionsDictionary
    :access: private
	
	


.. csharpdocsproperty:: System.String ERROR_CODE_NOT_FOUND
    :access: public static
	
	


.. csharpdocsproperty:: System.String ERROR_CODE_MANAGER_GENERIC
    :access: public static
	
	


.. csharpdocsproperty:: System.String ERROR_CODE_INTERNAL
    :access: public static
	
	


Wydarzenia
==========

