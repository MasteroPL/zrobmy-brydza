using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using GameManagerLib.Models;

public class Card: MonoBehaviour
{
    //  Card in UI
    public CardFigure Figure;
    public CardColor Color;
    public CardState CurrentState = CardState.ON_HAND;
    public PlayerTag PlayerID;
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