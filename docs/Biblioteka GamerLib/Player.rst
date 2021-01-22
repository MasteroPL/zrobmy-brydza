#######
Player
#######

Definicja
============

Klasa reprezentująca gracza.

Pola
======

Obiekt klasy Player zawiera informacje o pozycji, nazwie oraz ręce gracza.

+------------+----------------------------------------------+-------------------------------------------------------------+
| Pole       | Typ                                          | Opis                                                        |
+============+==============================================+=============================================================+
|   Tag      | PlayerTag                                    | pozycja gracza                                              |
|            |                                              |                                                             |
|            |                                              |                                                             |
+------------+----------------------------------------------+-------------------------------------------------------------+
| Name       | string                                       | nazwa gracza                                                |
|            |                                              |                                                             |
|            |                                              |                                                             |
+------------+----------------------------------------------+-------------------------------------------------------------+
| Hand       | Card[]                                       | lista kart na ręku gracza                                   |
|            |                                              | karty zagrane nie znikają z ręki,                           |
|            |                                              | zmieniają tylko swój stan                                   |
+------------+----------------------------------------------+-------------------------------------------------------------+

.. code-block:: C#

    public PlayerTag Tag { get; set; }
    public string Name { get; set; }
    public Card[] Hand;

Konstruktory
============

.. sphinxsharp:method:: public Player(PlayerTag Tag, string Name)
    :param(1): Tag miejsce zajmowane przez gracza.
    :param(2): Name nazwa gracza.

.. code-block:: C#

    public Player(PlayerTag Tag, string Name)
    {
        this.Hand = new Card[13];
        this.Tag = Tag;
        this.Name = Name;
    }

Typy wyliczeniowe
======================

Typ wyliczeniowy PlayerTag definiuje pozycje gracza przy stole.

.. code-block:: C#

    public enum PlayerTag
    {
        NOBODY = -1,
        N = 0,
        E = 1,
        S = 2,
        W = 3
    }