****************************
ConjuctionValidatorAttribute
****************************

.. csharpdocsclass:: EasyHosting.Meta.Validators.ConjuctionValidatorAttribute
    :access: public
    :baseclass: EasyHosting.Meta.Validators.FieldValidatorAttribute
	
	Klasa pozwalająca zdefiniować zestaw warunków, które pole musi spełniać (koniunkcja)

Konstruktory
============

.. csharpdocsconstructor:: ConjuctionValidatorAttribute(EasyHosting.Meta.Validators.FieldValidatorAttribute[] validators)
    :access: public
    :param(1): Ciąg kolejnych walidatorów
	
	Klasa pozwalająca zdefiniować zestaw warunków, które pole musi spełniać


Metody
======

.. csharpdocsmethod:: System.Object Validate(System.Object o, System.Boolean throwException=True)
    :access: public
    :param(1): Obiekt do walidacji
    :param(2): Definiuje czy ma być wyrzucony wyjątek w przypadku błędu walidacji wszystkich alternatyw
	
	Wykonuje walidację w oparciu o zdefiniowane warunki. Jeśli żaden walidator nie zwraca błędów, zwraca wynik


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

.. csharpdocsproperty:: EasyHosting.Meta.Validators.FieldValidatorAttribute[] Validators
    :access: private
	
	Zestaw walidatorów, które pola musi spełniać


Wydarzenia
==========

