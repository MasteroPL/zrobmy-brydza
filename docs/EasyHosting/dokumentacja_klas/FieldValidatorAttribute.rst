***********************
FieldValidatorAttribute
***********************

.. csharpdocsclass:: EasyHosting.Meta.Validators.FieldValidatorAttribute
    :access: public
    :baseclass: System.Attribute
	
	Bazowa klasa definiowania atrybutów walidacji danych serializatora

Konstruktory
============

.. csharpdocsconstructor:: FieldValidatorAttribute()
    :access: protected
	
	


Metody
======

.. csharpdocsmethod:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> get_Errors()
    :access: public
	
	


.. csharpdocsmethod:: System.String get_ErrorsText()
    :access: public
	
	


.. csharpdocsmethod:: System.Int32 get_ErrorsCount()
    :access: public
	
	


.. csharpdocsmethod:: System.Void AddError(System.String errorCode, System.String errorMessage)
    :access: protected
    :param(1): Kod błędu
    :param(2): Treść błędu
	
	Dodaje treść błędu do listy wszystkich błędów które wystąpiły podczas walidacji


.. csharpdocsmethod:: System.Void AddErrors(System.Collections.Generic.IEnumerable<Newtonsoft.Json.Linq.JObject> errors)
    :access: protected
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void ThrowException()
    :access: protected
	
	


.. csharpdocsmethod:: System.Object Validate(System.Object o, System.Boolean throwException=True)
    :access: public
    :param(1): Obiekt do zwalidowania
    :param(2): Określa czy wyrzucić wyjątek, jeśli walidacja się nie powiedzie
	
	Wykonuje walidację danych


Własności
=========

.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> Errors
    :access: public
	
	


.. csharpdocsproperty:: System.String ErrorsText
    :access: public
	
	Konwertuje listę błędów na tekst


.. csharpdocsproperty:: System.Int32 ErrorsCount
    :access: public
	
	Liczba błędów


.. csharpdocsproperty:: System.Object TypeId
    :access: public
	
	


Pola
====

.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> _Errors
    :access: private
	
	


Wydarzenia
==========

