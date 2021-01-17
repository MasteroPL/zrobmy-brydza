using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.UI;
using Assets.Code.Models;
using GameManagerLib.Models;

public class AuctionModule : MonoBehaviour
{
    private Game MainModule;
    [SerializeField] AuctionBaseState AuctionState;
    private UserData MyData;

    // UI Serialized components
    [SerializeField] Canvas AuctionDialog;

    // contract preview label
    [SerializeField] Text AuctionContractPreviewText;

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

    // TODO port for start method
    public void InitAuctionModule(Game MainModule, UserData UserData, PlayerTag StartingPlayer)
    {
        this.MainModule = MainModule;
        MyData = UserData;
        AuctionState.Init(StartingPlayer);

        // setting visibility of dialog for first render, later it'll be updated according to game state & current player
        if (AuctionState.CurrentPlayer == MyData.position)
            AuctionDialog.enabled = true;
        else
            AuctionDialog.enabled = false;

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
        OKButton.onClick.AddListener(() => { AuctionState.UpdateContract(); });
    }

    void Start()
    {
    }

    void Update()
    {
        /*if (MainModule.GameState == GameState.BIDDING) // TO RECODE
        {
            if (AuctionState.CurrentPlayer == MyData.position)
            {
                AuctionDialog.enabled = true;  // showing dialog

                // these 2 lines below should be updated after filling contract
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
                AuctionContractPreviewText.text += AuctionState.CurrentContract != null ? "\nAktualny kontrakt: " + AuctionState.CurrentContract.ToString():"";
            }
            else
            {
                AuctionDialog.enabled = false; // hiding dialog
            }
        }*/
    }

    /*void declareNewContract()
    {
        switch (AuctionState.currentPlayer)
        {
            case PlayerTag.N:
                NPlayerDeclarations.text += contractString + "\n";
                break;
            case PlayerTag.E:
                EPlayerDeclarations.text += contractString + "\n";
                break;
            case PlayerTag.S:
                SPlayerDeclarations.text += contractString + "\n";
                break;
            case PlayerTag.W:
                WPlayerDeclarations.text += contractString + "\n";
                break;
        }
        // setting initial contractCache value
        contractCache[0] = ' ';
        contractCache[1] = ' ';
        contractCache[2] = ' ';
        //AuctionState.currentPlayer = PlayerTag.E; // TODO setting proper player
    }*/
}
