using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Card: MonoBehaviour
{
    public CardFigure figure;
    public CardColor color;
    public CardState currentState = CardState.ON_HAND;
    public PlayerTag playerID;
    [SerializeField] GameObject Game;
    GameManagerScript GameManager;

    void Start()
    {
        GameManager = Game.GetComponent<GameManagerScript>();
    }

    void OnMouseDown()
    {

        bool response = GameManager.checkTurn();
        if (response)
        {
            GameManager.putCard(this);
        }
    }
}

[Serializable]
public class CardObject
{
    public CardFigure figure;
    public CardColor color;
    public CardObject(CardFigure figure, CardColor color)
    {
        this.figure = figure;
        this.color = color;
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