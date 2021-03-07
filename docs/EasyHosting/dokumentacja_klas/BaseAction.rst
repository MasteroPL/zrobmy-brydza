**********
BaseAction
**********

.. csharpdocsclass:: EasyHosting.Models.Actions.BaseAction
    :access: public
    :baseclass: System.Object
	
	Klasa do definiowania jednostkowych czynności, wywoływanych przez API

Konstruktory
============

.. csharpdocsconstructor:: BaseAction(System.Type requestSerializerType, System.Type responseSerializerType)
    :access: public
    :param(1): Serializator danych wejściowych
    :param(2): Serializator danych wyjściowych
	
	


Metody
======

.. csharpdocsmethod:: System.Void add_Invoked(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public static
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_Invoked(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public static
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void add_InvokedThis(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_InvokedThis(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void add_Finished(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public static
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_Finished(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public static
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void add_FinishedThis(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Void remove_FinishedThis(System.EventHandler<Newtonsoft.Json.Linq.JObject> value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.Type get_RequestSerializerType()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_RequestSerializerType(System.Type value)
    :access: protected
    :param(1): 
	
	


.. csharpdocsmethod:: System.Type get_ResponseSerializerType()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_ResponseSerializerType(System.Type value)
    :access: protected
    :param(1): 
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject Invoke(EasyHosting.Models.Server.ClientConnection conn, Newtonsoft.Json.Linq.JObject requestData)
    :access: public
    :param(1): 
    :param(2): Dane wejściowe
	
	Wywołuje wykonanie akcji


.. csharpdocsmethod:: EasyHosting.Models.Serialization.BaseSerializer InitializeResponseSerializer()
    :access: protected
	
	


.. csharpdocsmethod:: EasyHosting.Models.Serialization.BaseSerializer PerformAction(EasyHosting.Models.Server.ClientConnection conn, EasyHosting.Models.Serialization.BaseSerializer requestData)
    :access: protected
    :param(1): 
    :param(2): Dane wejściowe wpisane do serializatora. Serializator przekazywany na wejściu jest typu "requestSerializerType", definiowanego w konstruktorze
	
	Właściwa metoda wykonująca akcję. Otrzymuje na wejściu zwalidowane dane po walidacji


Własności
=========

.. csharpdocsproperty:: System.Type RequestSerializerType
    :access: public
	
	Serializator używany do walidacji danych wejściowych


.. csharpdocsproperty:: System.Type ResponseSerializerType
    :access: public
	
	Serializator używany do przygotowania odpowiedzi (danych wyjściowych)


Pola
====

.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> InvokedThis
    :access: private
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> FinishedThis
    :access: private
	
	


.. csharpdocsproperty:: System.Type _RequestSerializerType
    :access: private
	
	


.. csharpdocsproperty:: System.Type _ResponseSerializerType
    :access: private
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> Invoked
    :access: private static
	
	


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> Finished
    :access: private static
	
	


Wydarzenia
==========

.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> Invoked
    :access: public static event
	
	Wydarzenie wywoływane kiedy dowolna akcja zostanie wywołana


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> InvokedThis
    :access: public event
	
	,
Wydarzenie wywoływane kiedy ta akcja zostanie wywołana


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> Finished
    :access: public static event
	
	Wydarzenie wywoływane kiedy dowolna akcja się zakończy


.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> FinishedThis
    :access: public event
	
	Wydarzenie wywoływane kiedy ta akcja się zakończy


