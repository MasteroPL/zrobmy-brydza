using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : MonoBehaviour
{
    List<AudioClip> Taunts;
	AudioSource TauntToPlay;
        
    void Start (){
		this.Taunts = new List<AudioClip>();
		Taunts.AddRange((AudioClip[])Resources.LoadAll("Sound/Taunts",typeof(AudioClip)));
		Debug.Log(Taunts.Count);
		Debug.Log(Taunts[0]);
		this.TauntToPlay.clip = Taunts[0];
    }
  
    // Update is called once per frame
    void Update () {
		
    }
     
    public void playRandomTaunt() {
       this.TauntToPlay.clip = Taunts[Random.Range(0,Taunts.Count)] as AudioClip;
       this.TauntToPlay.Play();
    }
}
