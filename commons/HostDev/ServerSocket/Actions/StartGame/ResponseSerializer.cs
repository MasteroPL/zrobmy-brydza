using EasyHosting.Meta;
using EasyHosting.Models.Serialization;
using Newtonsoft.Json.Linq;

namespace ServerSocket.Actions.StartGame {
    public class ResponseSerializer : BaseSerializer {
        [SerializerField(apiName: "status")]
        public int Status;

        [SerializerField(apiName: "error")]
        public string Error;

        public ResponseSerializer() : base() { }
        public ResponseSerializer(JObject data) : base(data) { }
    }
}
