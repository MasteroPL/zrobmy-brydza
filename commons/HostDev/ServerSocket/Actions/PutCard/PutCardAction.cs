using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using ServerSocket.Models;
using ServerSocket.Serializers;
using GameManagerLib.Exceptions;
using GameManagerLib.Models;


namespace ServerSocket.Actions.PutCard
{
    public class PutCardAction : BaseAction
    {
        public PutCardAction() : base(
            typeof(CardSerializer),
            typeof(ResponseSerializer)
           ) { }

      protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData)
        {
            CardSerializer data = (CardSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;
            Player p = null;
            foreach (Player tmp in game.PlayerList)
            {
                if ((string)conn.Session.Get("username") == p.Name)
                {
                    p = tmp;
                    break;
                }
            }
            game.NextCard(p.Tag, (CardColor)data.Color, (CardFigure)data.Figure);

            return resp;
        }
    }
}
