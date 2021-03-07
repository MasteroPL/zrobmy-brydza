*************************
ActionsResponseSerializer
*************************

.. csharpdocsclass:: EasyHosting.Models.Actions.ActionsResponseSerializer
    :access: public
    :baseclass: EasyHosting.Models.Serialization.BaseSerializer
	
	

Konstruktory
============

.. csharpdocsconstructor:: ActionsResponseSerializer()
    :access: public
	
	


.. csharpdocsconstructor:: ActionsResponseSerializer(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): 
	
	


Metody
======

Własności
=========

.. csharpdocsproperty:: Newtonsoft.Json.Linq.JObject DataOrigin
    :access: public
	
	Przechowuje oryginalny obiekt JSONa przekazany do serializatora


.. csharpdocsproperty:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> Errors
    :access: public
	
	Słownik błędów, które wystąpiły podczas walidacji (nazwa pola -> lista błędów dla pola)


Pola
====

.. csharpdocsproperty:: EasyHosting.Models.Actions.ActionResponseSerializer[] Actions
    :access: public
	
	


Wydarzenia
==========

