using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PausedScreen : MonoBehaviour {
   [SerializeField] TextMeshProUGUI nicknameText ;

   
   void setText(string nickname) {
       nicknameText.text = nickname;
   }
}
