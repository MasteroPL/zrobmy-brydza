using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : MonoBehaviour
{
    List<AudioClip> Taunts;
    [SerializeField] AudioSource AudioSource;
        
    void Start (){
		this.Taunts = new List<AudioClip>();
        var objects = Resources.LoadAll<AudioClip>("Sound/Taunts");

        Taunts.AddRange(objects);
		Debug.Log(Taunts.Count);
		Debug.Log(Taunts[0]);
    }
  
    // Update is called once per frame
    void Update () {
		
    }
     
    public void playRandomTaunt() {
       this.AudioSource.clip = Taunts[Random.Range(0,Taunts.Count)] as AudioClip;
       this.AudioSource.Play();
    }
}
