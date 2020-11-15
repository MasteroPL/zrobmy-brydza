using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auction : MonoBehaviour
{
    public class Bet
    {
        public int Number; //1-7
        public int Suit; //1-5
        public int Player; //1-4
    }
    
    private Bet HighestBet;

    public bool MakeBet(Bet bet) {
        if (bet.Number > HighestBet.Number || (bet.Number == HighestBet.Number && bet.Suit > HighestBet.Suit)) {
            HighestBet.Number = bet.Number;
            HighestBet.Suit = bet.Suit;
            HighestBet.Player = bet.Player;
            return true;
        }
        else
        {
            return false;
        }
    }
}
