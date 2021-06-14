*******
Request
*******

.. csharpdocsclass:: EasyHosting.Models.Client.Request
    :access: public
    :baseclass: System.Object
	
	

Konstruktory
============

.. csharpdocsconstructor:: Request(EasyHosting.Models.Client.ClientSocket parentSocket, Newtonsoft.Json.Linq.JObject requestData, System.Int64 requestId)
    :access: public
    :param(1): 
    :param(2): 
    :param(3): 
	
	


Metody
======

.. csharpdocsmethod:: System.Void set_RequestId(System.Int64 value)
    :access: private
    :param(1): 
	
	


.. csharpdocsmethod:: System.Int64 get_RequestId()
    :access: public
	
	


.. csharpdocsmethod:: EasyHosting.Models.Client.ClientSocket get_ParentSocket()
    :access: public
	
	


.. csharpdocsmethod:: EasyHosting.Models.Client.RequestState get_RequestState()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_RequestState(EasyHosting.Models.Client.RequestState value)
    :access: protected
    :param(1): 
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject get_RequestData()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_RequestData(Newtonsoft.Json.Linq.JObject value)
    :access: protected
    :param(1): 
	
	


.. csharpdocsmethod:: Newtonsoft.Json.Linq.JObject get_ResponseData()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_ResponseData(Newtonsoft.Json.Linq.JObject value)
    :access: protected
    :param(1): 
	
	


.. csharpdocsmethod:: System.String get_ResponseType()
    :access: public
	
	


.. csharpdocsmethod:: System.Void set_ResponseType(System.String value)
    :access: public
    :param(1): 
	
	


.. csharpdocsmethod:: System.DateTime get_SentAt()
    :access: public
	
	


.. csharpdocsmethod:: System.DateTime get_ResponseReceivedAt()
    :access: public
	
	


.. csharpdocsmethod:: System.Void Send()
    :access: public
	
	


.. csharpdocsmethod:: System.Void AttachResponse(Newtonsoft.Json.Linq.JObject response)
    :access: public
    :param(1): Odpowiedź do przypięcia
	
	Przypina odpowiedź dla zapytania


Własności
=========

.. csharpdocsproperty:: System.Int64 RequestId
    :access: public
	
	


.. csharpdocsproperty:: EasyHosting.Models.Client.ClientSocket ParentSocket
    :access: public
	
	Socket klienta, za pośrednictwem którego zostało wysłane zapytanie


.. csharpdocsproperty:: EasyHosting.Models.Client.RequestState RequestState
    :access: public
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Linq.JObject RequestData
    :access: public
	
	


.. csharpdocsproperty:: Newtonsoft.Json.Linq.JObject ResponseData
    :access: public
	
	


.. csharpdocsproperty:: System.String ResponseType
    :access: public
	
	


.. csharpdocsproperty:: System.DateTime SentAt
    :access: public
	
	Określa moment czasowy, w którym zapytanie zostało wysłane. Jest ustawione tylko jeśli zapytanie ma status SENT lub RESPONSE_RECEIVED


.. csharpdocsproperty:: System.DateTime ResponseReceivedAt
    :access: public
	
	Określa moment czasowy, w którym przypisana została odpowiedź na wysłane zapytanie. Jest ustawione tylko jeżeli zapytanie ma status RESPONSE_RECEIVED


Pola
====

.. csharpdocsproperty:: EasyHosting.Models.Client.ClientSocket _ParentSocket
    :access: private
	
	


.. csharpdocsproperty:: System.DateTime _SentAt
    :access: private
	
	


.. csharpdocsproperty:: System.DateTime _ResponseReceivedAt
    :access: private
	
	


Wydarzenia
==========

