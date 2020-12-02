using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.UI;

public class AuctionModule : MonoBehaviour
{
    private GameManagerScript MainModule;
    [SerializeField] AuctionBaseState AuctionState;
    private UserData MyData;
    private string contractCache = "  ";

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
        MainModule = mainModule;
        MyData = userData;
        AuctionState.init(startingPlayer);

        // setting visibility of dialog for first render, later it'll be updated according to game state & current player
        if (AuctionState.currentPlayer == MyData.position)
            AuctionDialog.enabled = true;
        else
            AuctionDialog.enabled = false;

        // assigning handlers to buttons
        AuctionHeight1Button.onClick.AddListener(() => { contractCache = "1" + contractCache[1]; });
        AuctionHeight2Button.onClick.AddListener(() => { contractCache = "2" + contractCache[1]; });
        AuctionHeight3Button.onClick.AddListener(() => { contractCache = "3" + contractCache[1]; });
        AuctionHeight4Button.onClick.AddListener(() => { contractCache = "4" + contractCache[1]; });
        AuctionHeight5Button.onClick.AddListener(() => { contractCache = "5" + contractCache[1]; });
        AuctionHeight6Button.onClick.AddListener(() => { contractCache = "6" + contractCache[1]; });
        AuctionHeight7Button.onClick.AddListener(() => { contractCache = "7" + contractCache[1]; });

        ClubsSignButton.onClick.AddListener(() => { contractCache = contractCache[0] + "C"; });
        DiamondsSignButton.onClick.AddListener(() => { contractCache = contractCache[0] + "D"; });
        HeartsSignButton.onClick.AddListener(() => { contractCache = contractCache[0] + "H"; });
        SpadesSignButton.onClick.AddListener(() => { contractCache = contractCache[0] + "S"; });
        NTSignButton.onClick.AddListener(() => { contractCache = contractCache[0] + "NT"; });

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
                AuctionContractPreviewText.text = prefix + contractCache;
            }
            else
            {
                AuctionDialog.enabled = false; // hiding dialog
            }
        }
    }

    void declareNewContract()
    {
        if (AuctionState.currentContract == null)
        {
            AuctionState.currentContract = contractCache;
        }
        else
        {
            bool isContractConsistent = AuctionState.IsContractConsistent(contractCache);
            Debug.Log("Spójny kontrakt?" + isContractConsistent.ToString());
            if (isContractConsistent)
            {
                AuctionState.currentContract = contractCache;
            }
            else
                return;
        }
        switch (AuctionState.currentPlayer)
        {
            case PlayerTag.N:
                NPlayerDeclarations.text += contractCache + "\n";
                break;
            case PlayerTag.E:
                EPlayerDeclarations.text += contractCache + "\n";
                break;
            case PlayerTag.S:
                SPlayerDeclarations.text += contractCache + "\n";
                break;
            case PlayerTag.W:
                WPlayerDeclarations.text += contractCache + "\n";
                break;
        }
        contractCache = "  ";
        //AuctionState.currentPlayer = PlayerTag.E; // TODO setting proper player
    }
}
