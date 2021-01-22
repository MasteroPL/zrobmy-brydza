using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Actions.LeavePlace
{
    public class ResponseSerializer : BaseSerializer
    {
        [SerializerField(apiName: "status")]
        public string Status;
        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }
}
