****************
ActionSerializer
****************

.. csharpdocsclass:: EasyHosting.Models.Actions.ActionSerializer
    :access: public
    :baseclass: EasyHosting.Models.Serialization.BaseSerializer
	
	

Konstruktory
============

.. csharpdocsconstructor:: ActionSerializer()
    :access: public
	
	


.. csharpdocsconstructor:: ActionSerializer(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): 
	
	


Metody
======

.. csharpdocsmethod:: EasyHosting.Models.Actions.ActionMeta GetActionMeta()
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

.. csharpdocsproperty:: System.String ActionName
    :access: public
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Linq.JObject ActionData
    :access: public
	
	


Wydarzenia
==========

