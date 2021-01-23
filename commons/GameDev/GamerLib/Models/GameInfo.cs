using System;
using System.Collections;
using System.Collections.Generic;
using GameManagerLib.Exceptions;
namespace GameManagerLib.Models
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
            if (Card.PlayerID != this.CurrentPlayer)
            {
                throw new WrongPlayerException();
            }
 
            currentTrick.NextCard(Card, this.ContractColor);
            if (currentTrick.GetCount() == 4)
            {
                TrickList.Add(currentTrick);
                this.CurrentPlayer = currentTrick.Winner;
                for (int i = 0; i < 4; i++)
                {
                    currentTrick.CardList[i].CurrentState = CardState.DISPOSED;
                }
                currentTrick = new Trick();
            }
            else
            {
                this.CurrentPlayer = NextPlayer(this.CurrentPlayer);
            }

            Card.CurrentState = CardState.ON_TABLE;
            return true;
        }
        public bool CheckNextCard(Card Card)
        {
            if (Card.PlayerID != this.CurrentPlayer)
            {
                return false;
            }

        
            if (currentTrick.GetCount() == 4)
            {
                return false;
            }
            
            return true;
        }

        public bool IsEnd()
        {
            if (TrickList.Count == 13)
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