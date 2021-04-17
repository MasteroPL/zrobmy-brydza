using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    /// <summary>
    /// Serializator odpowiadający za komunikaty dotyczące stanu Lobby, np. zamknięcie Lobby, pauza, odpauzowanie Lobby
    /// </summary>
    public class LobbySignalsSerializer : BaseSerializer {
        public const string SIGNAL_REMOVED_FROM_LOBBY = "REMOVED_FROM_LOBBU";
        public const string SIGNAL_PAUSED = "PAUSED";
        public const string SIGNAL_UNPAUSED = "UNPAUSED";
        public const string SIGNAL_CLOSED = "CLOSED";

        /// <summary>
        /// Sygnał oznaczający przełączenie lobby w stan "bidding"
        /// </summary>
        public const string SIGNAL_STATE_BIDDING = "STATE_BIDDING";
        /// <summary>
        /// Przełączenie lobby w stan rozegrania
        /// </summary>
        public const string SIGNAL_STATE_PLAYING = "STATE_PLAYING";

        // ....

        public const string SIGNAL_ROUND_FINISHED = "ROUND_FINISHED";
        public const string SIGNAL_GAME_FINISHED = "GAME_FINISHED";

        [SerializerField(apiName:"signal", required:true)]
        public string Signal;
        [SerializerField(apiName:"message", required:false, defaultValue:null)]
        public string Message;

        public LobbySignalsSerializer() : base() { }
        public LobbySignalsSerializer(JObject data) : base(data) { }
    }
}
