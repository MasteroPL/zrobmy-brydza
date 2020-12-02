using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.UI;

public class AuctionModule : MonoBehaviour
{
    private GameManagerScript MainModule;
    [SerializeField] AuctionBaseState AuctionState;
    private UserData MyData;
    private StringBuilder contractCache;

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

    // Auction declarations listing
    [SerializeField] Text NPlayerDeclarations;
    [SerializeField] Text EPlayerDeclarations;
    [SerializeField] Text SPlayerDeclarations;
    [SerializeField] Text WPlayerDeclarations;

    public void initAuctionModule(GameManagerScript mainModule, UserData userData, PlayerTag startingPlayer)
    {
        contractCache = new StringBuilder("   ");
        MainModule = mainModule;
        MyData = userData;
        AuctionState.init(startingPlayer);

        // setting visibility of dialog for first render, later it'll be updated according to game state & current player
        if (AuctionState.currentPlayer == MyData.position)
            AuctionDialog.enabled = true;
        else
            AuctionDialog.enabled = false;

        // assigning handlers to buttons
        AuctionHeight1Button.onClick.AddListener(() => { contractCache[0] = '1'; });
        AuctionHeight2Button.onClick.AddListener(() => { contractCache[0] = '2'; });
        AuctionHeight3Button.onClick.AddListener(() => { contractCache[0] = '3'; });
        AuctionHeight4Button.onClick.AddListener(() => { contractCache[0] = '4'; });
        AuctionHeight5Button.onClick.AddListener(() => { contractCache[0] = '5'; });
        AuctionHeight6Button.onClick.AddListener(() => { contractCache[0] = '6'; });
        AuctionHeight7Button.onClick.AddListener(() => { contractCache[0] = '7'; });

        ClubsSignButton.onClick.AddListener(() => { contractCache[1] = 'C'; contractCache[2] = ' '; });
        DiamondsSignButton.onClick.AddListener(() => { contractCache[1] = 'D'; contractCache[2] = ' '; });
        HeartsSignButton.onClick.AddListener(() => { contractCache[1] = 'H'; contractCache[2] = ' '; });
        SpadesSignButton.onClick.AddListener(() => { contractCache[1] = 'S'; contractCache[2] = ' '; });
        NTSignButton.onClick.AddListener(() => { contractCache[1] = 'N'; contractCache[2] = 'T'; });

        XButton.onClick.AddListener(() => {
            if (AuctionState.currentContract != null)
            {
                contractCache[0] = 'X';
                contractCache[1] = ' ';
                contractCache[2] = ' ';
            }
        });

        XXButton.onClick.AddListener(() => { 
            if (AuctionState.xEnabled)
            {
                contractCache[0] = 'X'; contractCache[1] = 'X'; contractCache[2] = ' ';
            }
        });
        OKButton.onClick.AddListener(declareNewContract);
    }

    void Start()
    {
    }

    void Update()
    {
        if (MainModule.CurrentState == AuctionState)
        {
            if (AuctionState.currentPlayer == MyData.position)
            {
                AuctionDialog.enabled = true;  // showing dialog

                // these 2 lines below should be updated after filling contract
                string prefix = "Licytujesz : ";
                AuctionContractPreviewText.text = prefix + contractCache.ToString();
            }
            else
            {
                AuctionDialog.enabled = false; // hiding dialog
            }
        }
    }

    void declareNewContract()
    {
        string contractString = contractCache.ToString();
        if (!contractString.Contains("X")) // any contract that is not X or XX
        {
            if (AuctionState.currentContract == null)
            {
                AuctionState.currentContract = contractString;
            }
            else
            {
                bool isContractConsistent = AuctionState.IsContractConsistent(contractString);
                if (isContractConsistent)
                {
                    AuctionState.currentContract = contractString;
                    AuctionState.xEnabled = false;
                    AuctionState.xxEnabled = false;
                }
                else
                    return;
            }
        }
        else
        {
            if (contractString.Contains("XX"))
            {
                if (AuctionState.xEnabled)
                {
                    AuctionState.xEnabled = false;
                    AuctionState.xxEnabled = true;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (!AuctionState.xxEnabled && !AuctionState.xEnabled)
                {
                    AuctionState.xEnabled = true;
                }
                else
                {
                    return;
                }
            }
        }
        
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
    }
}
