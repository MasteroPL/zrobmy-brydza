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

    // TODO port for start method
    public void InitAuctionModule(Game MainModule, UserData UserData, PlayerTag StartingPlayer)
    {
        this.MainModule = MainModule;
        AuctionState.Init(StartingPlayer);

        // setting visibility of dialog for first render, later it'll be updated according to game state & current player
        if (AuctionState.CurrentPlayer == MainModule.UserData.position)
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
        OKButton.onClick.AddListener(() => {
            bool updatedCorrectly = MainModule.AddBid(AuctionState.ContractCache.ContractHeight,
                                AuctionState.ContractCache.ContractColor,
                                MainModule.UserData.position,
                                AuctionState.ContractCache.XEnabled,
                                AuctionState.ContractCache.XXEnabled);
            
            /*string baseString = "Height: " + AuctionState.ContractCache.ContractHeight.ToString() + "; Color: " + AuctionState.ContractCache.ContractColor.ToString();
            if (AuctionState.ContractCache.XEnabled)
            {
                baseString += "; X";
            }
            if (AuctionState.ContractCache.XXEnabled)
            {
                baseString += "; XX";
            }
            Debug.Log(baseString);
            Debug.Log("Updated correctly? " + updatedCorrectly.ToString());*/

            if (updatedCorrectly)
            {
                AuctionState.UpdateContract();
                UpdateContractList();
                AuctionState.CurrentPlayer = (PlayerTag)(((int)AuctionState.CurrentPlayer + 1) % 4);
                MainModule.UserData.position = (PlayerTag)(((int)MainModule.UserData.position + 1) % 4); // for dev mode
            }
        });
        PassButton.onClick.AddListener(() => {
            bool correctlyDeclared = MainModule.AddBid(ContractHeight.NONE, ContractColor.NONE, MainModule.UserData.position);
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
                AuctionState.CurrentPlayer = (PlayerTag)(((int)AuctionState.CurrentPlayer + 1) % 4);
                MainModule.UserData.position = (PlayerTag)(((int)MainModule.UserData.position + 1) % 4); // for dev mode
                //Debug.Log("PassCounter: " + MainModule.Match.CurrentBidding.GetPassCounter());
                //Debug.Log("Liczba przeprowadzonych licytacji: " + MainModule.Match.BiddingList.Count);
            }
        });
    }

    void Start()
    {
        DeclaredContractLabel.text = "";
        TeamTakenHandsCounterLabel.text = "";
    }

    void Update()
    {
        if (MainModule != null)
        {
            if (MainModule.GameState == GameState.BIDDING)
            {
                if (AuctionState.CurrentPlayer == MainModule.UserData.position)
                {
                    AuctionDialog.enabled = true;  // showing dialog
                    UpdateContractDisplayedText();
                }
                else
                {
                    AuctionDialog.enabled = false; // hiding dialog
                }

                Debug.Log("Aktualnie licytujący: " + MainModule.Match.CurrentBidding.CurrentPlayer.ToString());
                //Debug.Log("Liczba pass'ow : " + MainModule.Match.CurrentBidding.GetPassCounter() + "; Czy koniec licytacji? : " + MainModule.Match.CurrentBidding.IsEnd());
                Debug.Log("Aktualna pozycja: " + MainModule.UserData.position.ToString());
                Debug.Log("Status rozgrywki: " + MainModule.GameState.ToString());
                if (PassCounter == 4)
                {
                    PassCounter = 0;
                    MainModule.GameState = GameState.BIDDING;
                    MainModule.ShuffleAndGiveCardsAgain();
                    AuctionState.CurrentPlayer = MainModule.Match.CurrentBidding.CurrentPlayer;
                    MainModule.UserData.position = MainModule.Match.CurrentBidding.CurrentPlayer; // for dev mode
                }
                else if (MainModule.Match.CurrentBidding.IsEnd())
                {
                    DeclaredContractLabel.text = "Contract:\n" + MainModule.Match.CurrentBidding.HighestContract.ToString();
                    MainModule.GameState = GameState.PLAYING;
                    MainModule.UserData.position = (PlayerTag)(((int)MainModule.UserData.position + 1) % 4); // for dev mode
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
