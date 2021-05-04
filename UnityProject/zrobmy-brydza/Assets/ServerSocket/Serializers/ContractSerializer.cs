using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Serializers {
    public class ContractSerializer : BaseSerializer {
        [SerializerField(apiName:"contract_height")]
        public int ContractHeight;
        [SerializerField(apiName:"contract_color")]
        public int ContractColor;
        [SerializerField(apiName:"x_enabled")]
        public bool XEnabled;
        [SerializerField(apiName:"xx_enabled")]
        public bool XXEnabled;

        [SerializerField(apiName:"player_tag")]
        public int PlayerTag;

        public ContractSerializer() : base() { }
        public ContractSerializer(JObject data) : base(data) { }
    }
}
