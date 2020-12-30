using Models;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Models
{
    public class GameInfo
    {
        public PlayerTag CurrentPlayer { get; set; }
        public PlayerTag Declarer;
        public List<Player> PlayerList;
        public ContractColor ContractColor;
        public List<Trick> TrickList;
        public Trick currentTrick;

        public GameInfo(ContractColor ContractColor, PlayerTag Declarer)
        {
            this.CurrentPlayer = NextPlayer(Declarer);
            this.PlayerList = new List<Player>();
            this.TrickList = new List<Trick>();
            this.currentTrick = new Trick();
            this.ContractColor = ContractColor;
            this.Declarer = Declarer;
        }
        private void SetPlayer(PlayerTag NextPlayer)
        {
            CurrentPlayer = NextPlayer;
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
                return true;
            }
            else
            {
                return false;
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

        public bool NextCard(Card Card)
        {
            if (Card.PlayerID != CurrentPlayer)
            {
                return false;
            }
            currentTrick.NextCard(Card, this.ContractColor);
            if (currentTrick.GetCount() == 4)
            {
                TrickList.Add(currentTrick);
                this.CurrentPlayer = currentTrick.Winner;
                currentTrick = new Trick();
            }
            else
            {
                this.CurrentPlayer = NextPlayer(this.CurrentPlayer);
            }

            return true;
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