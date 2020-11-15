using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Card : MonoBehaviour
{
    private int value;
    private CardColor color;
    private CardState currentState = CardState.ON_HAND;
    private int playerID = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        bool response = checkTurn();
        if(response)
        {
            putCard();
        }
    }

    private bool checkTurn()
    {
        return true;
    }

    private void putCard()
    {
         float newXpos = -1.84f;
         float newYpos = 1.21f;
         transform.position = new Vector3(newXpos, newYpos);
            
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