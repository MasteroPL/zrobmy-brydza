##############
Biddging
##############

******************
Dokumentacja klasy
******************

.. class::
    Biddging

Obiekt klasy Bidding obsługuje przebieg pojedynczej licytacji.

Konstruktor
============

.. sphinxsharp:method:: public Bidding(PlayerTag Dealer)
    :param(1): Gracz, który rozdawał karty (następny po nim rozpoczyna licytację).

Metody
======

.. sphinxsharp:method:: public PlayerTag NextPlayer(PlayerTag CurrentPlayer)
    :param(1): Gracz, względem którego wyznaczamy następnego.
    :returns: Kolejny gracz.

   Zwraca kolejnego gracza w kolejności N E S W N...

.. sphinxsharp:method:: public bool AddBid(Contract Contract, bool X = false, bool XX = false)
    :param(1): Odzywka gracza ( nie uwzględnia kontry, rekontry, jest to podawane jako odzielny argument).
    :param(2): Kontra.
    :param(3): Rekonta
    :returns: True, jeżeli wszysko jest poprawne.


    Sprawdza, czy dana odzywka jest możliwa do zadeklarowania przez danego gracza, jest ona dodawana do listy.
    Zapamiętuje, który gracz z dróżyny jako pierwszy licytował dany kolor oraz sprawdza, czy licytacja dobiegłą końca.
    
