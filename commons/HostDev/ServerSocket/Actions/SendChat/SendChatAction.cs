using EasyHosting.Models.Actions;
using EasyHosting.Models.Serialization;
using EasyHosting.Models.Server;
using ServerSocket.Models;
using ServerSocket.Serializers;

namespace ServerSocket.Actions.SendChat {
    public class SendChatAction : BaseAction {

        public SendChatAction() : base(
            typeof(RequestSerializer),
            typeof(ResponseSerializer)
        ) { }

        protected override BaseSerializer PerformAction(ClientConnection conn, BaseSerializer requestData) {
            RequestSerializer data = (RequestSerializer)requestData;
            ResponseSerializer resp = (ResponseSerializer)InitializeResponseSerializer();
            resp.Status = "OK";

            Lobby lobby = (Lobby)conn.Session.Get("joined-lobby");
            string username = (string)conn.Session.Get("username");

            string message = username + data.Message;

            var chatData = new ChatMessageSerializer();
            chatData.Message = message;

            lobby.Broadcast(chatData.GetApiObject());

            return resp;
        }
    }
}
