﻿using Assets.Code.Utils;
using GameManagerLib.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeatsManagerScript : MonoBehaviour
{
    // seat states mock TODO
    public Dictionary<PlayerTag, bool> SeatStates;
    public Dictionary<PlayerTag, string> PlayersNicknames;
    [SerializeField] GameManagerScript GameManager;

    // Start is called before the first frame update
    void Start()
    {
        SeatStates = new Dictionary<PlayerTag, bool>();
        PlayersNicknames = new Dictionary<PlayerTag, string>();

        // set all 4 seats as free (true)
        SeatStates.Add(PlayerTag.N, true);
        SeatStates.Add(PlayerTag.E, true);
        SeatStates.Add(PlayerTag.S, true);
        SeatStates.Add(PlayerTag.W, true);

        PlayersNicknames.Add(PlayerTag.N, "");
        PlayersNicknames.Add(PlayerTag.E, "");
        PlayersNicknames.Add(PlayerTag.S, "");
        PlayersNicknames.Add(PlayerTag.W, "");
    }
    public bool CheckSeatAvailability(PlayerTag Position)
    {
        return SeatStates[Position];
    }

    public bool AllFourPlayersPresent()
    {
        return !SeatStates[PlayerTag.N] && !SeatStates[PlayerTag.S] && !SeatStates[PlayerTag.E] && !SeatStates[PlayerTag.W];
    }

    public void SitPlayer(PlayerTag Position, string PlayerNickname)
    {
        GameObject label = GameObject.Find(Position + "PlayerLabel");
        if (label != null)
        {
            label.GetComponent<Text>().text = PlayerNickname;
            SeatStates[Position] = false;
            PlayersNicknames[Position] = PlayerNickname;

            if (!GameConfig.DevMode)
            {
                GameManager.UserData.position = Position;
                GameManager.UserData.positionStart = Position;
                GameManager.UserData.Sitting = true;
            }
            GameManager.ShowHideStartGameButton(!SeatStates[PlayerTag.N] && !SeatStates[PlayerTag.S] && !SeatStates[PlayerTag.E] && !SeatStates[PlayerTag.W]);
            //Debug.Log("All 4 players: " + (!SeatStates[PlayerTag.N] && !SeatStates[PlayerTag.S] && !SeatStates[PlayerTag.E] && !SeatStates[PlayerTag.W]));
        }
    }

    public void SitOutPlayer(PlayerTag Position)
    {
        GameObject label = GameObject.Find(Position + "PlayerLabel");
        if (label != null)
        {
            label.GetComponent<Text>().text = "Oczekiwanie na gracza " + Position;
            SeatStates[Position] = true;
            PlayersNicknames[Position] = "";

            if (!GameConfig.DevMode)
            {
                GameManager.UserData.position = PlayerTag.NOBODY;
                GameManager.UserData.positionStart = PlayerTag.NOBODY;
                GameManager.UserData.Sitting = false;
            }
            GameManager.ShowHideStartGameButton(!SeatStates[PlayerTag.N] && !SeatStates[PlayerTag.S] && !SeatStates[PlayerTag.E] && !SeatStates[PlayerTag.W]);
        }
        //Debug.Log("All 4 players: " + (!SeatStates[PlayerTag.N] && !SeatStates[PlayerTag.S] && !SeatStates[PlayerTag.E] && !SeatStates[PlayerTag.W]));
    }
}
