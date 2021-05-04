using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class LobbySignalUserJoinedSerializer : BaseSerializer{
        public const string SIGNAL_USER_JOINED = "USER_JOINED";

        [SerializerField(apiName: "signal", required: true)]
        public string Signal;
        [SerializerField(apiName: "message", required: false, defaultValue: null)]
        public string Message;
        [SerializerField(apiName: "username", required: true)]
        public string Username;

        public LobbySignalUserJoinedSerializer() : base() { }
        public LobbySignalUserJoinedSerializer(JObject data) : base(data) { }
    }
}
