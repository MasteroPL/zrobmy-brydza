using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace UnityBridge
{
    public class Card
    {
        public Figure Figure { get; }
        public Color Color { get; }

        public Card(Figure figure, Color color)
        {
            this.Figure = figure;
            this.Color = color;
        }

        public string ToString()
        {
            return Figure + "_" + Color;
        }
    }

    public enum Color
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public enum Figure
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
}
