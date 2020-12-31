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
        public Player Dealer;

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
                        this.Dealer = NewPlayer;
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

        public bool Start()
        {
            if (this.GameState == (GameState)(1))
            {
                this.GameState = (GameState)(2);
                return true;
            }
            else
            {
                return false;
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
