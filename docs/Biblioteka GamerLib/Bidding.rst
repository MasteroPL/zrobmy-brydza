##############
Biddging
##############

******************
Dokumentacja klasy
******************

.. class::
    Biddging

Obiekt klasy Bidding obsługuje przebieg pojedynczej licytacji.

Pola
======

.. code-block:: C#

    public PlayerTag CurrentPlayer;
    public List<Contract> ContractList;
    public Contract HighestContract;
    public PlayerTag Dealer;
    private int PassCounter = 0;
    private bool End = false;
    public PlayerTag Declarer;

    // Tablice mówiące, kto jaki kolor jako pierwszy deklarował
    // 0 - C
    // 1 - D
    // 2 - H
    // 3 - S
    // 4 - NT
    private PlayerTag[] NS = new PlayerTag[5];
    private PlayerTag[] WE = new PlayerTag[5];

Konstruktor
============

.. sphinxsharp:method:: public Bidding(PlayerTag Dealer)
    :param(1): Gracz, który rozdawał karty (następny po nim rozpoczyna licytację).
    
    .. code-block:: C#

        public Bidding(PlayerTag Dealer)
		{
			this.Dealer = Dealer;
			this.ContractList = new List<Contract>();
			this.CurrentPlayer = NextPlayer(this.Dealer);
			this.HighestContract = null;
			for( int i = 0; i <5; i++)
            {
				NS[i] = (PlayerTag)(-1);
				WE[i] = (PlayerTag)(-1);
			}
		}


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

.. sphinxsharp:method:: public bool IsEnd()
    :returns: Inforamcja o tym czy licytacja się zakończyła.


.. sphinxsharp:method:: private bool IsTheSameTeam(PlayerTag Player1, PlayerTag Player2)
    :param(1): Pierwszy grasz.
    :param(2): Drugi gracz.
    :returns: True, jeżeli gracze są z jednej drużyny.

.. sphinxsharp:method:: private void SetColor(PlayerTag PlayerTag, ContractColor Color)
    :param(1): Gracz, który zalicytował dany kolor.
    :param(2): Zalicytowany kolor.
    
    Sprawdza czy dany kolor został zalicytowany w drużynie gracza pierwszy raz w tej licytacji, 
    jeżeli tak to zapamiętuje kto go zalicytował.