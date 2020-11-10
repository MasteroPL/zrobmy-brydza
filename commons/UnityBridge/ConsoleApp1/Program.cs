using System;
using System.Collections.Generic;
using UnityBridge;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Card> deck = GenerateDeck();
            CardShuffler<Card> cardShuffler = new CardShuffler<Card>(deck);

            double[] playersPoints = { 0, 0, 0, 0 };

            int maxIt = 5;
            for(int it = 0; it < maxIt; it++)
            {
                deck = cardShuffler.Shuffle();

                List<List<Card>> decks = cardShuffler.GiveCardsToPlayers(deck);
                Console.WriteLine("Game {0}/{1}", it + 1, maxIt);
                for (int i = 0; i < decks.Count; i++)
                {
                    playersPoints[i] += (double)CalculatePoints(decks[i]) / maxIt;
                    /*foreach(Card card in decks[i])
                    {
                        Console.WriteLine(card.ToString());
                    }
                    Console.WriteLine();*/
                    Console.WriteLine("Player {0} => points : {1}", i + 1, CalculatePoints(decks[i]));
                }

                Console.WriteLine("");
            }

            Console.WriteLine("Average game points after {0} attempts", maxIt);
            for(int i = 0; i < 4; i++)
            {
                Console.WriteLine( "Player {0}: {1} points per game", i + 1, Math.Round(playersPoints[i], 2) );
            }
            Console.ReadKey();
        }

        public static List<Card> GenerateDeck()
        {
            List<Card> cards = new List<Card>();
            for (int i = 0; i < Enum.GetNames(typeof(Color)).Length; i++)
            {
                for (int j = 0; j < Enum.GetNames(typeof(Figure)).Length; j++)
                {
                    cards.Add(new Card((Figure)j, (Color)i));
                }
            }
            return cards;
        }

        public static int CalculatePoints(List<Card> myCards)
        {
            int points = 0;
            int[] colors = { 0, 0, 0, 0 };
            for (int i = 0; i < myCards.Count; i++)
            {
                switch (myCards[i].Figure)
                {
                    case Figure.Ace:
                        points += 4;
                        break;
                    case Figure.King:
                        points += 3;
                        break;
                    case Figure.Queen:
                        points += 2;
                        break;
                    case Figure.Jack:
                        points += 1;
                        break;
                    default:
                        break;
                }

                switch (myCards[i].Color)
                {
                    case Color.Clubs:
                        colors[0]++;
                        break;
                    case Color.Diamonds:
                        colors[1]++;
                        break;
                    case Color.Hearts:
                        colors[2]++;
                        break;
                    case Color.Spades:
                        colors[3]++;
                        break;
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (colors[i] == 0)
                    points += 4;
            }
            return points;
        }
    }
}
