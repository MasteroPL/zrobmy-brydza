*********************************
TextLengthRangeValidatorAttribute
*********************************

.. csharpdocsclass:: EasyHosting.Meta.Validators.TextLengthRangeValidatorAttribute
    :access: 
    :baseclass: EasyHosting.Meta.Validators.FieldValidatorAttribute
	
	Weryfikuje dozwoloną długość tekstu

Konstruktory
============

.. csharpdocsconstructor:: TextLengthRangeValidatorAttribute(System.Int32 minLength=-1, System.Int32 maxLength=-1)
    :access: public
    :param(1): Minimalna dozwolona długość tekstu (jeśli -1, nie jest sprawdzana)
    :param(2): Maksymalna dozowlona długość tekstu (jeśli -1, nie jest sprawdzana)
	
	


Metody
======

.. csharpdocsmethod:: System.Int32 get_MinValue()
    :access: public
	
	


.. csharpdocsmethod:: System.Int32 get_MaxValue()
    :access: public
	
	


.. csharpdocsmethod:: System.Object Validate(System.Object o, System.Boolean throwException=True)
    :access: public
    :param(1): Obiekt do walidacji
    :param(2): Definiuje czy ma być wyrzucony wyjątek w przypadku błędu walidacji wszystkich alternatyw
	
	Wykonuje walidację


Własności
=========

.. csharpdocsproperty:: System.Int32 MinValue
    :access: public
	
	Minimalna dozwolona długość tekstu (jeśli -1, nie jest sprawdzana)


.. csharpdocsproperty:: System.Int32 MaxValue
    :access: public
	
	Maksymalna dozowlona długość tekstu (jeśli -1, nie jest sprawdzana)


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

.. csharpdocsproperty:: System.Int32 _MinLength
    :access: private
	
	


.. csharpdocsproperty:: System.Int32 _MaxLength
    :access: private
	
	


Wydarzenia
==========

