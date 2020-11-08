﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManagerScript : MonoBehaviour
{
    private GameState GameState = GameState.AWAITING_PLAYERS;
    private PlayerTag CurrentPlayer = PlayerTag.NOBODY;
    [SerializeField] Text text;
    [SerializeField] State startingState;
    [SerializeField] State pausedState;
    State state;

    // Start is called before the first frame update
    void Start()
    {   
        state = startingState;
        text.text = state.GetStateText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPausedState() {
        state = pausedState;
        text.text = state.GetStateText();
    }
}

public enum GameState
{
    AWAITING_PLAYERS = 0,
    STARTING = 1,
    BIDDING = 2,
    PLAYING = 3,
    PAUSED = 4,
    GAME_FINISHED = 5
}

public enum PlayerTag
{
    NOBODY = -1,
    N = 0,
    E = 1,
    S = 2,
    W = 3
}