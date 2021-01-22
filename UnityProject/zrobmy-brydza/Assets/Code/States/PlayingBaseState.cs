using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagerLib.Models;

[CreateAssetMenu(menuName = "PlayingState")]
public class PlayingBaseState : ScriptableObject
{
    public PlayerTag currentPlayer { get; set; }
    public string currentPutCardLabelForPlayerN { get; set; }
    public string currentPutCardLabelForPlayerS { get; set; }
    public string currentPutCardLabelForPlayerW { get; set; }
    public string currentPutCardLabelForPlayerE { get; set; }
}
