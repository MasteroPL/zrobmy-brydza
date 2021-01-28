***********************
FieldValidatorAttribute
***********************

.. sphinxsharp:type:: public class FieldValidatorAttribute
	
	Bazowa klasa definiowania atrybutów walidacji danych serializatora

Konstruktory
============

.. sphinxsharp:method:: protected FieldValidatorAttribute()
	
	


Metody
======

.. sphinxsharp:method:: public List<ValidationError> get_Errors()
	
	


.. sphinxsharp:method:: public String get_ErrorsText()
	
	


.. sphinxsharp:method:: public Int32 get_ErrorsCount()
	
	


.. sphinxsharp:method:: protected Void AddError(String errorCode, String errorMessage)
	:param(1): Kod błędu
	:param(2): Treść błędu
	
	Dodaje treść błędu do listy wszystkich błędów które wystąpiły podczas walidacji


.. sphinxsharp:method:: protected Void AddErrors(IEnumerable<ValidationError> errors)
	:param(1): 
	
	


.. sphinxsharp:method:: protected Void ThrowException()
	
	


.. sphinxsharp:method:: public Object Validate(Object o, Boolean throwException=True)
	:param(1): Obiekt do zwalidowania
	:param(2): Określa czy wyrzucić wyjątek, jeśli walidacja się nie powiedzie
	
	Wykonuje walidację danych


