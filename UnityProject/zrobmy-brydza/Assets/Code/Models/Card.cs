using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Card : MonoBehaviour
{
    private int value;
    private CardState currentState = CardState.ON_HAND;
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
        Debug.Log("Działa!!!");
        putCard();
    }

    private void putCard()
    {
        if (currentState == CardState.ON_HAND){
            currentState = CardState.ON_TABLE;
            
            var deltaX = 0;
            var newXpos = transform.position.x + deltaX;
            
            var deltaY = 1;
            var newYpos = transform.position.y + deltaY;

            transform.position = new Vector3(newXpos, newYpos);
            
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