using Models;
using System;
using System.Collections;
using System.Collections.Generic;
namespace Models
{
    public class GameInfo
    {
        public PlayerTag CurrentPlayer;
        public PlayerTag Declarer;
        public ContractColor ContractColor;
        public List<Trick> TrickList;
        public Trick currentTrick;

        public GameInfo(ContractColor ContractColor, PlayerTag Declarer)
        {
            this.CurrentPlayer = NextPlayer(Declarer);
            this.TrickList = new List<Trick>();
            this.currentTrick = new Trick();
            this.ContractColor = ContractColor;
            this.Declarer = Declarer;
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
}