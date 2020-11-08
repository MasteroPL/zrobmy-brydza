using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
	[SerializeField] int State;
	[SerializeField] string Chat;
	[SerializeField] string Points;
	[SerializeField] string Bets;
	[SerializeField] TextMeshProUGUI Display;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(State == 0){
			this.Display.text = this.Chat;
		}
		else if (State == 1){
			this.Display.text = this.Points;
		}
		else if (State == 2){
			this.Display.text = this.Bets;
		}
		else{
			this.Display.text = this.Chat;
		}
    }
	
	public void DisplayChat(){
		this.State = 0;
	}
	
	public void DisplayPoints(){
		this.State = 1;
	}
	
	public void DisplayBets(){
		this.State = 2;
	}
	
}