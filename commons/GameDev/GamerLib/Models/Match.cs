using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using GameManagerLib.Exceptions;

namespace GameManagerLib.Models
{
    public class Match
    {
        public List<Player> PlayerList;
        public GameState GameState;
        public PlayerTag Dealer;
        public Bidding CurrentBidding;
        public List<Bidding> BiddingList;
        public GameInfo CurrentGame;
        public List<GameInfo> GameList;
        /// <summary>
        /// 0 - pod kreską
        /// 1 - nad kreską
        /// </summary>
        public int[] PointsNS; // 0 - pod kreską; 1 - nad kreską
        /// <summary>
        /// 0 - pod kreską
        /// 1 - nad kreską
        /// </summary>
        public int[] PointsWE;
        public int RoundsNS = 0;
        public int RoundsWE = 0;
        public PointsHistory History;
        public Card[][] PlayerHandsCache = {
            null, null, null, null
        };
        public bool EnableCardsShufflingAndDistributing = true;

        public Match(bool enableCardsShufflingAndDistributing = true) {
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
            this.EnableCardsShufflingAndDistributing = enableCardsShufflingAndDistributing;
        }

        public Player GetPlayerAt(PlayerTag placeTag) {
            int index = PlayerList.FindIndex((player) => {
                return player.Tag == placeTag;
            });
            if(index != -1) {
                return PlayerList[index];
            }
            return null;
        }

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
                        if (this.GameState == GameState.AWAITING_PLAYERS) {
                            this.GameState = GameState.STARTING;
                            this.Dealer = NewPlayer.Tag;
                        }
                    }

                    if (PlayerHandsCache[(int)NewPlayer.Tag] != null) {
                        NewPlayer.Hand = PlayerHandsCache[(int)NewPlayer.Tag];
                    }
                    else {
                        PlayerHandsCache[(int)NewPlayer.Tag] = NewPlayer.Hand;
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

        public Player GetPlayerByUsername(string username) {
            int index = PlayerList.FindIndex((Player) => { return Player.Name == username; });

            if(index == -1) {
                return null;
            }
            return PlayerList[index];
        }

        public bool RemovePlayer(Player RPlayer)
        {
            int Index1 = PlayerList.FindIndex((Player) => { return Player.Name == RPlayer.Name; });
            if (Index1 != -1)
            {
                PlayerList.RemoveAt(Index1);
                if (this.GameState == GameState.STARTING) {
                    this.GameState = GameState.AWAITING_PLAYERS;
                }
                return true;
            }
            else
            {
                throw new WrongPlayerException();
            }
        }
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

        public bool Start()
        {
            bool isReady = IsGameReadyToStart();
            Console.WriteLine("isReady: {0}", isReady);
            if (isReady)
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
        public bool StartBidding()
        {
            if (this.GameState == GameState.BIDDING)
            {
                if (EnableCardsShufflingAndDistributing) {
                    List<Card> deck = GenerateDeck();
                    CardShuffler<Card> Shuffler = new CardShuffler<Card>(deck);
                    List<Card> ShuffledCards = Shuffler.Shuffle();

                    // metoda "GetShuffledPlayersCards" zostala stworzona specjalnie na potrzeby brydza - Shuffler jest napisany by tasowac dowolnego typu obiekty 
                    List<List<Card>> PlayersCards = Shuffler.GetShuffledPlayersCards(ShuffledCards);
                    var tmp = new Card[][] {
                        PlayersCards[0].OrderBy(c => c.Figure).OrderBy(c => c.Color).ToArray(),
                        PlayersCards[1].OrderBy(c => c.Figure).OrderBy(c => c.Color).ToArray(),
                        PlayersCards[2].OrderBy(c => c.Figure).OrderBy(c => c.Color).ToArray(),
                        PlayersCards[3].OrderBy(c => c.Figure).OrderBy(c => c.Color).ToArray()
                    };

                    for (int i = 0; i < 4; i++) {
                        for (int j = 0; j < 13; j++) {
                            PlayerList[i].Hand[j] = tmp[i][j];
                        }
                    }

                    for (int i = 0; i < 4; i++) {
                        foreach (Card card in PlayerList[i].Hand) {
                            card.PlayerID = PlayerList[i].Tag;
                        }
                    }
                }

                CurrentBidding = new Bidding(this.Dealer);
                this.Dealer = this.NextPlayer(Dealer);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ClearPlayerHands() {
            for(int i = 0; i < PlayerList.Count; i++) {
                for(int j = 0; j < 13; j++) {
                    PlayerList[i].Hand[j] = null;
                }
            }
        }

        private List<Card> GenerateDeck()
        {
            List<Card> cards = new List<Card>();
            Card tmp;
            if (GameList.Count > 0)
            {
                for(int i = 0; i < GameList[GameList.Count - 1].TrickList.Count; i++)
                {
                    for(int j = 0; j < GameList[GameList.Count - 1].TrickList[i].CardList.Count; j++)
                    {
                        tmp = new Card(GameList[GameList.Count - 1].TrickList[i].CardList[j].Figure, GameList[GameList.Count - 1].TrickList[i].CardList[j].Color, PlayerTag.NOBODY);
                        tmp.CurrentState = (CardState)(1);
                        cards.Add(tmp);
                    }
                }
            }
            else
            {
                for (int i = 0; i < Enum.GetNames(typeof(CardColor)).Length; i++)
                {
                    for (int j = 2; j <= 14; j++)
                    {
                        tmp = new Card((CardFigure)j, (CardColor)i, PlayerTag.NOBODY);
                        tmp.CurrentState = (CardState)(1);
                        cards.Add(tmp);
                    }
                }
            }
            return cards;
        }

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


        private bool PutNextCard(Card Card)
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

                    // Gra się skończyła a to mi następną lictację zaczynało. REALLY?
                    if (GameState != GameState.GAME_FINISHED) {
                        StartBidding();
                    }

                    return true;
                }
                return true;
            }
            throw new UnexpectedFunctionEndException();
        }

        private bool CheckPutNextCard(Card Card)
        {
            if ((int)this.GameState != 3)
            {
                return false;
            }
            if (PlayableCard(Card) == false)
            {
                return false;
            }
            return CurrentGame.CheckNextCard(Card);
        }

        public bool CheckNextCard(PlayerTag playerTag, CardColor cardColor, CardFigure cardFigure)
        {

            int playerIndex = PlayerList.FindIndex((Player) => { return Player.Tag == playerTag; });

            for (int i = 0; i < 13; i++)
            {
                if (PlayerList[playerIndex].Hand[i].Color == cardColor && PlayerList[playerIndex].Hand[i].Figure == cardFigure)
                { 
                    return CheckPutNextCard(PlayerList[playerIndex].Hand[i]);
                }
            }
            return false;
        }

        public bool NextCard(PlayerTag playerTag, CardColor cardColor, CardFigure cardFigure)
        {

            int playerIndex = PlayerList.FindIndex((Player) => { return Player.Tag == playerTag; });

            for (int i = 0; i < 13; i++)
            {
                if (PlayerList[playerIndex].Hand[i].Color == cardColor && PlayerList[playerIndex].Hand[i].Figure == cardFigure)
                {
                    PutNextCard(PlayerList[playerIndex].Hand[i]);
                    return true;
                }
            }
            throw new WrongCardException();
        }

        private void AddPoints(GameInfo Game)
        {
            PlayerTag declarer = Game.Declarer;
            int contractHigh = (int)this.CurrentBidding.HighestContract.ContractHeight;
            int declarerTricks = 0;
            for (int i = 0; i < 13; i++)
            {
                if (IsTheSameTeam(Game.TrickList[i].Winner, declarer))
                {
                    declarerTricks++;
                }
            }
            int result = declarerTricks - contractHigh - 6;
            int multiplier = 1;
            if (this.CurrentBidding.HighestContract.XXEnabled)
            {
                multiplier = 4;
            }
            else
            {
                if (this.CurrentBidding.HighestContract.XEnabled)
                {
                    multiplier = 2;
                }
            }
            if (result >= 0)
            {
                if ((int)Game.ContractColor == 0 || (int)Game.ContractColor == 1)
                {
                    if((int)declarer == 0 || (int)declarer == 2)
                    {
                        this.PointsNS[1] += result * multiplier * 20;
                        this.PointsNS[0] += contractHigh * multiplier * 20;

                        this.History.AddNSHistory(contractHigh * multiplier * 20, result * multiplier * 20);
                    }
                    else
                    {
                        this.PointsWE[1] += result * multiplier * 20;
                        this.PointsWE[0] += contractHigh * multiplier * 20;

                        this.History.AddWEHistory(contractHigh * multiplier * 20, result * multiplier * 20);
                    }


                }

                if ((int)Game.ContractColor == 2 || (int)Game.ContractColor == 3)
                {
                    if ((int)declarer == 0 || (int)declarer == 2)
                    {
                        this.PointsNS[1] += result * multiplier * 30;
                        this.PointsNS[0] += contractHigh * multiplier * 30;

                        this.History.AddNSHistory(contractHigh * multiplier * 30, result * multiplier * 30);
                    }
                    else
                    {
                        this.PointsWE[1] += result * multiplier * 30;
                        this.PointsWE[0] += contractHigh * multiplier * 30;

                        this.History.AddWEHistory(contractHigh * multiplier * 30, result * multiplier * 30);
                    }
                }

                if ((int)Game.ContractColor == 4)
                {
                    if ((int)declarer == 0 || (int)declarer == 2)
                    {
                        this.PointsNS[1] += result * multiplier * 30;
                        this.PointsNS[0] += contractHigh * multiplier * 30 + 10;

                        this.History.AddNSHistory(contractHigh * multiplier * 30 + 10, result * multiplier * 30);
                    }
                    else
                    {
                        this.PointsWE[1] += result * multiplier * 30;
                        this.PointsWE[0] += contractHigh * multiplier * 30 + 10;

                        this.History.AddWEHistory(contractHigh * multiplier * 30 + 10, result * multiplier * 30);
                    }
                }

                if(contractHigh == 6)
                {
                    if ((int)declarer == 0 || (int)declarer == 2)
                    {
                        this.PointsNS[1] += 500;

                        this.History.AddNSHistory(0, 500);
                    }
                    else
                    {
                        this.PointsWE[1] += 500;

                        this.History.AddWEHistory(0, 500);
                    }
                }
                if (contractHigh == 7)
                {
                    if ((int)declarer == 0 || (int)declarer == 2)
                    {
                        this.PointsNS[1] += 750;

                        this.History.AddNSHistory(0, 750);
                    }
                    else
                    {
                        this.PointsWE[1] += 750;

                        this.History.AddWEHistory(0, 750);
                    }
                }
            }
            else
            {
                if((int)declarer == 0 || (int)declarer == 2)
                {
                    this.PointsWE[1] += (-result) * multiplier * 50;

                    this.History.AddWEHistory(0, (-result) * multiplier * 50);
                }
                else
                {
                    this.PointsNS[1] += (-result) * multiplier * 50;

                    this.History.AddNSHistory(0, (-result) * multiplier * 50);
                }
            }

        }

        private void CheckPoints()
        {
            if(this.PointsNS[0] >= 100)
            {
                this.PointsNS[1] += this.PointsNS[0];
                this.PointsNS[0] = 0;
                this.PointsWE[1] += this.PointsWE[0];
                this.PointsWE[0] = 0;
                this.RoundsNS += 1;
                if( this.RoundsNS == 2)
                {
                    if(this.RoundsWE == 0)
                    {
                        this.PointsNS[1] += 750;
                    }
                    else
                    {
                        this.PointsNS[1] += 500;
                    }
                    this.GameState = GameState.GAME_FINISHED;
                }
                this.History.Round();
            }

            if (this.PointsWE[0] >= 100)
            {
                this.PointsWE[1] += this.PointsWE[0];
                this.PointsWE[0] = 0;
                this.PointsNS[1] += this.PointsNS[0];
                this.PointsNS[0] = 0;
                this.RoundsWE += 1;
                if (this.RoundsWE == 2)
                {
                    if (this.RoundsNS == 0)
                    {
                        this.PointsWE[1] += 750;
                    }
                    else
                    {
                        this.PointsWE[1] += 500;
                    }
                    this.GameState = (GameState)(5);
                }
                this.History.Round();
            }
            
        }

        private bool IsTheSameTeam(PlayerTag Player1, PlayerTag Player2)
        {
            if (Player1 == Player2)
            {
                return true;
            }
            else
            {
                Player1 = NextPlayer(NextPlayer(Player1));
                if (Player1 == Player2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        private bool PlayableCard(Card Card)
        {
            if ((int)Card.CurrentState != 1)
            {
                return false;
            }
            if (Card.PlayerID != this.CurrentGame.CurrentPlayer)
            {
                return false;
            }
            if (this.CurrentGame.currentTrick.CardList.Count == 0)
            {
                return true;
            }
            
            
            CardColor TrickColor = this.CurrentGame.currentTrick.CardList[0].Color;
            if(Card.Color == TrickColor)
            {
                return true;
            }
            else
            {
                var player = GetPlayerAt(CurrentGame.CurrentPlayer);
                bool noColor = true;
                for (int i = 0; i < 13; i++)
                {
                    if(player.Hand[i].Color == TrickColor && player.Hand[i].CurrentState == CardState.ON_HAND)
                    {
                        noColor = false;
                    }
                }
                if(noColor)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CheckAddPlayer(Player NewPlayer)
        {
            int Index1 = PlayerList.FindIndex((Player) => { return Player.Name == NewPlayer.Name; });
            if (Index1 == -1)
            {
                int Index2 = PlayerList.FindIndex((Player) => { return Player.Tag == NewPlayer.Tag; });
                if (Index2 == -1)
                {
                    if (this.PlayerList.Count == 4)
                    {
                        return false;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool CheckRemovePlayer(Player RPlayer)
        {
            int Index1 = PlayerList.FindIndex((Player) => { return Player.Name == RPlayer.Name; });
            if (Index1 != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckStart()
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
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public bool IsGameReadyToStart()
        {
            return GameState == GameState.STARTING && PlayerList.Count == 4;
        }

        public bool CheckAddBid(Contract Contract)
        {
            bool X = Contract.XEnabled;
            bool XX = Contract.XXEnabled;
            if (this.GameState != GameState.BIDDING)
            {
                return false;
            }
            bool GoodBid = CurrentBidding.CheckAddBid(Contract, X, XX);
            if (GoodBid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckStartBidding()
        {
            if (this.GameState == GameState.BIDDING)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public class PointsHistory {
            public List<String> NSHistory;
            public List<String> WEHistory;

            public PointsHistory()
            {
                this.NSHistory = new List<String>();
                this.WEHistory = new List<String>();
            }

            public void AddNSHistory(int pod, int nad)
            {
                this.NSHistory.Add(nad.ToString() + "|" + pod.ToString());
                this.WEHistory.Add("0|0");
            }

            public void AddWEHistory(int pod, int nad)
            {
                this.WEHistory.Add(nad.ToString() + "|" + pod.ToString());
                this.NSHistory.Add("0|0");
            }

            public void Round() {
                this.WEHistory.Add("Round");
                this.NSHistory.Add("Round");
            }
        }

    }
    public enum GameState
    {
        AWAITING_PLAYERS = 0,
        STARTING = 1,
        BIDDING = 2,
        PLAYING = 3,
        PAUSED = 4,
        GAME_FINISHED = 5
    }
}
