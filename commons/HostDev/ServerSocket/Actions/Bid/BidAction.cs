using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using ServerSocket.Models;
using ServerSocket.Serializers;
using GameManagerLib.Exceptions;
using GameManagerLib.Models;
using System;

namespace ServerSocket.Actions.Bid
{
    public class BidAction : BaseAction {
        public BidAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData) {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;

            string username = (string)conn.Session.Get("username");

            int playerIndex = game.PlayerList.FindIndex(x => { return username == x.Name; });
            // gdy nie ma takiego gracza przy stole => wywal wyjątek
            if (playerIndex == -1)
            {
                data.AddError("PlayerUsername", "INVALID_PLAYER", "Nie ma takiego gracza");
                data.ThrowException();
            }

            var player = game.PlayerList[playerIndex];

            try {
                game.AddBid(new Contract((ContractHeight)data.Height, (ContractColor)data.Color, player.Tag, data.X, data.XX));
            } catch (Exception e) {
                throw e;
            }

            return resp;
        }
    }
}
