using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using ServerSocket.Models;
using ServerSocket.Serializers;
using GameManagerLib.Exceptions;
using GameManagerLib.Models;

namespace ServerSocket.Actions.Bid
{
    public class BidAction : BaseAction
    {
        public BidAction : base(
        typeof(RequestSerializer),
        typeof(ResponseSerializer)
            ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData)
        {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;

            Player player;


            foreach (player in game.PlayerList)
            {
                if (player.Name == (string)conn.Session.Get("username"))
                {
                    break;
                }
            }

            try
            {
                game.AddBid(new Contract((Contract.ContractHeight)data.Height, (Contract.ContractColor)data.Color, player.Tag, data.X, data.XX);
            }

            return resp;
        }
}
