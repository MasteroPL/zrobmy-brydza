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
    [SerializeField] AuctionModule AuctionModule;

    public Game Game;
    private List<GameObject> HiddenCardsOfPlayerN;
    private List<GameObject> HiddenCardsOfPlayerE;
    private List<GameObject> HiddenCardsOfPlayerS;
    private List<GameObject> HiddenCardsOfPlayerW;

    void Start()
    {
    }

    public void UpdateTableCenter(Game Game)
    {
        GameObject.Find("Player3IndicatorText").GetComponent<Text>().text = Game.UserData.position.ToString();
        GameObject.Find("Player4IndicatorText").GetComponent<Text>().text = ((PlayerTag)(((int)Game.UserData.position + 1) % 4)).ToString();
        GameObject.Find("Player1IndicatorText").GetComponent<Text>().text = ((PlayerTag)(((int)Game.UserData.position + 2) % 4)).ToString();
        GameObject.Find("Player2IndicatorText").GetComponent<Text>().text = ((PlayerTag)(((int)Game.UserData.position + 3) % 4)).ToString();
    }

    // this method is implemented in case concrete user switches his place
    public void UpdateTable(Game Game, GameManagerLib.Models.Card[] PlayerHand)
    {
        UpdateTableCenter(Game);
        switch (Game.UserData.position)
        {
            case PlayerTag.N:
                GiveCardsToPlayer(PlayerTag.N, PlayerHand);
                break;
            case PlayerTag.E:
                GiveCardsToPlayer(PlayerTag.E, PlayerHand);
                break;
            case PlayerTag.S:
                GiveCardsToPlayer(PlayerTag.S, PlayerHand);
                break;
            case PlayerTag.W:
                GiveCardsToPlayer(PlayerTag.W, PlayerHand);
                break;
        }
        GiveHiddenCardsToPlayers(Game.UserData.position);
    }

    public void StartGame(Game Game, GameManagerLib.Models.Card[] PlayerHand)
    {
        this.Game = Game;
        HiddenCardsOfPlayerN = new List<GameObject>();
        HiddenCardsOfPlayerE = new List<GameObject>();
        HiddenCardsOfPlayerS = new List<GameObject>();
        HiddenCardsOfPlayerW = new List<GameObject>();

        //UpdateTable(Game, PlayerHand);

        // for dev mode
        GiveCardsToPlayer(PlayerTag.N, Game.Match.PlayerList[0].Hand);
        GiveCardsToPlayer(PlayerTag.S, Game.Match.PlayerList[2].Hand);
        GiveCardsToPlayer(PlayerTag.W, Game.Match.PlayerList[3].Hand);
        GiveCardsToPlayer(PlayerTag.E, Game.Match.PlayerList[1].Hand);

        PlayerTag StartingPlayer = Game.Match.CurrentBidding.CurrentPlayer;
        AuctionModule.InitAuctionModule(Game, Game.UserData, StartingPlayer);

        GameObject auctionObject = GameObject.Find("/Canvas/TableCanvas/AuctionDialog");
        auctionObject.SetActive(true);
        GameObject startButtonObject = GameObject.Find("/Canvas/StartButton");
        startButtonObject.SetActive(false);
    }

    public void RestartGame()
    {
        GameObject card;
        for(int i = 0; i < 4; i++)
        {
            for(int j = 2; j <=14 ; j++)
            {
                string cardName = "CARD_";

                if (j > 9)
                {
                    switch (j)
                    {
                        case 10:
                            cardName += "T";
                            break;
                        case 11:
                            cardName += "J";
                            break;
                        case 12:
                            cardName += "Q";
                            break;
                        case 13:
                            cardName += "K";
                            break;
                        case 14:
                            cardName += "A";
                            break;
                    }
                }
                else
                {
                    cardName += j.ToString();
                }

                switch (i)
                {
                    case 0:
                        cardName += "C";
                        break;
                    case 1:
                        cardName += "D";
                        break;
                    case 2:
                        cardName += "H";
                        break;
                    case 3:
                        cardName += "S";
                        break;
                }

                card = GameObject.Find(cardName);
                card.transform.position = new Vector3(-100, 0, 0);
            }
        }

        GiveCardsToPlayer(PlayerTag.N, Game.Match.PlayerList[0].Hand);
        GiveCardsToPlayer(PlayerTag.S, Game.Match.PlayerList[2].Hand);
        GiveCardsToPlayer(PlayerTag.W, Game.Match.PlayerList[3].Hand);
        GiveCardsToPlayer(PlayerTag.E, Game.Match.PlayerList[1].Hand);
    }

    public void ShowGrandCards(PlayerTag grand, GameManagerLib.Models.Card[] grandHand)
    {
        switch (grand)
        {
            case PlayerTag.N:
                DestroyHiddenCards(HiddenCardsOfPlayerN);
                break;
            case PlayerTag.E:
                DestroyHiddenCards(HiddenCardsOfPlayerE);
                break;
            case PlayerTag.S:
                DestroyHiddenCards(HiddenCardsOfPlayerS);
                break;
            case PlayerTag.W:
                DestroyHiddenCards(HiddenCardsOfPlayerW);
                break;
        }
        GiveCardsToPlayer(grand, grandHand);
    }

    private void DestroyHiddenCards(List<GameObject> cards)
    {
        for(int i = 0; i < cards.Count; i++)
        {
            Destroy(cards[i]);
        }
    }

    // TODO dynamic change place
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
                            card.transform.localPosition = new Vector3(4.61f + (CardsObject.gameObject.transform.position.x), (CardsObject.gameObject.transform.position.y) + opCardsY[i], 0);
                            break;
                        case (int)PlayerTag.E:
                            card.transform.localPosition = new Vector3(-4.61f + (CardsObject.gameObject.transform.position.x), (CardsObject.gameObject.transform.position.y) + opCardsY[i], 0);
                            break;
                    }
                    if (Mathf.Abs((int)MyPosition - (int)player) != 2)
                    {
                        card.transform.rotation = Quaternion.Euler(0, 0, 90f);
                    }

                    switch ((PlayerTag)player)
                    {
                        case PlayerTag.N:
                            HiddenCardsOfPlayerN.Add(card);
                            break;
                        case PlayerTag.E:
                            HiddenCardsOfPlayerE.Add(card);
                            break;
                        case PlayerTag.S:
                            HiddenCardsOfPlayerS.Add(card);
                            break;
                        case PlayerTag.W:
                            HiddenCardsOfPlayerW.Add(card);
                            break;
                    }
                }
            }
        }
    }

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
                    card.transform.localPosition = new Vector3(4.61f, opCardsY[i]);
                    break;
                case PlayerTag.E:
                    card.transform.localPosition = new Vector3(-4.61f, opCardsY[i]);
                    break;
            }
            
            card.GetComponent<Card>().PlayerID = PlayerIdentifier;
            SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
            sr.sortingOrder = i;
        }
    }

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

        if ((int)card.Figure < 2)
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
        bool putOK = Game.PutCard(card.Figure, card.Color, card.PlayerID);
        Debug.Log("Czy można położyć kartę? -" + putOK.ToString());
        if (putOK)
        {
            if (card.CurrentState == CardState.ON_HAND)
            {
                string c = "";

                switch (card.Color)
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
                if ((int)card.Figure > 9)
                {
                    switch ((int)card.Figure)
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
                }
                else
                {
                    cardName = "CARD_" + (int)card.Figure + c;
                }

                float newXpos = 0;
                float newYpos = 0;

                switch (card.PlayerID) // to reconsider, positions are relative to player who sits
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

                if (Game.Match.CurrentGame.TrickList.Count == 0 && Game.Match.CurrentGame.currentTrick.CardList.Count == 1)
                {
                    Game.ShowGrandCards();
                }
                Game.UserData.position = (PlayerTag)(((int)Game.UserData.position + 1) % 4); // for dev mode

                if (Game.IsTrickComplete())
                {
                    SleepFor2Seconds();
                    Trick lastTrick = Game.Match.CurrentGame.TrickList[Game.Match.CurrentGame.TrickList.Count - 1];
                    GameObject tmp;
                    for (int i = 0; i < lastTrick.CardList.Count; i++)
                    {
                        string tmpCardName = CalculateCardName(lastTrick.CardList[i]);
                        tmp = GameObject.Find(tmpCardName);
                        tmp.transform.position = new Vector3(-100, 0, 0);
                    }

                    Text TeamTakenHandsCounterLabel = GameObject.Find("TeamTakenHandsCounterLabel").GetComponent<Text>();
                    TeamTakenHandsCounterLabel.text = "NS : " + Game.CalculateTeamTricks(PlayerTag.N, PlayerTag.S).ToString() + "\n";
                    TeamTakenHandsCounterLabel.text += "EW : " + Game.CalculateTeamTricks(PlayerTag.E, PlayerTag.W).ToString();
                    Game.UserData.position = lastTrick.Winner; // for dev mode
                }
                //SpriteRenderer cardSpriteRenderer = cardToPut.GetComponent<SpriteRenderer>();

                /*string prevPutCardName = "";
                switch (card.PlayerID)
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
                }*/
            }
        }
    }

    IEnumerator SleepFor2Seconds()
    {
        yield return new WaitForSeconds(2);
    }

    public bool checkTurn()
    {
        return true;
    }
}