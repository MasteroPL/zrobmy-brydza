*****************************
StandardCommunicateSerializer
*****************************

.. csharpdocsclass:: EasyHosting.Models.Serialization.StandardCommunicateSerializer
    :access: public
    :baseclass: EasyHosting.Models.Serialization.BaseSerializer
	
	Serializator definiujący standardowy komunikat od serwera (dowolna odpowiedź lub wysyłana informacja)

Konstruktory
============

.. csharpdocsconstructor:: StandardCommunicateSerializer()
    :access: public
	
	


.. csharpdocsconstructor:: StandardCommunicateSerializer(Newtonsoft.Json.Linq.JObject data)
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

.. csharpdocsproperty:: System.String CommunicateType
    :access: public
	
	Określa typ komunikatu


.. csharpdocsproperty:: System.Int64 RequestCode
    :access: public
	
	Jeżeli zapytania przychodzące refiniowało kod zapytania, powinien on zostać zwrotnie przekazany w komunikacie wychodzącym


.. csharpdocsproperty:: Newtonsoft.Json.Linq.JObject Data
    :access: public
	
	Określa dane komunikatu


.. csharpdocsproperty:: System.String TYPE_RESPONSE
    :access: public static
	
	


.. csharpdocsproperty:: System.String TYPE_REQUEST_ERROR
    :access: public static
	
	


.. csharpdocsproperty:: System.String TYPE_LOBBY_SIGNAL
    :access: public static
	
	


.. csharpdocsproperty:: System.String TYPE_AUTHORIZATION
    :access: public static
	
	


.. csharpdocsproperty:: System.String TYPE_SERVER_SIGNAL
    :access: public static
	
	


.. csharpdocsproperty:: System.String TYPE_CONNECTION_CHECK
    :access: public static
	
	


Wydarzenia
==========

