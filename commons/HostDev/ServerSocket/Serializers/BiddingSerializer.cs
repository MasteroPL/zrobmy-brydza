using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerSocket.Serializers {
    public class BiddingSerializer : BaseSerializer {
        [SerializerField(apiName:"current_player_tag")]
        public int CurrentPlayerTag;

        [SerializerField(apiName:"contract_list")]
        public ContractSerializer[] ContractList;

        [SerializerField(apiName:"highest_contract")]
        public ContractSerializer HighestContract;

        [SerializerField(apiName:"dealer")]
        public int Dealer;

        [SerializerField(apiName:"pass_counter")]
        public int PassCounter;

        [SerializerField(apiName:"bidding_ended")]
        public bool BiddingEnded;


        public BiddingSerializer() : base() { }
        public BiddingSerializer(JObject data) : base(data) { }
    }
}
