using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.UI;
using Assets.Code.Models;
using GameManagerLib.Models;
using Assets.Code.Utils;

public class AuctionModule : MonoBehaviour
{
    private Game MainModule;
    [SerializeField] AuctionBaseState AuctionState;

    // UI Serialized components
    [SerializeField] Canvas AuctionDialog;

    // contract preview label
    [SerializeField] Text AuctionContractPreviewText;
    [SerializeField] Text TeamTakenHandsCounterLabel;

    // contract height buttons
    [SerializeField] Button AuctionHeight1Button;
    [SerializeField] Button AuctionHeight2Button;
    [SerializeField] Button AuctionHeight3Button;
    [SerializeField] Button AuctionHeight4Button;
    [SerializeField] Button AuctionHeight5Button;
    [SerializeField] Button AuctionHeight6Button;
    [SerializeField] Button AuctionHeight7Button;

    // contract color buttons
    [SerializeField] Button ClubsSignButton;
    [SerializeField] Button DiamondsSignButton;
    [SerializeField] Button HeartsSignButton;
    [SerializeField] Button SpadesSignButton;
    [SerializeField] Button NTSignButton;

    // bottom panel of auction buttons
    [SerializeField] Button PassButton;
    [SerializeField] Button XButton;
    [SerializeField] Button XXButton;
    [SerializeField] Button OKButton;

    // Auction declarations listing <- should be moved to other module
    [SerializeField] Text NPlayerDeclarations;
    [SerializeField] Text EPlayerDeclarations;
    [SerializeField] Text SPlayerDeclarations;
    [SerializeField] Text WPlayerDeclarations;

    [SerializeField] GameManagerScript GameManager;

    // Contract label
    [SerializeField] Text DeclaredContractLabel;

    private int PassCounter = 0;

    public void InitAuctionModule(Game MainModule, PlayerTag StartingPlayer)
    {
        this.MainModule = MainModule;
        AuctionState.Init();

        DeclaredContractLabel.text = "";
        TeamTakenHandsCounterLabel.text = "";

        AssignAuctionFirstRow();

        if (GameConfig.DevMode)
        {
            //MainModule.GameManagerScript.UserData.position = MainModule.Match.CurrentBidding.CurrentPlayer;
            UserData.Position = MainModule.Match.CurrentBidding.CurrentPlayer;
        }

        AssignControls();
        // setting visibility of dialog for first render, later it'll be updated according to game state & current player
        if (MainModule.Match.CurrentBidding.CurrentPlayer == UserData.Position)//MainModule.GameManagerScript.UserData.position
        {
            AuctionDialog.enabled = true;
        }
        else
        {
            AuctionDialog.enabled = false;
        }
    }

    private void AssignAuctionFirstRow()
    {
        NPlayerDeclarations.text = "";
        EPlayerDeclarations.text = "";
        SPlayerDeclarations.text = "";
        WPlayerDeclarations.text = "";
        Text[] auctionTexts = { NPlayerDeclarations, EPlayerDeclarations, SPlayerDeclarations, WPlayerDeclarations };

        for (int i = 0; i < (int)MainModule.Match.CurrentBidding.CurrentPlayer; i++)
        {
            auctionTexts[i].text = "-\n";
        }
    }

    private void AssignControls()
    {
        // assigning handlers to buttons
        AuctionHeight1Button.onClick.AddListener(() => { AuctionState.DeclareContractHeight(ContractHeight.ONE); });
        AuctionHeight2Button.onClick.AddListener(() => { AuctionState.DeclareContractHeight(ContractHeight.TWO); });
        AuctionHeight3Button.onClick.AddListener(() => { AuctionState.DeclareContractHeight(ContractHeight.THREE); });
        AuctionHeight4Button.onClick.AddListener(() => { AuctionState.DeclareContractHeight(ContractHeight.FOUR); });
        AuctionHeight5Button.onClick.AddListener(() => { AuctionState.DeclareContractHeight(ContractHeight.FIVE); });
        AuctionHeight6Button.onClick.AddListener(() => { AuctionState.DeclareContractHeight(ContractHeight.SIX); });
        AuctionHeight7Button.onClick.AddListener(() => { AuctionState.DeclareContractHeight(ContractHeight.SEVEN); });

        ClubsSignButton.onClick.AddListener(() => { AuctionState.DeclareContractColor(ContractColor.C); });
        DiamondsSignButton.onClick.AddListener(() => { AuctionState.DeclareContractColor(ContractColor.D); });
        HeartsSignButton.onClick.AddListener(() => { AuctionState.DeclareContractColor(ContractColor.H); });
        SpadesSignButton.onClick.AddListener(() => { AuctionState.DeclareContractColor(ContractColor.S); });
        NTSignButton.onClick.AddListener(() => { AuctionState.DeclareContractColor(ContractColor.NT); });

        XButton.onClick.AddListener(() => { AuctionState.DeclareX(); });
        XXButton.onClick.AddListener(() => { AuctionState.DeclareXX(); });
        OKButton.onClick.AddListener(SendBidRequest);
        PassButton.onClick.AddListener(SendBidRequest);
    }

    public void SendBidRequest() {
        // "OK" click shouldn't let pass
        //if (AuctionState.ContractCache.ContractHeight == ContractHeight.NONE || AuctionState.ContractCache.ContractColor == ContractColor.NONE) {
        //    return;
        //}

        // send request
        MainModule.GameManagerScript.SendBidRequest(
            AuctionState.ContractCache.ContractHeight,
            AuctionState.ContractCache.ContractColor,
            AuctionState.ContractCache.XEnabled,
            AuctionState.ContractCache.XXEnabled
        );
    }

    public void ReleaseListeners()
    {
        PassCounter = 0;
        AuctionHeight1Button.onClick.RemoveAllListeners();
        AuctionHeight2Button.onClick.RemoveAllListeners();
        AuctionHeight3Button.onClick.RemoveAllListeners();
        AuctionHeight4Button.onClick.RemoveAllListeners();
        AuctionHeight5Button.onClick.RemoveAllListeners();
        AuctionHeight6Button.onClick.RemoveAllListeners();
        AuctionHeight7Button.onClick.RemoveAllListeners();

        ClubsSignButton.onClick.RemoveAllListeners();
        DiamondsSignButton.onClick.RemoveAllListeners();
        HeartsSignButton.onClick.RemoveAllListeners();
        SpadesSignButton.onClick.RemoveAllListeners();
        NTSignButton.onClick.RemoveAllListeners();

        XButton.onClick.RemoveAllListeners();
        XXButton.onClick.RemoveAllListeners();
        OKButton.onClick.RemoveAllListeners();
        PassButton.onClick.RemoveAllListeners();
    }

    void Start()
    {
    }

    void Update()
    {
        if (MainModule != null)
        {
            if (MainModule.Match.GameState == GameState.BIDDING)
            {
                if (MainModule.Match.CurrentBidding.CurrentPlayer == UserData.Position)
                {
                    AuctionDialog.enabled = true;  // showing dialog
                    UpdateContractDisplayedText();
                }
                else
                {
                    AuctionDialog.enabled = false; // hiding dialog
                }

                /*if (PassCounter == 4)
                {
                    PassCounter = 0;
                    MainModule.GameState = GameState.BIDDING;
                    MainModule.ShuffleAndGiveCardsAgain();
                    /*AuctionState.CurrentPlayer = MainModule.Match.CurrentBidding.CurrentPlayer;
                    if (GameConfig.DevMode)
                    {
                        MainModule.UserData.position = MainModule.Match.CurrentBidding.CurrentPlayer; // for dev mode
                    }
                    AssignAuctionFirstRow();
                }*/
                if (MainModule.Match.CurrentBidding.IsEnd())
                {
                    Debug.Log("Current bidding is ended");
                    if(MainModule.Match.CurrentGame.Declarer == PlayerTag.N || MainModule.Match.CurrentGame.Declarer == PlayerTag.S)
                    {
                        DeclaredContractLabel.text = "Contract:\nNS, " + MainModule.Match.CurrentBidding.HighestContract.ToString();
                    } 
                    else if (MainModule.Match.CurrentGame.Declarer == PlayerTag.E || MainModule.Match.CurrentGame.Declarer == PlayerTag.W)
                    {
                        DeclaredContractLabel.text = "Contract:\nEW, " + MainModule.Match.CurrentBidding.HighestContract.ToString();
                    }
                    TeamTakenHandsCounterLabel.text = "NS : 0\nEW : 0";

                    //if (GameConfig.DevMode)
                    //{
                    //    UserData.Position = MainModule.Match.CurrentGame.CurrentPlayer;
                    //}
                    AuctionDialog.enabled = false;
                }
            }
        }
    }

    

    private string UpdateContractDisplayedText()
    {
        string prefix = "Licytujesz : ";
        string displayedText = "";
        if (AuctionState.ContractCache.XXEnabled)
        {
            displayedText = "XX";
        }
        else if (AuctionState.ContractCache.XEnabled)
        {
            displayedText = "X";
        }
        else
        {
            displayedText = AuctionState.ContractCache.ToString();
        }
        AuctionContractPreviewText.text = prefix + displayedText;
        AuctionContractPreviewText.text += AuctionState.CurrentContract != null ? "\nAktualny kontrakt: " + AuctionState.CurrentContract.ToString() : "";
        return displayedText;
    }

    public void AddContractToList(Contract contract)
    {
        switch (contract.DeclaredBy) {
            case PlayerTag.N:
                NPlayerDeclarations.text += (contract.ToString() + "\n");
                break;
            case PlayerTag.E:
                EPlayerDeclarations.text += (contract.ToString() + "\n");
                break;
            case PlayerTag.S:
                SPlayerDeclarations.text += (contract.ToString() + "\n");
                break;
            case PlayerTag.W:
                WPlayerDeclarations.text += (contract.ToString() + "\n");
                break;
        }
        //switch (MainModule.Match.CurrentBidding.CurrentPlayer)
        //{
        //    case PlayerTag.N:
        //        NPlayerDeclarations.text += MainModule.Match.CurrentBidding.HighestContract.ToString() + "\n";
        //        break;
        //    case PlayerTag.E:
        //        EPlayerDeclarations.text += MainModule.Match.CurrentBidding.HighestContract.ToString() + "\n";
        //        break;
        //    case PlayerTag.S:
        //        SPlayerDeclarations.text += MainModule.Match.CurrentBidding.HighestContract.ToString() + "\n";
        //        break;
        //    case PlayerTag.W:
        //        WPlayerDeclarations.text += MainModule.Match.CurrentBidding.HighestContract.ToString() + "\n";
        //        break;
        //}
    }

    public void ReloadDeclarations()
    {
        NPlayerDeclarations.text = "";
        EPlayerDeclarations.text = "";
        SPlayerDeclarations.text = "";
        WPlayerDeclarations.text = "";
        Text[] auctionTexts = { NPlayerDeclarations, EPlayerDeclarations, SPlayerDeclarations, WPlayerDeclarations };

        Debug.Log(MainModule.Match.CurrentBidding.CurrentPlayer);

        for (int i = 0; i < (int)MainModule.Match.CurrentBidding.ContractList[0].DeclaredBy; i++)
        {
            auctionTexts[i].text = "-\n";
        }

        for (int i = 0; i < MainModule.Match.CurrentBidding.ContractList.Count; i++)
        {
            if (MainModule.Match.CurrentBidding.ContractList[i].ContractHeight == ContractHeight.NONE && !MainModule.Match.CurrentBidding.ContractList[i].XEnabled && !MainModule.Match.CurrentBidding.ContractList[i].XXEnabled)
            {
                auctionTexts[(int)MainModule.Match.CurrentBidding.ContractList[i].DeclaredBy].text += "pass\n";
            }
            else
            {
                auctionTexts[(int)MainModule.Match.CurrentBidding.ContractList[i].DeclaredBy].text += MainModule.Match.CurrentBidding.ContractList[i].ToString() + "\n";
            }
        }
    }
}
