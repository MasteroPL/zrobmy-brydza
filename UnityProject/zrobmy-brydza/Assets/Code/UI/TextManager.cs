using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextManager : MonoBehaviour
{
    Points NS;
    Points WE;
    string space;
    InputField ChatField;
    void Start()
    {
        System.Console.WriteLine("dupa");
        this.space = "\n \n";
        this.NS = GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/NSPoints").GetComponent<Points>();
        this.WE = GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/EWPoints").GetComponent<Points>();

        this.setNSPointsValue("0", "0", "");
        this.setWEPointsValue("100", "0", "R");

        this.ChatButton();

        AddBid("n", "1C");
        AddBid("s", "1H");
        AddBid("e", "1D");
        AddBid("w", "1S");
        AddBid("n", "PAS");
        /*
        this.AddMessage("Derp", "jak grasz cwelu");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Herp", "sklej wary lapsie");
        this.AddMessage("Derp", "jak grasz cwelu");
        this.AddMessage("Derp", "jak grasz cwelu");*/
    }

    /// <summary> 
    /// ustaw punkty NS
    /// <para>above - punkty nad kreską (string)</para> 
    /// <para>below - punkty pod kreską (string)</para>
    /// <para>rounds - liczba ugranych rund (string)</para> 
    /// </summary>
    public void setNSPointsValue(string above, string below, string rounds)
    {
        this.NS.setValue(above + this.space + below + this.space + rounds);
    }

    /// <summary> 
    /// ustaw punkty WE
    /// <para>above - punkty nad kreską (string)</para> 
    /// <para>below - punkty pod kreską (string)</para>
    /// <para>rounds - liczba ugranych rund (string)</para> 
    /// </summary>
    public void setWEPointsValue(string above, string below, string rounds)
    {
        this.WE.setValue(above + this.space + below + this.space + rounds);
    }

    public void PointsButton()
    {
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/NSPoints").SetActive(true);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/EWPoints").SetActive(true);

        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/Chat").SetActive(false);

        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/NPlayerDeclarations/NBidList").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/SPlayerDeclarations/SBidList").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/EPlayerDeclarations/EBidList").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/WPlayerDeclarations/WBidList").SetActive(false);

        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/EPlayer" +
            "Declarations/EPlayerDeclarationsBackground/EPlayerDeclarationsHeader").GetComponent<UnityEngine.UI.Text>().text = "S";
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/SPlayer" +
            "Declarations/SPlayerDeclarationsBackground/SPlayerDeclarationsHeader").GetComponent<UnityEngine.UI.Text>().text = "E";
    }

    /// <summary> 
    /// dodaj odzywkę
    /// <para>player - S, W, E lub N (string)</para> 
    /// <para>value - odzywka np. 1C (string)</para>
    /// </summary>
    public void AddBid(string player, string value)
    {
        player = player.ToUpper();
        GameObject textfield;   
        textfield = GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/"+ player + "PlayerDeclarations/" + player + "BidList");
        textfield.GetComponent<UnityEngine.UI.Text>().text += value + "\n";
    }

    public void BidButton()
    {
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/NSPoints").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/EWPoints").SetActive(false);

        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/Chat").SetActive(false);

        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/NPlayerDeclarations/NBidList").SetActive(true);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/SPlayerDeclarations/SBidList").SetActive(true);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/EPlayerDeclarations/EBidList").SetActive(true);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/WPlayerDeclarations/WBidList").SetActive(true);

        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/EPlayer" +
            "Declarations/EPlayerDeclarationsBackground/EPlayerDeclarationsHeader").GetComponent<UnityEngine.UI.Text>().text = "E";
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/SPlayer" +
            "Declarations/SPlayerDeclarationsBackground/SPlayerDeclarationsHeader").GetComponent<UnityEngine.UI.Text>().text = "S";
    }

    public void ChatButton()
    {
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/NSPoints").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/EWPoints").SetActive(false);

        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/Chat").SetActive(true);

        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/NPlayerDeclarations/NBidList").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/SPlayerDeclarations/SBidList").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/EPlayerDeclarations/EBidList").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/WPlayerDeclarations/WBidList").SetActive(false);

    }

    /// <summary> 
    /// dodaj wiadomość do chatu
    /// <para>message - wiadomość (string)</para>
    /// </summary>
    public void AddMessage(string message)
    {
        Debug.Log("reeee");
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/Chat/Viewport/Content").GetComponent<UnityEngine.UI.Text>().text 
            += message + "\n";
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/InputField/Text").GetComponent<UnityEngine.UI.Text>().text = "";
        Debug.Log(GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/InputField/Text").GetComponent<UnityEngine.UI.Text>().text);
    }
}
