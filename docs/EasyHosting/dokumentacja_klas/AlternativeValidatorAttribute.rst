*****************************
AlternativeValidatorAttribute
*****************************

.. csharpdocsclass:: EasyHosting.Meta.Validators.AlternativeValidatorAttribute
    :access: public
    :baseclass: EasyHosting.Meta.Validators.FieldValidatorAttribute
	
	Klasa pozwalająca zdefiniować zestaw alternatyw pod kątem konfiguracyjnej walidacji pól

Konstruktory
============

.. csharpdocsconstructor:: AlternativeValidatorAttribute(System.String errorCodeOnFail, EasyHosting.Meta.Validators.FieldValidatorAttribute[] alternateValidators)
    :access: public
    :param(1): Kod błędu, który ma się zwrócić w przypadku nieudanej walidacji. Jeśli null, zostanie przypisany ALTERNATIVE_CHECK_FAILED
    :param(2): Ciąg kolejnych, alternatywnych walidatorów
	
	Klasa pozwalająca zdefiniować zestaw alternatyw pod kątem konfiguracyjnej walidacji pól


Metody
======

.. csharpdocsmethod:: System.Object Validate(System.Object o, System.Boolean throwException=True)
    :access: public
    :param(1): 
    :param(2): 
	
	


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

.. csharpdocsproperty:: EasyHosting.Meta.Validators.FieldValidatorAttribute[] AlternateValidators
    :access: private
	
	


.. csharpdocsproperty:: System.String ErrorCodeOnFail
    :access: private
	
	


Wydarzenia
==========

