***********************
RangeValidatorAttribute
***********************

.. csharpdocsclass:: EasyHosting.Meta.Validators.RangeValidatorAttribute
    :access: public
    :baseclass: EasyHosting.Meta.Validators.FieldValidatorAttribute
	
	Walidator uwzględniający zakres dozwolonych wartości. Typ danych dla walidacji musi być możliwy do porównań większe/mniejsze równe

Konstruktory
============

.. csharpdocsconstructor:: RangeValidatorAttribute(System.Object minValue=null, System.Object maxValue=null, System.Boolean allowNull=False)
    :access: public
    :param(1): Minimalna dozwolona wartość (jeśli NULL, warunek nie jest sprawdzany)
    :param(2): Maksymalna dozwolona wartość (jeśli NULL, warunek nie jest sprawdzany)
    :param(3): Czy wartość może być NULLem
	
	


Metody
======

.. csharpdocsmethod:: System.Object get_MinValue()
    :access: public
	
	


.. csharpdocsmethod:: System.Object get_MaxValue()
    :access: public
	
	


.. csharpdocsmethod:: System.Boolean get_AllowNull()
    :access: public
	
	


.. csharpdocsmethod:: System.Object Validate(System.Object o, System.Boolean throwException=True)
    :access: public
    :param(1): Obiekt do walidacji
    :param(2): Definiuje czy ma być wyrzucony wyjątek w przypadku błędu walidacji wszystkich alternatyw
	
	Wykonuje walidację


Własności
=========

.. csharpdocsproperty:: System.Object MinValue
    :access: public
	
	Minimalna dozwolona wartość


.. csharpdocsproperty:: System.Object MaxValue
    :access: public
	
	Maksymalna dozwolona wartość


.. csharpdocsproperty:: System.Boolean AllowNull
    :access: public
	
	Czy dozwolony jest NULL


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

.. csharpdocsproperty:: System.Object _MinValue
    :access: private
	
	


.. csharpdocsproperty:: System.Object _MaxValue
    :access: private
	
	


.. csharpdocsproperty:: System.Boolean _AllowNull
    :access: private
	
	


Wydarzenia
==========

