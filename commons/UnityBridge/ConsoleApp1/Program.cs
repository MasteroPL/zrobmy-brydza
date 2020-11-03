using System;
using System.Collections.Generic;
using UnityBridge;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            CardShuffler cardShuffler = new CardShuffler();
            List<Card> deck = cardShuffler.GenerateDeck();

            double[] playersPoints = { 0, 0, 0, 0 };

            int maxIt = 5;
            for(int it = 0; it < maxIt; it++)
            {
                deck = cardShuffler.Shuffle(deck);

                List<List<Card>> decks = cardShuffler.GiveCardsToPlayers(deck);
                Console.WriteLine("Game {0}/{1}", it + 1, maxIt);
                for (int i = 0; i < decks.Count; i++)
                {
                    playersPoints[i] += (double)cardShuffler.CalculatePoints(decks[i]) / maxIt;
                    /*foreach(Card card in decks[i])
                    {
                        Console.WriteLine(card.ToString());
                    }
                    Console.WriteLine();*/
                    Console.WriteLine("Player {0} => points : {1}", i + 1, cardShuffler.CalculatePoints(decks[i]));
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
    }
}
