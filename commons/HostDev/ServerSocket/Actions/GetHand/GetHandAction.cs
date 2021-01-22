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

            Player player = null;

            CardSerializer[] cards = data.Hand;
            Card[] hand = new Card[13];

            for (int i = 0; i < hand.Length; i++) {
                hand[i] = new Card((CardFigure)cards[i].Figure, (CardColor)cards[i].Color, (PlayerTag)data.PlayerID, (CardState)cards[i].State);
            }

            foreach (var p in game.PlayerList) {
                if (p.Tag == (PlayerTag)data.PlayerID) {
                    player = p;
                    break;
                }
            }

            try {
                player.Hand = hand;
            } catch (Exception e) {
                // ?????
            }

            return resp;
        }
    }
}
