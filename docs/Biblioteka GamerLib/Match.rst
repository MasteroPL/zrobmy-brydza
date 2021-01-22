########
Match
########

Definicja
============

Klasa reprezentująca rozgrywkę od stworzenia stołu do wygrania 2 rund przez którąś ze stron.

Pola
======


+--------------+----------------------------------------------+-------------------------------------------------------------+
| Pole         | Typ                                          | Opis                                                        |
+==============+==============================================+=============================================================+
|PlayerList    | List<Player>                                 | lista graczy przy stole                                     |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|GameState     | GameState                                    | stan rozgrywki                                              |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
| Dealer       | PlayerTag                                    | pozycja gracza  rozdającego                                 |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|CurrentBidding| Bidding                                      | aktualny najwyższa odzywka                                  |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|BiddingList   | List<Trick>                                  | lista poprzednich odzywek                                   |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|CurrentGame   | GameInfo                                     | aktualnie rozgrywane rozdanie                               |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|GameList      | List<GameInfo>                               | lista poprzednich rozdań                                    |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|PointsNS      | int[]                                        | liczba punktów pary NS                                      |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|PointsWE      | int[]                                        | liczba punktów pary WE                                      |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|RoundsNS      | int                                          | liczba rund ugranych przez parę NS                          |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|RoundsWE      | int                                          | liczba rund ugranych przez parę NS                          |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+
|History       | PointsHistory                                | historia punktacji                                          |
|              |                                              |                                                             |
|              |                                              |                                                             |
+--------------+----------------------------------------------+-------------------------------------------------------------+

.. code-block:: C#

    public List<Player> PlayerList;
    public GameState GameState;
    public PlayerTag Dealer;
    public Bidding CurrentBidding;
    public List<Bidding> BiddingList;
    public GameInfo CurrentGame;
    public List<GameInfo> GameList;
    public int[] PointsNS; // 0 - pod kreską; 1 - nad kreską
    public int[] PointsWE;
    public int RoundsNS = 0;
    public int RoundsWE = 0;
    public PointsHistory History;

Konstruktory
============

.. sphinxsharp:method:: public Match()

.. code-block:: C#

    public Match() {
            this.PlayerList = new List<Player>();
            this.BiddingList = new List<Bidding>();
            this.GameList = new List<GameInfo>();
            this.GameState = GameState.AWAITING_PLAYERS;
            this.PointsNS = new int[2];
            this.PointsWE = new int[2];
            this.PointsNS[0] = 0;
            this.PointsNS[1] = 0;
            this.PointsWE[0] = 0;
            this.PointsWE[1] = 0;
            this.History = new PointsHistory();
        }

Metody
=========

.. sphinxsharp:method:: public bool AddPlayer(Player NewPlayer)
    :param(1): NewPlayer nowy gracz

    Metoda AddPlayer dodaje gracza.

    .. code-block:: C#

        public bool AddPlayer(Player NewPlayer)
        {
            int Index1 = PlayerList.FindIndex((Player) => { return Player.Name == NewPlayer.Name; });
            if (Index1 == -1)
            {
                int Index2 = PlayerList.FindIndex((Player) => { return Player.Tag == NewPlayer.Tag; });
                if (Index2 == -1)
                {
                    PlayerList.Add(NewPlayer);
                    if (this.PlayerList.Count == 4) 
                    {
                        this.GameState = GameState.STARTING;
                        this.Dealer = NewPlayer.Tag;
                    }

                    return true;
                }
                else
                {
                    throw new SeatTakenException();
                }
            }
            else
            {
                throw new DuplicatedPlayerNameException();
            }
        }

    Wyjątki:
        :SeatTakenException: Rzucany, jeśli miejsce które próbuje zająć nowy gracz jest zajętę.
        :DuplicatedPlayerNameException: Rzucany, jeśli nazwa nowego gracza jest już zajęta. 

.. sphinxsharp:method:: public bool RemovePlayer(Player RPlayer)
    :param(1): RPlayer gracz którego usuwamy
    :returns: bool czy udało się usunąć gracza

    Metoda RemovePlayer usuwa gracza.

    .. code-block:: C#

        public bool RemovePlayer(Player RPlayer)
        {
            int Index1 = PlayerList.FindIndex((Player) => { return Player.Name == RPlayer.Name; });
            if (Index1 != -1)
            {
                PlayerList.RemoveAt(Index1);
                this.GameState = GameState.AWAITING_PLAYERS;
                return true;
            }
            else
            {
                throw new WrongPlayerException();
            }
        }

    Wyjątki:
        :WrongPlayerException: Rzucany, jeśli gracz nie istnieje.

.. sphinxsharp:method:: private PlayerTag NextPlayer(PlayerTag CurrentPlayer)
    :param(1): CurrentPlayer gracz, którego aktualnie jest ruch
    :returns: PlayerTag pozycja następnego gracza

    Metoda NextPlayer wyznacza następnego gracza (na prawo od aktualnego).

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

.. sphinxsharp:method:: public bool Start()
    :returns: bool czy udało się zacząć rozgrywkę

    Metoda Start ustawia stan rozgrywki na BIDDING.

    .. code-block:: C#

        public bool Start()
        {
            if (this.GameState == GameState.STARTING)
            {
                this.GameState = GameState.BIDDING;
                if (this.StartBidding())
                {
                    return true;
                }
                else
                {
                    throw new WrongGameStateException();
                }
            }
            else
            {
                throw new WrongGameStateException();
            }

        }

    Wyjątki:
        :WrongGameStateException: Rzucany, jeśli stan gry jest nieodpowiedni (przy wywołaniu inny niż STARTING).

.. sphinxsharp:method:: private bool StartBidding()
    :returns: bool czy udało się zacząć licytację

    Metoda StartBidding rozdaje karty i rozpoczyna licytację.

    .. code-block:: C#

        private bool StartBidding()
        {
            if (this.GameState == GameState.BIDDING)
            {
                // kijowe rozdawanie kart
                int a = 2;
                int b = 0;
                for( int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        PlayerList[i].Hand[j] = new Card((CardFigure)a, (CardColor)b, PlayerList[i].Tag);
                        PlayerList[i].Hand[j].CurrentState = (CardState)(1);
                        a++;
                        b++;
                        if(a == 15)
                        {
                            a = 2;
                        }
                        if(b == 4)
                        {
                            b = 0;
                        }
                    }
                }
                //TODO tu trza porządnie rozdać karty
                CurrentBidding = new Bidding(this.Dealer);
                this.Dealer = this.NextPlayer(Dealer);
                return true;
            }
            else
            {
                return false;
            }
        }

.. sphinxsharp:method:: public bool AddBid(Contract Contract)
    :param(1): Contract deklarowana odzywka
    :returns: bool czy udało się zadeklarować odzywkę

    Metoda AddBid dodaje odzywkę.

    .. code-block:: C#

        public bool AddBid(Contract Contract)
        {
            bool X = Contract.XEnabled;
            bool XX = Contract.XXEnabled;
            if (this.GameState != GameState.BIDDING)
            {
                throw new WrongGameStateException();
            }
            bool GoodBid = CurrentBidding.AddBid(Contract, X, XX);
            if (GoodBid)
            {
                if (CurrentBidding.IsEnd())
                {
                    BiddingList.Add(CurrentBidding);
                    if (CurrentBidding.HighestContract.ContractColor != ContractColor.NONE)
                    {
                        this.GameState = GameState.PLAYING;
                        CurrentGame = new GameInfo(CurrentBidding.HighestContract.ContractColor, CurrentBidding.Declarer);
                    }
                    else
                    {
                        this.GameState = GameState.BIDDING;
                        StartBidding();
                    }
                    return true;

                }
                CurrentBidding.CurrentPlayer = CurrentBidding.NextPlayer(CurrentBidding.CurrentPlayer);
                return true;
            }
            else
            {
                throw new WrongBidException();
            }
        }

    Wyjątki:
        :WrongGameStateException: Rzucany, jeśli stan gry jest nieodpowiedni.
        :WrongBidException: Rzucany, jeśli odzywka jest nieprawidłowa.

.. sphinxsharp:method:: public bool NextCard(Card Card)
    :param(1): Card grana karta.
    :returns: bool, czy karta została położona

    Metoda NextCard przyjmuje jako argument kartę.
    Jeżeli można ją położyć kładzie (poprzez analogiczną funkcję z klasy GameInfo), zmienia status i zwraca True,
    jeśli nie zwraca False.

    .. code-block:: C#

        public bool NextCard(Card Card)
        {
            if ((int)this.GameState != 3)
            {
                throw new WrongGameStateException();
            }
            if (PlayableCard(Card) == false)
            {
                throw new WrongCardException();
            }
            if (CurrentGame.NextCard(Card)) 
            {
                if (CurrentGame.IsEnd())
                {
                    GameList.Add(CurrentGame);
                    this.GameState = GameState.BIDDING;
                    this.AddPoints(CurrentGame);
                    this.CheckPoints();
                    StartBidding();
                    return true;
                }
                return true;
            }
            throw new UnexpectedFunctionEndException();
        }

    Wyjątki:
        :WrongGameStateException: Rzucany, jeśli stan gry jest nieodpowiedni.
        :WrongCardException: Rzucany, jeśli nie można zagrać danej karty.
        :UnexpectedFunctionEndException: Rzucany jeżeli zadrzy się coś nieprzewidzianego.

.. sphinxsharp:method:: private void AddPoints(GameInfo Game)
    :param(1): Game właśnie zakończone rozdanie.

    Metoda AddPoints dodaje punkty po zakończeniu rozdania.


.. sphinxsharp:method:: private void CheckPoints()

    Sprawdza, czy któraś z dróżyn posiada 100 punktów pod kreską, jeżeli tak to sumuje punkty i zapisuje je nad kreską
    oraz dodaje drużynie, która zdobyła 100 punktów zdobytą rundę. Jeśli, któraś drużyna wygrała właśnie drugą rundę to
    zmienia stan gry na *GAME_FINISHED* i gra zostaje zakończona.

.. sphinxsharp:method:: private bool IsTheSameTeam(PlayerTag Player1, PlayerTag Player2)
    :param(1): Pierwszy grasz.
    :param(2): Drugi gracz.
    :returns: True, jeżeli gracze są z jednej drużyny.


.. sphinxsharp:method:: private bool PlayableCard(Card Card)
    :param(1): Karta, która ma zostać zweryfikowana.
    :returns: True, jeżli karta może zostać wyłożona.

    Sprawdza, czy karta została wyłożona przez odpowiedniego gracza oraz czy jest odpowiedniego koloru.

Klasy wewnętrzne
================

.. class::
    PointsHistory

    .. code-block:: C#

        public List<String> NSHistory; // historia punktacji drużyny NS
        public List<String> WEHistory; // historia punktacji drużyny WE
        public PointsHistory()
        {
            this.NSHistory = new List<String>();
            this.WEHistory = new List<String>();
        }
    .. sphinxsharp:method:: public void AddNSHistory(int pod, int nad)
        :param(1): Punkty pod kreską.
        :param(2): Punkty nad kreską.

        Dodaje punkty do historii drużyny NS.
    
    .. sphinxsharp:method:: public void AddWEHistory(int pod, int nad)
        :param(1): Punkty pod kreską.
        :param(2): Punkty nad kreską.

        Dodaje punkty do historii drużyny WE.

    .. sphinxsharp:method:: public void Round()

        Dodaje informację o zakończonej rundzie.



Typy wyliczeniowe
======================

Typ wyliczeniowy GameState definiuje możliwe stany gry.

.. code-block:: C#

    public enum GameState
    {
        AWAITING_PLAYERS = 0,
        STARTING = 1,
        BIDDING = 2,
        PLAYING = 3,
        PAUSED = 4,
        GAME_FINISHED = 5
    }