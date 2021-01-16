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
        public int[] PointsNS; // 0 - pod kreską; 1 - nad kreską
        public int[] PointsWE;
        public int RoundsNS = 0;
        public int RoundsWE = 0;
        public PointsHistory History;
        public Match() {
            this.PlayerList = new List<Player>();
            this.BiddingList = new List<Bidding>();
            this.GameList = new List<GameInfo>();
            this.GameState = (GameState)(0);
            this.PointsNS = new int[2];
            this.PointsWE = new int[2];
            this.PointsNS[0] = 0;
            this.PointsNS[1] = 0;
            this.PointsWE[0] = 0;
            this.PointsWE[1] = 0;
            this.History = new PointsHistory();
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
                        this.GameState = (GameState)(1);
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

        public bool RemovePlayer(Player RPlayer)
        {
            int Index1 = PlayerList.FindIndex((Player) => { return Player.Name == RPlayer.Name; });
            if (Index1 != -1)
            {
                PlayerList.RemoveAt(Index1);
                this.GameState = (GameState)(0);
                return true;
            }
            else
            {
                throw new WrongPlayerException();
            }
        }
        public PlayerTag NextPlayer(PlayerTag CurrentPlayer)
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
            if (this.GameState == (GameState)(1))
            {
                this.GameState = (GameState)(2);
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
            if ((int)this.GameState == 2)
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

        public bool AddBid(Contract Contract)
        {
            bool X = Contract.XEnabled;
            bool XX = Contract.XXEnabled;
            if ((int)this.GameState != 2)
            {
                throw new WrongGameStateException();
            }
            bool GoodBid = CurrentBidding.AddBid(Contract, X, XX);
            if (GoodBid)
            {
                if (CurrentBidding.IsEnd())
                {
                    BiddingList.Add(CurrentBidding);
                    this.GameState = (GameState)(3);
                    CurrentGame = new GameInfo(CurrentBidding.HighestContract.ContractColor, CurrentBidding.Declarer);
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
                    this.GameState = (GameState)(2);
                    this.AddPoints(CurrentGame);
                    this.CheckPoints();
                    StartBidding();
                    return true;
                }
                return true;
            }
            throw new UnexpectedFunctionEndException();
        }
        public void AddPoints(GameInfo Game)
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
                    this.GameState = (GameState)(5);
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
                bool noColor = true;
                for (int i = 0; i < 13; i++)
                {
                    if(PlayerList[(int)CurrentGame.CurrentPlayer].Hand[i].Color == TrickColor && PlayerList[(int)CurrentGame.CurrentPlayer].Hand[i].CurrentState == (CardState)(1))
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
