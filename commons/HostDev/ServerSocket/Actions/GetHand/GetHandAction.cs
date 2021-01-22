using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using ServerSocket.Models;
using ServerSocket.Serializers;
using GameManagerLib.Exceptions;
using GameManagerLib.Models;

namespace ServerSocket.Actions.GetHand
{
    class GetHandAction : BaseAction
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

            Player player;

            CardSerializer[] cards = data.Hand;
            Card[] hand = new Card[13];

            for (int i = 0; i < hand.Length; i++)
            {
                hand[i] = new Card((Card.CardFigure)cards[i].Figure, (Card.CardColor)cards[i].Color, (Card.PlayerTag)data.PlayerID, (Card.CardState)cards[i].State);
            }

            Player player;

            foreach (player in game.PlayerList)
            {
                if(player.Tag == (Player.PlayerTag)data.PlayerID)
                {
                    break;
                }
            }

            try
            {
                player.Hand = hand;
            }

            return resp;
        }
}
