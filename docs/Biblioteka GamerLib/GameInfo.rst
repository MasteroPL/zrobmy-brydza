########
GameInfo
########

Definicja
============

Klasa reprezentująca rozegranie jednego rozdania.

Pola
======

Obiekt klasy Player zawiera informacje o pozycji, nazwie oraz ręce gracza.

+--------------+----------------------------------------------+-------------------------------------------------------------+
| Pole         | Typ                                          | Opis                                                        |
+==============+==============================================+=============================================================+
|CurrentPlayer | PlayerTag                                    | pozycja gracza, którego jest teraz ruch                     |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
| Declarer     | PlayerTag                                    | pozycja gracza  rozgrywającego                              |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|ContractColor | ContractColor                                | kolor kontraktu                                             |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|TrickList     | List<Trick>                                  | lista pełnych lew w rozdaniu                                |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|currentTrick  | Trick                                        | aktualnie rozgrywana lewa                                   |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+

.. code-block:: C#

    public PlayerTag CurrentPlayer;
    public PlayerTag Declarer;
    public ContractColor ContractColor;
    public List<Trick> TrickList;
    public Trick currentTrick;

Konstruktory
============

.. sphinxsharp:method:: GameInfo(ContractColor ContractColor, PlayerTag Declarer)
    :param(1): ContractColor kolor kontraktu.
    :param(2): Declarer pozycja gracza rozgrywającego.

.. code-block:: C#

    public GameInfo(ContractColor ContractColor, PlayerTag Declarer)
    {
        this.CurrentPlayer = NextPlayer(Declarer);
        this.TrickList = new List<Trick>();
        this.currentTrick = new Trick();
        this.ContractColor = ContractColor;
        this.Declarer = Declarer;
    }

Metody
======

.. sphinxsharp:method:: PlayerTag NextPlayer(PlayerTag CurrentPlayer)
    :param(1): CurrentPlayer pozycja gracza, któego jest aktualnie ruch.
    :returns: PlayerTag pozycja następnego gracza

    Metoda NextPlayer przyjmuje jako argument pozycję aktualnego gracza.
    Zwraca pozycję następnego np N->E

    .. code-block:: C#

        private PlayerTag NextPlayer(PlayerTag CurrentPlayer)
        {
            int ID = (int)(CurrentPlayer);
            if (ID == 3)
            {
                return (PlayerTag)(0);
            }
            else
            {
                return (PlayerTag)(ID + 1);
            }
        }

.. sphinxsharp:method:: public bool NextCard(Card Card)
    :param(1): Card grana karta.
    :returns: bool, czy karta została położona

    Metoda NextCard przyjmuje jako argument kartę.
    Jeżeli można ją położyć kładzie, zmienia status i zwraca True,
    jeśli nie zwraca False.

    .. code-block:: C#

        public bool NextCard(Card Card)
        {
            if (Card.PlayerID != this.CurrentPlayer)
            {
                throw new WrongPlayerException();
            }
 
            currentTrick.NextCard(Card, this.ContractColor);
            if (currentTrick.GetCount() == 4)
            {
                TrickList.Add(currentTrick);
                this.CurrentPlayer = currentTrick.Winner;
                for (int i = 0; i < 4; i++)
                {
                    currentTrick.CardList[i].CurrentState = CardState.DISPOSED;
                }
                currentTrick = new Trick();
            }
            else
            {
                this.CurrentPlayer = NextPlayer(this.CurrentPlayer);
            }

            Card.CurrentState = CardState.ON_TABLE;
            return true;
        }

.. sphinxsharp:method:: public bool IsEnd()
    :returns: bool czy rozdanie się zakończyło (pełne 13 lewych)

    Metoda IsEnd zwraca informację, czy rozdanie jest zakończone.

    .. code-block:: C#

        public bool IsEnd()
        {
            if (TrickList.Count == 13)
            {
                return true;
            }
            else
            {
                return false;
            }

        }