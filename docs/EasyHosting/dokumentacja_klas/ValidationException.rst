*******************
ValidationException
*******************

.. csharpdocsclass:: EasyHosting.Meta.Validators.ValidationException
    :access: public
    :baseclass: System.Exception
	
	

Konstruktory
============

.. csharpdocsconstructor:: ValidationException(System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> errors=null, EasyHosting.Meta.Validators.ValidationException originException=null)
    :access: public
    :param(1): 
    :param(2): 
	
	


.. csharpdocsconstructor:: ValidationException(System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> errors)
    :access: public
    :param(1): 
	
	


Metody
======

.. csharpdocsmethod:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> get_Errors()
    :access: public
	
	


.. csharpdocsmethod:: System.Void Init(System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> errors, EasyHosting.Meta.Validators.ValidationException originException)
    :access: private
    :param(1): 
    :param(2): 
	
	


.. csharpdocsmethod:: System.Collections.Generic.List<Newtonsoft.Json.Linq.JObject> GetErrorsList()
    :access: public
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject GetJson()
    :access: public
	
	


Własności
=========

.. csharpdocsproperty:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> Errors
    :access: public
	
	


.. csharpdocsproperty:: System.String Message
    :access: public
	
	


.. csharpdocsproperty:: System.Collections.IDictionary Data
    :access: public
	
	


.. csharpdocsproperty:: System.Exception InnerException
    :access: public
	
	


.. csharpdocsproperty:: System.Reflection.MethodBase TargetSite
    :access: public
	
	


.. csharpdocsproperty:: System.String StackTrace
    :access: public
	
	


.. csharpdocsproperty:: System.String HelpLink
    :access: public
	
	


.. csharpdocsproperty:: System.String Source
    :access: public
	
	


.. csharpdocsproperty:: System.UIntPtr IPForWatsonBuckets
    :access: 
	
	


.. csharpdocsproperty:: System.Object WatsonBuckets
    :access: 
	
	


.. csharpdocsproperty:: System.String RemoteStackTrace
    :access: 
	
	


.. csharpdocsproperty:: System.Int32 HResult
    :access: public
	
	


.. csharpdocsproperty:: System.Boolean IsTransient
    :access: 
	
	


Pola
====

.. csharpdocsproperty:: System.Collections.Generic.Dictionary<System.String, EasyHosting.Models.Actions.BaseAction> _Errors
    :access: private
	
	


.. csharpdocsproperty:: System.String _message
    :access: 
	
	


.. csharpdocsproperty:: System.Int32 _HResult
    :access: 
	
	


Wydarzenia
==========

.. csharpdocsproperty:: System.EventHandler<Newtonsoft.Json.Linq.JObject> SerializeObjectState
    :access: protected event
	
	


