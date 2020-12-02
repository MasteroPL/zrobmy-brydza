using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Code.Models;
using Assets.Code.UI;

public class GameManagerScript : MonoBehaviour
{
    //private GameState GameState = GameState.AWAITING_PLAYERS; <- is this deprecated?

    [SerializeField] AuctionModule AuctionMod;
    private UserData userData;

    // States
    public ScriptableObject CurrentState; // keeping current app state
    public AuctionBaseState AuctionState;
    public GameState currentState;

    // Start is called before the first frame update
    void Start()
    {
        //Here will be method, checking if there are 4 players ready.
        currentState = GameState.STARTING;
        if (currentState == GameState.STARTING)
        {
            //Tutaj wywołujemy metodę rozdającą karty
            currentState = GameState.BIDDING;
            userData = new UserData();
            CurrentState = AuctionState;
            AuctionMod.initAuctionModule(this, userData, PlayerTag.N);
        }
        
    }

    public void putCard(Card card)
    {
        if(card.currentState == CardState.ON_HAND)
        {
            string c = "";

            switch (card.color)
            {
                case CardColor.CLUB:
                    c = "C";
                    break;
                case CardColor.DIAMOND:
                    c = "D";
                    break;
                case CardColor.HEART:
                    c = "H";
                    break;
                case CardColor.SPADE:
                    c = "S";
                    break;
            }

            string cardName = "CARD_" + (int)card.figure + c;

            float newXpos = 0;
            float newYpos = 0;

            switch (card.playerID)
            {
                case 0:
                    newXpos = -3.02f;
                    newYpos = -1.03f;
                    break;
                case 1:
                    newXpos = -4.19f;
                    newYpos = 0.41f;
                    break;
                case 2:
                    newXpos = -3.02f;
                    newYpos = 1.87f;
                    break;
                case 3:
                    newXpos = -1.8f;
                    newYpos = 0.41f;
                    break;
            }
            
            GameObject.Find(cardName).transform.position = new Vector3(newXpos, newYpos);
        }
        

    }

    public bool checkTurn()
    {
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

public enum PlayerTag
{
    NOBODY = -1,
    N = 0,
    E = 1,
    S = 2,
    W = 3
}