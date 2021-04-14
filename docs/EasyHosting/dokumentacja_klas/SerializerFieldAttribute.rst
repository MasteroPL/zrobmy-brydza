************************
SerializerFieldAttribute
************************

.. csharpdocsclass:: EasyHosting.Meta.SerializerFieldAttribute
    :access: public
    :baseclass: System.Attribute
	
	Określa pole do uwzględnienia w serializacji

Konstruktory
============

.. csharpdocsconstructor:: SerializerFieldAttribute(System.String apiName, System.Boolean required=True, System.Object defaultValue=null)
    :access: public
    :param(1): Nazwa pola w komunikacji sieciowej
    :param(2): Określa, czy pole jest wymagane
    :param(3): Określa wartość domyślną dla pola (jeśli pole nie jest wymagane, powinno definiować wartość domyślną)
	
	


Metody
======

.. csharpdocsmethod:: System.String get_ApiName()
    :access: public
	
	


.. csharpdocsmethod:: System.Boolean get_Required()
    :access: public
	
	


.. csharpdocsmethod:: System.Object get_Default()
    :access: public
	
	


.. csharpdocsmethod:: EasyHosting.Meta.Validators.FieldValidatorAttribute[] get_Validators()
    :access: public
	
	


Własności
=========

.. csharpdocsproperty:: System.String ApiName
    :access: public
	
	Nazwa pola w komunikacji sieciowej


.. csharpdocsproperty:: System.Boolean Required
    :access: public
	
	Określa, czy pole jest wymagane


.. csharpdocsproperty:: System.Object Default
    :access: public
	
	Określa wartość domyślną dla pola (jeśli pole nie jest wymagane, powinno definiować wartość domyślną)


.. csharpdocsproperty:: EasyHosting.Meta.Validators.FieldValidatorAttribute[] Validators
    :access: public
	
	Określa zestaw walidatorów dla pola


.. csharpdocsproperty:: System.Object TypeId
    :access: public
	
	


Pola
====

.. csharpdocsproperty:: System.String _ApiName
    :access: private
	
	


.. csharpdocsproperty:: System.Boolean _Required
    :access: private
	
	


.. csharpdocsproperty:: System.Object _Default
    :access: private
	
	


.. csharpdocsproperty:: EasyHosting.Meta.Validators.FieldValidatorAttribute[] _Validators
    :access: private
	
	


Wydarzenia
==========

