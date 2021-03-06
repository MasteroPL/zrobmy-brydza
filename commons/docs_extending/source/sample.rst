##################
Przykładowa strona
##################

Będę tu se testował formatowanie.

Some intro text here...

.. helloworld::

Some more text here...

.. sphinxsharp:method:: public JObject PerformAction(ClientConnection conn, String actionName, JObject actionData)
	:param(1): 
	:param(2): Nazwa akcji
	:param(3): Dane akcji
	
	Wykonuje pojedynczą akcję

FUUUUU


.. csharpdocsclass:: SampleClassName
    :access: public
    :namespace: sample.namespace
    :baseclassname: BaseClassName
    :baseclassnamespace: another.namespace.but.it.is.very.very.very.very.very.very.very.very.long
    
    Przykladowy opis


.. csharpdocsconstructor:: SimpleClassName(some.namespace.ClientConnection conn, another.namespace.JObject actionsData=None)
    :access: public
    :param(1): Połączenie użytkownika
    :param(2): Nazwa akcji
    :throws(1): {some.namespace.Exception} Wyjątek od tak sobie a co
    :throws(2): {OtherException} Inny wyjątek co się rzuca
    
    Wykonuje pojedynczą akcję

.. csharpdocsmethod:: some.namespace.JObject<namespace.for.generic.MyType1, MyType2> PerformActions(some.namespace.ClientConnection conn, another.namespace.JObject actionsData=None)
    :access: public
    :param(1): Połączenie użytkownika
    :param(2): Nazwa akcji
    :returns: Zwraca odpowiedź od akcji
    :throws(1): {some.namespace.Exception} Wyjątek od tak sobie a co
    :throws(2): {OtherException} Inny wyjątek co się rzuca
    
    Wykonuje pojedynczą akcję