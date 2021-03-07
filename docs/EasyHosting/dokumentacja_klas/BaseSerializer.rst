**************
BaseSerializer
**************

.. csharpdocsclass:: EasyHosting.Models.Serialization.BaseSerializer
    :access: public
    :baseclass: System.Object
	
	

Konstruktory
============

.. csharpdocsconstructor:: BaseSerializer()
    :access: public
	
	


.. csharpdocsconstructor:: BaseSerializer(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): 
	
	User -> Server   Serializer constructor


Metody
======

.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject get_DataOrigin()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_DataOrigin(Newtonsoft.Json.Linq.JObject value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> get_Errors()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_Errors(System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void AddError(System.Reflection.FieldInfo field, EasyHosting.Meta.Validators.ValidationError error)
    :access: private
    :param(1): 
    :param(2): 
	
	


.. csharpdocsmethod:: System.Void AddError(System.String fieldName, EasyHosting.Meta.Validators.ValidationError error)
    :access: protected
    :param(1): 
    :param(2): 
	
	


.. csharpdocsmethod:: System.Void AddError(System.String fieldName, System.String errorCode, System.String errorMessage)
    :access: public
    :param(1): Nazwa pola
    :param(2): Kod błędu
    :param(3): Treść błędu
	
	Dodaje błąd do listy błędów dla wybranego pola


.. csharpdocsmethod:: System.Void AddErrors(System.Reflection.FieldInfo field, System.Collections.Generic.IEnumerable<Newtonsoft.Json.Linq.JObject> errors)
    :access: private
    :param(1): 
    :param(2): 
	
	


.. csharpdocsmethod:: System.Void AddErrors(System.String fieldName, System.Collections.Generic.IEnumerable<Newtonsoft.Json.Linq.JObject> errors)
    :access: protected
    :param(1): 
    :param(2): 
	
	


.. csharpdocsmethod:: System.Void AddErrors(System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> errors)
    :access: protected
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void ThrowException()
    :access: public
	
	


.. csharpdocsmethod:: System.Void Init()
    :access: private
	
	


.. csharpdocsmethod:: System.Void SetData(Newtonsoft.Json.Linq.JObject data)
    :access: public
    :param(1): Dane źródłowe dla serializatora
	
	Ustawia dane źródłowe dla serializatora


.. csharpdocsmethod:: System.Void Validate(System.Boolean throwException=True)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject GetApiObject()
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

.. csharpdocsproperty:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> _Errors
    :access: private
	
	


Wydarzenia
==========

