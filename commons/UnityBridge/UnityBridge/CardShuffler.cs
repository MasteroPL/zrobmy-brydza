using System;
using System.Collections.Generic;

namespace UnityBridge
{
    public class CardShuffler<T>
    {
        private Random randomizer;
        private List<T> deck;

        public CardShuffler(List<T> deck)
        {
            randomizer = new Random();
            this.deck = deck;
        }

        public List<T> Shuffle()
        {
            List<T> leftHandCards = new List<T>();
            List<T> rightHandCards = new List<T>();

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

                deck = ReplaceCards(deck); // second shuffling method added
            }
            return deck;
        }

        private List<T> ReplaceCards(List<T> deck)
        {
            int numberOfCardsToReplace = randomizer.Next(7, deck.Count - 7 + 1);
            for(int i = 0; i < numberOfCardsToReplace; i++)
            {
                T element = deck[0];
                deck.RemoveAt(0);
                deck.Add(element);
            }
            return deck;
        }

        private double CalculateProbability(List<T> hand, int wholeDeckSize)
        {
            if (hand.Count == 0)
                return 0;
            double probability = (double)hand.Count / wholeDeckSize;
            probability *= randomizer.NextDouble();
            return probability;
        }

        public List<List<T>> GiveCardsToPlayers(List<T> shuffledDeck)
        {
            List<List<T>> playersDecks = new List<List<T>>();
            playersDecks.Add(new List<T>());
            playersDecks.Add(new List<T>());
            playersDecks.Add(new List<T>());
            playersDecks.Add(new List<T>());

            int index;
            for (int i = 0; i < shuffledDeck.Count; i++)
            {
                index = i % 4;
                playersDecks[index].Add(shuffledDeck[i]);
            }
            return playersDecks;
        }
    }
}
