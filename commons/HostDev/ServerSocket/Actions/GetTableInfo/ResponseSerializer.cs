using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;
using EasyHosting.Meta.Validators;

namespace ServerSocket.Actions.GetTableInfo {
    public class ResponseSerializer : BaseSerializer {
        [SerializerField(apiName: "status")]
        [NullValidator(canBeNull: true)]
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

        [SerializerField(apiName: "number_of_players")]
        public int NumberOfPlayers;
        [SerializerField(apiName: "players")]
        [NullValidator(canBeNull: true)]
        public PlayerSerializer[] Players;

        [SerializerField(apiName: "number_of_users_in_lobby")]
        public int NumberOfLobbyUsers;
        [SerializerField(apiName: "lobby_users")]
        [NullValidator(canBeNull: true)]
        public LobbyUserSerializer[] LobbyUsers;

        [SerializerField(apiName: "current_bidding")]
        [NullValidator(canBeNull: true)]
        public BiddingSerializer CurrentBidding;

        [SerializerField(apiName: "rounds_ns")]
        public int RoundsNS;
        [SerializerField(apiName: "rounds_we")]
        public int RoundsWE;

        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }
}
