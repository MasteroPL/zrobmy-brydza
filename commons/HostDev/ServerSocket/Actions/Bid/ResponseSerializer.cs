using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Actions.Bid
{
    public class ResponseSerializer : BaseSerializer
    {
        [SerializerField(apiName: "status")]
        public string Status;

        [SerializerField(apiName: "height")]
        public int Height;
        [SerializerField(apiName: "color")]
        public int Color;
        // Double - kontra
        [SerializerField(apiName: "X")]
        public bool X;
        // redouble - rekontra
        [SerializerField(apiName: "XX")]
        public bool XX;

        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }
}
