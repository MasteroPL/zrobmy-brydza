*************************
StandardRequestSerializer
*************************

.. csharpdocsclass:: EasyHosting.Models.Serialization.StandardRequestSerializer
    :access: public
    :baseclass: EasyHosting.Models.Serialization.BaseSerializer
	
	Serializator do inicjalnej walidacji danych przychodzących (przed przekazaniem do menadżera akcji)

Konstruktory
============

.. csharpdocsconstructor:: StandardRequestSerializer()
    :access: public
	
	


.. csharpdocsconstructor:: StandardRequestSerializer(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): 
	
	


Metody
======

Własności
=========

.. csharpdocsproperty:: Newtonsoft.Json.Linq.JObject DataOrigin
    :access: public
	
	Przechowuje oryginalny obiekt JSONa przekazany do serializatora


.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> GlobalErrors
    :access: public
	
	


.. csharpdocsproperty:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> Errors
    :access: public
	
	Słownik błędów, które wystąpiły podczas walidacji (nazwa pola -> lista błędów dla pola)


Pola
====

.. csharpdocsproperty:: System.Int64 RequestCode
    :access: public
	
	Jeżeli zapytanie przychodzące definiuje kod zapytania, zostanie on zwrócony w odpowiedzi (wyniku zapytania)


.. csharpdocsproperty:: Newtonsoft.Json.Linq.JObject Data
    :access: public
	
	Dane zapytania


Wydarzenia
==========

