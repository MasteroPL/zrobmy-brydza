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

            if (data.PlayerID < 0 || data.PlayerID > 3)
            {
                data.AddError("PlayerID", "INVALID_POSITION", "Nie masz uprawnień by pobrać dane o ręce danego użytkownika");
                data.ThrowException();
            }
            bool playerFound = false;
            for(int i = 0; i < game.PlayerList.Count; i++)
            {
                if (game.PlayerList[i] != null)
                {
                    if (game.PlayerList[i].Name == data.Username)
                    {
                        playerFound = true;
                    }
                }
            }
            if (!playerFound)
            {
                data.AddError("Username", "INVALID_PLAYER", "Nieprawidłowy użytkownik");
                data.ThrowException();
            }

            CardSerializer[] cards = new CardSerializer[13];

            for (int i = 0; i < game.PlayerList[data.PlayerID].Hand.Length; i++) {
                cards[i] = new CardSerializer();
                cards[i].Figure = (int)game.PlayerList[data.PlayerID].Hand[i].Figure;
                cards[i].Color = (int)game.PlayerList[data.PlayerID].Hand[i].Color;
                cards[i].State = (int)game.PlayerList[data.PlayerID].Hand[i].CurrentState;
            }

            resp.Cards = cards;
            return resp;
        }
    }
}
