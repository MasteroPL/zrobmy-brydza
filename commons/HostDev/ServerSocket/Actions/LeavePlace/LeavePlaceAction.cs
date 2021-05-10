using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using GameManagerLib.Models;
using ServerSocket.Models;
using GameManagerLib.Exceptions;
using System;
using ServerSocket.Serializers;

namespace ServerSocket.Actions.LeavePlace{

    public class LeavePlaceAction : BaseAction{

        public LeavePlaceAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData){
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            Match game = lobby.Game;
            Player player = new Player((PlayerTag)data.PlaceTag, (string)conn.Session.Get("username"));

            if (player != null)
            {
                try
                {                    
                    game.RemovePlayer(player);
                    conn.Session.Remove("player");

                    var username = conn.Session.Get<string>("username");

                    var signal = new LobbySignalUserSittedOutSerializer() {
                        Signal = LobbySignalUserSittedOutSerializer.SIGNAL_USER_SITTED_OUT,
                        Message = "User " + username + " joined the lobby",
                        Username = username
                    };
                    var result = new StandardCommunicateSerializer() {
                        CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                        Data = signal.GetApiObject()
                    };
                    lobby.Broadcast(result.GetApiObject());

                    Console.WriteLine("Player left at " + data.PlaceTag + ": " + conn.Session.Get("username"));
                }
                catch (WrongPlayerException) {
                    data.AddError("PlaceTag", "NOT_SEATEN_HERE", "Gracz nie siedzi na tym miejscu");
                    data.ThrowException();
                }

            }
            return resp;
        }


    }
}
