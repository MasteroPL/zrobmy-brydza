using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Models
{
    class ServerDialler
    {
        public static void SendRequestToServer()
        {
            CardObject card = new CardObject(CardFigure.ACE, CardColor.DIAMOND);
            PlayerTag position = PlayerTag.N;
            
            string json = PrepareRequestData(card, position);
            // TODO sending request
        }

        private static string PrepareRequestData(CardObject card, PlayerTag position)
        {
            ServerPutCardRequest requestData = new ServerPutCardRequest(card, position);
            string json = JsonUtility.ToJson(requestData);
            System.IO.File.WriteAllText("./RequestFormat.txt", json); // That's just for checking request and sharing its format
            return json;
        }

        public static List<string> GetPlayerCards()
        {
            string cardsJson = PrepareDummyCardsResponse(); // TODO receiving request
            ServerPlayerCardsResponse data = JsonUtility.FromJson<ServerPlayerCardsResponse>(cardsJson); // Deserializing JSON data

            List<string> cards = new List<string>();
            char color = ' ', figure = ' ';
            bool errorOccurred = false;
            for(int i = 0; i < data.PlayerHand.Count; i++)
            {
                switch (data.PlayerHand[i].color)
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

                if ((int)data.PlayerHand[i].figure < 1)
                {
                    errorOccurred = true;
                }
                else if((int)data.PlayerHand[i].figure > 9)
                {
                    switch (data.PlayerHand[i].figure)
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
                    figure = ((int)data.PlayerHand[i].figure).ToString()[0];
                }

                if (!errorOccurred)
                {
                    cards.Add("CARD_" + figure + color);
                }
            }
            return cards;
        }

        private static string PrepareDummyCardsResponse()
        {
            // hardcoded player deck prepared by algorithm and server
            List<CardObject> cards = new List<CardObject>();
            cards.Add(new CardObject(CardFigure.ACE, CardColor.CLUB));
            cards.Add(new CardObject(CardFigure.KING, CardColor.DIAMOND));
            cards.Add(new CardObject(CardFigure.QUEEN, CardColor.HEART));
            cards.Add(new CardObject(CardFigure.JACK, CardColor.SPADE));
            cards.Add(new CardObject(CardFigure.TEN, CardColor.CLUB));
            cards.Add(new CardObject(CardFigure.NINE, CardColor.DIAMOND));
            cards.Add(new CardObject(CardFigure.EIGHT, CardColor.HEART));
            cards.Add(new CardObject(CardFigure.SEVEN, CardColor.SPADE));
            cards.Add(new CardObject(CardFigure.SIX, CardColor.CLUB));
            cards.Add(new CardObject(CardFigure.FIVE, CardColor.DIAMOND));
            cards.Add(new CardObject(CardFigure.FOUR, CardColor.HEART));
            cards.Add(new CardObject(CardFigure.THREE, CardColor.SPADE));
            cards.Add(new CardObject(CardFigure.TWO, CardColor.CLUB));

            ServerPlayerCardsResponse responseObject = new ServerPlayerCardsResponse(cards);
            string json = JsonUtility.ToJson(responseObject);

            System.IO.File.WriteAllText("./CardsResponseFromServer.txt", json); // That's just for checking response and sharing its format
            return json;
        }
    }
}
