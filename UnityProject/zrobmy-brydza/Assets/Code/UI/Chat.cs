using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{
    private void Start()
    {
        GameObject textfield;
        textfield = GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/Chat");
        textfield.GetComponent<UnityEngine.UI.Text>().text = this.value;
    }
    [SerializeField] string value;

    public void setValue(string value)
    {
        this.value = value;
        GameObject textfield;
        textfield = GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/Chat");
        textfield.GetComponent<UnityEngine.UI.Text>().text = this.value;
    }
}
