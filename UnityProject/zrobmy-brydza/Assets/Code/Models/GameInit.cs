using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Models;

public class GameInit : MonoBehaviour
{
    Game game;
    [SerializeField] GameManagerScript GameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(GameManagerScript);
        game = new Game(GameManagerScript);
    }

    public void InitBridgeGame()
    {
        game.StartGame();
        //Debug.Log("debug");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
