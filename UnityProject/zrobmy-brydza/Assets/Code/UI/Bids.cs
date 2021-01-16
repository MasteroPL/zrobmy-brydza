using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bids : MonoBehaviour
{
    private void Start()
    {
        GameObject textfield;
        textfield = GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/NPlayerDeclarations/" + this.name);
        textfield.GetComponent<UnityEngine.UI.Text>().text = this.value;
    }
    [SerializeField] string value;
    [SerializeField] string name;

    public void setValue(string value)
    {
        this.value = value;
        GameObject textfield;
        textfield = GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/NPlayerDeclarations/" + this.name);
        textfield.GetComponent<UnityEngine.UI.Text>().text = this.value;
    }
}
