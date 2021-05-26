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

        [SerializerField(apiName: "points_ns_al", required: true)]
        public int PointsNSAboveLine;
        [SerializerField(apiName: "points_ns_bl", required: true)]
        public int PointsNSBelowLine;

        [SerializerField(apiName: "points_we_al", required: true)]
        public int PointsWEAboveLine;
        [SerializerField(apiName: "points_we_bl", required: true)]
        public int PointsWEBelowLine;

        [SerializerField(apiName: "rounds_ns", required: true)]
        public int RoundsNS;
        [SerializerField(apiName: "rounds_we", required: true)]
        public int RoundsWE;

        public LobbySignalGameStartedNextBiddingSerializer() : base() { }
        public LobbySignalGameStartedNextBiddingSerializer(JObject data) : base(data) { }
    }
}
