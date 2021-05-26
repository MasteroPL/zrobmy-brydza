using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;
using ServerSocket.Serializers;

namespace ServerSocket.Actions.RetrieveTableData
{
    public class ResponseSerializer : BaseSerializer
    {
        [SerializerField(apiName: "status")]
        public string Status;

        [SerializerField(apiName: "match_info")]
        public MatchSerializer MatchInfo;

        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }
}
