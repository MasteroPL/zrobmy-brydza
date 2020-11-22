using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auction : MonoBehaviour
{
    public class Bet
    {
        public int Number = 0; //1-7
        public int Suit = 0; //1-5
        public int Player; //1-4
    }

    void Start() {
        this.HighestBet = new Bet();
        this.CurrentBet = new Bet();
    }

    private Bet HighestBet;
    private Bet CurrentBet;
    public bool MakeBet(Bet bet) {
        if (bet.Suit > 0 && bet.Number > 0 &&(bet.Number > HighestBet.Number || (bet.Number == HighestBet.Number && bet.Suit > HighestBet.Suit))) {
            HighestBet.Number = bet.Number;
            HighestBet.Suit = bet.Suit;
            HighestBet.Player = bet.Player;
            return true;
        }
        else
        {
            if (bet.Number == -1) {
                return true;
            }
            return false;
        }
    }

    public void one(){
        this.CurrentBet.Number = 1;
    }
    public void two()
    {
        this.CurrentBet.Number = 2;
    }
    public void three()
    {
        this.CurrentBet.Number = 3;
    }
    public void four()
    {
        this.CurrentBet.Number = 4;
    }
    public void five()
    {
        this.CurrentBet.Number = 5;
    }
    public void six()
    {
        this.CurrentBet.Number = 6;
    }
    public void seven()
    {
        this.CurrentBet.Number = 7;
    }
    public void C()
    {
        this.CurrentBet.Suit = 1;
    }
    public void D()
    {
        this.CurrentBet.Suit = 2;
    }
    public void H()
    {
        this.CurrentBet.Suit = 3;
    }
    public void S()
    {
        this.CurrentBet.Suit = 4;
    }
    public void NT()
    {
        this.CurrentBet.Suit = 5;
    }
    public void pas(){
        this.CurrentBet.Number = -1;
    }
    public void ok() {
        MakeBet(CurrentBet);
        // if(b){zmiengracza()}
        CurrentBet.Number = 0;
        CurrentBet.Suit = 0;
        Debug.Log(HighestBet.Number);
        Debug.Log(HighestBet.Suit);
    }
}
