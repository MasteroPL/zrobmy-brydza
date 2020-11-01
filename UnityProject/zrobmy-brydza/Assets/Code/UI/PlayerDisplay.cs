using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDisplay : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI Name;
	
    // Start is called before the first frame update
    void Start()
    {
		Name.text = "wolne miejsce";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
