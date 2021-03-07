**********************
TypeValidatorAttribute
**********************

.. csharpdocsclass:: EasyHosting.Meta.Validators.TypeValidatorAttribute
    :access: public
    :baseclass: EasyHosting.Meta.Validators.FieldValidatorAttribute
	
	

Konstruktory
============

.. csharpdocsconstructor:: TypeValidatorAttribute(System.Type type)
    :access: public
    :param(1): 
	
	


Metody
======

.. csharpdocsmethod:: System.Type get_Type()
    :access: public
	
	


.. csharpdocsmethod:: System.Object Validate(System.Object o, System.Boolean throwException=True)
    :access: public
    :param(1): 
    :param(2): 
	
	


Własności
=========

.. csharpdocsproperty:: System.Type Type
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

.. csharpdocsproperty:: System.Type _Type
    :access: private
	
	


Wydarzenia
==========

