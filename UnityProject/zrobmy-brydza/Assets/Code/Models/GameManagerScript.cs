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

    // Start is called before the first frame update
    void Start()
    {
        userData = new UserData();
        CurrentState = AuctionState;
        AuctionMod.initAuctionModule(this, userData, PlayerTag.N);
    }

    public void putCard(Card card)
    {
        if(card.currentState == CardState.ON_HAND)
        {
            string c = "";
            if(card.color == CardColor.CLUB)
            {
                c = "C";
            }
            if (card.color == CardColor.DIAMOND)
            {
                c = "D";
            }
            if (card.color == CardColor.HEART)
            {
                c = "H";
            }
            if (card.color == CardColor.SPADE)
            {
                c = "S";
            }
            string cardName = "CARD_" + (int)card.figure + c;
            float newXpos = -1.84f;
            float newYpos = 1.21f;
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