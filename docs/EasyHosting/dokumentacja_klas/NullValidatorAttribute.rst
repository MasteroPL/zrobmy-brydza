**********************
NullValidatorAttribute
**********************

.. csharpdocsclass:: EasyHosting.Meta.Validators.NullValidatorAttribute
    :access: public
    :baseclass: EasyHosting.Meta.Validators.FieldValidatorAttribute
	
	Defiuje, czy pole może być NULLem

Konstruktory
============

.. csharpdocsconstructor:: NullValidatorAttribute(System.Boolean canBeNull=False)
    :access: public
    :param(1): 
	
	


Metody
======

.. csharpdocsmethod:: System.Object Validate(System.Object o, System.Boolean throwException=True)
    :access: public
    :param(1): Obiekt do walidacji
    :param(2): Definiuje czy ma być wyrzucony wyjątek w przypadku błędu walidacji wszystkich alternatyw
	
	Wykonuje walidację


Własności
=========

.. csharpdocsproperty:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> Errors
    :access: public
	
	Lista błędów walidacji


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

.. csharpdocsproperty:: System.Boolean CanBeNull
    :access: private
	
	Określa sposób walidacji (pozwala lub blokuje wartość NULL)


Wydarzenia
==========

