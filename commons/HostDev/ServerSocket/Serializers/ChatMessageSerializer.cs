using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class ChatMessageSerializer : BaseSerializer {
        public const string SIGNAL_CHAT_MESSAGE = "NEW_MESSAGE";

        [SerializerField(apiName:"communicate_type", defaultValue: "CHAT_MESSAGE")]
        public string CommunicateType;

        [SerializerField(apiName:"message")]
        public string Message;

        [SerializerField(apiName: "signal")]
        public string Signal;

        [SerializerField(apiName: "username")]
        public string Username;

        public ChatMessageSerializer() { }
        public ChatMessageSerializer(JObject data) : base(data) { }
    }
}
