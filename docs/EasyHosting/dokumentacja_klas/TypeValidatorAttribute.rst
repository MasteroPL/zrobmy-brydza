**********************
TypeValidatorAttribute
**********************

.. csharpdocsclass:: EasyHosting.Meta.Validators.TypeValidatorAttribute
    :access: public
    :baseclass: EasyHosting.Meta.Validators.FieldValidatorAttribute
	
	Walidator typu. Walidacja typu jest domyślnie obsługiwana przez serializatory poprzez typ pola, do którego przypisujemy wartość. Tego atrybutu można użyć jako dodatkową walidację, jeśli przyjmujemy tylko określone typy dziedzicące z bazowego

Konstruktory
============

.. csharpdocsconstructor:: TypeValidatorAttribute(System.Type type, System.Boolean allowInheritance=True)
    :access: public
    :param(1): Wymagany typ
    :param(2): Określa, czy akceptowane są typy dziedzące z podanego
	
	


Metody
======

.. csharpdocsmethod:: System.Type get_Type()
    :access: public
	
	


.. csharpdocsmethod:: System.Boolean get_AllowInheritance()
    :access: public
	
	


.. csharpdocsmethod:: System.Object Validate(System.Object o, System.Boolean throwException=True)
    :access: public
    :param(1): Obiekt do walidacji
    :param(2): Definiuje czy ma być wyrzucony wyjątek w przypadku błędu walidacji wszystkich alternatyw
	
	Wykonuje walidację


Własności
=========

.. csharpdocsproperty:: System.Type Type
    :access: public
	
	Wymagany typ


.. csharpdocsproperty:: System.Boolean AllowInheritance
    :access: public
	
	Określa, czy akceptowane są typy dziedzące z podanego


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

.. csharpdocsproperty:: System.Type _Type
    :access: private
	
	


.. csharpdocsproperty:: System.Boolean _AllowInheritance
    :access: private
	
	


Wydarzenia
==========

