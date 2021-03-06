﻿using System.Collections;
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

    // Contract label
    [SerializeField] Text DeclaredContractLabel;

    private int PassCounter = 0;

    public void InitAuctionModule(Game MainModule, UserData UserData, PlayerTag StartingPlayer)
    {
        this.MainModule = MainModule;
        AuctionState.Init(StartingPlayer);

        DeclaredContractLabel.text = "";
        TeamTakenHandsCounterLabel.text = "";

        AssignAuctionFirstRow();

        if (GameConfig.DevMode)
        {
            MainModule.GameManagerScript.UserData.position = MainModule.Match.CurrentBidding.CurrentPlayer;
        }

        // setting visibility of dialog for first render, later it'll be updated according to game state & current player
        if (AuctionState.CurrentPlayer == MainModule.GameManagerScript.UserData.position)
        {
            AuctionDialog.enabled = true;
            AssignControls();
        }
        else
        {
            AuctionDialog.enabled = false;
            ReleaseListeners();
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
        OKButton.onClick.AddListener(() =>
        {
            // "OK" click shouldn't let pass
            if (AuctionState.ContractCache.ContractHeight == ContractHeight.NONE || AuctionState.ContractCache.ContractColor == ContractColor.NONE)
            {
                return;
            }

            bool updatedCorrectly = MainModule.AddBid(AuctionState.ContractCache.ContractHeight,
                                AuctionState.ContractCache.ContractColor,
                                MainModule.GameManagerScript.UserData.position,
                                AuctionState.ContractCache.XEnabled,
                                AuctionState.ContractCache.XXEnabled);

            if (updatedCorrectly)
            {
                AuctionState.UpdateContract();
                UpdateContractList();
                AuctionState.CurrentPlayer = MainModule.Match.CurrentBidding.CurrentPlayer;
                if (GameConfig.DevMode)
                {
                    MainModule.GameManagerScript.UserData.position = MainModule.Match.CurrentBidding.CurrentPlayer; // for dev mode
                }
                PassCounter = 0;
            }
        });
        PassButton.onClick.AddListener(() =>
        {
            bool correctlyDeclared = MainModule.AddBid(ContractHeight.NONE, ContractColor.NONE, MainModule.GameManagerScript.UserData.position);
            if (correctlyDeclared)
            {
                PassCounter++;
                switch (AuctionState.CurrentPlayer)
                {
                    case PlayerTag.N:
                        NPlayerDeclarations.text += "pas" + "\n";
                        break;
                    case PlayerTag.E:
                        EPlayerDeclarations.text += "pas" + "\n";
                        break;
                    case PlayerTag.S:
                        SPlayerDeclarations.text += "pas" + "\n";
                        break;
                    case PlayerTag.W:
                        WPlayerDeclarations.text += "pas" + "\n";
                        break;
                }
                AuctionState.CurrentPlayer = MainModule.Match.CurrentBidding.CurrentPlayer;
                if (GameConfig.DevMode)
                {
                    MainModule.GameManagerScript.UserData.position = MainModule.Match.CurrentBidding.CurrentPlayer;
                }
            }
        });
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
            if (MainModule.GameState == GameState.BIDDING)
            {
                if (AuctionState.CurrentPlayer == MainModule.GameManagerScript.UserData.position)
                {
                    AuctionDialog.enabled = true;  // showing dialog
                    UpdateContractDisplayedText();
                }
                else
                {
                    AuctionDialog.enabled = false; // hiding dialog
                }

                if (PassCounter == 4)
                {
                    PassCounter = 0;
                    MainModule.GameState = GameState.BIDDING;
                    MainModule.ShuffleAndGiveCardsAgain();
                    /*AuctionState.CurrentPlayer = MainModule.Match.CurrentBidding.CurrentPlayer;
                    /*if (GameConfig.DevMode)
                    {
                        MainModule.UserData.position = MainModule.Match.CurrentBidding.CurrentPlayer; // for dev mode
                    }
                    AssignAuctionFirstRow();*/
                }
                else if (MainModule.Match.CurrentBidding.IsEnd())
                {
                    if(MainModule.Match.CurrentGame.Declarer == PlayerTag.N || MainModule.Match.CurrentGame.Declarer == PlayerTag.S)
                    {
                        DeclaredContractLabel.text = "Contract:\nNS, " + MainModule.Match.CurrentBidding.HighestContract.ToString();
                    } 
                    else if (MainModule.Match.CurrentGame.Declarer == PlayerTag.E || MainModule.Match.CurrentGame.Declarer == PlayerTag.W)
                    {
                        DeclaredContractLabel.text = "Contract:\nEW, " + MainModule.Match.CurrentBidding.HighestContract.ToString();
                    }
                    TeamTakenHandsCounterLabel.text = "NS : 0\nEW : 0";
                    MainModule.GameState = GameState.PLAYING;

                    if (GameConfig.DevMode)
                    {
                        MainModule.GameManagerScript.UserData.position = MainModule.Match.CurrentGame.CurrentPlayer;
                    }
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

    void UpdateContractList()
    {
        switch (AuctionState.CurrentPlayer)
        {
            case PlayerTag.N:
                NPlayerDeclarations.text += MainModule.Match.CurrentBidding.HighestContract.ToString() + "\n";
                break;
            case PlayerTag.E:
                EPlayerDeclarations.text += MainModule.Match.CurrentBidding.HighestContract.ToString() + "\n";
                break;
            case PlayerTag.S:
                SPlayerDeclarations.text += MainModule.Match.CurrentBidding.HighestContract.ToString() + "\n";
                break;
            case PlayerTag.W:
                WPlayerDeclarations.text += MainModule.Match.CurrentBidding.HighestContract.ToString() + "\n";
                break;
        }
        // setting initial contractCache value
        //AuctionState.currentPlayer = PlayerTag.E; // TODO setting proper player
    }
}
