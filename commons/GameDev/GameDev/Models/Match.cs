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

        public Match() {
            this.PlayerList = new List<Player>();
            this.GameState = (GameState)(0);
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
                    //TODO w licytacji sprawdzanie, kto dał jako pierwszy licytowany kolor
                    CurrentGame = new GameInfo(CurrentBidding.HighestContract.ContractColor, CurrentBidding.HighestContract.DeclaredBy);
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
            if (CurrentGame.NextCard(Card)) 
            {
                if (CurrentGame.IsEnd())
                {
                    GameList.Add(CurrentGame);
                    this.GameState = (GameState)(2);
                    //TODO punkty!!!!!!!!!!!!!!!
                    StartBidding();
                    return true;
                }
            }
            return false;
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
