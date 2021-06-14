***********************
RetrieveTableDataAction
***********************

.. csharpdocsclass:: ServerSocket.Actions.RetrieveTableData.RetrieveTableDataAction
    :access: public
    :baseclass: EasyHosting.Models.Actions.BaseAction
	
	

Konstruktory
============

.. csharpdocsconstructor:: RetrieveTableDataAction()
    :access: public
	
	


Metody
======

.. csharpdocsmethod:: EasyHosting.Models.Serialization.BaseSerializer PerformAction(EasyHosting.Models.Server.ClientConnection conn, EasyHosting.Models.Serialization.BaseSerializer requestData)
    :access: protected
    :param(1): 
    :param(2): 
	
	


.. csharpdocsmethod:: System.Boolean AreGrandCardsVisible(GameManagerLib.Models.Match game)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.Boolean IsPlayerGrand(GameManagerLib.Models.PlayerTag playerTag, GameManagerLib.Models.Match game)
    :access: private
    :param(1): 
    :param(2): 
	
	


Własności
=========

.. csharpdocsproperty:: System.Type RequestSerializerType
    :access: public
	
	


.. csharpdocsproperty:: System.Type ResponseSerializerType
    :access: public
	
	


Pola
====

Wydarzenia
==========

.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> InvokedThis
    :access: public event
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> FinishedThis
    :access: public event
	
	


