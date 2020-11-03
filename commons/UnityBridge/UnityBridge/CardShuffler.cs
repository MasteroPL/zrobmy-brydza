using System;
using System.Collections.Generic;

namespace UnityBridge
{
    public class CardShuffler
    {
        private Random randomizer;

        public CardShuffler()
        {
            randomizer = new Random();
        }

        public List<Card> GenerateDeck()
        {
            List<Card> cards = new List<Card>();
            for(int i = 0; i < Enum.GetNames(typeof(Color)).Length; i++)
            {
                for(int j = 0; j < Enum.GetNames(typeof(Figure)).Length; j++)
                {
                    cards.Add( new Card( (Figure)j, (Color)i ) );
                }
            }
            return cards;
        }

        public List<Card> Shuffle(List<Card> deck)
        {
            List<Card> leftHandCards = new List<Card>();
            List<Card> rightHandCards = new List<Card>();

            int[] shuffleTimes = { 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
            int numberOfShuffles = randomizer.Next(0, shuffleTimes.Length);
            int divider;
            double probabilityForLeftHand, probabilityForRightHand;
            for (int i = 0; i < numberOfShuffles; i++)
            {
                divider = randomizer.Next(21, 32); // getting random number from range [21, 31] -> it's dividing deck by 2 with a toleration of +/- 5 cards

                // division for hands
                for (int j = 0; j < deck.Count; j++)
                {
                    if (j <= divider)
                        leftHandCards.Add(deck[j]);
                    else
                        rightHandCards.Add(deck[j]);
                }
                deck.Clear();

                // shuffling according to probability
                while(leftHandCards.Count > 0 && rightHandCards.Count > 0)
                {
                    probabilityForLeftHand = CalculateProbability(leftHandCards, leftHandCards.Count + rightHandCards.Count);
                    probabilityForRightHand = CalculateProbability(rightHandCards, leftHandCards.Count + rightHandCards.Count);

                    if (probabilityForLeftHand > probabilityForRightHand)
                    {
                        deck.Add(leftHandCards[0]);
                        leftHandCards.RemoveAt(0);
                    } else
                    {
                        deck.Add(rightHandCards[0]);
                        rightHandCards.RemoveAt(0);
                    }
                }

                while(leftHandCards.Count > 0)
                {
                    deck.Add(leftHandCards[0]);
                    leftHandCards.RemoveAt(0);
                }

                while (rightHandCards.Count > 0)
                {
                    deck.Add(rightHandCards[0]);
                    rightHandCards.RemoveAt(0);
                }
            }
            return deck;
        }

        private double CalculateProbability(List<Card> hand, int wholeDeckSize)
        {
            if (hand.Count == 0)
                return 0;
            double probability = (double)hand.Count / wholeDeckSize;
            probability *= randomizer.NextDouble();
            return probability;
        }

        public List<List<Card>> GiveCardsToPlayers(List<Card> shuffledDeck)
        {
            List<List<Card>> playersDecks = new List<List<Card>>();
            playersDecks.Add(new List<Card>());
            playersDecks.Add(new List<Card>());
            playersDecks.Add(new List<Card>());
            playersDecks.Add(new List<Card>());

            int index;
            for (int i = 0; i < shuffledDeck.Count; i++)
            {
                index = i % 4;
                playersDecks[index].Add(shuffledDeck[i]);
            }
            return playersDecks;
        }

        public int CalculatePoints(List<Card> myCards)
        {
            int points = 0;
            int[] colors = { 0, 0, 0, 0 };
            for(int i = 0; i < myCards.Count; i++)
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
