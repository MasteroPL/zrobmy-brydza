*******
Session
*******

.. csharpdocsclass:: EasyHosting.Models.Server.Session
    :access: public
    :baseclass: System.Object
	
	

Konstruktory
============

.. csharpdocsconstructor:: Session()
    :access: public
	
	


Metody
======

.. csharpdocsmethod:: System.Void Set(System.String name, System.Object value)
    :access: public
    :param(1): Nazwa wartości do ustawienia
    :param(2): Wartość do ustawienia
	
	Ustawia wartość w sesji


.. csharpdocsmethod:: System.Object Get(System.String name)
    :access: public
    :param(1): Nazwa wartości do pobrania
	
	Pobiera wartość z sesji


.. csharpdocsmethod:: System.Boolean Has(System.String name)
    :access: public
    :param(1): Nazwa do sprawdzenia
	
	Określa czy wartość o podanej nazwie jest zapisana w sesji


.. csharpdocsmethod:: System.Boolean Remove(System.String name)
    :access: public
    :param(1): Nazwa wartości do usunięcia
	
	Usuwa wartość z sesji


Własności
=========

Pola
====

.. csharpdocsproperty:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> SessionData
    :access: private
	
	


Wydarzenia
==========

