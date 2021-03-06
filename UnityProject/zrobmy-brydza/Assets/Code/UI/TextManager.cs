﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Models;
using EasyHosting.Models.Actions;
using Newtonsoft.Json.Linq;
using EasyHosting.Models.Client;
using Assets.Code.UI;
using System;
using Assets.Code.Models.Exceptions;

public class TextManager : MonoBehaviour
{
    [SerializeField] GameObject NS;
    [SerializeField] GameObject WE;
    [SerializeField] GameObject ChatContent;
    [SerializeField] GameObject InputField;
    [SerializeField] GameManagerScript GameManager;
    [SerializeField] GameObject MessageTemplate;

    string space = "\n____________\n\n";

    void Start()
    {
        /*for(int i=0; i<15; i++)
        {
            AddMessage("Are you mad bro? That's not gonna work");
        }*/
    }

    public static void ChatButton()
    {
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/PointsPanel").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/Chat").SetActive(true);
    }

    public static void BidButton()
    {
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/Chat").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/PointsPanel").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList").SetActive(true);
    }

    public static void PointsButton(Game CurrentGame)
    {
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/Chat").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/AuctionList").SetActive(false);
        GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/PointsPanel").SetActive(true);

        if(CurrentGame != null && CurrentGame.Match != null)
        {
            //test for debugging
            /*List<string> testNS = new List<string>();
            testNS.Add("0|30");
            testNS.Add("0|0");
            testNS.Add("0|0");
            testNS.Add("0|0");
            testNS.Add("0|0");
            List<string> testEW = new List<string>();
            testEW.Add("0|0");
            testEW.Add("20|50");
            testEW.Add("0|0");
            testEW.Add("10|50");
            testEW.Add("40|10");*/

            //string[] ShownTexts = CalculateShownText(CurrentGame.Match.History.NSHistory, CurrentGame.Match.History.WEHistory);
            //GameObject.Find("NSPoints").GetComponent<Text>().text = ShownTexts[0];
            //GameObject.Find("WEPoints").GetComponent<Text>().text = ShownTexts[1];
        } else
        {
            //GameObject.Find("NSPoints").GetComponent<Text>().text = "____________";
            //GameObject.Find("WEPoints").GetComponent<Text>().text = "____________";
        }
    }

    private static string[] CalculateShownText(List<string> TeamNSPointsHistory, List<string> TeamEWPointsHistory)
    {
        // to return
        string NSText = "", EWText = "";

        string[] NSTmp, EWTmp;
        char[] separators = { '|' };
        List<string> EWAbovePoints = new List<string>();
        List<string> NSAbovePoints = new List<string>();
        List<string> EWUnderPoints = new List<string>();
        List<string> NSUnderPoints = new List<string>();

        for (int i = 0; i < TeamNSPointsHistory.Count; i++)
        {
            if (TeamNSPointsHistory[i] == "Round" || TeamEWPointsHistory[i] == "Round")
            {
                if (EWUnderPoints[EWUnderPoints.Count - 1] != "0") // NS team got the round
                {
                    NSUnderPoints[NSUnderPoints.Count - 1] = "\n";
                }
                else
                {
                    EWUnderPoints[EWUnderPoints.Count - 1] = "\n";
                }
                EWUnderPoints.Add("Round");
                NSUnderPoints.Add("Round");
            }
            else
            {
                NSTmp = TeamNSPointsHistory[i].Split(separators);
                EWTmp = TeamEWPointsHistory[i].Split(separators);

                EWAbovePoints.Add(EWTmp[0]);
                NSAbovePoints.Add(NSTmp[0]);
                EWUnderPoints.Add(EWTmp[1]);
                NSUnderPoints.Add(NSTmp[1]);
            }
        }

        string NSAbovePrefix = "";
        foreach(string AbovePoints in NSAbovePoints)
        {
            if(AbovePoints == "0")
            {
                NSAbovePrefix += "\n";
            }
            else
            {
                NSText = AbovePoints + "\n" + NSText;
            }
        }

        NSText += "____________\n";
        foreach (string UnderPoints in NSUnderPoints)
        {
            if (UnderPoints != "0" && UnderPoints != "Round")
            {
                NSText += UnderPoints + "\n";
            } 
            else if (UnderPoints == "Round")
            {
                NSText += "____________\n";
            }
        }

        string EWAbovePrefix = "";
        foreach (string AbovePoints in EWAbovePoints)
        {
            if(AbovePoints == "0")
            {
                EWAbovePrefix += "\n";
            }
            else
            {
                EWText = AbovePoints + "\n" + EWText;
            }
        }

        EWText += "____________\n";
        foreach (string UnderPoints in EWUnderPoints)
        {
            if (UnderPoints != "0" && UnderPoints != "Round")
            {
                EWText += UnderPoints + "\n";
            }
            else if (UnderPoints == "Round")
            {
                EWText += "____________\n";
            }
        }

        string FinalNSPrefixLines = "", FinalEWPrefixLines = "";
        if (NSAbovePrefix.Length > EWAbovePrefix.Length)
        {
            for(int i = 0; i < NSAbovePrefix.Length - EWAbovePrefix.Length; i++)
            {
                FinalNSPrefixLines += "\n";
            }
        } 
        else
        {
            for (int i = 0; i < EWAbovePrefix.Length - NSAbovePrefix.Length; i++)
            {
                FinalEWPrefixLines += "\n";
            }
        }

        string[] RenderedStrings = { FinalNSPrefixLines + NSText, FinalEWPrefixLines + EWText };
        return RenderedStrings;
    }

    /// <summary> 
    /// ustaw punkty NS
    /// <para>above - punkty nad kreską (string)</para> 
    /// <para>below - punkty pod kreską (string)</para>
    /// <para>rounds - liczba ugranych rund (string)</para> 
    /// </summary>
    public void setNSPointsValue(string above, string below, string rounds)
    {
        this.NS.GetComponent<Text>().text = above + this.space + below + this.space + rounds;
    }

    /// <summary> 
    /// ustaw punkty WE
    /// <para>above - punkty nad kreską (string)</para> 
    /// <para>below - punkty pod kreską (string)</para>
    /// <para>rounds - liczba ugranych rund (string)</para> 
    /// </summary>
    public void setWEPointsValue(string above, string below, string rounds)
    {
        this.WE.GetComponent<Text>().text = above + this.space + below + this.space + rounds;
    }

    /*public void PointsButton()
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
    }*/

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

    /// <summary> 
    /// dodaj wiadomość do chatu
    /// <para>message - wiadomość (string)</para>
    /// </summary>
    public void AddMessage(string message)
    {
        float height = MessageTemplate.GetComponent<RectTransform>().rect.height;
        ChatContent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (ChatContent.transform.childCount + 1) * height + 5 * ChatContent.transform.childCount );

        GameObject ChatElement = Instantiate(MessageTemplate);
        ChatElement.SetActive(true);
        ChatElement.GetComponent<Text>().text = message;

        ChatElement.transform.localPosition = new Vector3(
                                                    ChatElement.transform.localPosition.x,
                                                    ChatElement.transform.localPosition.y - (ChatContent.transform.childCount - 1) * (height + 5),
                                                    ChatElement.transform.localPosition.z
                                              );

        ChatElement.transform.SetParent(ChatContent.transform, false);
        //ListItems.Add(ListElement);
        //ChatContent.GetComponent<Text>().text += message + "\n";
        InputField.GetComponent<Text>().text = "";
    }

    public ActionsSerializer WrapRequestData(string actionName, JObject data)
    {
        var result = new ActionsSerializer();
        result.Actions = new ActionSerializer[1];
        var tmp = new ActionSerializer();

        tmp.ActionName = actionName;
        tmp.ActionData = data;

        result.Actions[0] = tmp;

        return result;
    }

    /// <summary>
    /// Metoda typu "Callback" uruchamiana po wysłaniu wiadomości do innych graczy
    /// </summary>
    /// <param name="request">Zapytanie wysłania wiadomości</param>
    /// <param name="response">Odpowiedź na wysłane żądanie</param>
    /// <param name="additionalData">Informacje dodatkowe</param>
    private void SendMessageCallback(Request request, ActionsSerializer response, object additionalData)
    {
        if (((string)response.Actions[0].ActionData.GetValue("status")).CompareTo("OK") != 0)
        {
            return;
        }
        string message = (string)additionalData;
        AddMessage(UserData.Username + ": " + message);
    }

    /// <summary>
    /// Wysyła podaną jako argument wiadomość do pozostałych graczy
    /// </summary>
    /// <param name="message">Wiadomość do innych graczy</param>
    public void SendMessage(string message)
    {
        var sendMessageRequestData = new ServerSocket.Actions.SendMessage.RequestSerializer();
        sendMessageRequestData.Message = message;
        GameManager.PerformServerAction("send-message", sendMessageRequestData.GetApiObject(), SendMessageCallback, message);
    }
}
