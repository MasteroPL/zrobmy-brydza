##########
BaseAction
##########

******************
Dokumentacja klasy
******************

.. class::
    BaseAction

Klasa do definiowania jednostkowych czynności, wywoływanych przez API


Konstruktory
============

.. method::
    public BaseAction(Type requestSerializerType, Type responseSerializerType)
    :noindex:

.

    :Parametry:
        * requestSerializerType: Type
            Serializator danych wejściowych
        * responseSerializerType: Type
            Serializator danych wyjściowych

    :Wyjątki:
        * ArgumentException
            Rzucany, jeśli podane typy nie dziedziczą z BaseSerializer


Wydarzenia
==========

.. attribute::
    static Invoked

Wydarzenie wywoływane kiedy dowolna akcja zostanie wywołana

.. attribute::
    InvokedThis

Wydarzenie wywoływane kiedy ta akcja zostanie wywołana

.. attribute::
    static Finished

Wydarzenie wywoływane kiedy dowolna akcja się zakończy

.. attribute::
    FinishedThis

Wydarzenie wywoływane kiedy ta akcja się zakończy


Metody
======

.. method::
    public JObject Invoke(JObject requestData)

Wywołuje wykonanie akcji

    :Parametry:
        * requestData: JObject
            Dane wejściowe
    
    :Zwraca:
        Odpowiedź od akcji

    :Wyjątki:
        * Models.Serialization.ValidationException
            Rzucany w przypadku nieudanej walidacji.

.. method::
    protected BaseSerializer InitializeResponseSerializer()

Inicjalizuje serializator odpowiedzi w oparciu o zdefiniowany w konstruktorze typu serializator

    :Zwraca:
        Zainicjalizowany serializator

.. method::
    protected abstract BaseSerializer PerformAction(BaseSerializer requestData)

Właściwa metoda wykonująca akcję. Otrzymuje na wejściu zwalidowane dane po walidacji

    :Parametry:
        * requestData: BaseSerializer
            Dane wejściowe wpisane do serializatora. Serializator przekazywany na wejściu jest typu "requestSerializerType", definiowanego w konstruktorze
    
    :Zwraca:
        Odpowiedź w postaci serializatora