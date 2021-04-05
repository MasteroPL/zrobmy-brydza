***********************
RangeValidatorAttribute
***********************

.. csharpdocsclass:: EasyHosting.Meta.Validators.RangeValidatorAttribute
    :access: public
    :baseclass: EasyHosting.Meta.Validators.FieldValidatorAttribute
	
	

Konstruktory
============

.. csharpdocsconstructor:: RangeValidatorAttribute(System.Object minValue=null, System.Object maxValue=null)
    :access: public
    :param(1): 
    :param(2): 
	
	


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
    :param(1): 
    :param(2): 
	
	


Własności
=========

.. csharpdocsproperty:: System.Object MinValue
    :access: public
	
	


.. csharpdocsproperty:: System.Object MaxValue
    :access: public
	
	


.. csharpdocsproperty:: System.Boolean AllowNull
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

.. csharpdocsproperty:: System.Object _MinValue
    :access: private
	
	


.. csharpdocsproperty:: System.Object _MaxValue
    :access: private
	
	


.. csharpdocsproperty:: System.Boolean _AllowNull
    :access: private
	
	


Wydarzenia
==========

