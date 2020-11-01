using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingScreen : MonoBehaviour {
   [SerializeField] TextMeshProUGUI numberOfMissingPlayersText ;
   int numberOfMissingPlayers;

   void Start () {
       numberOfMissingPlayers = 3;
       setText();
   }


   void increaseNumberOfMissingPlayers() {
      if (numberOfMissingPlayers < 3) {
         numberOfMissingPlayers = numberOfMissingPlayers + 1;
         setText();
      }
   }
   void decreaseNumberOfMissingPlayers() {
      if (numberOfMissingPlayers > 0) {
         numberOfMissingPlayers = numberOfMissingPlayers -1;
         setText();
      }
   }
   void setText() {
       numberOfMissingPlayersText.text = numberOfMissingPlayers.ToString();
   }
}
