using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    class Match
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
        public Match() {
            this.PlayerList = new List<Player>();
            this.GameState = (GameState)(0);
            this.PointsNS = new int[2];
            this.PointsWE = new int[2];
            this.PointsNS[0] = 0;
            this.PointsNS[1] = 0;
            this.PointsWE[0] = 0;
            this.PointsWE[1] = 0;

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
                    return false;
                }
            }
            else
            {
                return false;
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
                return false;
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
                    return false;
                }
            }
            else
            {
                return false;
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

        public bool AddBid(Contract Contract, bool X = false, bool XX = false)
        {
            if ((int)this.GameState != 2)
            {
                return false;
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
                return false;
            }
        }

        public bool NextCard(Card Card)
        {
            if ((int)this.GameState != 3)
            {
                return false;
            }
            if (PlayableCard(Card) == false)
            {
                return false;
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
            }
            return false;
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
                    }
                    else
                    {
                        this.PointsWE[1] += result * multiplier * 20;
                        this.PointsWE[0] += contractHigh * multiplier * 20;
                    }


                }

                if ((int)Game.ContractColor == 2 || (int)Game.ContractColor == 3)
                {
                    if ((int)declarer == 0 || (int)declarer == 2)
                    {
                        this.PointsNS[1] += result * multiplier * 30;
                        this.PointsNS[0] += contractHigh * multiplier * 30;
                    }
                    else
                    {
                        this.PointsWE[1] += result * multiplier * 30;
                        this.PointsWE[0] += contractHigh * multiplier * 30;
                    }
                }

                if ((int)Game.ContractColor == 4)
                {
                    if ((int)declarer == 0 || (int)declarer == 2)
                    {
                        this.PointsNS[1] += result * multiplier * 30;
                        this.PointsNS[0] += contractHigh * multiplier * 30 + 10;
                    }
                    else
                    {
                        this.PointsWE[1] += result * multiplier * 20;
                        this.PointsWE[0] += contractHigh * multiplier * 30 + 10;
                    }
                }

                if(contractHigh == 6)
                {
                    if ((int)declarer == 0 || (int)declarer == 2)
                    {
                        this.PointsNS[1] += 500;
                    }
                    else
                    {
                        this.PointsWE[1] += 500;
                    }
                }
                if (contractHigh == 7)
                {
                    if ((int)declarer == 0 || (int)declarer == 2)
                    {
                        this.PointsNS[1] += 750;
                    }
                    else
                    {
                        this.PointsWE[1] += 750;
                    }
                }
            }
            else
            {
                if((int)declarer == 0 || (int)declarer == 2)
                {
                    this.PointsWE[1] += (-result) * multiplier * 50;
                }
                else
                {
                    this.PointsNS[1] += (-result) * multiplier * 50;
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
            if(Card.PlayerID != this.CurrentGame.CurrentPlayer)
            {
                return false;
            }
            if((int)Card.CurrentState != 1)
            {
                return false;
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
