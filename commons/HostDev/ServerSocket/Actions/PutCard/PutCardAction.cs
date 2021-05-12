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

            Player player = game.GetPlayerByUsername(data.Playername);

            // jeśli użytkownik nie uczestniczy w grze => wywal błąd
            if (player == null)
            {
                data.AddError("Playername", "INVALID_USER", "Podany użytkownik nie jest uczestnikiem rozgrywki");
                data.ThrowException();
            }

            // czy gracz ma taką kartę na ręce
            bool cardFound = false;
            Card card = null;
            for(int i = 0; i < player.Hand.Length; i++)
            {
                if(player.Hand[i].Color == (CardColor)data.Color && player.Hand[i].Figure == (CardFigure)data.Figure)
                {
                    cardFound = true;
                    card = player.Hand[i];
                    break;
                }
            }

            // gracz nie ma takiej karty na ręce
            if (!cardFound) {
                data.AddError(null, "INVALID_CARD", "Gracz nie posiada takiej karty na ręce");
                data.ThrowException();
            }

            bool canPlayerPutCard = game.CurrentGame.CheckNextCard(card);
            // gracz nie może wyłożyć tej karty
            if (!canPlayerPutCard) {
                data.AddError(null, "INVALID_CARD", "Nie można wyłożyć tej karty");
                data.ThrowException();
            }

            try
            {
                game.NextCard((PlayerTag)data.CardOwnerPosition, card.Color, card.Figure);
                var broadcastData = new PutCardSignalSerializer()
                {
                    Signal = PutCardSignalSerializer.SIGNAL_USER_PUT_CARD,
                    Username = data.Playername,
                    CardFigure = data.Figure,
                    CardColor = data.Color
                };
                var broadcastWrapper = new StandardCommunicateSerializer()
                {
                    CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                    Data = broadcastData.GetApiObject()
                };
                lobby.Broadcast(broadcastWrapper.GetApiObject());
            }
            catch(WrongCardException e)
            {
                data.AddError(null, "INVALID_CARD", "Nie można wyłożyć tej karty");
                data.ThrowException();
            }

            return resp;
        }
    }
}
