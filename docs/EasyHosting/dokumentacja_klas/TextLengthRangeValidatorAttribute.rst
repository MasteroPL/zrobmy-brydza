*********************************
TextLengthRangeValidatorAttribute
*********************************

.. csharpdocsclass:: EasyHosting.Meta.Validators.TextLengthRangeValidatorAttribute
    :access: 
    :baseclass: EasyHosting.Meta.Validators.FieldValidatorAttribute
	
	

Konstruktory
============

.. csharpdocsconstructor:: TextLengthRangeValidatorAttribute(System.Int32 minLength=-1, System.Int32 maxLength=-1)
    :access: public
    :param(1): 
    :param(2): 
	
	


Metody
======

.. csharpdocsmethod:: System.Int32 get_MinValue()
    :access: public
	
	


.. csharpdocsmethod:: System.Int32 get_MaxValue()
    :access: public
	
	


.. csharpdocsmethod:: System.Object Validate(System.Object o, System.Boolean throwException=True)
    :access: public
    :param(1): 
    :param(2): 
	
	


Własności
=========

.. csharpdocsproperty:: System.Int32 MinValue
    :access: public
	
	


.. csharpdocsproperty:: System.Int32 MaxValue
    :access: public
	
	


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

.. csharpdocsproperty:: System.Int32 _MinLength
    :access: private
	
	


.. csharpdocsproperty:: System.Int32 _MaxLength
    :access: private
	
	


Wydarzenia
==========

