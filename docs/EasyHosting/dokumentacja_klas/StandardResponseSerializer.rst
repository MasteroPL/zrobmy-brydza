**************************
StandardResponseSerializer
**************************

.. csharpdocsclass:: EasyHosting.Models.Server.Serializers.StandardResponseSerializer
    :access: public
    :baseclass: EasyHosting.Models.Serialization.BaseSerializer
	
	

Konstruktory
============

.. csharpdocsconstructor:: StandardResponseSerializer()
    :access: public
	
	


.. csharpdocsconstructor:: StandardResponseSerializer(Newtonsoft.Json.Linq.JObject data)
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

.. csharpdocsproperty:: System.String Status
    :access: public
	
	


.. csharpdocsproperty:: System.String Message
    :access: public
	
	


Wydarzenia
==========

