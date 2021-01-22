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

        [SerializerField(apiName:"communicate_type", defaultValue: "CHAT_MESSAGE")]
        public string CommunicateType;

        [SerializerField(apiName:"message")]
        public string Message;

        public ChatMessageSerializer() { }
        public ChatMessageSerializer(JObject data) : base(data) { }
    }
}
