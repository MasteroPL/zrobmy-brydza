using Assets.Code.Utils;
using GameManagerLib.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.UI;

public class SeatsManagerScript : MonoBehaviour
{
    // seat states mock TODO
    public Dictionary<PlayerTag, bool> SeatStates;
    public Dictionary<PlayerTag, string> PlayersNicknames;
    [SerializeField] GameManagerScript GameManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeSeatManager()
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

    public bool IsSeatTaken(PlayerTag position) {
        return !SeatStates[position];
    }

    public void SitPlayer(PlayerTag Position, string PlayerNickname, bool ClickedByMe=false)
    {
        //Debug.Log(Position.ToString() + " " + PlayerNickname);
        GameObject label = GameObject.Find(Position + "PlayerLabel");
        if (label != null)
        {
            label.GetComponent<Text>().text = PlayerNickname;
            SeatStates[Position] = false;
            PlayersNicknames[Position] = PlayerNickname;

            if (ClickedByMe)
            {
                UserData.Position = Position;
                UserData.PositionStart = Position;
                UserData.Sitting = true;
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
                UserData.Position = PlayerTag.NOBODY;
                UserData.PositionStart = PlayerTag.NOBODY;
                UserData.Sitting = false;
            }
            GameManager.ShowHideStartGameButton(!SeatStates[PlayerTag.N] && !SeatStates[PlayerTag.S] && !SeatStates[PlayerTag.E] && !SeatStates[PlayerTag.W]);
        }
        //Debug.Log("All 4 players: " + (!SeatStates[PlayerTag.N] && !SeatStates[PlayerTag.S] && !SeatStates[PlayerTag.E] && !SeatStates[PlayerTag.W]));
    }
}
