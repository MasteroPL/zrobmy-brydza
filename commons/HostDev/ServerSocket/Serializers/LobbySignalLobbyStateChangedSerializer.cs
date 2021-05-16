using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class LobbySignalLobbyStateChangedSerializer :  BaseSerializer {
        public const string SIGNAL_LOBBY_STATE_CHANGED = "LOBBY_STATE_CHANGED";

        [SerializerField(apiName:"signal", required:true)]
        public string Signal;

        [SerializerField(apiName:"lobby_state", required:true)]
        public int LobbyState;

        public LobbySignalLobbyStateChangedSerializer() : base() { }
        public LobbySignalLobbyStateChangedSerializer(JObject data) : base(data) { }
    }
}
