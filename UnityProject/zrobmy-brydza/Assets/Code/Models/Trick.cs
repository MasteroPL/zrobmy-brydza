using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Models
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
            PlayerTag CurrentWinner = StrongestCard.playerID;

            for(int i = 1; i <= 4; i++)
            {
                Card CurrentCard = CardList[i];
                if (CurrentCard.color == StrongestCard.color)
                {
                    if (CurrentCard.figure > StrongestCard.figure)
                    {
                        StrongestCard = CurrentCard;
                        CurrentWinner = StrongestCard.playerID;
                    }
                }
                else
                {
                    if ((int)(CurrentCard.color) == (int)(ContractColor))
                    {
                        StrongestCard = CurrentCard;
                        CurrentWinner = StrongestCard.playerID;
                    }
                }
            }
        }
    }
}
