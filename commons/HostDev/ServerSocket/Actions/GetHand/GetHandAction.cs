using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using ServerSocket.Models;
using ServerSocket.Serializers;
using GameManagerLib.Exceptions;
using GameManagerLib.Models;
using System;

namespace ServerSocket.Actions.GetHand
{
    public class GetHandAction : BaseAction
    {
    public GetHandAction() : base(
        typeof(RequestSerializer),
        typeof(ResponseSerializer)
        ){ }
        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData)
        {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;

            if (
                game.GameState != GameState.BIDDING
                && game.GameState != GameState.PLAYING
                && game.GameState != GameState.PAUSED
            ) {
                data.AddError(null, "INVALID_GAME_STATE", "W tym stanie gry nie można pobrać kart");
                data.ThrowException();
            }

            if (!conn.Session.Has("player")) {
                data.AddError(null, "INVALID_USER", "Nie masz uprawnień do pobrania kart tego gracza");
                data.ThrowException();
            }

            var player = conn.Session.Get<Player>("player");
            if((int)player.Tag != data.PlayerTag) {
                if(game.GameState != GameState.PLAYING) {
                    data.AddError(null, "INVALID_USER", "Nie masz uprawnień do pobrania kart tego gracza");
                    data.ThrowException();
                }
                if(
                    game.CurrentBidding.Declarer != player.Tag
                    || ((int)player.Tag + 2)%4 != data.PlayerTag
                ) {
                    data.AddError(null, "INVALID_USER", "Nie masz uprawnień do pobrania kart tego gracza");
                    data.ThrowException();
                }
            }

            int playerId = game.PlayerList.FindIndex((p) => {
                return (int)p.Tag == data.PlayerTag;
            });
            if(playerId == -1) {
                data.AddError(null, "INTERNAL_SERVER_ERROR", "Wystąpił błąd wewnętrzny serwera");
                data.ThrowException();
            }

            CardSerializer[] cards = new CardSerializer[13];

            for (int i = 0; i < game.PlayerList[playerId].Hand.Length; i++) {
                cards[i] = new CardSerializer();
                cards[i].Figure = (int)game.PlayerList[playerId].Hand[i].Figure;
                cards[i].Color = (int)game.PlayerList[playerId].Hand[i].Color;
                cards[i].State = (int)game.PlayerList[playerId].Hand[i].CurrentState;
            }

            resp.Cards = cards;
            return resp;
        }
    }
}
