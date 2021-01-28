**************
BaseSerializer
**************

.. sphinxsharp:type:: public class BaseSerializer
	
	

Konstruktory
============

.. sphinxsharp:method:: public BaseSerializer()
	
	


.. sphinxsharp:method:: public BaseSerializer(JObject data)
	:param(1): 
	
	


Metody
======

.. sphinxsharp:method:: public JObject get_DataOrigin()
	
	


.. sphinxsharp:method:: private Void set_DataOrigin(JObject value)
	:param(1): 
	
	


.. sphinxsharp:method:: public Dictionary<FieldInfo,List<ValidationError>> get_Errors()
	
	


.. sphinxsharp:method:: private Void set_Errors(Dictionary<FieldInfo, List<ValidationError>> value)
	:param(1): 
	
	


.. sphinxsharp:method:: private Void AddError(FieldInfo field, ValidationError error)
	:param(1): 
	:param(2): 
	
	


.. sphinxsharp:method:: protected Void AddError(String fieldName, ValidationError error)
	:param(1): 
	:param(2): 
	
	


.. sphinxsharp:method:: public Void AddError(String fieldName, String errorCode, String errorMessage)
	:param(1): Nazwa pola
	:param(2): Kod błędu
	:param(3): Treść błędu
	
	Dodaje błąd do listy błędów dla wybranego pola


.. sphinxsharp:method:: private Void AddErrors(FieldInfo field, IEnumerable<ValidationError> errors)
	:param(1): 
	:param(2): 
	
	


.. sphinxsharp:method:: protected Void AddErrors(String fieldName, IEnumerable<ValidationError> errors)
	:param(1): 
	:param(2): 
	
	


.. sphinxsharp:method:: protected Void AddErrors(Dictionary<FieldInfo, List<ValidationError>> errors)
	:param(1): 
	
	


.. sphinxsharp:method:: public Void ThrowException()
	
	


.. sphinxsharp:method:: private Void Init()
	
	


.. sphinxsharp:method:: public Void SetData(JObject data)
	:param(1): Dane źródłowe dla serializatora
	
	Ustawia dane źródłowe dla serializatora


.. sphinxsharp:method:: public Void Validate(Boolean throwException=True)
	:param(1): 
	
	


.. sphinxsharp:method:: public JObject GetApiObject()
	
	


