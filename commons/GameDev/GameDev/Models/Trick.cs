using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace GameManagerLib.Models
{
    public class Trick
    {
        public PlayerTag Winner { get; set; }
        public List<Card> CardList;

        public Trick()
        {
            this.CardList = new List<Card>();
        }

        public void NextCard(Card Card, ContractColor ContractColor)
        {
            CardList.Add(Card);
            if (CardList.Count == 4)
            {
                SetWinner(ContractColor);
            }
        }

        public int GetCount()
        {
            return this.CardList.Count;
        }

        private void SetWinner(ContractColor ContractColor) {
            Card StrongestCard = CardList[0];
            PlayerTag CurrentWinner = StrongestCard.PlayerID;

            for(int i = 1; i <= 3; i++)
            {
                Card CurrentCard = CardList[i];
                if (CurrentCard.Color == StrongestCard.Color)
                {
                    if (CurrentCard.Figure > StrongestCard.Figure)
                    {
                        StrongestCard = CurrentCard;
                        CurrentWinner = StrongestCard.PlayerID;
                    }
                }
                else
                {
                    if ((int)(CurrentCard.Color) == (int)(ContractColor))
                    {
                        StrongestCard = CurrentCard;
                        CurrentWinner = StrongestCard.PlayerID;
                    }
                }
            }
            this.Winner = CurrentWinner;
        }
    }
}
