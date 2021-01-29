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
        Debug.Log("Initialization...");
        GameObject.Find("ChatButton").GetComponent<Button>().onClick.AddListener(() => { ChatButton(); });
        GameObject.Find("AuctionButton").GetComponent<Button>().onClick.AddListener(() => { BidButton(); });

        this.ChatButton();
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
    /// Not used, rendering new bid was implemented in auction module (after contract confirmation)
    /*public void AddBid(string player, string value)
    {
        player = player.ToUpper();
        GameObject textfield;   
        textfield = GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList/"+ player + "PlayerDeclarations/" + player + "BidList");
        textfield.GetComponent<UnityEngine.UI.Text>().text += value + "\n";
    }*/

    public void BidButton()
    {
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/Chat").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList").SetActive(true);
    }

    public void ChatButton()
    {
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/Chat").SetActive(true);
    }

    /// <summary> 
    /// dodaj wiadomość do chatu
    /// <para>message - wiadomość (string)</para>
    /// </summary>
    public void AddMessage(string message)
    {
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/Chat/ChatViewport/ChatContent").GetComponent<Text>().text += message + "\n";
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/Chat/ChatInputField/Text").GetComponent<Text>().text = "";
    }
}
