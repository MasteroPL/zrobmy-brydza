using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;


namespace Models
{
    public class Card
    {
        public CardFigure Figure;
        public CardColor Color;
        public CardState CurrentState = CardState.ON_HAND;
        public PlayerTag PlayerID;

        public Card(CardFigure Figure, CardColor Color, PlayerTag PlayerID)
        {
            this.Figure = Figure;
            this.Color = Color;
            this.PlayerID = PlayerID;
        }
    }
 
    public enum CardState
    {
        IN_DECK = 0,
        ON_HAND = 1,
        ON_TABLE = 2,
        DISPOSED = 3
    }

    public enum CardColor
    {
        CLUB = 0,
        DIAMOND = 1,
        HEART = 2,
        SPADE = 3
    }

    public enum CardFigure
    {
        TWO = 2,
        THREE = 3,
        FOUR = 4,
        FIVE = 5,
        SIX = 6,
        SEVEN = 7,
        EIGHT = 8,
        NINE = 9,
        TEN = 10,
        JACK = 11,
        QUEEN = 12,
        KING = 13,
        ACE = 14
    }
}