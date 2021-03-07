*****************
ActionsSerializer
*****************

.. csharpdocsclass:: EasyHosting.Models.Actions.ActionsSerializer
    :access: public
    :baseclass: EasyHosting.Models.Serialization.BaseSerializer
	
	

Konstruktory
============

.. csharpdocsconstructor:: ActionsSerializer()
    :access: public
	
	


.. csharpdocsconstructor:: ActionsSerializer(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): 
	
	


Metody
======

.. csharpdocsmethod:: EasyHosting.Models.Actions.ActionMeta[] GetActionsMeta()
    :access: public
	
	


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

.. csharpdocsproperty:: EasyHosting.Models.Actions.ActionSerializer[] Actions
    :access: public
	
	


Wydarzenia
==========

