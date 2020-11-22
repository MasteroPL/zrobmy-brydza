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
            ServerRequest requestData = new ServerRequest(card, position);
            string json = JsonUtility.ToJson(requestData);
            System.IO.File.WriteAllText("./RequestFormat.txt", json); // That's just for checking request and sharing its format
            return json;
        }

        public static ServerResponse GetResponseFromServer()
        {
            string response = PrepareDummyResponse(); // TODO receiving request
            ServerResponse data = JsonUtility.FromJson<ServerResponse>(response); // Deserializing JSON data
            return data;
        }

        private static string PrepareDummyResponse()
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

            ServerResponse responseObject = new ServerResponse(cards);
            string json = JsonUtility.ToJson(responseObject);

            System.IO.File.WriteAllText("./ResponseFromServer.txt", json); // That's just for checking response and sharing its format
            return json;
        }
    }
}
