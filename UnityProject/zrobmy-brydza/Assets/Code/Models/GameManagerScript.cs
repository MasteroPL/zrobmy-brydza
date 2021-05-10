using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Code.Models;
using Assets.Code.UI;
using GameManagerLib.Models;
using Assets.Code.Utils;
using UnityEngine.SceneManagement;
using EasyHosting.Models.Serialization;
using EasyHosting.Meta;
using EasyHosting.Models.Client;
using EasyHosting.Models.Actions;
using Newtonsoft.Json.Linq;
using GetTableInfoSerializer = ServerSocket.Actions.GetTableInfo.ResponseSerializer;
using System;
using Assets.Code.Models.Exceptions;
using EasyHosting.Models.Client.Serializers;
using ServerSocket.Serializers;

public class GameManagerScript : MonoBehaviour
{
    public enum ConnectionState {
        /// <summary>
        /// Pierwsze wczytywanie ekranu
        /// </summary>
        PRELOADING = 0,
        /// <summary>
        /// Brak requestow w trakcie oczekiwania
        /// </summary>
        IDLE = 1,
        /// <summary>
        /// Oczekiwanie na odpowiedź od zapytania
        /// </summary>
        LOADING = 2
    }

    class AuthData : BaseSerializer {
        [SerializerField(apiName: "lobby-id")]
        public string LobbyId;

        [SerializerField(apiName: "login")]
        public string Login;

        [SerializerField(apiName: "lobby-password")]
        public string LobbyPassword;
    }

    /**
     * My hand cards in deck coordinates on screen (according to main camera)
     * ======================================================================
     * Y = -2.73
     * X = [-5.69277, -5.25, -4.80724, -4.36446, -3.92169, -3.47892, -3.03615, -2.59344, -2.150609, -1.70784, -1.26507, -0.8223, -0.37953]
     * 
     * Partner hand cards in deck coordinates on screen (according to main camera)
     * ===========================================================================
     * X = [-5.69277, -5.25, -4.80724, -4.36446, -3.92169, -3.47892, -3.03615, -2.59344, -2.150609, -1.70784, -1.26507, -0.8223, -0.37953]
     * Y = 3.597
     */
    [SerializeField] GameObject hiddenCard;
    [SerializeField] AuctionModule AuctionModule;
    //public UserData UserData;
    [SerializeField] public SeatsManagerScript SeatManager;

    public Game Game;
    private List<GameObject> HiddenCardsOfPlayerN;
    private List<GameObject> HiddenCardsOfPlayerE;
    private List<GameObject> HiddenCardsOfPlayerS;
    private List<GameObject> HiddenCardsOfPlayerW;

    private ConnectionState _CurrentConnectionState = ConnectionState.PRELOADING;
    public ConnectionState CurrentConnectionState { get { return _CurrentConnectionState;  } private set { _CurrentConnectionState = value; } }
    private Request CurrentRequest = null;
    private Action<Request, ActionsSerializer, object> CurrentRequestCallback = null;
    private object CurrentRequestAddtionalData = null;

    // referencje do przyciskow z panelu chatu/licytacji/punktacji
    [SerializeField] Button ChatButton;
    [SerializeField] Button AuctionButton;
    [SerializeField] Button PointsButton;

    // przyciski przydatne podczas gry - "biore wszystko", "nie biore nic", "pauza", "wyjdź" (taunt jest oddzielnie)
    [SerializeField] Button TakeEverythingButton;
    [SerializeField] Button TakeNothingButton;
    [SerializeField] Button PauseRequestButton;
    [SerializeField] Button QuitButton;
    [SerializeField] Button StartButton;
    [SerializeField] TextManager TextManager;

    // lista graczy obecnych w lobby
    LobbyUserSerializer[] LobbyUsers;
    const int MAX_USERS_IN_LOBBY = 10;

    /**
     * -1 - neutral state
     * 0  - take everything
     * 1  - take nothing
     */
    private int ButtonPanelCanvasState = -1;
    private bool RequestingForPause = false;

    public static ActionsSerializer WrapRequestData(string actionName, JObject data) {
        var result = new ActionsSerializer();
        result.Actions = new ActionSerializer[1];
        var tmp = new ActionSerializer();

        tmp.ActionName = actionName;
        tmp.ActionData = data;

        result.Actions[0] = tmp;

        return result;
    }


    private void HandleLobbySignal(JObject signalData) {
        var signalName = (string)signalData.GetValue("signal");

        // Nowy użytkownik dołączył do stołu
        if(signalName == LobbySignalUserJoinedSerializer.SIGNAL_USER_JOINED) {
            var serializer = new LobbySignalUserJoinedSerializer(signalData);
            serializer.Validate();

            TextManager.AddMessage("Użytkownik " + serializer.Username + " dołączył do stołu.");
            // Jakaś lista?
        }
        // Użytkownik usiadł na wybranym miejscu
        else if(signalName == LobbySignalUserSatSerializer.SIGNAL_USER_SAT) {
            var serializer = new LobbySignalUserSatSerializer(signalData);
            serializer.Validate();

            if (serializer.Username != UserData.Username) {
                if (SeatManager.IsSeatTaken((PlayerTag)serializer.PlaceTag)) {
                    SeatManager.SitOutPlayer((PlayerTag)serializer.PlaceTag);
                }

                SeatManager.SitPlayer((PlayerTag)serializer.PlaceTag, serializer.Username);
            }
        }
        // Użytkownik wstał/został wysadzony z wybranego miejsca
        else if(signalName == LobbySignalUserSittedOutSerializer.SIGNAL_USER_SITTED_OUT) {
            var serializer = new LobbySignalUserSittedOutSerializer(signalData);
            serializer.Validate();

            var placeTag = (PlayerTag)serializer.PlaceTag;
            if (SeatManager.IsSeatTaken(placeTag)) {
                if(SeatManager.PlayersNicknames[placeTag] == serializer.Username) {
                    SeatManager.SitOutPlayer(placeTag);
                }
            }
        }
        // Użytkownik wyszedł/został usunięty z lobby
        else if(signalName == LobbySignalUserRemovedSerializer.SINGAL_USER_REMOVED) {
            var serializer = new LobbySignalUserRemovedSerializer(signalData);
            serializer.Validate();

            if (serializer.WasSitted) {
                var placeTag = (PlayerTag)serializer.PlaceTag;
                if (SeatManager.IsSeatTaken(placeTag) && SeatManager.PlayersNicknames[placeTag] == serializer.Username) {
                    SeatManager.SitOutPlayer(placeTag);
                }
            }

            TextManager.AddMessage("Użytkownik " + serializer.Username + " odszedł od stołu.");
        }
    }
    private void OnServerSignalReceive(object sender, StandardResponseWrapperSerializer data) {
        Debug.Log(data.CommunicateType);
        Debug.Log(data.Data);

        switch (data.CommunicateType) {
            case "LOBBY_SIGNAL":
                HandleLobbySignal(data.Data);
                break;

            default:
                Debug.Log("Unrecognized signal");
                break;
        }
    }

    /// <summary>
    /// Generyczna metoda obsługi wykonywania akcji synchronicznie z serwerem
    /// 
    /// Po zakończeniu zapytania dane wynikowe wysyłane zostają do metody callback
    /// </summary>
    /// <param name="actionName">Nazwa akcji</param>
    /// <param name="data">Dane akcji (dane zapytania)</param>
    /// <param name="callback">Metoda do której należy wysłać odpowiedź zwrotną</param>
    /// <param name="additionalData">Ten parametr zostanie bezpośrednio przekazany do metody zwrotnej</param>
    /// <exception cref="RequestingInProgressException">Rzucany w przypadku kiedy inne zapytanie jest juz w trakcie wykonywania</exception>
    public void PerformServerAction(string actionName, JObject data, Action<Request, ActionsSerializer, object> callback = null, object additionalData = null) {
        if(CurrentConnectionState != ConnectionState.IDLE) {
            throw new RequestInProgressException("Inne zapytanie jest juz w trakcie wykonywania");
        }

        CurrentConnectionState = ConnectionState.LOADING;
        var requestData = WrapRequestData(actionName, data);
        CurrentRequest = UserData.ClientConnection.SendRequest(requestData.GetApiObject());
        CurrentRequestCallback = callback;
        CurrentRequestAddtionalData = additionalData;
    }

    void Start()
    {
        GameObject.Find("TeamTakenHandsCounterLabel").GetComponent<Text>().text = "";
        GameObject.Find("DeclaredContractLabel").GetComponent<Text>().text = "";

        // przypisanie click handlerów do przyciskow panelu chatu/licytacji/punktacji
        ChatButton.onClick.AddListener(() => { TextManager.ChatButton(); });
        AuctionButton.onClick.AddListener(() => { TextManager.BidButton(); });
        PointsButton.onClick.AddListener(() => { TextManager.PointsButton(Game); });
        TextManager.ChatButton(); // inicjalnie otwarty jest panel chatu

        TakeEverythingButton.onClick.AddListener(() => { TakeEverythingHandler(); });
        TakeNothingButton.onClick.AddListener(() => { TakeNothingHandler(); });
        PauseRequestButton.onClick.AddListener(() => { PauseRequestHandler(); });
        QuitButton.onClick.AddListener(() => { QuitHandler(); });

        // przycisk "start" pokaze sie tylko gdy bedzie 4 graczy
        if (StartButton != null && UserData.Position != PlayerTag.NOBODY)
        {
            StartButton.gameObject.SetActive(false);
        }

        if (!UserData.LoggedIn) {
            // DEBUG: Przypisuję sobie domyślnie dane użytkownika z autoryzacją
            // auth request
            var authData = new AuthData() {
                LobbyId = "DEFAULT",
                Login = "DEBUG#1",
                LobbyPassword = ""
            };
            var clientSocket = new ClientSocket("127.0.0.1");
            var authRequest = clientSocket.SendRequest(authData.GetApiObject());

            while (authRequest.RequestState != RequestState.RESPONSE_RECEIVED) {
                clientSocket.UpdateCommunication();
            }

            UserData.Username = "DEBUG#1";
            UserData.LoggedIn = true;
            UserData.ClientConnection = clientSocket;
        }
        UserData.ClientConnection.SignalReceived += OnServerSignalReceive;

        SeatManager.InitializeSeatManager();
    }
    /// <summary>
    /// Metoda obsługuje przypadek preloadingu w pętli Update
    /// </summary>
    private void HandlePreloading() {
        // Pierwsze wywołanie
        if(CurrentRequest == null) {
            var tableInfoRequestData = WrapRequestData("get-table-info", null);
            CurrentRequest = UserData.ClientConnection.SendRequest(tableInfoRequestData.GetApiObject());
        }

        // Aktualizacja komunikacji
        CurrentRequest.ParentSocket.UpdateCommunication();

        // Otrzymaliśmy odpowiedź na zapytanie
        if(CurrentRequest.RequestState == RequestState.RESPONSE_RECEIVED) {
            var request = CurrentRequest;
            CurrentRequest = null;
            this.CurrentConnectionState = ConnectionState.IDLE;
            Debug.Log(request.ResponseData);
            var actionsSerializer = new ActionsSerializer(request.ResponseData);
            actionsSerializer.Validate();
            var responseSerializer = new GetTableInfoSerializer(actionsSerializer.Actions[0].ActionData);
            try {
                responseSerializer.Validate();

                if (responseSerializer.Status == "OK") {
                    // Przetwarzanie odpowiedzi
                    if (responseSerializer.NumberOfLobbyUsers == 1) {
                        UserData.IsAdmin = true;
                    }
                    else {
                        UserData.IsAdmin = false;
                    }

                    switch (responseSerializer.GameState)
                    {
                        case (int)GameState.AWAITING_PLAYERS:
                            ReloadTableAwaitingState(responseSerializer);
                            break;
                        case (int)GameState.BIDDING:
                            ReloadTableBiddingState(responseSerializer);
                            break;
                    }
                    // TODO Inne stany gry
                }
            } catch (Exception ex) {
                Debug.Log(ex.Message);
            }
        }
    }
    /// <summary>
    /// Obsługa dla przypadku "Loading"
    /// </summary>
    private void HandleLoading() {
        CurrentRequest.ParentSocket.UpdateCommunication();

        if(CurrentRequest.RequestState == RequestState.RESPONSE_RECEIVED) {
            Debug.Log(CurrentRequest.ResponseData);
            var request = CurrentRequest;
            var requestCallback = CurrentRequestCallback;
            var additionalData = CurrentRequestAddtionalData;
            CurrentRequest = null;
            CurrentRequestCallback = null;
            CurrentRequestAddtionalData = null;

            CurrentConnectionState = ConnectionState.IDLE;

            var actsSer = new ActionsSerializer(request.ResponseData);
            actsSer.Validate();

            // ?.Invoke - jeśli nie null, to wywołaj Invoke
            requestCallback?.Invoke(request, actsSer, additionalData);
        }
    }

    void Update() {
        switch (this.CurrentConnectionState) {
            // Przypadek, kiedy ekran nie otrzymal jeszcze informacji od serwera
            case ConnectionState.PRELOADING:
                this.HandlePreloading();
                break;
            case ConnectionState.LOADING:
                this.HandleLoading();
                break;
            case ConnectionState.IDLE:
                UserData.ClientConnection.UpdateCommunication();
                break;
        }

        if (SeatManager.AllFourPlayersPresent() && UserData.Position != PlayerTag.NOBODY && Game.GameState == GameState.AWAITING_PLAYERS)
        {
            if (StartButton != null) StartButton.gameObject.SetActive(true);
        }
        else
        {
            if (StartButton != null) StartButton.gameObject.SetActive(false);
        }
    }

    private void ReloadTableAwaitingState(GetTableInfoSerializer tableData)
    {
        Game = new Game(this);
        Game.GameState = GameState.AWAITING_PLAYERS;

        Game.Match.RoundsNS = tableData.RoundsNS;
        Game.Match.RoundsWE = tableData.RoundsWE;

        Game.Match.PointsNS[0] = tableData.PointsNSBelowLine; // [0] - under line, [1] - above line
        Game.Match.PointsNS[1] = tableData.PointsNSAboveLine;

        Game.Match.PointsWE[0] = tableData.PointsWEBelowLine; // [0] - under line, [1] - above line
        Game.Match.PointsWE[1] = tableData.PointsWEAboveLine;

        for (int i = 0; i < 4; i++)
        {
            if (tableData.Players[i] != null)
            {
                Game.Match.AddPlayer(new Player((PlayerTag)tableData.Players[i].PlayerTag, tableData.Players[i].Username));
                SeatManager.SitPlayer((PlayerTag)tableData.Players[i].PlayerTag, tableData.Players[i].Username);
            }
        }

        LobbyUsers = new LobbyUserSerializer[MAX_USERS_IN_LOBBY];
        for(int i = 0; i < tableData.LobbyUsers.Length; i++)
        {
            LobbyUsers[i] = tableData.LobbyUsers[i];
        }
    }

    private void ReloadTableBiddingState(GetTableInfoSerializer tableData)
    {
        Game = new Game(this);
        Game.GameState = GameState.BIDDING;

        for (int i = 0; i < 4; i++)
        {
            if (tableData.Players[i] != null)
            {
                Game.Match.AddPlayer(new Player((PlayerTag)tableData.Players[i].PlayerTag, tableData.Players[i].Username));
                SeatManager.SitPlayer((PlayerTag)tableData.Players[i].PlayerTag, tableData.Players[i].Username);
            }
        }

        Game.Match.Dealer = (PlayerTag)tableData.Dealer;
        Game.Match.Start();

        foreach (var contract in tableData.CurrentBidding.ContractList)
        {
            if (contract != null)
            {
                Game.Match.AddBid(new Contract(
                    (ContractHeight)contract.ContractHeight,
                    (ContractColor)contract.ContractColor,
                    (PlayerTag)contract.PlayerTag,
                    contract.XEnabled,
                    contract.XXEnabled
                ));
            }
        }

        Game.Match.RoundsNS = tableData.RoundsNS;
        Game.Match.RoundsWE = tableData.RoundsWE;

        Game.Match.History.AddNSHistory(tableData.PointsNSBelowLine, tableData.PointsNSAboveLine);
        Game.Match.History.AddWEHistory(tableData.PointsWEBelowLine, tableData.PointsWEAboveLine);

        Game.Match.PointsNS[0] = tableData.PointsNSBelowLine; // [0] - under line, [1] - above line
        Game.Match.PointsNS[1] = tableData.PointsNSAboveLine;

        Game.Match.PointsWE[0] = tableData.PointsWEBelowLine; // [0] - under line, [1] - above line
        Game.Match.PointsWE[1] = tableData.PointsWEAboveLine;

        LobbyUsers = new LobbyUserSerializer[MAX_USERS_IN_LOBBY];
        for (int i = 0; i < tableData.LobbyUsers.Length; i++)
        {
            LobbyUsers[i] = tableData.LobbyUsers[i];
        }

        PlayerTag StartingPlayer = Game.Match.CurrentBidding.CurrentPlayer;
        AuctionModule.InitAuctionModule(Game, StartingPlayer);
        AuctionModule.ReloadDeclarations();
    }

    public void ShowHideStartGameButton(bool AllPlayersPresent)
    {
        if (AllPlayersPresent && UserData.Position != PlayerTag.NOBODY)
        {
            if (StartButton != null)
            {
                StartButton.gameObject.SetActive(true);
            }
        }
        else
        {
            if (StartButton != null)
            {
                StartButton.gameObject.SetActive(false);
            }
        }
    }

    void OnDestroy()
    {
        UserData.ClientConnection.SignalReceived -= OnServerSignalReceive;

        if (ChatButton) ChatButton.onClick.RemoveAllListeners();
        if (AuctionButton) AuctionButton.onClick.RemoveAllListeners();
        if (PointsButton) PointsButton.onClick.RemoveAllListeners();

        if (TakeEverythingButton) TakeEverythingButton.onClick.RemoveAllListeners();
        if (TakeNothingButton) TakeNothingButton.onClick.RemoveAllListeners();
        if (PauseRequestButton) PauseRequestButton.onClick.RemoveAllListeners();
        if (QuitButton) QuitButton.onClick.RemoveAllListeners();
        if (StartButton) StartButton.onClick.RemoveAllListeners();
    }

    private void TakeEverythingHandler()
    {
        Debug.Log("I'm taking everything");
        if (ButtonPanelCanvasState != 0)
        {
            ButtonPanelCanvasState = 0;
        } 
        else
        {
            ButtonPanelCanvasState = -1;
        }
        UpdateButtonPanelCanvas();
    }

    private void TakeNothingHandler()
    {
        Debug.Log("I take nothing");
        if (ButtonPanelCanvasState != 1)
        {
            ButtonPanelCanvasState = 1;
        }
        else
        {
            ButtonPanelCanvasState = -1;
        }
        UpdateButtonPanelCanvas();
    }

    private void PauseRequestHandler()
    {
        Debug.Log("Pause request");
        RequestingForPause = !RequestingForPause;
        UpdateButtonPanelCanvas();
    }

    private void UpdateButtonPanelCanvas()
    {
        TakeEverythingButton.image.color = new Color32(255, 255, 255, 255);
        TakeNothingButton.image.color = new Color32(255, 255, 255, 255);
        if(ButtonPanelCanvasState == 0)
        {
            TakeEverythingButton.image.color = new Color32(0, 0, 0, 255);
        } 
        else if(ButtonPanelCanvasState == 1)
        {
            TakeNothingButton.image.color = new Color32(0, 0, 0, 255);
        }

        PauseRequestButton.image.color = new Color32(255, 255, 255, 255);
        if (RequestingForPause)
        {
            PauseRequestButton.image.color = new Color32(0, 0, 0, 255);
        } 
        else
        {
            PauseRequestButton.image.color = new Color32(255, 255, 255, 255);
        }
    }

    private void QuitHandler()
    {
        SceneManager.LoadScene(0);
    }

    private void CurrentPlayerLight()
    {
        GameObject.Find("Player3IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 255f, 100f);
        GameObject.Find("Player4IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 255f, 100f);
        GameObject.Find("Player1IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 255f, 100f);
        GameObject.Find("Player2IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 255f, 100f);

        if (Game != null && Game.Match != null)
        {
            string PlayerTagString = "";
            if (Game.GameState == GameState.BIDDING)
            {
                PlayerTagString = Game.Match.CurrentBidding.CurrentPlayer.ToString();
            }
            else if (Game.GameState == GameState.PLAYING)
            {
                PlayerTagString = Game.Match.CurrentGame.CurrentPlayer.ToString();
            }

            if (PlayerTagString == GameObject.Find("Player3IndicatorText").GetComponent<Text>().text)
            {
                GameObject.Find("Player3IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 0f, 242f);
            }
            else if (PlayerTagString == GameObject.Find("Player4IndicatorText").GetComponent<Text>().text)
            {
                GameObject.Find("Player4IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 0f, 242f);
            }
            else if (PlayerTagString == GameObject.Find("Player1IndicatorText").GetComponent<Text>().text)
            {
                GameObject.Find("Player1IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 0f, 242f);
            }
            else if (PlayerTagString == GameObject.Find("Player2IndicatorText").GetComponent<Text>().text)
            {
                GameObject.Find("Player2IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 0f, 242f);
            }
        }
    }

    public void UpdateTableCenter(Game Game)
    {
        GameObject.Find("Player3IndicatorText").GetComponent<Text>().text = UserData.Position.ToString();
        GameObject.Find("Player4IndicatorText").GetComponent<Text>().text = ((PlayerTag)(((int)UserData.Position + 1) % 4)).ToString();
        GameObject.Find("Player1IndicatorText").GetComponent<Text>().text = ((PlayerTag)(((int)UserData.Position + 2) % 4)).ToString();
        GameObject.Find("Player2IndicatorText").GetComponent<Text>().text = ((PlayerTag)(((int)UserData.Position + 3) % 4)).ToString();
    }

    // this method is implemented in case concrete user switches his place - currently not used
    public void UpdateTable(Game Game, GameManagerLib.Models.Card[] PlayerHand)
    {
        UpdateTableCenter(Game);
        switch (UserData.Position)
        {
            case PlayerTag.N:
                GiveCardsToPlayer(PlayerTag.N, PlayerHand);
                break;
            case PlayerTag.E:
                GiveCardsToPlayer(PlayerTag.E, PlayerHand);
                break;
            case PlayerTag.S:
                GiveCardsToPlayer(PlayerTag.S, PlayerHand);
                break;
            case PlayerTag.W:
                GiveCardsToPlayer(PlayerTag.W, PlayerHand);
                break;
        }
        GiveHiddenCardsToPlayers(UserData.Position);
    }

    public void StartGame(Game Game, GameManagerLib.Models.Card[] PlayerHand)
    {
        this.Game = Game;
        HiddenCardsOfPlayerN = new List<GameObject>();
        HiddenCardsOfPlayerE = new List<GameObject>();
        HiddenCardsOfPlayerS = new List<GameObject>();
        HiddenCardsOfPlayerW = new List<GameObject>();

        // for dev mode
        if (GameConfig.DevMode)
        {
            GiveCardsToPlayer(PlayerTag.N, Game.Match.PlayerList[0].Hand);
            GiveCardsToPlayer(PlayerTag.S, Game.Match.PlayerList[2].Hand);
            GiveCardsToPlayer(PlayerTag.W, Game.Match.PlayerList[3].Hand);
            GiveCardsToPlayer(PlayerTag.E, Game.Match.PlayerList[1].Hand);
        } else
        {
            UpdateTable(Game, PlayerHand);
        }

        PlayerTag StartingPlayer = Game.Match.CurrentBidding.CurrentPlayer;
        AuctionModule.InitAuctionModule(Game, StartingPlayer);

        GameObject auctionObject = GameObject.Find("/Canvas/TableCanvas/AuctionDialog");
        auctionObject.SetActive(true);
        if (StartButton != null)
        {
            StartButton.gameObject.SetActive(false);
        }
        InvokeRepeating("CurrentPlayerLight", 0.5f, 0.05f);
    }

    public void RestartGame()
    {
        Game.Match.GameState = GameState.BIDDING;
        if (GameConfig.DevMode)
        {
            UserData.Position = UserData.PositionStart;
        }

        GameObject.Find("TeamTakenHandsCounterLabelBackground").GetComponent<Image>().color = new Color32(219, 31, 35, 0);
        GameObject.Find("DeclaredContractLabelBackground").GetComponent<Image>().color = new Color32(219, 31, 35, 0);

        if (GameObject.Find("/Canvas/InfoCanvas/InfoTable/Body/PointsPanel").activeInHierarchy)
        {
            TextManager.PointsButton(Game);
        }

        AuctionModule.ReleaseListeners();

        ReleaseCardsOwners();
        CancelInvoke();
        Game.RestartGame();
    }

    private void ReleaseCardsOwners()
    {
        GameObject cardObject;
        for (int i = 0; i < Game.Match.PlayerList.Count; i++)
        {
            foreach (GameManagerLib.Models.Card card in Game.Match.PlayerList[i].Hand)
            {
                string cardName = CalculateCardName(card);
                cardObject = GameObject.Find(cardName);
                cardObject.GetComponent<Card>().PlayerID = PlayerTag.NOBODY;
                cardObject.transform.position = new Vector3(-100, 0, 0);
            }
        }
    }

    public void ShowGrandCards(PlayerTag grand, GameManagerLib.Models.Card[] grandHand)
    {
        switch (grand)
        {
            case PlayerTag.N:
                DestroyHiddenCards(HiddenCardsOfPlayerN);
                break;
            case PlayerTag.E:
                DestroyHiddenCards(HiddenCardsOfPlayerE);
                break;
            case PlayerTag.S:
                DestroyHiddenCards(HiddenCardsOfPlayerS);
                break;
            case PlayerTag.W:
                DestroyHiddenCards(HiddenCardsOfPlayerW);
                break;
        }
        GiveCardsToPlayer(grand, grandHand);
    }

    private void DestroyHiddenCards(List<GameObject> cards)
    {
        for(int i = 0; i < cards.Count; i++)
        {
            Destroy(cards[i]);
        }
    }

    private void GiveHiddenCardsToPlayers(PlayerTag MyPosition)
    {
        foreach (int player in System.Enum.GetValues(typeof(PlayerTag)))
        {
            GameObject CardsObject = GameObject.Find("Cards");

            if (player != (int)MyPosition && player != (int)PlayerTag.NOBODY)
            {
                List<float[]> coordinates = CalculateCardsCoordinates((PlayerTag)player);
                for (int i = 0; i < 13; i++)
                {
                    GameObject card = Instantiate(hiddenCard);
                    card.transform.localPosition = new Vector3(coordinates[i][0] + CardsObject.gameObject.transform.position.x, coordinates[i][1] + CardsObject.gameObject.transform.position.y, 0);
                    if (Mathf.Abs((int)MyPosition - (int)player) != 2)
                    {
                        card.transform.rotation = Quaternion.Euler(0, 0, 90f);
                    }

                    switch ((PlayerTag)player)
                    {
                        case PlayerTag.N:
                            HiddenCardsOfPlayerN.Add(card);
                            break;
                        case PlayerTag.E:
                            HiddenCardsOfPlayerE.Add(card);
                            break;
                        case PlayerTag.S:
                            HiddenCardsOfPlayerS.Add(card);
                            break;
                        case PlayerTag.W:
                            HiddenCardsOfPlayerW.Add(card);
                            break;
                    }
                }
            }
        }
    }

    private List<float[]> CalculateCardsCoordinates(PlayerTag Position)
    {
        float[] myCardsX = { -2.37f, -1.975f, -1.58f, -1.185f, -0.79f, -0.395f, 0.0f, 0.395f, 0.79f, 1.185f, 1.58f, 1.975f, 2.37f };
        float[] opCardsY = { 1.72f, 1.4334f, 1.1468f, 0.86f, 0.5736f, 0.287f, 0.0f, -0.2862f, -0.5728f, -0.8594f, -1.146f, -1.43f, -1.72f };

        List<float[]> coordinates = new List<float[]>();
        if ((int)Position == (int)UserData.Position) // down
        {
            for(int i = 0; i < 13; i++)
            {
                float[] tmp = new float[3];
                tmp[0] = myCardsX[i];
                tmp[1] = -2.58f; // previous y value: -3.28f;
                tmp[2] = -i;
                coordinates.Add(tmp);
            }
        }
        else if ((int)Position == (((int)UserData.Position + 1) % 4)) // left
        {
            for (int i = 0; i < 13; i++)
            {
                float[] tmp = new float[3];
                tmp[0] = -4.37f; // previous x value: -4.61f;
                tmp[1] = opCardsY[i] + 0.2f;
                tmp[2] = -i;
                coordinates.Add(tmp);
            }
        }
        else if ((int)Position == (((int)UserData.Position + 2) % 4)) // up
        {
            for (int i = 0; i < 13; i++)
            {
                float[] tmp = new float[3];
                tmp[0] = myCardsX[i];
                tmp[1] = 2.78f; // previous y value: 3.07f;
                tmp[2] = -i;
                coordinates.Add(tmp);
            }
        }
        else if ((int)Position == (((int)UserData.Position + 3) % 4)) // right
        {
            for (int i = 0; i < 13; i++)
            {
                float[] tmp = new float[3];
                tmp[0] = 4.37f; // previous x value: 4.61f;
                tmp[1] = opCardsY[i] + 0.2f;
                tmp[2] = -i;
                coordinates.Add(tmp);
            }
        }
        return coordinates;
    }

    private void GiveCardsToPlayer(PlayerTag PlayerIdentifier, GameManagerLib.Models.Card[] cards)
    {
        List<float[]> coordinates = CalculateCardsCoordinates(PlayerIdentifier);
        for (int i = 0; i < cards.Length; i++)
        {
            string cardName = CalculateCardName(cards[i]);
            GameObject card = GameObject.Find(cardName);
            card.transform.localPosition = new Vector3(coordinates[i][0], coordinates[i][1], coordinates[i][2]);
            
            card.GetComponent<Card>().PlayerID = PlayerIdentifier;
            SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
            sr.sortingOrder = i + 1;
        }
    }

    private string CalculateCardName(GameManagerLib.Models.Card card)
    {
        char color = ' ', figure = ' ';
        bool errorOccurred = false;
        switch (card.Color)
        {
            case CardColor.CLUB:
                color = 'C';
                break;
            case CardColor.DIAMOND:
                color = 'D';
                break;
            case CardColor.HEART:
                color = 'H';
                break;
            case CardColor.SPADE:
                color = 'S';
                break;
            default:
                errorOccurred = true;
                break;
        }

        if ((int)card.Figure < 2)
        {
            errorOccurred = true;
        }
        else if ((int)card.Figure > 9)
        {
            switch (card.Figure)
            {
                case CardFigure.ACE:
                    figure = 'A';
                    break;
                case CardFigure.KING:
                    figure = 'K';
                    break;
                case CardFigure.QUEEN:
                    figure = 'Q';
                    break;
                case CardFigure.JACK:
                    figure = 'J';
                    break;
                case CardFigure.TEN:
                    figure = 'T';
                    break;
                default:
                    errorOccurred = true;
                    break;
            }
        }
        else
        {
            figure = ((int)card.Figure).ToString()[0];
        }

        if (!errorOccurred)
        {
            return "CARD_" + figure + color;
        }
        return "";
    }

    public void putCard(Card card)
    {
        bool putOK = Game.PutCard(card.Figure, card.Color, card.PlayerID);
        if (putOK)
        {
            if (card.CurrentState == CardState.ON_HAND)
            {
                string c = "";

                switch (card.Color)
                {
                    case CardColor.CLUB:
                        c = "C";
                        break;
                    case CardColor.DIAMOND:
                        c = "D";
                        break;
                    case CardColor.HEART:
                        c = "H";
                        break;
                    case CardColor.SPADE:
                        c = "S";
                        break;
                }

                string cardName = "";
                if ((int)card.Figure > 9)
                {
                    switch ((int)card.Figure)
                    {
                        case 10: // 10
                            cardName = "CARD_T" + c;
                            break;
                        case 11: // J
                            cardName = "CARD_J" + c;
                            break;
                        case 12: // Q
                            cardName = "CARD_Q" + c;
                            break;
                        case 13: // K
                            cardName = "CARD_K" + c;
                            break;
                        case 14: // A
                            cardName = "CARD_A" + c;
                            break;
                    }
                }
                else
                {
                    cardName = "CARD_" + (int)card.Figure + c;
                }

                /*float newXpos = 0;
                float newYpos = 0;

                switch (card.PlayerID) // to reconsider, positions are relative to player who sits
                {
                    case PlayerTag.N: // down
                        newXpos = -3.02f;
                        newYpos = -1.03f;
                        break;
                    case PlayerTag.E: // left
                        newXpos = -4.19f;
                        newYpos = 0.41f;
                        break;
                    case PlayerTag.S: // up
                        newXpos = -3.02f;
                        newYpos = 1.87f;
                        break;
                    case PlayerTag.W: // right
                        newXpos = -1.8f;
                        newYpos = 0.41f;
                        break;
                }*/


                float[] newPos = new float[2];
                // WARNING! For production 'positionStart' should be replaced with 'position' 
                if (card.PlayerID == UserData.PositionStart)
                {
                    newPos = CalculatePutCardPosition('D');
                } 
                else if(card.PlayerID == (PlayerTag)(( (int)UserData.PositionStart + 1) % 4 ))
                {
                    newPos = CalculatePutCardPosition('L');
                }
                else if (card.PlayerID == (PlayerTag)(((int)UserData.PositionStart + 2) % 4))
                {
                    newPos = CalculatePutCardPosition('U');
                }
                else if (card.PlayerID == (PlayerTag)(((int)UserData.PositionStart + 3) % 4))
                {
                    newPos = CalculatePutCardPosition('R');
                }

                GameObject cardToPut = GameObject.Find(cardName);
                cardToPut.transform.localPosition = new Vector3(newPos[0], newPos[1]);

                if (Game.Match.CurrentGame.TrickList.Count == 0 && Game.Match.CurrentGame.currentTrick.CardList.Count == 1 && !GameConfig.DevMode)
                {
                    Game.ShowGrandCards();
                }
                if (GameConfig.DevMode)
                {
                    UserData.Position = (PlayerTag)(((int)UserData.Position + 1) % 4); // for dev mode
                }

                if (Game.IsTrickComplete())
                {
                    SleepFor2Seconds();
                    Trick lastTrick = Game.Match.CurrentGame.TrickList[Game.Match.CurrentGame.TrickList.Count - 1];
                    GameObject tmp;
                    for (int i = 0; i < lastTrick.CardList.Count; i++)
                    {
                        string tmpCardName = CalculateCardName(lastTrick.CardList[i]);
                        tmp = GameObject.Find(tmpCardName);
                        tmp.transform.position = new Vector3(-100, 0, 0);
                    }

                    Text TeamTakenHandsCounterLabel = GameObject.Find("TeamTakenHandsCounterLabel").GetComponent<Text>();
                    int NSTaken = Game.CalculateTeamTricks(PlayerTag.N, PlayerTag.S);
                    int EWTaken = Game.CalculateTeamTricks(PlayerTag.E, PlayerTag.W);

                    TeamTakenHandsCounterLabel.text = "NS : " + NSTaken.ToString() + "\n";
                    TeamTakenHandsCounterLabel.text += "EW : " + EWTaken.ToString();

                    if (Game.Match.CurrentGame.Declarer == PlayerTag.N || Game.Match.CurrentGame.Declarer == PlayerTag.S)
                    {
                        PaintContractLabel(NSTaken, EWTaken);
                    }
                    else if (Game.Match.CurrentGame.Declarer == PlayerTag.E || Game.Match.CurrentGame.Declarer == PlayerTag.W)
                    {
                        PaintContractLabel(EWTaken, NSTaken);
                    }

                    if (GameConfig.DevMode)
                    {
                        UserData.Position = lastTrick.Winner; // for dev mode
                    }

                    if (Game.Match.CurrentGame.IsEnd())
                    {
                        RestartGame(); // restart game if all 13 tricks were put on the table
                    }
                }
            }
        }
    }

    private void PaintContractLabel(int TakenHands, int EnemyTakenHands)
    {
        if (Game.Match.CurrentBidding.HighestContract != null)
        {
            if (6 + (int)Game.Match.CurrentBidding.HighestContract.ContractHeight <= TakenHands)
            {
                GameObject.Find("TeamTakenHandsCounterLabelBackground").GetComponent<Image>().color = new Color32(29, 143, 35, 255);
                GameObject.Find("DeclaredContractLabelBackground").GetComponent<Image>().color = new Color32(29, 143, 35, 255);
            }
            else if (EnemyTakenHands > 13 - 6 - (int)Game.Match.CurrentBidding.HighestContract.ContractHeight)
            {
                GameObject.Find("TeamTakenHandsCounterLabelBackground").GetComponent<Image>().color = new Color32(219, 31, 35, 255);
                GameObject.Find("DeclaredContractLabelBackground").GetComponent<Image>().color = new Color32(219, 31, 35, 255);
            }
        }
    }

    private float[] CalculatePutCardPosition(char Position)
    {
        float[] returned = new float[2];
        switch (Position)
        {
            case 'U':
                returned[0] = 0f;
                returned[1] = 1.25f;
                break;
            case 'D':
                returned[0] = 0f;
                returned[1] = -1.15f;
                break;
            case 'L':
                returned[0] = -1.0f;
                returned[1] = 0f;
                break;
            case 'R':
                returned[0] = 1.0f;
                returned[1] = 0f;
                break;
        }
        return returned;
    }

    IEnumerator SleepFor2Seconds()
    {
        yield return new WaitForSeconds(2);
    }

    public bool checkTurn()
    {
        return true;
    }
}