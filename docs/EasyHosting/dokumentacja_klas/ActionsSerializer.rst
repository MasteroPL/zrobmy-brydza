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
	
	


.. csharpdocsmethod:: System.Void Validate(System.Boolean throwException=True)
    :access: public
    :param(1): 
	
	


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

.. csharpdocsproperty:: EasyHosting.Models.Actions.ActionSerializer[] Actions
    :access: public
	
	


.. csharpdocsproperty:: System.Int64 Identifier
    :access: public
	
	


.. csharpdocsproperty:: System.Int64[] RESERVED_IDS
    :access: public static
	
	


.. csharpdocsproperty:: System.Int64 NO_IDENTIFIER
    :access: public static
	
	


.. csharpdocsproperty:: System.Int64 BROADCAST_IDENTIFIER
    :access: public static
	
	


.. csharpdocsproperty:: System.String RESERVED_NAMESPACE
    :access: public static
	
	


Wydarzenia
==========

