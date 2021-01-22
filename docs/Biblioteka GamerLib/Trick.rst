#####
Trick
#####

Definicja
============

Klasa reprezentująca lewę.

Pola
======

Obiekt klasy Trick zawiera informacje o kartach w lewie oraz pozycję gracza, który wział daną lewę.

+------------+----------------------------------------------+-------------------------------------------------------------+
| Pole       | Typ                                          | Opis                                                        |
+============+==============================================+=============================================================+
|   Winner   | PlayerTag                                    | pozycja gracza, który wziął tę lewę                         |
|            |                                              |                                                             |
|            |                                              |                                                             |
+------------+----------------------------------------------+-------------------------------------------------------------+
| CardList   | List<Card>                                   | lista kart w danej lewie                                    |
|            |                                              |                                                             |
|            |                                              |                                                             |
|            |                                              |                                                             |
|            |                                              |                                                             |
+------------+----------------------------------------------+-------------------------------------------------------------+

.. code-block:: C#

    public PlayerTag Winner { get; set; }
    public List<Card> CardList;

Konstruktory
============

Konstruktor nie wymaga żadnych argumentów.

.. sphinxsharp:method:: public Trick()

.. code-block:: C#

    public Trick()
    {
        this.CardList = new List<Card>();
    }

Metody
======

.. sphinxsharp:method:: public void NextCard(Card Card, ContractColor ContractColor)
    :param(1): Card grana karta.
    :param(2): ContractColor kolor atowy.

    Metoda NextCard przyjmuje jako argument kartę i kolor kontraktu.
    Dodaje kartę do listy kart i jeżeli po dodaniu liczba kart wynosi 4
    wywołuje metodę SetWinner przekazując jej kolor kontraktu.

    .. code-block:: C#

        public void NextCard(Card Card, ContractColor ContractColor)
        {
            CardList.Add(Card);
            if (CardList.Count == 4)
            {
                SetWinner(ContractColor);
            }
        }

.. sphinxsharp:method:: public int GetCount()

    Metoda zwracająca aktualną liczbę kart w lewie.

    .. code-block:: C#

        public int GetCount()
        {
            return this.CardList.Count;
        }

.. sphinxsharp:method:: SetWinner(ContractColor ContractColor)
    :param(1): ContractColor kolor atowy.

    Metoda ustalająca kto wziął lewę (pole Winner) na podstawie kart i koloru atowego.

    .. code-block:: C#

        private void SetWinner(ContractColor ContractColor) {
            Card StrongestCard = CardList[0];
            PlayerTag CurrentWinner = StrongestCard.PlayerID;

            for(int i = 1; i <= 3; i++)
            {
                Card CurrentCard = CardList[i];
                if (CurrentCard.Color == StrongestCard.Color)
                {
                    if (CurrentCard.Figure > StrongestCard.Figure)
                    {
                        StrongestCard = CurrentCard;
                        CurrentWinner = StrongestCard.PlayerID;
                    }
                }
                else
                {
                    if ((int)(CurrentCard.Color) == (int)(ContractColor))
                    {
                        StrongestCard = CurrentCard;
                        CurrentWinner = StrongestCard.PlayerID;
                    }
                }
            }
            this.Winner = CurrentWinner;
        }