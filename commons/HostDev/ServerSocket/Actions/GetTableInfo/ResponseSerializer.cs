using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;

namespace ServerSocket.Actions.GetTableInfo {
    public class ResponseSerializer : BaseSerializer {
        [SerializerField(apiName: "status")]
        public string Status;

        [SerializerField(apiName:"game_state")]
        public int GameState;

        [SerializerField(apiName:"dealer")]
        public int Dealer;

        [SerializerField(apiName: "points_we_above_line")]
        public int PointsWEAboveLine;
        [SerializerField(apiName: "points_we_below_line")]
        public int PointsWEBelowLine;
        [SerializerField(apiName: "points_ns_above_line")]
        public int PointsNSAboveLine;
        [SerializerField(apiName: "points_ns_below_line")]
        public int PointsNSBelowLine;

        [SerializerField(apiName: "players")]
        public PlayerSerializer[] Players;

        [SerializerField(apiName: "current_bidding")]
        public BiddingSerializer CurrentBidding;

        [SerializerField(apiName: "rounds_ns")]
        public int RoundsNS;
        [SerializerField(apiName: "rounds_we")]
        public int RoundsWE;

        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }
}
