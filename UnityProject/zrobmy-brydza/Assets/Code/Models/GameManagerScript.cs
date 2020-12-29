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
    [SerializeField] PlayingModule PlayingMod;
    private UserData userData;

    // States
    public ScriptableObject CurrentState; // keeping current app state
    public AuctionBaseState AuctionState;
    public PlayingBaseState PlayingState;

    void Start()
    {
        userData = new UserData();
        AuctionMod.InitAuctionModule(this, userData, PlayerTag.N);
        PlayingMod.InitPlayingModule(this);
    }

    public void startGame()
    {
        CurrentState = AuctionState;
        //Here will be method, checking if there are 4 players ready.
        if (CurrentState.Equals(AuctionState))
        {
            // position
            //float[] myCardsX = { -5.69277f, -5.25f, -4.80724f, -4.36446f, -3.92169f, -3.47892f, -3.03615f, -2.59344f, -2.150609f, -1.70784f, -1.26507f, -0.8223f, -0.37953f };
            // uzywam localposition

            GiveCardsToPlayer(PlayerTag.N);
            GiveCardsToPlayer(PlayerTag.S);
            GiveCardsToPlayer(PlayerTag.W);
            GiveCardsToPlayer(PlayerTag.E);

            GameObject auctionObject = GameObject.Find("/Canvas/TableCanvas/AuctionDialog");
            auctionObject.SetActive(true);
            GameObject startButtonObject = GameObject.Find("/Canvas/StartButton");
            startButtonObject.SetActive(false);
        } 
    }

    // temporary method, it is here to make the code shorter
    private void GiveCardsToPlayer(PlayerTag PlayerIdentifier)
    {
        float[] myCardsX = { -2.37f, -1.975f, -1.58f, -1.185f, -0.79f, -0.395f, 0.0f, 0.395f, 0.79f, 1.185f, 1.58f, 1.975f, 2.37f };
        float[] opCardsY = { 1.72f, 1.4334f, 1.1468f, 0.86f, 0.5736f, 0.287f, 0.0f, -0.2862f, -0.5728f, -0.8594f, -1.146f, -1.43f, -1.72f };

        List<string> cards = ServerDialler.GetPlayerCards(PlayerIdentifier);
        for (int i = 0; i < cards.Count; i++)
        {
            GameObject card = GameObject.Find(cards[i]);
            switch (PlayerIdentifier)
            {
                case PlayerTag.N:
                    card.transform.localPosition = new Vector3(myCardsX[i], -3.28f);
                    break;
                case PlayerTag.S:
                    card.transform.localPosition = new Vector3(myCardsX[i], 3.07f);
                    break;
                case PlayerTag.W:
                    card.transform.localPosition = new Vector3(-4.61f, opCardsY[i]);
                    break;
                case PlayerTag.E:
                    card.transform.localPosition = new Vector3(4.61f, opCardsY[i]);
                    break;
            }
            
            card.GetComponent<Card>().playerID = userData.position;
            SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
            sr.sortingOrder = i;
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

            switch (card.playerID) // to reconsider, positions are relative to player who sits
            {
                case PlayerTag.N:
                    newXpos = -3.02f;
                    newYpos = -1.03f;
                    break;
                case PlayerTag.E:
                    newXpos = -4.19f;
                    newYpos = 0.41f;
                    break;
                case PlayerTag.S:
                    newXpos = -3.02f;
                    newYpos = 1.87f;
                    break;
                case PlayerTag.W:
                    newXpos = -1.8f;
                    newYpos = 0.41f;
                    break;
            }

            GameObject cardToPut = GameObject.Find(cardName);
            cardToPut.transform.position = new Vector3(newXpos, newYpos);
            SpriteRenderer cardSpriteRenderer = cardToPut.GetComponent<SpriteRenderer>();

            string prevPutCardName = "";
            switch (card.playerID)
            {
                case PlayerTag.N:
                    prevPutCardName = PlayingState.currentPutCardLabelForPlayerN;
                    break;
                case PlayerTag.E:
                    prevPutCardName = PlayingState.currentPutCardLabelForPlayerE;
                    break;
                case PlayerTag.S:
                    prevPutCardName = PlayingState.currentPutCardLabelForPlayerS;
                    break;
                case PlayerTag.W:
                    prevPutCardName = PlayingState.currentPutCardLabelForPlayerW;
                    break;
            }
            if (prevPutCardName != null)
            {
                cardSpriteRenderer.sortingOrder = GameObject.Find(prevPutCardName).GetComponent<SpriteRenderer>().sortingOrder + 1;
            }
            PlayingMod.putCard(cardName, card.playerID);
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