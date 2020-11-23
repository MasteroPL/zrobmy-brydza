using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Models;
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

        ServerDialler.GetResponseFromServer();
        ServerDialler.SendRequestToServer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPausedState() {
        state = pausedState;
        text.text = state.GetStateText();
    }

    public void putCard(Card card)
    {
        if(card.currentState == CardState.ON_HAND)
        {
            string c = "";
            if(card.color == CardColor.CLUB)
            {
                c = "C";
            }
            if (card.color == CardColor.DIAMOND)
            {
                c = "D";
            }
            if (card.color == CardColor.HEART)
            {
                c = "H";
            }
            if (card.color == CardColor.SPADE)
            {
                c = "S";
            }
            string cardName = "CARD_" + (int)card.figure + c;
            float newXpos = -1.84f;
            float newYpos = 1.21f;
            GameObject.Find(cardName).transform.position = new Vector3(newXpos, newYpos);
        }
        

    }

    public bool checkTurn()
    {
        return true;
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