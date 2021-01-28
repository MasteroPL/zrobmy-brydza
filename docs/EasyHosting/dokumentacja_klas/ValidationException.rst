*******************
ValidationException
*******************

.. sphinxsharp:type:: public class ValidationException
	
	

Konstruktory
============

.. sphinxsharp:method:: public ValidationException(Dictionary<FieldInfo, List<ValidationError>> errors=null, ValidationException originException=null)
	:param(1): 
	:param(2): 
	
	


.. sphinxsharp:method:: public ValidationException(List<ValidationError> errors)
	:param(1): 
	
	


Metody
======

.. sphinxsharp:method:: public Dictionary<FieldInfo,List<ValidationError>> get_Errors()
	
	


.. sphinxsharp:method:: private Void Init(Dictionary<FieldInfo, List<ValidationError>> errors, ValidationException originException)
	:param(1): 
	:param(2): 
	
	


.. sphinxsharp:method:: public List<ValidationError> GetErrorsList()
	
	


.. sphinxsharp:method:: public JObject GetJson()
	
	


