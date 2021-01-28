**************
ActionsManager
**************

.. sphinxsharp:type:: public class ActionsManager
	
	Klasa do zarządzania dostępnymi akcjami

Konstruktory
============

.. sphinxsharp:method:: public ActionsManager(Dictionary<FieldInfo, List<ValidationError>> actionsDictionary)
	:param(1): 
	
	


.. sphinxsharp:method:: public ActionsManager()
	
	


Metody
======

.. sphinxsharp:method:: public Void AddAction(String actionName, BaseAction action)
	:param(1): Nazwa (identyfikator) akcji
	:param(2): Obiekt definiujący akcję
	
	Dodaje akcję do listy dostępnych akcji w tym managerze


.. sphinxsharp:method:: public Void AddActions(Dictionary<FieldInfo, List<ValidationError>> actions)
	:param(1): 
	
	


.. sphinxsharp:method:: public JObject PerformActions(ClientConnection conn, JObject actionsData)
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


.. sphinxsharp:method:: public JObject PerformActions(ClientConnection conn, IEnumerable<ValidationError> actions)
	:param(1): 
	:param(2): 
	
	


.. sphinxsharp:method:: public JObject PerformAction(ClientConnection conn, String actionName, JObject actionData)
	:param(1): 
	:param(2): Nazwa akcji
	:param(3): Dane akcji
	
	Wykonuje pojedynczą akcję


.. sphinxsharp:method:: public JObject PerformAction(ClientConnection conn, JObject actionData)
	:param(1): 
	:param(2): 
            Dane pojedycznej akcji
            Struktura:
            {
            	"name": "action-name-1",
            	"data": { ... }
            }
            
	
	Wykonuje pojedynczą akcję


