using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Models;

public class GameInit : MonoBehaviour
{
    Game game;
    // Start is called before the first frame update
    void Start()
    {
        game = new Game();
    }

    public void InitBridgeGame()
    {
        game.StartGame();
        Debug.Log("debug");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
