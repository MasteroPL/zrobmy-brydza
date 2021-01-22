##############
Card
##############


.. class::
    Card

Obiekt klasy Card reprezentuje kartę.

+------------+----------------------------------------------+-------------------------------------------------------------+
| Pole       | Typ                                          | Opis                                                        |
+============+==============================================+=============================================================+
| Figure     | CardFigure                                   | figura karty                                                |
|            |                                              |                                                             |
|            |                                              |                                                             |
+------------+----------------------------------------------+-------------------------------------------------------------+
| Color      | CardColor                                    | kolor karty                                                 |
|            |                                              |                                                             |
|            |                                              |                                                             |
+------------+----------------------------------------------+-------------------------------------------------------------+
| CardState  | CurrentState                                 | aktualny stan karty                                         |
|            |                                              |                                                             |
|            |                                              |                                                             |
+------------+----------------------------------------------+-------------------------------------------------------------+
| PlayerID   | PlayerTag                                    | gracz posiadajacy tę kartę                                  |
|            |                                              |                                                             |
|            |                                              |                                                             |
+------------+----------------------------------------------+-------------------------------------------------------------+

.. code-block:: C#

    public CardFigure Figure;
    public CardColor Color;
    public CardState CurrentState = CardState.ON_HAND;
    public PlayerTag PlayerID;



Konstruktor
============

.. sphinxsharp:method:: public Card(CardFigure Figure, CardColor Color, PlayerTag PlayerID, CardState CurrentState = CardState.ON_HAND)
    :param(1): Figura.
    :param(2): Kolor.
    :param(3): Gracz posiadający tę kartę.
    :param(4): Stan, domyślnie na ręku.

    .. code-block:: C#

        public Card(CardFigure Figure, CardColor Color, PlayerTag PlayerID, CardState CurrentState = CardState.ON_HAND)
        {
            this.Figure = Figure;
            this.Color = Color;
            this.PlayerID = PlayerID;
            this.CurrentState = CurrentState;
        }


Typy wyliczeniowe
=================

Typ wyliczeniowy CardState definiuje stany kart.

    .. code-block:: C#

        public enum CardState
        {
            IN_DECK = 0,
            ON_HAND = 1,
            ON_TABLE = 2,
            DISPOSED = 3
        }


Typ wyliczeniowy CardColor definiuje kolory kart.

    .. code-block:: C#

        public enum CardColor
        {
            CLUB = 0,
            DIAMOND = 1,
            HEART = 2,
            SPADE = 3
        }

Typ wyliczeniowy CardFigure definiuje figury kart.

    .. code-block:: C#

        public enum CardFigure
        {
            TWO = 2,
            THREE = 3,
            FOUR = 4,
            FIVE = 5,
            SIX = 6,
            SEVEN = 7,
            EIGHT = 8,
            NINE = 9,
            TEN = 10,
            JACK = 11,
            QUEEN = 12,
            KING = 13,
            ACE = 14
        }