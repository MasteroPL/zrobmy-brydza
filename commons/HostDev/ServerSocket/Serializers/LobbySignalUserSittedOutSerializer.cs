using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class LobbySignalUserSittedOutSerializer : BaseSerializer {
        public const string SIGNAL_USER_SITTED_OUT= "USER_SITTED_OUT";

        [SerializerField(apiName: "signal", required: true)]
        public string Signal;
        [SerializerField(apiName: "message", required: false, defaultValue: null)]
        public string Message;
        [SerializerField(apiName: "username", required: true)]
        public string Username;

        public LobbySignalUserSittedOutSerializer() : base() { }
        public LobbySignalUserSittedOutSerializer(JObject data) : base(data) { }
    }
}
