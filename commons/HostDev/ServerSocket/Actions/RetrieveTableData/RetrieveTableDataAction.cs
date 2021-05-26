using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using ServerSocket.Models;
using ServerSocket.Serializers;
using GameManagerLib.Models;

namespace ServerSocket.Actions.RetrieveTableData
{
    public class RetrieveTableDataAction : BaseAction
    {
        public RetrieveTableDataAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        )
        { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData)
        {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;

            string username = (string)conn.Session.Get("username");

            resp.MatchInfo = new MatchSerializer();

            resp.MatchInfo.EWPointsHistory = new string[game.History.WEHistory.Count];
            resp.MatchInfo.NSPointsHistory = new string[game.History.NSHistory.Count];
            for(int i = 0; i < game.History.WEHistory.Count; i++)
                resp.MatchInfo.EWPointsHistory[i] = game.History.WEHistory[i];
            for (int i = 0; i < game.History.NSHistory.Count; i++)
                resp.MatchInfo.NSPointsHistory[i] = game.History.NSHistory[i];

            resp.MatchInfo.CurrentBidding = new BiddingSerializer();
            resp.MatchInfo.CurrentBidding.ContractList = new ContractSerializer[game.CurrentBidding.ContractList.Count];
            for(int i = 0; i < game.CurrentBidding.ContractList.Count; i++)
            {
                var tmp = new ContractSerializer();
                tmp.ContractColor = (int)game.CurrentBidding.ContractList[i].ContractColor;
                tmp.ContractHeight = (int)game.CurrentBidding.ContractList[i].ContractHeight;
                tmp.PlayerTag = (int)game.CurrentBidding.ContractList[i].DeclaredBy;
                tmp.XEnabled = game.CurrentBidding.ContractList[i].XEnabled;
                tmp.XXEnabled = game.CurrentBidding.ContractList[i].XXEnabled;

                resp.MatchInfo.CurrentBidding.ContractList[i] = tmp;
            }
            resp.MatchInfo.CurrentBidding.BiddingEnded = game.CurrentBidding.End;
            resp.MatchInfo.CurrentBidding.CurrentPlayerTag = (int)game.CurrentBidding.CurrentPlayer;
            resp.MatchInfo.CurrentBidding.Dealer = (int)game.CurrentBidding.Dealer;

            var tmpHighestBidContract = new ContractSerializer();
            tmpHighestBidContract.ContractColor = (int)game.CurrentBidding.HighestContract.ContractColor;
            tmpHighestBidContract.ContractHeight = (int)game.CurrentBidding.HighestContract.ContractHeight;
            tmpHighestBidContract.PlayerTag = (int)game.CurrentBidding.HighestContract.DeclaredBy;
            tmpHighestBidContract.XEnabled = game.CurrentBidding.HighestContract.XEnabled;
            tmpHighestBidContract.XXEnabled = game.CurrentBidding.HighestContract.XXEnabled;

            resp.MatchInfo.CurrentBidding.HighestContract = tmpHighestBidContract;
            //resp.MatchInfo.CurrentBidding.PassCounter = game.CurrentBidding.HighestContract

            resp.MatchInfo.PointsNS = new int[game.PointsNS.Length];
            resp.MatchInfo.PointsEW = new int[game.PointsWE.Length];
            for (int i = 0; i < game.PointsNS.Length; i++)
                resp.MatchInfo.PointsNS[i] = game.PointsNS[i];
            for (int i = 0; i < game.PointsWE.Length; i++)
                resp.MatchInfo.PointsEW[i] = game.PointsWE[i];

            resp.MatchInfo.RoundsNS = game.RoundsNS;
            resp.MatchInfo.RoundsEW = game.RoundsWE;

            resp.MatchInfo.Dealer = (int)game.Dealer;
            resp.MatchInfo.GameState = (int)game.GameState;
            resp.MatchInfo.PlayerList = new PlayerSerializer[4];
            resp.MatchInfo.GrandpaCards = new CardSerializer[13];

            PlayerSerializer tmpPlayerSerializer;
            for(int i = 0; i < 4; i++)
            {
                if (game.PlayerList[i] != null)
                {
                    tmpPlayerSerializer = new PlayerSerializer();
                    tmpPlayerSerializer.PlayerTag = (int)game.PlayerList[i].Tag;
                    tmpPlayerSerializer.Username = game.PlayerList[i].Name;
                    resp.MatchInfo.PlayerList[i] = tmpPlayerSerializer;
                    switch (game.PlayerList[i].Tag)
                    {
                        case PlayerTag.N:
                            resp.MatchInfo.NPlayerHandSize = game.PlayerList[i].Hand.Length;
                            break;
                        case PlayerTag.E:
                            resp.MatchInfo.EPlayerHandSize = game.PlayerList[i].Hand.Length;
                            break;
                        case PlayerTag.S:
                            resp.MatchInfo.SPlayerHandSize = game.PlayerList[i].Hand.Length;
                            break;
                        case PlayerTag.W:
                            resp.MatchInfo.WPlayerHandSize = game.PlayerList[i].Hand.Length;
                            break;
                    }

                    bool IsGrand = IsPlayerGrand(game.PlayerList[i].Tag, game);
                    if (game.PlayerList[i].Name == username) // przypadek gdy to są moje karty
                    {
                        resp.MatchInfo.PlayerCards = new CardSerializer[game.PlayerList[i].Hand.Length];
                        for (int c = 0; c < game.PlayerList[i].Hand.Length; c++)
                        {
                            resp.MatchInfo.PlayerCards[c] = new CardSerializer();
                            resp.MatchInfo.PlayerCards[c].Color = (int)game.PlayerList[i].Hand[c].Color;
                            resp.MatchInfo.PlayerCards[c].Figure = (int)game.PlayerList[i].Hand[c].Figure;
                            resp.MatchInfo.PlayerCards[c].State = (int)game.PlayerList[i].Hand[c].CurrentState;
                        }
                        resp.MatchInfo.GrandpaCards = null;
                    }
                    else if (IsGrand) // przypadek gdy to nie są moje karty
                    {
                        resp.MatchInfo.PlayerCards = null;
                        bool GrandCardsVisible = AreGrandCardsVisible(game);
                        if (GrandCardsVisible)
                        {
                            resp.MatchInfo.GrandpaCards = new CardSerializer[game.PlayerList[i].Hand.Length];
                            for (int c = 0; c < game.PlayerList[i].Hand.Length; c++)
                            {
                                resp.MatchInfo.GrandpaCards[c] = new CardSerializer();
                                resp.MatchInfo.GrandpaCards[c].Color = (int)game.PlayerList[i].Hand[c].Color;
                                resp.MatchInfo.GrandpaCards[c].Figure = (int)game.PlayerList[i].Hand[c].Figure;
                                resp.MatchInfo.GrandpaCards[c].State = (int)game.PlayerList[i].Hand[c].CurrentState;
                            }
                        }
                        else
                        {
                            resp.MatchInfo.GrandpaCards = null;
                        }
                    }
                    
                }
                else
                {
                    resp.MatchInfo.PlayerList[i] = null;
                }
            }

            resp.MatchInfo.GameInfo = new GameInfoSerializer();
            resp.MatchInfo.GameInfo.ContractColor = (int)game.CurrentGame.ContractColor;
            resp.MatchInfo.GameInfo.CurrentPlayer = (int)game.CurrentGame.CurrentPlayer;

            resp.MatchInfo.GameInfo.CurrentTrick = new TrickSerializer();
            resp.MatchInfo.GameInfo.CurrentTrick.CardList = new CardSerializer[4];

            for (int i = 0; i < 4; i++)
                resp.MatchInfo.GameInfo.CurrentTrick.CardList[i] = null;
            for (int i = 0; i < game.CurrentGame.currentTrick.CardList.Count; i++)
            {
                resp.MatchInfo.GameInfo.CurrentTrick.CardList[i] = new CardSerializer();
                resp.MatchInfo.GameInfo.CurrentTrick.CardList[i].Color = (int)game.CurrentGame.currentTrick.CardList[i].Color;
                resp.MatchInfo.GameInfo.CurrentTrick.CardList[i].Figure = (int)game.CurrentGame.currentTrick.CardList[i].Figure;
                resp.MatchInfo.GameInfo.CurrentTrick.CardList[i].State = (int)game.CurrentGame.currentTrick.CardList[i].CurrentState;
            }
            resp.MatchInfo.GameInfo.CurrentTrick.Winner = (int)game.CurrentGame.currentTrick.Winner;
            resp.MatchInfo.GameInfo.Declarer = (int)game.CurrentGame.Declarer;
            return resp;
        }

        private bool AreGrandCardsVisible(Match game)
        {
            if (
                game.GameState == GameState.PLAYING
                && (
                    game.CurrentGame.TrickList.Count > 0
                    || (game.CurrentGame.TrickList.Count == 0 && game.CurrentGame.currentTrick.CardList.Count > 0)
                )
            )
            {
                return true;
            }
            return false;
        }

        private bool IsPlayerGrand(PlayerTag playerTag, Match game)
        {
            if (game.GameState == GameState.PLAYING && game.CurrentGame != null)
            {
                if ( ((int)game.CurrentGame.Declarer + 2) % 4 == (int)playerTag ){
                    return true;
                }
            }
            return false;
        }
    }
}
