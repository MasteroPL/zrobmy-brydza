using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using ServerSocket.Models;
using ServerSocket.Serializers;
using GameManagerLib.Exceptions;
using GameManagerLib.Models;
using System;

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

            if(lobby.LobbyState != LobbyState.IN_GAME) {
                data.AddError(null, "INVALID_LOBBY_STATE", "Nie można wyłożyć karty w tym stanie lobby");
                data.ThrowException();
            }

            if(game.GameState != GameState.PLAYING) {
                data.AddError(null, "INVALID_GAME_STATE", "Gra nie jest w poprawnym stanie do tej akcji");
                data.ThrowException();
            }

            var username = conn.Session.Get<string>("username");
            if (!conn.Session.Has("player")) {
                data.AddError(null, "INVALID_USER", "Podany użytkownik nie jest uczestnikiem rozgrywki");
                data.ThrowException();
            }
            var player = conn.Session.Get<Player>("player");
            var cardOwner = game.GetPlayerAt((PlayerTag)data.CardOwnerPosition);

            // Inny gracz niz my (gracz musi byc dziadkiem)
            if(player != game.GetPlayerAt((PlayerTag)data.CardOwnerPosition)) {
                if(player.Tag != game.CurrentBidding.Declarer) {
                    data.AddError(null, "INVALID_HAND", "Nie można wyłożyć karty z ręki tego gracza");
                    data.ThrowException();
                }

                if (((int)player.Tag + 2) % 4 == (int)game.CurrentBidding.Declarer) {
                    // Próbujemy wyłożyć kartę kogoś innego niż dziadka
                    data.AddError(null, "INVALID_HAND", "Nie można wyłożyć karty jako dziadek");
                    data.ThrowException();
                }
            }
            else {
                if(((int)player.Tag + 2) % 4 == (int)game.CurrentBidding.Declarer) {
                    // Jesteśmy dziadkiem
                    data.AddError(null, "INVALID_HAND", "Nie można wyłożyć karty jako dziadek");
                    data.ThrowException();
                }
            }

            // czy gracz ma taką kartę na ręce
            bool cardFound = false;
            Card card = null;
            for(int i = 0; i < cardOwner.Hand.Length; i++)
            {
                if(cardOwner.Hand[i].Color == (CardColor)data.Color && cardOwner.Hand[i].Figure == (CardFigure)data.Figure)
                {
                    cardFound = true;
                    card = cardOwner.Hand[i];
                    break;
                }
            }

            // gracz nie ma takiej karty na ręce
            if (!cardFound) {
                data.AddError(null, "INVALID_CARD", "Gracz nie posiada takiej karty na ręce");
                data.ThrowException();
            }

            bool canPlayerPutCard = game.CheckNextCard(cardOwner.Tag, card.Color, card.Figure);
            // gracz nie może wyłożyć tej karty
            if (!canPlayerPutCard) {
                data.AddError(null, "INVALID_CARD", "Nie można wyłożyć tej karty");
                data.ThrowException();
            }

            game.NextCard((PlayerTag)data.CardOwnerPosition, card.Color, card.Figure);

            Console.WriteLine(username + "> Card: " + data.CardOwnerPosition + " " + (int)card.Figure + " " + card.Color.ToString());

            var broadcastData = new PutCardSignalSerializer()
            {
                Signal = PutCardSignalSerializer.SIGNAL_USER_PUT_CARD,
                Username = username,
                OwnerPosition = data.CardOwnerPosition,
                CardFigure = data.Figure,
                CardColor = data.Color
            };
            var broadcastWrapper = new StandardCommunicateSerializer()
            {
                CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                Data = broadcastData.GetApiObject()
            };
            lobby.Broadcast(broadcastWrapper.GetApiObject());

            resp.CardFigure = (int)card.Figure;
            resp.CardColor = (int)card.Color;
            resp.OwnerPosition = (int)data.CardOwnerPosition;
            return resp;
        }
    }
}
