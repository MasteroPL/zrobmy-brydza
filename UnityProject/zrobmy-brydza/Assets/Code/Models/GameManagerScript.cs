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
    /**
     * My hand cards in deck coordinates on screen (according to main camera)
     * ======================================================================
     * Y = -2.73
     * X = [-5.69277, -5.25, -4.80724, -4.36446, -3.92169, -3.47892, -3.03615, -2.59344, -2.150609, -1.70784, -1.26507, -0.8223, -0.37953]
     * 
     * Partner hand cards in deck coordinates on screen (according to main camera)
     * ===========================================================================
     * X = [-5.69277, -5.25, -4.80724, -4.36446, -3.92169, -3.47892, -3.03615, -2.59344, -2.150609, -1.70784, -1.26507, -0.8223, -0.37953]
     * Y = 3.597
     */

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
            List<string> myCards = AdjustGivenCards();
            //Tutaj wywołujemy metodę rozdającą karty
            float[] myCardsX = { -5.69277f, -5.25f, -4.80724f, -4.36446f, -3.92169f, -3.47892f, -3.03615f, -2.59344f, -2.150609f, -1.70784f, -1.26507f, -0.8223f, -0.37953f };
            for (int i = 0; i < myCards.Count; i++)
            {
                GameObject card = GameObject.Find("CARD_" + myCards[i]);
                card.transform.position = new Vector3(myCardsX[i], -2.73f);
                SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
                sr.sortingOrder = i;
            }

            currentState = GameState.BIDDING;
            userData = new UserData();
            CurrentState = AuctionState;
            AuctionMod.initAuctionModule(this, userData, PlayerTag.N);
        }
        
    }

    // TODO parsing JSON
    private List<string> AdjustGivenCards()
    {
        List<string> l = new List<string>();

        l.Add("9C");
        l.Add("QC");
        l.Add("AC");

        l.Add("5H");
        l.Add("6H");
        l.Add("9H");
        l.Add("TH");

        l.Add("KD");
        l.Add("3D");
        l.Add("7D");
        l.Add("QD");

        l.Add("AS");
        l.Add("QS");
        return l;
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

            string cardName = "";
            if ((int)card.figure > 9)
            {
                switch ((int)card.figure)
                {
                    case 10: // 10
                        cardName = "CARD_T" + c;
                        break;
                    case 11: // J
                        cardName = "CARD_J" + c;
                        break;
                    case 12: // Q
                        cardName = "CARD_Q" + c;
                        break;
                    case 13: // K
                        cardName = "CARD_K" + c;
                        break;
                    case 14: // A
                        cardName = "CARD_A" + c;
                        break;
                }
            } else
            {
                cardName = "CARD_" + (int)card.figure + c;
            }

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