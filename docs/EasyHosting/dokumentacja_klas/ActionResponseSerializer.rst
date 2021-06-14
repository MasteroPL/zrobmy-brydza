************************
ActionResponseSerializer
************************

.. csharpdocsclass:: EasyHosting.Models.Actions.ActionResponseSerializer
    :access: public
    :baseclass: EasyHosting.Models.Serialization.BaseSerializer
	
	

Konstruktory
============

.. csharpdocsconstructor:: ActionResponseSerializer()
    :access: public
	
	


.. csharpdocsconstructor:: ActionResponseSerializer(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): 
	
	


Metody
======

.. csharpdocsmethod:: EasyHosting.Models.Actions.ActionResponseSerializer CreateInstance(System.String status, System.String actionName, Newtonsoft.Json.Linq.JObject data, System.Collections.Generic.IEnumerable<Newtonsoft.Json.Linq.JObject> errors)
    :access: public static
    :param(1): 
    :param(2): 
    :param(3): 
    :param(4): 
	
	


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

.. csharpdocsproperty:: System.String Status
    :access: public
	
	


.. csharpdocsproperty:: System.String ActionName
    :access: public
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Linq.JObject Data
    :access: public
	
	


.. csharpdocsproperty:: EasyHosting.Models.Server.Serializers.ErrorSerializer[] ResponseErrors
    :access: public
	
	


Wydarzenia
==========

