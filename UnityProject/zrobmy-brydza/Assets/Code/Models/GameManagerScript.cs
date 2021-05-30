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
using GameManagerLib.Exceptions;
using ServerSocket.Models;

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

    [SerializeField] GameObject DeclaredContractLabel;
    [SerializeField] GameObject DeclaredContractLabelBackground;
    [SerializeField] GameObject TeamTakenHandsCounterLabel;
    [SerializeField] GameObject TeamTakenHandsCounterLabelBackground;

    /// <summary>
    /// W pewnych sytuacjach, na określony czas chcemy ignorować komunikację z serwerem. Ustawiamy wtedy tą flagę na "TRUE"
    /// </summary>
    public bool IgnoreCommunication = false;

    public Game Game;
    private List<GameObject> HiddenCardsOfPlayerN;
    private List<GameObject> HiddenCardsOfPlayerE;
    private List<GameObject> HiddenCardsOfPlayerS;
    private List<GameObject> HiddenCardsOfPlayerW;

    public LobbyState LobbyState = LobbyState.IDLE;
    bool LobbyOwner = true;

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
    [SerializeField] Button QuitButton;
    [SerializeField] Button StartButton;
    [SerializeField] TextManager TextManager;
    [SerializeField] Canvas GameStoppedDialog;

    public List<GameManagerLib.Models.Card> MyCards = null;
    public List<GameManagerLib.Models.Card> CurrentGrandpaCards = null;
    public List<GameManagerLib.Models.Card> CurrentDeclarerCards = null;

    // lista graczy obecnych w lobby
    List<LobbyUserData> LobbyUsers;

    [SerializeField] GameObject LeftPlayerCardsCanvas;
    [SerializeField] GameObject RightPlayerCardsCanvas;

    public bool PlayingAsGrandpa {
        get {
            var match = Game.Match;
            if (match.GameState != GameState.PLAYING) return false;

            if (((((int)match.CurrentGame.Declarer) + 2) % 4 == (int)UserData.Position)){
                return true;
            }
            return false;
        }
    }
    public bool GrandpaCardsVisible {
        get {
            var match = Game.Match;
            if (match.GameState == GameState.PLAYING) {
                if (match.CurrentGame.currentTrick.CardList.Count > 0
                    || match.CurrentGame.TrickList.Count > 0
                ) {
                    return true;
                }
            }

            return false;
        }
    }

    public bool DeclarerCardsVisible {
        get {
            var match = Game.Match;
            // Karty rozgrywającego są widoczne tylko dla jego dziadka
            if (!PlayingAsGrandpa) return false;

            // Widoczne wtedy kiedy widoczne stają się karty dziadka
            return GrandpaCardsVisible;
        }
    }

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
        if (signalName == LobbySignalUserJoinedSerializer.SIGNAL_USER_JOINED) {
            var serializer = new LobbySignalUserJoinedSerializer(signalData);
            serializer.Validate();

            TextManager.AddMessage("Użytkownik " + serializer.Username + " dołączył do stołu.");
            LobbyUsers.Add(new LobbyUserData(serializer.Username));
        }
        // Użytkownik usiadł na wybranym miejscu
        else if (signalName == LobbySignalUserSatSerializer.SIGNAL_USER_SAT) {
            var serializer = new LobbySignalUserSatSerializer(signalData);
            serializer.Validate();

            if (serializer.Username != UserData.Username) {
                if (SeatManager.IsSeatTaken((PlayerTag)serializer.PlaceTag)) {
                    SeatManager.SitOutPlayer((PlayerTag)serializer.PlaceTag);
                }
                SeatManager.SitPlayer((PlayerTag)serializer.PlaceTag, serializer.Username);
            }
            this.UpdateTableCenter(Game);
        }
        // Użytkownik wstał/został wysadzony z wybranego miejsca
        else if (signalName == LobbySignalUserSittedOutSerializer.SIGNAL_USER_SITTED_OUT) {
            var serializer = new LobbySignalUserSittedOutSerializer(signalData);
            serializer.Validate();

            var placeTag = (PlayerTag)serializer.PlaceTag;
            if (SeatManager.IsSeatTaken(placeTag)) {
                if (SeatManager.PlayersNicknames[placeTag] == serializer.Username) {
                    var index = Game.Match.PlayerList.FindIndex(x => { return x.Tag == placeTag; });
                    SeatManager.SitOutPlayer(placeTag);
                }
                this.UpdateTableCenter(Game);
            }
        }
        // Użytkownik wyszedł/został usunięty z lobby
        else if (signalName == LobbySignalUserRemovedSerializer.SINGAL_USER_REMOVED) {
            var serializer = new LobbySignalUserRemovedSerializer(signalData);
            serializer.Validate();

            if (serializer.WasSitted) {
                var placeTag = (PlayerTag)serializer.PlaceTag;
                if (SeatManager.IsSeatTaken(placeTag) && SeatManager.PlayersNicknames[placeTag] == serializer.Username) {
                    SeatManager.SitOutPlayer(placeTag);
                }
            }

            var index = LobbyUsers.FindIndex((user) => {
                return user.Username == serializer.Username;
            });
            if (index != -1) {
                LobbyUsers.RemoveAt(index);
            }

            TextManager.AddMessage("Użytkownik " + serializer.Username + " odszedł od stołu.");
        }
        // start gry
        else if (signalName == PlayerClickedGameStartSerializer.SIGNAL_PLAYER_READY) {
            var serializer = new PlayerClickedGameStartSerializer(signalData);
            serializer.Validate();

            if (serializer.Username != UserData.Username)
            {
                LobbyState = LobbyState.IN_GAME;
                Game.Match.Start();

                //var getHandRequestData = new ServerSocket.Actions.GetHand.RequestSerializer();
                //getHandRequestData.PlayerTag = (int)UserData.Position;
                //PerformServerAction("get-hand", getHandRequestData.GetApiObject(), this.GetHandCallback);

                TextManager.setNSPointsValue("0", "0", "0");
                TextManager.setWEPointsValue("0", "0", "0");

                ClearContractLabelPaint();
            }
        }
        else if (signalName == LobbySignalNewBidSerializer.SIGNAL_NEW_BID) {
            var serializer = new LobbySignalNewBidSerializer(signalData);
            serializer.Validate();

            if (serializer.Username != UserData.Username) {
                try {
                    var contract = new Contract(
                        (ContractHeight)serializer.Height,
                        (ContractColor)serializer.Color,
                        (PlayerTag)serializer.PlaceTag,
                        serializer.X,
                        serializer.XX
                    );
                    Game.Match.AddBid(contract);
                    AuctionModule.AddContractToList(contract);

                    if (Game.Match.CurrentBidding.IsEnd()) {
                        if (Game.Match.CurrentGame.Declarer == PlayerTag.N || Game.Match.CurrentGame.Declarer == PlayerTag.S) {
                            DeclaredContractLabel.GetComponent<Text>().text = "Contract:\nNS, " + Game.Match.CurrentBidding.HighestContract.ToString();
                        }
                        else if (Game.Match.CurrentGame.Declarer == PlayerTag.E || Game.Match.CurrentGame.Declarer == PlayerTag.W) {
                            DeclaredContractLabel.GetComponent<Text>().text = "Contract:\nEW, " + Game.Match.CurrentBidding.HighestContract.ToString();
                        }
                        TeamTakenHandsCounterLabel.GetComponent<Text>().text = "NS : 0\nEW : 0";
                    }

                } catch (WrongBidException) {
                    // TODO: Pobrać całą licytację jeszcze raz
                }
            }
        }
        else if (signalName == PutCardSignalSerializer.SIGNAL_USER_PUT_CARD)
        {
            var serializer = new PutCardSignalSerializer(signalData);
            serializer.Validate();
            Debug.Log(PutCardSignalSerializer.SIGNAL_USER_PUT_CARD + "|Player who put card: " + serializer.Username + "|Me: " + UserData.Username);

            if (serializer.Username != UserData.Username)
            {
                try
                {
                    PutCardOnTable((CardFigure)serializer.CardFigure, (CardColor)serializer.CardColor, (PlayerTag)serializer.OwnerPosition);
                }
                catch (WrongCardException)
                {
                    // TODO: coś zrobić
                }
            }
        }
        else if (signalName == LobbySignalLobbyStateChangedSerializer.SIGNAL_LOBBY_STATE_CHANGED) {
            var serializer = new LobbySignalLobbyStateChangedSerializer(signalData);
            serializer.Validate();

            LobbyState = (LobbyState)serializer.LobbyState;
        }
        else if (signalName == LobbySignalGameStartedNextBiddingSerializer.SIGNAL_GAME_STARTED_NEXT_BIDDING) {
            var serializer = new LobbySignalGameStartedNextBiddingSerializer(signalData);
            serializer.Validate();

            Game.Match.ClearPlayerHands();

            if (MyCards != null)
                MyCards.Clear();
            MyCards = null;
            if (CurrentGrandpaCards != null)
                CurrentGrandpaCards.Clear();
            CurrentGrandpaCards = null;
            if (CurrentDeclarerCards != null)
                CurrentDeclarerCards.Clear();
            CurrentDeclarerCards = null;

            ResetPlayersCards();
            // SendGetHandRequest = true;

            // TODO Rozpoczac licytacje

            if (Game.Match.GameState != GameState.BIDDING) {
                Game.Match.GameState = GameState.BIDDING;
                Game.Match.StartBidding();
            }

            Game.Match.PointsNS[0] = serializer.PointsNSBelowLine;
            Game.Match.PointsNS[1] = serializer.PointsNSAboveLine;
            Game.Match.PointsWE[0] = serializer.PointsWEBelowLine;
            Game.Match.PointsWE[1] = serializer.PointsWEAboveLine;
            Game.Match.RoundsNS = serializer.RoundsNS;
            Game.Match.RoundsWE = serializer.RoundsWE;

            TextManager.setNSPointsValue(Game.Match.PointsNS[1].ToString(), Game.Match.PointsNS[0].ToString(), Game.Match.RoundsNS.ToString());
            TextManager.setWEPointsValue(Game.Match.PointsWE[1].ToString(), Game.Match.PointsWE[0].ToString(), Game.Match.RoundsWE.ToString());

            ClearContractLabelPaint();
        }
        else if (signalName == ChatMessageSerializer.SIGNAL_CHAT_MESSAGE)
        {
            var serializer = new ChatMessageSerializer(signalData);
            serializer.Validate();

            if (serializer.Username != UserData.Username)
            {
                TextManager.AddMessage(serializer.Message);
            }
        }
        else if(signalName == LobbySignalGameFinishedSerializer.SIGNAL_GAME_FINISHED) {
            var serializer = new LobbySignalGameFinishedSerializer(signalData);
            serializer.Validate();

            Game.Match.ClearPlayerHands();
            if (MyCards != null)
                MyCards.Clear();
            MyCards = null;
            if (CurrentGrandpaCards != null)
                CurrentGrandpaCards.Clear();
            CurrentGrandpaCards = null;
            if (CurrentDeclarerCards != null)
                CurrentDeclarerCards.Clear();
            CurrentDeclarerCards = null;

            ResetPlayersCards();

            Game.Match.GameState = GameState.GAME_FINISHED;

            LobbyState = LobbyState.IDLE;

            Game.Match.PointsNS[1] = serializer.PointsNS;
            Game.Match.PointsNS[0] = 0;
            Game.Match.PointsWE[1] = serializer.PointsWE;
            Game.Match.PointsWE[0] = 0;
            Game.Match.RoundsNS = serializer.RoundsNS;
            Game.Match.RoundsWE = serializer.RoundsWE;

            TextManager.setNSPointsValue(Game.Match.PointsNS[1].ToString(), Game.Match.PointsNS[0].ToString(), Game.Match.RoundsNS.ToString());
            TextManager.setWEPointsValue(Game.Match.PointsWE[1].ToString(), Game.Match.PointsWE[0].ToString(), Game.Match.RoundsWE.ToString());

            ClearContractLabelPaint();
        }
    }
    private void OnServerSignalReceive(object sender, StandardResponseWrapperSerializer data) {
        //Debug.Log(data.CommunicateType);
        //Debug.Log(data.Data);

        switch (data.CommunicateType) {
            case "LOBBY_SIGNAL":
                HandleLobbySignal(data.Data);
                break;

            default:
                //Debug.Log("Unrecognized signal");
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
        TeamTakenHandsCounterLabel.GetComponent<Text>().text = "";
        DeclaredContractLabel.GetComponent<Text>().text = "";

        // przypisanie click handlerów do przyciskow panelu chatu/licytacji/punktacji
        ChatButton.onClick.AddListener(() => { TextManager.ChatButton(); });
        AuctionButton.onClick.AddListener(() => { TextManager.BidButton(); });
        PointsButton.onClick.AddListener(() => { TextManager.PointsButton(Game); });
        TextManager.ChatButton(); // inicjalnie otwarty jest panel chatu

        QuitButton.onClick.AddListener(() => { QuitHandler(); });
        StartButton.onClick.AddListener(() => { StartGame(); });

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
            //Debug.Log(request.ResponseData);
            var actionsSerializer = new ActionsSerializer(request.ResponseData);
            actionsSerializer.Validate();
            var responseSerializer = new GetTableInfoSerializer(actionsSerializer.Actions[0].ActionData);
            try {
                responseSerializer.Validate();
                //Debug.Log(responseSerializer);
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
            //Debug.Log(CurrentRequest.ResponseData);
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
        if (!IgnoreCommunication) {
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
                    if(
                        MyCards == null
                        && (Game.Match.GameState == GameState.BIDDING || Game.Match.GameState == GameState.PLAYING)
                    ) {
                        var getHandRequestData = new ServerSocket.Actions.GetHand.RequestSerializer();
                        getHandRequestData.PlayerTag = (int)UserData.Position;
                        PerformServerAction("get-hand", getHandRequestData.GetApiObject(), this.GetHandCallback);
                    }
                    else if (
                        Game.Match.GameState == GameState.PLAYING
                        && (
                                Game.Match.CurrentGame.TrickList.Count > 0
                                || (Game.Match.CurrentGame.TrickList.Count == 0 && Game.Match.CurrentGame.currentTrick.CardList.Count > 0)
                           )
                        && CurrentGrandpaCards == null
                    ) {
                        this.FetchGrandpaCards();
                    }
                    else if (CurrentDeclarerCards == null && DeclarerCardsVisible) {
                        var getHandRequestData = new ServerSocket.Actions.GetHand.RequestSerializer() {
                            PlayerTag = (int)Game.Match.CurrentGame.Declarer
                        };
                        PerformServerAction("get-hand", getHandRequestData.GetApiObject(), this.GetDeclarerHandCallback, Game.Match.CurrentGame.Declarer);
                    }
                    break;
            }
        }

        CheckCurrentPlayerLight();

        if (Game != null) {
            if (SeatManager.AllFourPlayersPresent() && (LobbyState == LobbyState.IDLE)) {
                if (LobbyOwner) {
                    StartButton.gameObject.SetActive(true);
                    GameStoppedDialog.gameObject.SetActive(false);
                }
            }
            else if (Game.Match.GameState != GameState.AWAITING_PLAYERS && LobbyState == LobbyState.IDLE) {
                GameStoppedDialog.gameObject.SetActive(true);
                StartButton.gameObject.SetActive(false);
            }
            else {
                if (StartButton != null) StartButton.gameObject.SetActive(false);
                GameStoppedDialog.gameObject.SetActive(false);
            }
        }
    }

    private void ReloadTableAwaitingState(GetTableInfoSerializer tableData)
    {
        Game = new Game(this);

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
                SeatManager.SitPlayer((PlayerTag)tableData.Players[i].PlayerTag, tableData.Players[i].Username);
            }
        }

        LobbyUsers = new List<LobbyUserData>();
        for(int i = 0; i < tableData.LobbyUsers.Length; i++)
        {
            LobbyUsers.Add(new LobbyUserData(tableData.LobbyUsers[i].Username));
        }
    }

    private void ReloadTableBiddingState(GetTableInfoSerializer tableData)
    {
        Game = new Game(this);

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

        LobbyUsers = new List<LobbyUserData>();
        for (int i = 0; i < tableData.LobbyUsers.Length; i++)
        {
            LobbyUsers[i] = new LobbyUserData(tableData.LobbyUsers[i].Username);
        }

        PlayerTag StartingPlayer = Game.Match.CurrentBidding.CurrentPlayer;
        AuctionModule.InitAuctionModule(Game, StartingPlayer);
        AuctionModule.ReloadDeclarations();
    }

    public void SendBidRequest(ContractHeight Height, ContractColor Color, bool XEnabled, bool XXEnabled)
    {
        if (CurrentConnectionState == ConnectionState.IDLE) {
            var bidData = new ServerSocket.Actions.Bid.RequestSerializer();
            bidData.Height = (int)Height;
            bidData.Color = (int)Color;
            bidData.X = XEnabled;
            bidData.XX = XXEnabled;

            PerformServerAction("bid", bidData.GetApiObject(), this.BidCallback);
        }
    }

    private void BidCallback(Request request, ActionsSerializer response, object additionalData) {
        if (((string)response.Actions[0].ActionData.GetValue("status")).CompareTo("OK") != 0) {
            return;
        }

        var data = new ServerSocket.Actions.Bid.ResponseSerializer(response.Actions[0].ActionData);
        data.Validate();

        try {
            var contract = new Contract(
                (ContractHeight)data.Height,
                (ContractColor)data.Color,
                UserData.Position,
                data.X,
                data.XX
            );
            Game.Match.AddBid(contract);
            AuctionModule.AddContractToList(contract);

            if (Game.Match.CurrentBidding.IsEnd())
            {
                if (Game.Match.CurrentGame.Declarer == PlayerTag.N || Game.Match.CurrentGame.Declarer == PlayerTag.S)
                {
                    DeclaredContractLabel.GetComponent<Text>().text = "Contract:\nNS, " + Game.Match.CurrentBidding.HighestContract.ToString();
                }
                else if (Game.Match.CurrentGame.Declarer == PlayerTag.E || Game.Match.CurrentGame.Declarer == PlayerTag.W)
                {
                    DeclaredContractLabel.GetComponent<Text>().text = "Contract:\nEW, " + Game.Match.CurrentBidding.HighestContract.ToString();
                }
                TeamTakenHandsCounterLabel.GetComponent<Text>().text = "NS : 0\nEW : 0";
            }
        } catch (WrongBidException) {
            // TODO: Pobierz całą licytację jeszcze raz
        }

        //AuctionModule.AuctionState.UpdateContract();
        //UpdateContractList();
        //if (GameConfig.DevMode) {
        //    UserData.Position = MainModule.Match.CurrentBidding.CurrentPlayer; // for dev mode
        //}
        //PassCounter = 0;
    }

    void OnDestroy()
    {
        UserData.ClientConnection.SignalReceived -= OnServerSignalReceive;

        if (ChatButton) ChatButton.onClick.RemoveAllListeners();
        if (AuctionButton) AuctionButton.onClick.RemoveAllListeners();
        if (PointsButton) PointsButton.onClick.RemoveAllListeners();

        if (QuitButton) QuitButton.onClick.RemoveAllListeners();
        if (StartButton) StartButton.onClick.RemoveAllListeners();
    }

    private void QuitHandler()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    private void CheckCurrentPlayerLight()
    {
        GameObject.Find("Player3IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 255f, 100f);
        GameObject.Find("Player4IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 255f, 100f);
        GameObject.Find("Player1IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 255f, 100f);
        GameObject.Find("Player2IndicatorLight").GetComponent<Image>().color = new Color(255f, 255f, 255f, 100f);

        if (Game != null && Game.Match != null)
        {
            string PlayerTagString = "";
            if (Game.Match.GameState == GameState.BIDDING)
            {
                PlayerTagString = Game.Match.CurrentBidding.CurrentPlayer.ToString();
            }
            else if (Game.Match.GameState == GameState.PLAYING)
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
        if (UserData.Position != PlayerTag.NOBODY)
        {
            GameObject.Find("Player3IndicatorText").GetComponent<Text>().text = UserData.Position.ToString();
            GameObject.Find("Player4IndicatorText").GetComponent<Text>().text = ((PlayerTag)(((int)UserData.Position + 1) % 4)).ToString();
            GameObject.Find("Player1IndicatorText").GetComponent<Text>().text = ((PlayerTag)(((int)UserData.Position + 2) % 4)).ToString();
            GameObject.Find("Player2IndicatorText").GetComponent<Text>().text = ((PlayerTag)(((int)UserData.Position + 3) % 4)).ToString();
        }
    }

    public void UpdateTable(Game Game, List<GameManagerLib.Models.Card> PlayerHand)
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

    public void StartGame()
    {
        if (CurrentConnectionState == ConnectionState.IDLE) {
            var startGameRequestData = new ServerSocket.Actions.StartGame.RequestSerializer();
            startGameRequestData.PlaceTag = (int)UserData.Position;
            startGameRequestData.Username = UserData.Username;
            PerformServerAction("start-game", startGameRequestData.GetApiObject(), this.StartGameCallback);
        }
    }

    public void GetHandCallback(Request request, ActionsSerializer response, object additionalData)
    {
        if (((string)response.Actions[0].ActionData.GetValue("status")).CompareTo("OK") != 0) {
            return;
        }

        var data = new ServerSocket.Actions.GetHand.ResponseSerializer(response.Actions[0].ActionData);
        data.Validate();
        Debug.Log("GetHandCallback...");

        if (data.Status == "OK")
        {
            try
            {
                UserData.Cards = new List<GameManagerLib.Models.Card>();
                GameManagerLib.Models.Card card;
                MyCards = new List<GameManagerLib.Models.Card>();
                var player = Game.Match.GetPlayerAt(UserData.Position);
                for (int i = 0; i < data.Cards.Length; i++)
                {
                    card = new GameManagerLib.Models.Card(
                                (CardFigure)data.Cards[i].Figure,
                                (CardColor)data.Cards[i].Color,
                                UserData.Position,
                                (CardState)data.Cards[i].State
                            );
                    UserData.Cards.Add(card);
                    
                    player.Hand[i] = card;
                    MyCards.Add(card);
                }

                HiddenCardsOfPlayerN = new List<GameObject>();
                HiddenCardsOfPlayerE = new List<GameObject>();
                HiddenCardsOfPlayerS = new List<GameObject>();
                HiddenCardsOfPlayerW = new List<GameObject>();
                UpdateTable(Game, UserData.Cards);

                PlayerTag StartingPlayer = Game.Match.CurrentBidding.CurrentPlayer;
                AuctionModule.InitAuctionModule(Game, StartingPlayer);

                GameObject auctionObject = GameObject.Find("/Canvas/TableCanvas/AuctionDialog");
                auctionObject.SetActive(true);
                if (StartButton != null)
                {
                    StartButton.gameObject.SetActive(false);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                // jakaś obsługa jeśli musi być?
            }
        }
    }

    public void GetDeclarerHandCallback(Request request, ActionsSerializer response, object additionalData) {
        if (((string)response.Actions[0].ActionData.GetValue("status")).CompareTo("OK") != 0) {
            return;
        }
        PlayerTag declarer = (PlayerTag)additionalData;

        var data = new ServerSocket.Actions.GetHand.ResponseSerializer(response.Actions[0].ActionData);
        data.Validate();
        Debug.Log("GetHandCallback...");

        if (data.Status == "OK") {
            try {
                if (CurrentDeclarerCards != null) CurrentDeclarerCards.Clear();
                CurrentDeclarerCards = new List<GameManagerLib.Models.Card>();
                GameManagerLib.Models.Card card;
                var player = Game.Match.GetPlayerAt(declarer);
                for (int i = 0; i < data.Cards.Length; i++) {
                    card = new GameManagerLib.Models.Card(
                                (CardFigure)data.Cards[i].Figure,
                                (CardColor)data.Cards[i].Color,
                                declarer,
                                (CardState)data.Cards[i].State
                            );

                    player.Hand[i] = card;
                    CurrentDeclarerCards.Add(card);
                }

                this.ShowPlayerCards(declarer, player.Hand);
            } catch (Exception e) {
                Debug.Log(e.Message);
                // jakaś obsługa jeśli musi być?
            }
        }
    }


    public void FetchGrandpaCards()
    {
        int grandId = ((int)Game.Match.CurrentGame.Declarer + 2) % 4;
        var getGrandpaHandRequestData = new ServerSocket.Actions.GetHand.RequestSerializer();
        getGrandpaHandRequestData.PlayerTag = grandId;
        PerformServerAction("get-hand", getGrandpaHandRequestData.GetApiObject(), this.GetGrandpaHandCallback, (PlayerTag)grandId);
    }

    public void GetGrandpaHandCallback(Request request, ActionsSerializer response, object additionalData)
    {
        if (((string)response.Actions[0].ActionData.GetValue("status")).CompareTo("OK") != 0)
        {
            return;
        }

        PlayerTag grandpaTag = (PlayerTag)additionalData;

        var data = new ServerSocket.Actions.GetHand.ResponseSerializer(response.Actions[0].ActionData);
        data.Validate();
        Debug.Log("GetGrandpaHandCallback...");
        if (data.Status == "OK")
        {
            try
            {
                GameManagerLib.Models.Card card;
                CurrentGrandpaCards = new List<GameManagerLib.Models.Card>();
                Player grandpa = Game.Match.GetPlayerAt(grandpaTag);
                for (int i = 0; i < data.Cards.Length; i++)
                {
                    card = new GameManagerLib.Models.Card(
                                (CardFigure)data.Cards[i].Figure,
                                (CardColor)data.Cards[i].Color,
                                grandpaTag,
                                (CardState)data.Cards[i].State
                            );
                    grandpa.Hand[i] = card;
                    CurrentGrandpaCards.Add(card);
                }

                this.ShowPlayerCards(grandpa.Tag, grandpa.Hand);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                // jakaś obsługa jeśli musi być?
            }
        }
    }

    public void StartGameCallback(Request request, ActionsSerializer response, object additionalData)
    {
        if (((string)response.Actions[0].ActionData.GetValue("status")).CompareTo("OK") != 0) {
            return;
        }

        var data = new ServerSocket.Actions.StartGame.ResponseSerializer(response.Actions[0].ActionData);
        data.Validate();
        if (Game.Match.GameState == GameState.STARTING || Game.Match.GameState == GameState.GAME_FINISHED) {
            try {
                LobbyState = LobbyState.IN_GAME;
                Game.Match.Start();
            } catch (GameManagerLib.Exceptions.WrongGameStateException e) {
                // TODO
            }
        }
        else {
            LobbyState = LobbyState.IN_GAME;
        }

        var getHandRequestData = new ServerSocket.Actions.GetHand.RequestSerializer();
        getHandRequestData.PlayerTag = (int)UserData.Position;
        TextManager.setNSPointsValue("0", "0", "0");
        TextManager.setWEPointsValue("0", "0", "0");

        ClearContractLabelPaint();
        PerformServerAction("get-hand", getHandRequestData.GetApiObject(), this.GetHandCallback);
    }

    /*public void RestartGame()
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
    }*/

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

    public void ShowPlayerCards(PlayerTag player, GameManagerLib.Models.Card[] playerHand)
    {
        switch (player)
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
        GiveCardsToPlayer(player, playerHand);
    }

    private void DestroyHiddenCards(List<GameObject> cards)
    {
        for(int i = 0; i < cards.Count; i++)
        {
            Destroy(cards[i]);
        }
        cards.Clear();
    }

    private void GiveHiddenCardsToPlayers(PlayerTag MyPosition)
    {
        foreach (int player in System.Enum.GetValues(typeof(PlayerTag)))
        {
            GameObject CardsObject = GameObject.Find("Cards");

            if (player != (int)MyPosition && player != (int)PlayerTag.NOBODY)
            {
                List<float[]> coordinates = CalculateCardsCoordinates((PlayerTag)player);

                int numOfCards = 13;
                if(Game.Match.GameState == GameState.PLAYING) {
                    int tmp1 = Game.Match.CurrentGame.currentTrick.CardList.Count;
                    int tmp2 = (int)Game.Match.CurrentGame.CurrentPlayer;
                    int tmp3 = tmp1 - tmp2;
                    if (tmp3 < 0) tmp3 += 4;

                    PlayerTag tmpFirstPlacer = (PlayerTag)tmp3;
                    int tmpThisPlayerId = (int)player;
                    int tmpFirstPlacerId = (int)tmpFirstPlacer;
                    if(tmpThisPlayerId < tmpFirstPlacerId) {
                        tmpThisPlayerId += 4;
                    }

                    if(tmpThisPlayerId >= tmpFirstPlacerId + tmp1) {
                        numOfCards = 13 - Game.Match.CurrentGame.TrickList.Count;
                    }
                    else {
                        numOfCards = 12 - Game.Match.CurrentGame.TrickList.Count;
                        if (numOfCards < 0) numOfCards = 0;
                    }

                }

                for (int i = 0; i < numOfCards; i++)
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
                    // adding parent components for cards => better responsivity
                    if ((int)MyPosition == (((int)player + 1) % 4) )
                    {
                        card.transform.localPosition = new Vector3(LeftPlayerCardsCanvas.transform.position.x, coordinates[i][1], 0);
                        card.transform.parent = LeftPlayerCardsCanvas.transform;
                    }
                    else if ((int)MyPosition == (((int)player + 3) % 4) )
                    {
                        card.transform.localPosition = new Vector3(RightPlayerCardsCanvas.transform.position.x, coordinates[i][1], 0);
                        card.transform.parent = RightPlayerCardsCanvas.transform;
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
            for (int i = 0; i < 13; i++)
            {
                float[] tmp = new float[3];
                tmp[0] = myCardsX[i];
                tmp[1] = -3.28f; //-2.58f; // previous y value: -3.28f;
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
                tmp[1] = 3.28f;// 2.78f; // previous y value: 3.07f;
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

    /// <summary>
    /// Resets all card prefabs data (card isn't possessed by anyone and initially is in deck)
    /// </summary>
    private void ResetPlayersCards()
    {
        foreach(int cardHeight in System.Enum.GetValues(typeof(CardFigure)))
        {
            foreach(int cardColor in System.Enum.GetValues(typeof(CardColor)))
            {
                string cardName = CalculateCardName((CardFigure)cardHeight, (CardColor)cardColor);
                GameObject cardObject = GameObject.Find(cardName);
                cardObject.GetComponent<Card>().PlayerID = PlayerTag.NOBODY;
                cardObject.GetComponent<Card>().CurrentState = CardState.IN_DECK;
                cardObject.transform.localPosition = new Vector3(-1000, 0, cardObject.transform.localPosition.z);
            }
        }
        DestroyHiddenCards(HiddenCardsOfPlayerN);
        DestroyHiddenCards(HiddenCardsOfPlayerS);
        DestroyHiddenCards(HiddenCardsOfPlayerE);
        DestroyHiddenCards(HiddenCardsOfPlayerW);
    }

    /// <summary>
    /// Metoda rozdaje karty graczowi
    /// </summary>
    /// <param name="PlayerIdentifier">Identyfikator gracza, któremu rozdawane są karty</param>
    /// <param name="cards">Lista kart na ręce danego gracza. Obiekt listy musi być typu "GameManagerLib.Models.Card", gdzie klasa "Card" musi być poprzedzona przestrzenią nazw - inaczej pojawia się konflikt</param>
    private void GiveCardsToPlayer(PlayerTag PlayerIdentifier, List<GameManagerLib.Models.Card> cards)
    {
        List<float[]> coordinates = CalculateCardsCoordinates(PlayerIdentifier);
        for (int i = 0; i < cards.Count; i++)
        {
            string cardName = CalculateCardName(cards[i]);
            GameObject card = GameObject.Find(cardName);
            card.transform.localPosition = new Vector3(coordinates[i][0], coordinates[i][1], coordinates[i][2]);

            card.GetComponent<Card>().PlayerID = PlayerIdentifier;
            SpriteRenderer sr = card.GetComponent<SpriteRenderer>();
            sr.sortingOrder = i + 1;
        }
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

    /// <summary>
    /// Calculates Unity object name for card according to given figure and color
    /// </summary>
    /// <param name="Figure">Figure of card which name we want to get to know</param>
    /// <param name="Color">Color of card which name we want to get to know</param>
    /// <returns>Calculated card name if given data are correct, otherwise returns an empty string</returns>
    private string CalculateCardName(CardFigure Figure, CardColor Color)
    {
        char color = ' ', figure = ' ';
        bool errorOccurred = false;
        switch (Color)
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

        if ((int)Figure < 2)
        {
            errorOccurred = true;
        }
        else if ((int)Figure > 9)
        {
            switch (Figure)
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
            figure = ((int)Figure).ToString()[0];
        }

        if (!errorOccurred)
        {
            return "CARD_" + figure + color;
        }
        return "";
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

    public void PutCardOnTable(CardFigure Figure, CardColor Color, PlayerTag ownerTag, GameManagerLib.Models.Card cardOrigin = null)
    {
        // ten kawałek jest tylko po to by przechodziła walidacja w GamerLib -> dostajemy sygnał od serwera który ma ZAWSZE rację
        GameManagerLib.Models.Card card;
        var Player = Game.Match.GetPlayerAt(ownerTag);

        if (cardOrigin == null)
            card = new GameManagerLib.Models.Card(Figure, Color, Player.Tag, CardState.ON_HAND);
        else
            card = cardOrigin;

        string cardName = CalculateCardObjectName(Figure, Color);

        switch (Player.Tag) {
            case PlayerTag.N:
                if (HiddenCardsOfPlayerN.Count == 0) break;
                HiddenCardsOfPlayerN[12 - Game.Match.CurrentGame.TrickList.Count].transform.localPosition = new Vector3(-100, 0, 0);
                break;
            case PlayerTag.E:
                if (HiddenCardsOfPlayerE.Count == 0) break;
                HiddenCardsOfPlayerE[12 - Game.Match.CurrentGame.TrickList.Count].transform.localPosition = new Vector3(-100, 0, 0);
                break;
            case PlayerTag.S:
                if (HiddenCardsOfPlayerS.Count == 0) break;
                HiddenCardsOfPlayerS[12 - Game.Match.CurrentGame.TrickList.Count].transform.localPosition = new Vector3(-100, 0, 0);
                break;
            case PlayerTag.W:
                if (HiddenCardsOfPlayerW.Count == 0) break;
                HiddenCardsOfPlayerW[12 - Game.Match.CurrentGame.TrickList.Count].transform.localPosition = new Vector3(-100, 0, 0);
                break;
        }

        //Debug.Log("Putting card on table from player " + PlayerName);
        // kładę kartę na stół w logice gry GamerLib
        try
        {
            Game.Match.CurrentGame.NextCard(card);
        }
        catch (WrongCardException)
        {
            Debug.Log("WrongCardException was thrown");
        }

        // kładę kartę na stół wizualnie w Unity
        float[] newPos = new float[2];
        if (Player.Tag == UserData.Position)
        {
            newPos = CalculatePutCardPosition('D');
        }
        else if (Player.Tag == (PlayerTag)(((int)UserData.Position + 1) % 4))
        {
            newPos = CalculatePutCardPosition('L');
        }
        else if (Player.Tag == (PlayerTag)(((int)UserData.Position + 2) % 4))
        {
            newPos = CalculatePutCardPosition('U');
        }
        else if (Player.Tag == (PlayerTag)(((int)UserData.Position + 3) % 4))
        {
            newPos = CalculatePutCardPosition('R');
        }

        GameObject cardToPut = GameObject.Find(cardName);
        cardToPut.transform.localPosition = new Vector3(newPos[0], newPos[1], -1);

        //if (GameConfig.DevMode)
        //{
        //    UserData.Position = (PlayerTag)(((int)UserData.Position + 1) % 4); // for dev mode
        //}

        if (Game.IsTrickComplete())
        {
            IgnoreCommunication = true;

            // Opóźnienie pół sekundy przed tym jak karty znikną
            StartCoroutine(OnTrickCompleteWithDelay(0.5f));                
        }
    }
    IEnumerator OnTrickCompleteWithDelay(float time) {
        yield return new WaitForSeconds(time);

        Trick lastTrick = Game.Match.CurrentGame.TrickList[Game.Match.CurrentGame.TrickList.Count - 1];
        GameObject tmp;
        for (int i = 0; i < lastTrick.CardList.Count; i++) {
            string tmpCardName = CalculateCardName(lastTrick.CardList[i]);
            tmp = GameObject.Find(tmpCardName);
            tmp.transform.position = new Vector3(-100, 0, 0);
        }
        if (Game.Match.GameState == GameState.PLAYING) {

            Text TeamTakenHandsCounterLabelText = TeamTakenHandsCounterLabel.GetComponent<Text>();
            int NSTaken = Game.CalculateTeamTricks(PlayerTag.N, PlayerTag.S);
            int EWTaken = Game.CalculateTeamTricks(PlayerTag.E, PlayerTag.W);

            TeamTakenHandsCounterLabelText.text = "NS : " + NSTaken.ToString() + "\n";
            TeamTakenHandsCounterLabelText.text += "EW : " + EWTaken.ToString();

            if (Game.Match.CurrentGame.Declarer == PlayerTag.N || Game.Match.CurrentGame.Declarer == PlayerTag.S) {
                PaintContractLabel(NSTaken, EWTaken);
            }
            else if (Game.Match.CurrentGame.Declarer == PlayerTag.E || Game.Match.CurrentGame.Declarer == PlayerTag.W) {
                PaintContractLabel(EWTaken, NSTaken);
            }

            if (GameConfig.DevMode) {
                UserData.Position = lastTrick.Winner; // for dev mode
            }
        }

        IgnoreCommunication = false;

        //if (Game.Match.CurrentGame.IsEnd())
        //{
        //    RestartGame(); // restart game if all 13 tricks were put on the table
        //}
    }
    
    private string CalculateCardObjectName(CardFigure Figure, CardColor Color)
    {
        string c = "";
        switch (Color)
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
        if ((int)Figure > 9)
        {
            switch ((int)Figure)
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
            cardName = "CARD_" + (int)Figure + c;
        }
        return cardName;
    }

    public void putCard(Card card) // Card jako klasa podpięta pod obiekt Unity -> "Card" używane w Game.Match to "GameManagerLib.Models.Card"
    {
        // Tak na wszelki wypadek, bo niedowierzam kodowi Radka i Marcina
        if (PlayingAsGrandpa) return;

        bool canUserPutCard = Game.Match.CheckNextCard(card.PlayerID, card.Color, card.Figure);
        Debug.Log(canUserPutCard);
        if (canUserPutCard)
        {
            this.SendPutCardRequest(card.Figure, card.Color, card.PlayerID);
        }
    }

    public void PutCardCallback(Request request, ActionsSerializer response, object additionalData)
    {
        if (((string)response.Actions[0].ActionData.GetValue("status")).CompareTo("OK") != 0)
        {
            return;
        }

        var cardOrigin = (GameManagerLib.Models.Card)additionalData;
        var data = new ServerSocket.Actions.PutCard.ResponseSerializer(response.Actions[0].ActionData);
        data.Validate();
        try
        {
            PutCardOnTable((CardFigure)data.CardFigure, (CardColor)data.CardColor, (PlayerTag)data.OwnerPosition, cardOrigin);
        }
        catch (GameManagerLib.Exceptions.WrongGameStateException e)
        {
            // TODO
        }

        if(Game.Match.GameState != GameState.PLAYING) {
            ClearContractLabelPaint();
        }
    }

    public void SendPutCardRequest(CardFigure Figure, CardColor Color, PlayerTag OwnerPosition)
    {
        var player = Game.Match.GetPlayerAt(OwnerPosition);
        GameManagerLib.Models.Card cardOrigin = null;
        GameManagerLib.Models.Card tmp;
        for(int i = 0; i < player.Hand.Length; i++) {
            tmp = player.Hand[i];
            if(tmp.Color == Color && tmp.Figure == Figure) {
                cardOrigin = tmp;
                break;
            }
        }

        if (cardOrigin != null && CurrentConnectionState == ConnectionState.IDLE) {
            var putCardRequestData = new ServerSocket.Actions.PutCard.RequestSerializer();
            putCardRequestData.CardOwnerPosition = (int)OwnerPosition;
            putCardRequestData.Color = (int)Color;
            putCardRequestData.Figure = (int)Figure;

            PerformServerAction("put-card", putCardRequestData.GetApiObject(), this.PutCardCallback, cardOrigin);
        }
    }

    private void PaintContractLabel(int TakenHands, int EnemyTakenHands)
    {
        if (Game.Match.CurrentBidding.HighestContract != null)
        {
            if (6 + (int)Game.Match.CurrentBidding.HighestContract.ContractHeight <= TakenHands)
            {
                TeamTakenHandsCounterLabelBackground.GetComponent<Image>().color = new Color32(29, 143, 35, 255);
                DeclaredContractLabelBackground.GetComponent<Image>().color = new Color32(29, 143, 35, 255);
            }
            else if (EnemyTakenHands > 13 - 6 - (int)Game.Match.CurrentBidding.HighestContract.ContractHeight)
            {
                TeamTakenHandsCounterLabelBackground.GetComponent<Image>().color = new Color32(219, 31, 35, 255);
                DeclaredContractLabelBackground.GetComponent<Image>().color = new Color32(219, 31, 35, 255);
            }
        }
        else {
            TeamTakenHandsCounterLabelBackground.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            DeclaredContractLabelBackground.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        }
    }
    private void ClearContractLabelPaint() {
        TeamTakenHandsCounterLabelBackground.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        DeclaredContractLabelBackground.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
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
}