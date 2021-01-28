**********
BaseAction
**********

.. sphinxsharp:type:: public class BaseAction
	
	Klasa do definiowania jednostkowych czynności, wywoływanych przez API

Konstruktory
============

.. sphinxsharp:method:: public BaseAction(Type requestSerializerType, Type responseSerializerType)
	:param(1): 
	:param(2): 
	
	


Metody
======

.. sphinxsharp:method:: public Void add_InvokedThis(EventHandler<ValidationError> value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Void remove_InvokedThis(EventHandler<ValidationError> value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Void add_FinishedThis(EventHandler<ValidationError> value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Void remove_FinishedThis(EventHandler<ValidationError> value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Type get_RequestSerializerType()
	
	


.. sphinxsharp:method:: protected Void set_RequestSerializerType(Type value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Type get_ResponseSerializerType()
	
	


.. sphinxsharp:method:: protected Void set_ResponseSerializerType(Type value)
	:param(1): 
	
	


.. sphinxsharp:method:: public JObject Invoke(ClientConnection conn, JObject requestData)
	:param(1): 
	:param(2): Dane wejściowe
	
	Wywołuje wykonanie akcji


.. sphinxsharp:method:: protected BaseSerializer InitializeResponseSerializer()
	
	


.. sphinxsharp:method:: protected BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData)
	:param(1): 
	:param(2): Dane wejściowe wpisane do serializatora. Serializator przekazywany na wejściu jest typu "requestSerializerType", definiowanego w konstruktorze
	
	Właściwa metoda wykonująca akcję. Otrzymuje na wejściu zwalidowane dane po walidacji


