using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class LobbySignalGameStartedNextBiddingSerializer : BaseSerializer {
        public const string SIGNAL_GAME_STARTED_NEXT_BIDDING = "GAME_STARTED_NEXT_BIDDING";

        [SerializerField(apiName: "signal", required: true)]
        public string Signal;

        public LobbySignalGameStartedNextBiddingSerializer() : base() { }
        public LobbySignalGameStartedNextBiddingSerializer(JObject data) : base(data) { }
    }
}
