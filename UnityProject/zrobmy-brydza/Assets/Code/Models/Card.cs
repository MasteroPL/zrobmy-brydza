using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Card: MonoBehaviour
{
    public int value;
    public CardColor color;
    public CardState currentState = CardState.ON_HAND;
    public int playerID = 0;

    void OnMouseDown()
    {
        bool response = StaticData.GameManager.checkTurn();
        if (response)
        {
            StaticData.GameManager.putCard(this);
        }
    }
}
    
public enum CardState
{
    IN_DECK = 0,
    ON_HAND = 1,
    ON_TABLE = 2,
    USED = 3
}

public enum CardColor
{
    CLUB = 0,
    DIAMOND = 1,
    HEART = 2,
    SPADE = 3
}