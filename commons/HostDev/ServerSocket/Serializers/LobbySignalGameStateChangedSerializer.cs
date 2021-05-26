using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class LobbySignalGameStateChangedSerializer : BaseSerializer {
        public const string SIGNAL_GAME_STATE_CHANGED = "GAME_STATE_CHANGED";

        [SerializerField(apiName: "signal", required: true)]
        public string Signal;

        [SerializerField(apiName: "game_state", required: true)]
        public int GameState;

        public LobbySignalGameStateChangedSerializer() : base() { }
        public LobbySignalGameStateChangedSerializer(JObject data) : base(data) { }
    }
}
