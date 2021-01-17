using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Code.Models;
using Assets.Code.UI;
using GameManagerLib.Models;

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
    [SerializeField] GameObject hiddenCard;
    //[SerializeField] AuctionModule AuctionModule;
    //private UserData userData;

    void Start()
    {
        //userData = new UserData();
        //AuctionMod.InitAuctionModule(this, userData, PlayerTag.N);
        //PlayingMod.InitPlayingModule(this);
    }

    public void StartGame(GameManagerLib.Models.Card[] handN, GameManagerLib.Models.Card[] handE, GameManagerLib.Models.Card[] handS, GameManagerLib.Models.Card[] handW)
    {
        //GiveCardsToPlayer(userData.position);
        //GiveHiddenCardsToPlayers(userData.position);

        GiveCardsToPlayer(PlayerTag.N, handN);
        GiveCardsToPlayer(PlayerTag.S, handS);
        GiveCardsToPlayer(PlayerTag.W, handW);
        GiveCardsToPlayer(PlayerTag.E, handE);

        GameObject auctionObject = GameObject.Find("/Canvas/TableCanvas/AuctionDialog");
        auctionObject.SetActive(true);
        GameObject startButtonObject = GameObject.Find("/Canvas/StartButton");
        startButtonObject.SetActive(false);
    }

    private void GiveHiddenCardsToPlayers(PlayerTag MyPosition)
    {
        float[] myCardsX = { -2.37f, -1.975f, -1.58f, -1.185f, -0.79f, -0.395f, 0.0f, 0.395f, 0.79f, 1.185f, 1.58f, 1.975f, 2.37f };
        float[] opCardsY = { 1.72f, 1.4334f, 1.1468f, 0.86f, 0.5736f, 0.287f, 0.0f, -0.2862f, -0.5728f, -0.8594f, -1.146f, -1.43f, -1.72f };

        foreach (int player in System.Enum.GetValues(typeof(PlayerTag)))
        {
            GameObject CardsObject = GameObject.Find("Cards");

            if (player != (int)MyPosition)
            {
                for (int i = 0; i < 13; i++)
                {
                    GameObject card = Instantiate(hiddenCard);
                    switch (player)
                    {
                        case (int)PlayerTag.N:
                            card.transform.localPosition = new Vector3(myCardsX[i] + (CardsObject.gameObject.transform.position.x), (CardsObject.gameObject.transform.position.y) + 3.28f, 0);
                            break;
                        case (int)PlayerTag.S:
                            card.transform.localPosition = new Vector3(myCardsX[i] + (CardsObject.gameObject.transform.position.x), (CardsObject.gameObject.transform.position.y) + 3.07f, 0);
                            break;
                        case (int)PlayerTag.W:
                            card.transform.localPosition = new Vector3(-4.61f + (CardsObject.gameObject.transform.position.x), (CardsObject.gameObject.transform.position.y) + opCardsY[i], 0);
                            break;
                        case (int)PlayerTag.E:
                            card.transform.localPosition = new Vector3(4.61f + (CardsObject.gameObject.transform.position.x), (CardsObject.gameObject.transform.position.y) + opCardsY[i], 0);
                            break;
                    }
                    if (Mathf.Abs((int)MyPosition - (int)player) != 2)
                    {
                        card.transform.rotation = Quaternion.Euler(0, 0, 90f);
                    }
                }
            }
        }
    }

    // temporary method, it is here to make the code shorter
    private void GiveCardsToPlayer(PlayerTag PlayerIdentifier, GameManagerLib.Models.Card[] cards)
    {
        float[] myCardsX = { -2.37f, -1.975f, -1.58f, -1.185f, -0.79f, -0.395f, 0.0f, 0.395f, 0.79f, 1.185f, 1.58f, 1.975f, 2.37f };
        float[] opCardsY = { 1.72f, 1.4334f, 1.1468f, 0.86f, 0.5736f, 0.287f, 0.0f, -0.2862f, -0.5728f, -0.8594f, -1.146f, -1.43f, -1.72f };

        for (int i = 0; i < cards.Length; i++)
        {
            string cardName = CalculateCardName(cards[i]);

            GameObject card = GameObject.Find(cardName);
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
            
            card.GetComponent<Card>().playerID = PlayerIdentifier;
            SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
            sr.sortingOrder = i;
        }
    }

    // TO RECONSIDER
    private string CalculateCardName(GameManagerLib.Models.Card card)
    {
        char color = ' ', figure = ' ';
        bool errorOccurred = false;
        switch (card.Color)
        {
            case CardColor.CLUB:
                color = 'C';
                break;
            case CardColor.DIAMOND:
                color = 'D';
                break;
            case CardColor.HEART:
                color = 'H';
                break;
            case CardColor.SPADE:
                color = 'S';
                break;
            default:
                errorOccurred = true;
                break;
        }

        if ((int)card.Figure < 1)
        {
            errorOccurred = true;
        }
        else if ((int)card.Figure > 9)
        {
            switch (card.Figure)
            {
                case CardFigure.ACE:
                    figure = 'A';
                    break;
                case CardFigure.KING:
                    figure = 'K';
                    break;
                case CardFigure.QUEEN:
                    figure = 'Q';
                    break;
                case CardFigure.JACK:
                    figure = 'J';
                    break;
                case CardFigure.TEN:
                    figure = 'T';
                    break;
                default:
                    errorOccurred = true;
                    break;
            }
        }
        else
        {
            figure = ((int)card.Figure).ToString()[0];
        }

        if (!errorOccurred)
        {
            return "CARD_" + figure + color;
        }
        return "";
    }

    public void putCard(Card card)
    {
        /*
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
        */
    }

    public bool checkTurn()
    {
        return true;
    }
}