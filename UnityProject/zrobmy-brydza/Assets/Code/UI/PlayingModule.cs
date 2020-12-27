using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingModule : MonoBehaviour
{
    private GameManagerScript MainModule;
    [SerializeField] PlayingBaseState PlayingState;

    public void InitPlayingModule(GameManagerScript MainModule)
    {
        this.MainModule = MainModule;
        //PlayingState.currentPlayer = startingPlayer;

        PlayingState.currentPutCardLabelForPlayerN = null;
        PlayingState.currentPutCardLabelForPlayerE = null;
        PlayingState.currentPutCardLabelForPlayerS = null;
        PlayingState.currentPutCardLabelForPlayerW = null;
    }

    public void putCard(string cardName, PlayerTag playerPosition)
    {
        switch (playerPosition)
        {
            case PlayerTag.N:
                PlayingState.currentPutCardLabelForPlayerN = cardName;
                break;
            case PlayerTag.E:
                PlayingState.currentPutCardLabelForPlayerE = cardName;
                break;
            case PlayerTag.S:
                PlayingState.currentPutCardLabelForPlayerS = cardName;
                break;
            case PlayerTag.W:
                PlayingState.currentPutCardLabelForPlayerW = cardName;
                break;
        }
    }
}
