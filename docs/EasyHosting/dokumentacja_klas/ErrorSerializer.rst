***************
ErrorSerializer
***************

.. csharpdocsclass:: EasyHosting.Models.Server.Serializers.ErrorSerializer
    :access: public
    :baseclass: EasyHosting.Models.Serialization.BaseSerializer
	
	

Konstruktory
============

.. csharpdocsconstructor:: ErrorSerializer()
    :access: public
	
	


.. csharpdocsconstructor:: ErrorSerializer(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): 
	
	


Metody
======

.. csharpdocsmethod:: EasyHosting.Models.Server.Serializers.ErrorSerializer CreateInstance(System.String errorCode, System.String errorMessage)
    :access: public static
    :param(1): 
    :param(2): 
	
	


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

.. csharpdocsproperty:: System.String ErrorCode
    :access: public
	
	


.. csharpdocsproperty:: System.String ErrorMessage
    :access: public
	
	


Wydarzenia
==========

