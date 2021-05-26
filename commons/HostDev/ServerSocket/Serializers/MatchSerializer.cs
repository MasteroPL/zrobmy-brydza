using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHosting.Meta;
using EasyHosting.Meta.Validators;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Serializers
{
    public class MatchSerializer : BaseSerializer
    {
        [SerializerField(apiName: "game_info")]
        public GameInfoSerializer GameInfo;

        [SerializerField(apiName: "player_list")]
        public PlayerSerializer[] PlayerList;

        [SerializerField(apiName: "game_state")]
        public int GameState;
        [SerializerField(apiName: "dealer")]
        public int Dealer;

        [SerializerField(apiName: "points_ns")]
        public int[] PointsNS;
        [SerializerField(apiName: "points_ew")]
        public int[] PointsEW;
        [SerializerField(apiName: "rounds_ns")]
        public int RoundsNS;
        [SerializerField(apiName: "rounds_ew")]
        public int RoundsEW;

        [SerializerField(apiName: "current_bidding")]
        public BiddingSerializer CurrentBidding;

        [SerializerField(apiName: "ns_points_history")]
        public string[] NSPointsHistory;
        [SerializerField(apiName: "ew_points_history")]
        public string[] EWPointsHistory;

        // players hands sizes
        [SerializerField(apiName: "n_player_hand_size")]
        public int NPlayerHandSize;
        [SerializerField(apiName: "e_player_hand_size")]
        public int EPlayerHandSize;
        [SerializerField(apiName: "s_player_hand_size")]
        public int SPlayerHandSize;
        [SerializerField(apiName: "w_player_hand_size")]
        public int WPlayerHandSize;

        [SerializerField(apiName: "grandpa_cards")]
        [NullValidator(canBeNull: true)]
        public CardSerializer[] GrandpaCards;

        [SerializerField(apiName: "player_cards")]
        [NullValidator(canBeNull: true)]
        public CardSerializer[] PlayerCards;

        public MatchSerializer() : base() { }
        public MatchSerializer(JObject data) : base(data) { }
    }
}
