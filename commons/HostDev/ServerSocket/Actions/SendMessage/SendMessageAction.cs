using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using ServerSocket.Models;
using ServerSocket.Serializers;
using System;

namespace ServerSocket.Actions.SendMessage {
    public class SendMessageAction : BaseAction {

        public SendMessageAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData) {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            string username = (string)conn.Session.Get("username");

            var signal = new ChatMessageSerializer() 
            { 
                Signal = ChatMessageSerializer.SIGNAL_CHAT_MESSAGE,
                Username = username,
                Message = username + ": " + data.Message
            };

            Console.WriteLine("New message > {0}", username + ": " + data.Message);

            var result = new StandardCommunicateSerializer()
            {
                CommunicateType = StandardCommunicateSerializer.TYPE_LOBBY_SIGNAL,
                Data = signal.GetApiObject()
            };
            lobby.Broadcast(result.GetApiObject());
            return resp;
        }
    }
}
